using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour, IPickable
{
    [SerializeField] Vector3 m_InteractDir;
    public Vector3 InteractedDirection { get => m_InteractDir; protected set => m_InteractDir = value; }
    public Collider Col { get; protected set; }
    public Rigidbody Rb { get; protected set; }
    public Transform Anchor { get; set; } = null;
    public Bounds SavedBounds { get; protected set; }

    [SerializeField] Transform m_presetAnchor = null;

    private void Start()
    {
        Col = GetComponent<Collider>();
        Rb = GetComponent<Rigidbody>();

        Anchor = m_presetAnchor;
        /*if (Anchor)
        {
            Anchor = null;
            TryAnchor();
        }*/
    }

    public virtual void OnPick()
    {
        Col.enabled = false;
        Rb.isKinematic = true;
    }

    public virtual void OnDrop()
    {
        Col.enabled = true;
        Rb.isKinematic = false;
    }

    public void Init(Vector3 _interDir)
    {
        if (Anchor && Anchor != transform)
            Destroy(Anchor.gameObject);
        Anchor = null;
        InteractedDirection = _interDir;
        gameObject.tag = "Pickable";
        SavedBounds = Col.bounds;
    }

    public void TryAnchor()
    {
        if (!Anchor)
        {
            var a = new GameObject("woodAnchor").transform;
            a.parent = transform.parent;
            a.SetPositionAndRotation(Rb.worldCenterOfMass, transform.rotation);
            transform.SetParent(a, true);
            Anchor = a;
        }
    }
}
