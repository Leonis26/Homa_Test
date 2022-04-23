using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cuttable : MonoBehaviour, ICuttable
{
    [SerializeField] Material m_InsideMat;
    public Material InsideMat { get => m_InsideMat; set => m_InsideMat = value; }

    [SerializeField] int m_RemainingSplits = 2;
    public int RemainingSplits { get => m_RemainingSplits; }// set => m_RemainingSplits = value; }

    public Vector3 InteractedDirection { get; protected set; }

    public Collider Col { get; protected set; }
    public Rigidbody Rb { get; protected set; }

    public Bounds SavedBounds { get; protected set; }

    private void Start()
    {
        Col = GetComponent<Collider>();
    }

    public virtual void Init(int _splits, in Material _insideMat)
    {
        m_RemainingSplits = _splits;
        InsideMat = _insideMat;
        Col = GetComponent<Collider>();
        //InteractedDirection = _interDir;
    }

    public virtual void OnCut()
    {
        gameObject.tag = "Pickable";
    }
}
