using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(NavMeshAgent))]
public class PersonController : MonoBehaviour
{
    protected CharacterController m_Controller;
    protected Animator m_Animator;
    public NavMeshAgent Agent { get; private set; }
    float m_StartingSpeed = 2;
    public float CurrentSpeed { get; protected set; }
    [SerializeField] protected DropSlotsManager m_handsPick;
    [SerializeField] Transform m_backDiscarder;

    public enum STATE
    {
        FREE_MOVE = 0,
        INTERACTING,
        CARRYING,
        DROPPING
    }
    public STATE State { get; set; } = 0;


    protected virtual void Start()
    {
        m_Controller = GetComponent<CharacterController>();
        m_Animator = GetComponent<Animator>();
        Agent = GetComponent<NavMeshAgent>();
        CurrentSpeed = m_StartingSpeed;
    }

    protected virtual void Update()
    {
        //m_Animator.SetFloat("speed", new Vector3(m_Controller.velocity.x, 0, m_Controller.velocity.z).magnitude);
    }

    public void DoMove(Vector3 _dir)
    {
        LookAt(Vector3.RotateTowards(transform.forward, _dir, 1, Mathf.Infinity));
        m_Controller.SimpleMove(transform.forward * CurrentSpeed);
        m_Animator.SetFloat("speed", CurrentSpeed);
    }

    public void LookAt(Vector3 _dir)
    {
        transform.rotation = Quaternion.LookRotation(_dir);
    }

    public void Stop()
    {
        m_Animator.SetFloat("speed", 0);
        m_Controller.SimpleMove(Vector3.zero);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var t = hit.transform;
        if (t.CompareTag("Pickable"))
        {
            if (State != STATE.CARRYING)
                OnCarry();
            var p = t.GetComponent<IPickable>();
            var a = new GameObject("woodAnchor").transform;
            a.SetPositionAndRotation(p.Rb.worldCenterOfMass, t.rotation);//.transform;
            t.SetParent(a, true);
            p.OnPick();
            StartCoroutine(LerpObject(a, m_handsPick.GetEmptySlot(), p.InteractedDirection));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
    }

    IEnumerator LerpObject(Transform _obj, Transform _toT, Vector3 _toDir, bool apparentDirectly = false)
    {
        if (apparentDirectly)
            _obj.SetParent(_toT);
        var timer = 1f;
        var time = 0f;
        while (time < timer)
        {
            time += Time.deltaTime;
            var ratio = time / timer;
            _obj.position = Vector3.Lerp(_obj.position, _toT.position, ratio);
            _obj.rotation = Quaternion.Lerp(_obj.rotation, _toT.rotation * Quaternion.LookRotation(_toDir), ratio);
            yield return new WaitForEndOfFrame();
        }
        _obj.position = _toT.position;
        _obj.SetParent(_toT);
        _obj.localRotation = Quaternion.LookRotation(_toDir);// Quaternion.identity;        
    }

    public virtual void DoInteraction(InteractionPlate _interactionPlate, Vector3 _rotationCorrection)
    {
        Drop(_interactionPlate.DropSlotter);
    }

    protected virtual void OnCarry()
    {
        State = STATE.CARRYING;
        m_Animator.SetBool("carrying", true);
    }

    protected virtual void Drop(in DropSlotsManager _dropSlotter)
    {
        State = STATE.INTERACTING;
        var handsSlots = m_handsPick.ObjSlots;
        for (int i = 0; i < handsSlots.Count; i++)
        {
            if (handsSlots[i].childCount == 0)
                break;
            var piece = handsSlots[i].GetChild(0);
            var eSlot = _dropSlotter.GetEmptySlot();
            if (!eSlot)
                eSlot = m_backDiscarder;
            StartCoroutine(LerpObject(piece, eSlot, handsSlots[i].eulerAngles, true));
        }
        m_Animator.SetBool("carrying", false);
    }

    public virtual bool CanInteract(in InteractionPlate _plate)
    {
        return (_plate.InteractionType == InteractionPlate.INTERACTION_TYPE.DROPPER ||
            _plate.InteractionType == InteractionPlate.INTERACTION_TYPE.CHOPPER)
            && _plate.DropSlotter.GetEmptySlot() && m_handsPick.ObjSlots[0].childCount > 0;
    }
}
