using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentPanel : BasePanel
{
    public GameObject itemPrefab;

    //所有格子，需要按照下列顺序存放
    //Headgear,
    //Armor,
    //RightHand,
    //LeftHand,
    //Shoe,
    //Accessory
    
    public Transform[] equipSlotArray;
     new void Awake()
    {
        base.Awake();
        PlayerEquipment.Instance.equipmentPanel = this;
        PlayerEquipment.Instance.SyncEquip();
    }
    public void OnClosePanel()
    {
        UIManager.Instance.PopPanel();
    }

    public  void SetEquipment(int id)
    {
        ObjectInfo info = ObjectsInfo.Instance.GetObjectInfoById(id);
        int index = (int)info.dressType;
        Transform equipT = equipSlotArray[index].Find("EquipmentItem(Clone)");
        if(equipT == null)//如果该槽位还没有装备
        {
            equipT = Instantiate(itemPrefab, equipSlotArray[index]).GetComponent<Transform>();
        }

        //调用该物品上的EquipmentItem中的修改函数即可
        EquipmentItem item = equipT.GetComponent<EquipmentItem>();
        item.ChangeShowEquipment(id);

    } 
}
