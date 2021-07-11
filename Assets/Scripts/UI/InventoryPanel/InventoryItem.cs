using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    private Image image;
    private Vector3 offsetPos;
    [System.NonSerialized]
    public Transform originParent;
    private bool isMoving = false;
    private float moveSpeed = 0.5f;
    [System.NonSerialized]
    public ObjectInfo objectInfo;
    private ItemDetail itemDetail;
    private bool isEnter = false;
    void Awake()
    {
        image = transform.GetComponent<Image>();
        itemDetail = transform.parent.parent.parent.Find("ItemDetail").GetComponent<ItemDetail>();
    }
    void Update()
    {
        if(isMoving == true)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, moveSpeed);
            if(transform.localPosition.magnitude < 0.1f)
            {
                transform.localPosition = Vector3.zero;
                isMoving = false;
            }

        }

        if (isEnter)
        {
            itemDetail.transform.position = Input.mousePosition;
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        offsetPos = transform.position - Input.mousePosition;
        originParent = transform.parent;
        transform.SetParent(transform.root);
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = offsetPos + Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject go = eventData.pointerCurrentRaycast.gameObject;
      
        if (go!= null && go.CompareTag("InventoryGrid"))//如果移到了新格子上
        {
            originParent.GetComponent<InventoryGrid>().Clear();
            originParent = go.transform;

        }
        else if(go != null && go.CompareTag("InventoryItem"))//如果移到了其它物品上
        {
          
            //清除数量文本
            go.transform.parent.GetComponent<InventoryGrid>().Clear();
            originParent.GetComponent<InventoryGrid>().Clear();

            Transform newParent = go.transform.parent;
            go.GetComponent<InventoryItem>().SetParent(originParent);
            originParent = newParent;

        }else if(objectInfo.type == ObjectType.Drug && go != null && go.CompareTag("SkillShotcutGrid") )
        {//如果当前物体是药品且拖住到了快捷栏格子上。
            MainShotcut mainShotcut = go.GetComponent<MainShotcut>();
            mainShotcut.SetObject(objectInfo,this,image);
        }

        image.raycastTarget = true;
        SetParent(originParent);
    }


    public void SetItemById(int id)
    {
        objectInfo = ObjectsInfo.Instance.GetObjectInfoById(id);
        //  Sprite sprite = Resources.Load("Icon/" + objectInfo.icon_name, typeof(Sprite)) as Sprite; //Resources.Load<Sprite>("/Icon/" + objectInfo.icon_name);
        Sprite sprite = Resources.Load<Sprite>("Icon/" + objectInfo.icon_name);
        image.sprite = sprite;

    }

    public void SetParent(Transform parent)
    {
        isMoving = true;
        transform.SetParent(parent);
        transform.SetSiblingIndex(0);

        InventoryGrid inventoryGrid = parent.GetComponent<InventoryGrid>();
        if(inventoryGrid)
            inventoryGrid.SetCount(InventoryManager.Instance.GetNumById(objectInfo.id));
        
        InventoryManager.Instance.UpdateSiblingIndex(objectInfo.id, transform.parent.GetSiblingIndex());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        itemDetail.gameObject.SetActive(true);
        itemDetail.transform.position = eventData.position;
        itemDetail.ShowDetailInfo(objectInfo.id);
        isEnter = true;
       // Debug.Log("Enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemDetail.gameObject.SetActive(false);
        isEnter = false;
        //Debug.Log("Exit");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //点击右键则穿上装备
        if(eventData.button == PointerEventData.InputButton.Right)
        {      
            if(objectInfo.type == ObjectType.Equip &&
                (objectInfo.applicationType.ToString() == PlayerStatus.Instance.heroType.ToString() || 
                objectInfo.applicationType == ApplicationType.Common))//如果物品符合条件才可穿戴
            {
                InventoryManager.Instance.DeleteItem(objectInfo.id, 1);//从背包中删除
                PlayerEquipment.Instance.ChangeEquip(objectInfo.id);//添加到装备栏
              //  print("Equip!");
            }else if(objectInfo.type == ObjectType.Drug)//如果是药物则使用
            {
                PlayerStatus.Instance.AddHP(objectInfo.hp);
                PlayerStatus.Instance.AddMP(objectInfo.mp);
                InventoryManager.Instance.DeleteItem(objectInfo.id, 1);
            }
            
        }
    }
}
