using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float scrollSpeed = 1;
    public float minDistance = 1;
    public float maxDistance = 10;
    public float rotateSpeed = 1;

    private Transform player;
    private Vector3 offset;
    private bool isRotating = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        offset = this.transform.position - player.position;
        transform.LookAt(player);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = player.position + offset;
        ScrollView();
        RotateView();
    }

    void ScrollView()
    {
        float scrollValue = Input.GetAxis("Mouse ScrollWheel");
        float distance = offset.magnitude - scrollSpeed * scrollValue;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
        offset = offset.normalized * (distance);
    }

    void RotateView()
    {
        if (Input.GetMouseButtonDown(1))
            isRotating = true;

        if (Input.GetMouseButtonUp(1))
            isRotating = false;

        if (isRotating)
        {
            //根据鼠标输入进行旋转
            float horizontal = Input.GetAxis("Mouse X");
            float vertical = -Input.GetAxis("Mouse Y");
            //先进行横向旋转
            this.transform.RotateAround(player.position, Vector3.up, horizontal * rotateSpeed * Time.deltaTime);
            //再进行纵向旋转
            Vector3 originalPos = transform.position;
            Quaternion originaRot = transform.rotation;
            this.transform.RotateAround(player.position, this.transform.right, vertical * rotateSpeed * Time.deltaTime);
            //对纵向旋转限制旋转角度
            float x = transform.eulerAngles.x;
            if(x<10 || x > 80)
            {
                transform.position = originalPos;
                transform.rotation = originaRot;
            }

            //更新摄像头和目标角色的位置差值
            offset = this.transform.position - player.position;
        }
    }
}
