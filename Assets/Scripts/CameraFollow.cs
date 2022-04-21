using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    Vector3 spacing;

    void Start()
    {
        spacing = transform.position - target.position;
    }

    Vector3 vel;
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target.position + spacing, ref vel, .1f);
    }
}
