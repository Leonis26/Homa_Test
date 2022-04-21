using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

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
        if (State == STATE.MOVABLE)
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

    public IEnumerator DoAttack(Vector3 rotationCorrection)
    {
        Stop();
        m_AxeCutter.Activate();
        LookAt(rotationCorrection);
        m_Animator.SetTrigger("chop");
        while (!m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Chop"))
        {
            LookAt(rotationCorrection);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitUntil(() => !m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Chop"));
        State = 0;
    }
}
