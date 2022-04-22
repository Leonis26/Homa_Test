using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttablePickable : Pickable, ICuttable
{
    [SerializeField] Material m_InsideMat;
    public Material InsideMat { get => m_InsideMat; set => m_InsideMat = value; }

    [SerializeField] int m_RemainingSplits = 2;
    public int RemainingSplits { get => m_RemainingSplits; }// set => m_RemainingSplits = value; }

    public void Init(int _splits, in Material _insideMat, in MeshCollider _col, in Rigidbody _rb)
    {
        Rb = _rb;
        Rb.ResetCenterOfMass();
        Rb.ResetInertiaTensor();
        //transform.SetParent(Anchor = Instantiate(new GameObject("woodAnchor"), Rb.worldCenterOfMass, transform.rotation).transform);
        //Anchor = Instantiate(new GameObject("woodAnchor"), Rb.worldCenterOfMass, transform.rotation).transform);
        //Init(InteractedDirection);
        m_RemainingSplits = _splits;
        InsideMat = _insideMat;
        Col = _col;
        if (_splits == 1)
            InteractedDirection = Vector3.up;
        else
            InteractedDirection = Vector3.right;
        gameObject.tag = "Pickable";
    }

    public void OnCut()
    {
        gameObject.tag = "Pickable";
    }

    public override void OnPick()
    {
        base.OnPick();
    }

    public override void OnDrop()
    {
        gameObject.tag = "Cuttable";
        base.OnDrop();
    }
}
