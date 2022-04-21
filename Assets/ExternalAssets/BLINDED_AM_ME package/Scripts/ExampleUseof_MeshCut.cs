using UnityEngine;
using System.Collections;

public class ExampleUseof_MeshCut : MonoBehaviour
{

    public Material capMaterial;
    public Collider knifeBreakTargetCollider;
    public float explosionForce;

    // Use this for initialization
    void Start()
    {


    }

    void Update()
    {

        //RaycastHit hit;

        //if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
        //{

        //    if (hit.collider.gameObject.tag == "KnifeTarget")
        //    {
        //        //hit.collider.gameObject.tag = "Untagged";
        //        GameObject victim = hit.collider.gameObject;
        //        victim.tag = "Untagged";
        //        Destroy(hit.collider);
        //        //ITargetable trgtMat;
        //        Material mat;
        //        //if ((trgtMat = victim.GetComponent<ITargetable>()) != null)
        //        //{
        //        //    mat = trgtMat.CapMaterial;
        //        //}
        //        //else
        //        //{
        //        //    mat = capMaterial;
        //        //}
        //        GameObject[] pieces = BLINDED_AM_ME.MeshCut.Cut(victim, hit.point, transform.right, capMaterial);

        //        //ITargetable trgt;
        //        //if ((trgt = victim.GetComponent<ITargetable>()) != null)
        //        //{
        //        //    trgt.OnHit(); 
        //        //}
        //        Rigidbody rb;
        //        for (int i = 0; i < pieces.Length; i++)
        //        {
        //            //Destroy(pieces[i].GetComponent<KnifeTargetSlice>());
        //            if ((rb = pieces[i].GetComponent<Rigidbody>()) != null)
        //            {   
        //                rb.mass = 100f; 
        //            }
        //            else
        //            {
        //                rb = pieces[i].AddComponent<Rigidbody>();
        //                rb.mass = 100f;
        //            }
        //            MeshCollider msh = pieces[i].AddComponent<MeshCollider>();
        //            msh.convex = true;
        //            rb.AddExplosionForce(explosionForce, transform.position, 10f);
        //        }
        //        //if (!pieces[1].GetComponent<Rigidbody>())
        //        //    pieces[1].AddComponent<Rigidbody>();

        //        //Destroy(pieces[1], 1);
        //    }
        //    else if (hit.collider.gameObject.tag == "BreacableTarget")
        //    {
        //       knifeBreakTargetCollider.enabled = true;
        //    }
        //}
        //if (Input.GetMouseButtonDown(0))
        //{
        //}
    }

    //void OnDrawGizmosSelected()
    //{

    //    //Gizmos.color = Color.green;

    //    //Gizmos.DrawLine(transform.position, transform.position + transform.forward * 5.0f);
    //    //Gizmos.DrawLine(transform.position + transform.up * 0.5f, transform.position + transform.up * 0.5f + transform.forward * 5.0f);
    //    //Gizmos.DrawLine(transform.position + -transform.up * 0.5f, transform.position + -transform.up * 0.5f + transform.forward * 5.0f);

    //    //Gizmos.DrawLine(transform.position, transform.position + transform.up * 0.5f);
    //    //Gizmos.DrawLine(transform.position, transform.position + -transform.up * 0.5f);

    //}

}
