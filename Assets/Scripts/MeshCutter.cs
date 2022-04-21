using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BLINDED_AM_ME;

public class MeshCutter : MonoBehaviour
{
    [SerializeField] Material m_WoodInside;

    void Start()
    {
        var col = Physics.OverlapSphere(transform.position, .1f);
        if (col != null && col.Length > 0)
        {
            var v = col[0].gameObject;
            Destroy(col[0]);
            var pieces = MeshCut.Cut(v, transform.position, transform.right, m_WoodInside);
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
    }

    void Update()
    {
        
    }
}
