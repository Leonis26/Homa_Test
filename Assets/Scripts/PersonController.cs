using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using System;
using System.Linq;

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
            p.TryAnchor();
            p.OnPick();
            StartCoroutine(LerpObject(p, m_handsPick.GetEmptySlot(), p.InteractedDirection));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
    }

    IEnumerator LerpObject(IPickable _p, Transform _toT, Vector3 _toDir, bool apparentDirectly = false, Action _onOver = null)
    {
        var t = _p.Anchor;
        if (apparentDirectly)
            t.SetParent(_toT);
        var timer = 1f;
        var time = 0f;
        var toPosAdd = Vector3.up * _p.SavedBounds.extents.y;
        while (time < timer)
        {
            time += Time.deltaTime;
            var ratio = time / timer;
            t.position = Vector3.Lerp(t.position, _toT.position + toPosAdd, ratio);
            t.rotation = Quaternion.Lerp(t.rotation, _toT.rotation * Quaternion.LookRotation(_toDir), ratio);
            yield return new WaitForEndOfFrame();
        }
        t.position = _toT.position + toPosAdd;
        t.SetParent(_toT, true);
        t.localRotation = Quaternion.LookRotation(_toDir);// Quaternion.identity;
        _onOver?.Invoke();
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

    protected virtual void Drop(in DropSlotsManager _dropSlotter, Action _onOver = null)
    {
        State = STATE.INTERACTING;
        var handsSlots = m_handsPick.ObjSlots;
        Action onDrop = () => { };
        _onOver += () => m_Animator.SetBool("carrying", false);
        for (int i = 0; i < handsSlots.Count; i++)
        {
            if (handsSlots[i].childCount == 0)
            {
                if (i == 0)
                    _onOver.Invoke();
                break;
            }
            var piece = handsSlots[i].GetChild(0);
            var eSlot = _dropSlotter.GetEmptySlot();
            var p = piece.GetComponentInChildren<IPickable>();
            onDrop = () => p.OnDrop();
            var nextSlot = handsSlots.ElementAtOrDefault(i + 1);
            if (!nextSlot || nextSlot.childCount == 0)
                onDrop += _onOver;
            if (!eSlot)
                eSlot = m_backDiscarder;
            StartCoroutine(LerpObject(p, eSlot, piece.InverseTransformDirection(piece.right), true, onDrop));
        }
    }

    public virtual bool CanInteract(in InteractionPlate _plate)
    {
        return (_plate.InteractionType == InteractionPlate.INTERACTION_TYPE.DROPPER ||
            _plate.InteractionType == InteractionPlate.INTERACTION_TYPE.CHOPPER)
            && _plate.DropSlotter.GetEmptySlot() && m_handsPick.ObjSlots[0].childCount > 0;
    }
}
