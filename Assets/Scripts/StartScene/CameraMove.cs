using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform endPos;
    public float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(endPos != null)
        {
            transform.position = Vector3.Lerp(transform.position, endPos.position,moveSpeed*Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, endPos.rotation, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, endPos.position) <= 0.1f)
                Destroy(endPos.gameObject);
        }
    }
}
