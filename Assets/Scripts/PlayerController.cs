using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class PlayerController : PersonController
{
    [SerializeField] Cutter m_AxeCutter;

    protected override void Start()
    {
        base.Start();
    }

    Vector3 startPos;
    protected override void Update()
    {
        base.Update();
        if (State != STATE.INTERACTING)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPos = Input.mousePosition;
            }
            if (Input.GetMouseButton(0))
            {
                var nPos = Input.mousePosition;
                if (nPos != startPos)
                {
                    var move = nPos - startPos;
                    move = new Vector3(move.x, 0, move.y).normalized;
                    DoMove(move * CurrentSpeed);
                }
            }
            if (Input.GetMouseButtonUp(0))
                Stop();
        }
    }

    public override void DoInteraction(InteractionPlate _interactionPlate, Vector3 _rotationCorrection){
        switch (_interactionPlate.InteractionType)
        {
            case InteractionPlate.INTERACTION_TYPE.CHOPPER:
                Drop(_interactionPlate.DropSlotter, () => StartCoroutine(DoAttack(_rotationCorrection)));
                break;
            case InteractionPlate.INTERACTION_TYPE.TREE:
                StartCoroutine(DoAttack(_rotationCorrection));
                break;
            case InteractionPlate.INTERACTION_TYPE.DROPPER:
                Drop(_interactionPlate.DropSlotter, () => State = STATE.FREE_MOVE);
                break;
        }
    }

    IEnumerator WaitAndDoAction(Coroutine _toWait, IEnumerator _toDo)
    {
        yield return new WaitWhile(() => _toWait != null);
        StartCoroutine(_toDo);
    }

    public IEnumerator DoAttack(Vector3 _rotationCorrection)
    {
        Stop();
        m_AxeCutter.ShowHide(true);
        m_AxeCutter.Activate();
        LookAt(_rotationCorrection);
        m_Animator.SetBool("carrying", false);
        m_Animator.SetTrigger("chop");
        while (!m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Chop"))
        {
            LookAt(_rotationCorrection);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitUntil(() => !m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Chop"));
        State = STATE.FREE_MOVE;
    }

    protected override void OnCarry()
    {
        base.OnCarry();
        m_AxeCutter.ShowHide(false);
    }

    protected override void Drop(in DropSlotsManager _dropSlotter, Action _onOver = null)
    {
        base.Drop(_dropSlotter, _onOver);
    }

    public override bool CanInteract(in InteractionPlate _plate)
    {
        return (_plate.InteractionType == InteractionPlate.INTERACTION_TYPE.CHOPPER
            && (_plate.DropSlotter.ObjSlots[0].childCount > 0 || m_handsPick.ObjSlots[0].childCount > 0))
            || (_plate.InteractionType == InteractionPlate.INTERACTION_TYPE.DROPPER
            && _plate.DropSlotter.GetEmptySlot() && m_handsPick.ObjSlots[0].childCount > 0)
            || (_plate.InteractionType == InteractionPlate.INTERACTION_TYPE.TREE
            && m_handsPick.ObjSlots[0].childCount == 0)
            ;
    }

}
