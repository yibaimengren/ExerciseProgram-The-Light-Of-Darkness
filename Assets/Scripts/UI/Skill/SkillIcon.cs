using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillIcon : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public SkillItem skillItem;
    private GameObject skillDragItem;
    private Image image;
    private Vector3 posOffset;
    private Transform dragIcon;
    private bool isDrag = false;
    private bool moving = false;
    void Awake()
    {
        skillDragItem = Resources.Load<GameObject>("SkillDragItem");
        image = GetComponent<Image>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isDrag)
        {
            GameObject go = Instantiate(skillDragItem, transform.root);
            dragIcon = go.transform;
            go.GetComponent<Image>().sprite = image.sprite;
            dragIcon.position = Input.mousePosition;
            moving = true;
            image.color = Color.gray;
        }
       
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(moving)
            dragIcon.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!moving)
            return;

        Destroy(dragIcon.gameObject);
        RaycastResult result = eventData.pointerCurrentRaycast;
        if (result.gameObject && result.gameObject.CompareTag("SkillShotcutGrid"))
        {
            MainShotcut mainShotcut = result.gameObject.GetComponent<MainShotcut>();
            mainShotcut.SetSkill(this,skillItem.skill,image);           
            isDrag = true;
            moving = false;
            return;
        }
        ResetState();

    }

    public void ResetState()
    {
        image.color = Color.white;
        isDrag = false;
    }
}
