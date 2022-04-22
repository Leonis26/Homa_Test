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
        var pieces = MeshCut.Cut(v, collision.contacts[0].point, cuttable.transform.forward, cuttable.InsideMat);
        var remainingSplits = cuttable.RemainingSplits - 1;
        Rigidbody rb;
        for (int i = 0; i < pieces.Length; i++)
        {
            var piece = pieces[i];
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
            cuttable.Init(remainingSplits, cuttable.InsideMat, msh, rb);
            rb.AddExplosionForce(1, transform.position, 10f);

            piece.layer = LayerMask.NameToLayer("InteractibleObject");
        }
    }

    public void Activate() => m_collider.enabled = true;
    public void ShowHide(bool _onOff) => gameObject.SetActive(_onOff);
}
