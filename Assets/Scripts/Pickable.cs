using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour, IPickable
{
    [SerializeField] Vector3 m_InteractDir;
    public Vector3 InteractedDirection { get => m_InteractDir; protected set => m_InteractDir = value; }
    public Collider Col { get; protected set; }
    public Rigidbody Rb { get; protected set; }
    public Transform Anchor { get; set; }

    private void Start()
    {
        Col = GetComponent<Collider>();
        Rb = GetComponent<Rigidbody>();
    }

    public virtual void OnPick()
    {
        Col.enabled = false;
        Rb.isKinematic = true;
    }

    public virtual void OnDrop()
    {
        Col.enabled = true;
    }

    public virtual void Init(Vector3 _interDir)
    {
        InteractedDirection = _interDir;
        gameObject.tag = "Pickable";
    }
}
