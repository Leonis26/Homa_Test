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
        var cutable = v.GetComponent<Cuttable>();
        var pieces = MeshCut.Cut(v, collision.contacts[0].point, cutable.transform.forward, cutable.InsideMat);
        Rigidbody rb;
        for (int i = 0; i < pieces.Length; i++)
        {
            //Destroy(pieces[i].GetComponent<KnifeTargetSlice>());
            if ((rb = pieces[i].GetComponent<Rigidbody>()) != null)
                rb.mass = 100f;
            else
            {
                rb = pieces[i].AddComponent<Rigidbody>();
                rb.mass = 100f;
            }
            MeshCollider msh = pieces[i].AddComponent<MeshCollider>();
            msh.convex = true;
            rb.AddExplosionForce(1, transform.position, 10f);
        }
    }

    public void Activate() => m_collider.enabled = true;
}
