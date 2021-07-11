using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    public Transform followTarget;
    private Vector3 offset;
    void Start()
    {
        if(!followTarget)
           followTarget = GameObject.FindWithTag("Player").transform;
        offset = transform.position - followTarget.position;
    }

    void LateUpdate()
    {
        transform.position = offset + followTarget.position;
    }
}
