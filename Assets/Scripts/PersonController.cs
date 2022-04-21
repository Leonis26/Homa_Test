using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    public enum STATE
    {
        MOVABLE = 0,
        INTERACTING
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

}
