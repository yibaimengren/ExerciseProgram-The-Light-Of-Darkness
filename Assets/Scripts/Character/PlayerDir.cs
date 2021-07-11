using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerDir : MonoBehaviour
{
    private GameObject greenClick;
    private const string hitTag = "Ground";
    private bool isMoving = false;
    private PlayerAttack playerAttack;
    [System.NonSerialized]
    public Vector3 targetPos;
    // Start is called before the first frame update
    void Start()
    {
        targetPos = this.transform.position;
        greenClick = Resources.Load<GameObject>("Efx_Click_Green");
        playerAttack = GetComponent<PlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {

        if (PlayerStatus.Instance.IsDeath)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())//如果鼠标左键点击的是UI
            {
                targetPos = transform.position;
            }
            else//如果鼠标左键点击的不是UI
            {

                isMoving = true;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit casthit;
                Physics.Raycast(ray, out casthit);

                if(casthit.collider && casthit.collider.tag == hitTag)
                {
                    Vector3 pos = new Vector3(casthit.point.x, casthit.point.y + 0.3f, casthit.point.z);
                    Instantiate(greenClick, pos, Quaternion.identity);
                    LookAtPos(casthit.point);
                    targetPos = casthit.point;
                    playerAttack.attackTarget = null;
                }
            }         
        }

        if (isMoving)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit casthit;
            if (Physics.Raycast(ray, out casthit) && casthit.collider.tag == hitTag)
            {
                LookAtPos(casthit.point);
                targetPos = casthit.point;
            }

        }

        if (Input.GetMouseButtonUp(0))
            isMoving = false;   
    }

    private void LookAtPos(Vector3 pos)
    {
        Vector3 lookDir = new Vector3(pos.x, transform.position.y, pos.z);
        transform.LookAt(lookDir);
    }
}
