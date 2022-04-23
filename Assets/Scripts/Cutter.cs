using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BLINDED_AM_ME;

public class Cutter : MonoBehaviour
{
    [SerializeField] Collider m_collider;
    public Collider AxeCollider => m_collider;

    private void OnCollisionEnter(Collision collision)
    {
        m_collider.enabled = false;
        var v = collision.gameObject;
        Destroy(collision.collider);
        var cuttable = v.GetComponent<CuttablePickable>();
        var pieces = MeshCut.Cut(v, cuttable.Anchor.position, cuttable.InteractedDirection, cuttable.InsideMat);
        var aPos = collision.GetContact(0).point;// cuttable.Anchor.position;
        var remainingSplits = cuttable.RemainingSplits - 1;
        Rigidbody rb;
        for (int i = 0; i < pieces.Length; i++)
        {
            var piece = pieces[i];
            var insideMat = cuttable.InsideMat;
            if ((rb = piece.GetComponent<Rigidbody>()) != null)
                rb.mass = 100f;
            else
            {
                rb = piece.AddComponent<Rigidbody>();
                rb.mass = 100f;
                cuttable = piece.AddComponent<CuttablePickable>();
            }
            MeshCollider msh = piece.AddComponent<MeshCollider>();
            msh.convex = true;
            cuttable.Init(remainingSplits, insideMat, msh, rb);
            rb.AddExplosionForce(10, aPos, 10, 2, ForceMode.Acceleration);
            piece.layer = LayerMask.NameToLayer("InteractibleObject");
        }
    }

    public void Activate() => m_collider.enabled = true;
    public void ShowHide(bool _onOff) => gameObject.SetActive(_onOff);
}
