using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    static private PlayerEquipment _instance;
    static public PlayerEquipment Instance
    {
        get
        {
            return _instance;
        }
    }

    Dictionary<DressType, int> partsEquipIdDict;//DressType-id

    public EquipmentPanel equipmentPanel;
    private PlayerStatus playerStatus;

    void Awake()
    {
        _instance = this;
        playerStatus = GetComponent<PlayerStatus>();
        partsEquipIdDict = new Dictionary<DressType, int>();       
    }

    public void ChangeEquip(int id)
    {
        int originId = 0;
        ObjectInfo info = ObjectsInfo.Instance.GetObjectInfoById(id);
        DressType equipType = info.dressType;
        partsEquipIdDict.TryGetValue(equipType, out originId);
        //修改装备数据
        //print("orginId:"+originId);
        if (originId != 0)//如果不为空,就表示要替换现有装备
        {
            //将原装备提供的属性去除
            playerStatus.AddAttackByEquipment(-ObjectsInfo.Instance.GetObjectInfoById(originId).attack);
            playerStatus.AddDefenseByEquipment(-ObjectsInfo.Instance.GetObjectInfoById(originId).def);
            playerStatus.AddSpeed(-ObjectsInfo.Instance.GetObjectInfoById(originId).speed);

            //将原装备放置到背包中
            InventoryManager.Instance.AddItem(originId, 1);
            //将原装备从字典中删除
            partsEquipIdDict.Remove(equipType);
        }

        //添加新装备提供的属性
        playerStatus.AddAttackByEquipment(info.attack);
        playerStatus.AddDefenseByEquipment(info.def);
        playerStatus.AddSpeedByEquipment(info.speed);

        partsEquipIdDict.Add(equipType, id);

//修改EquipmentPanel上的显示效果
        if (equipmentPanel)
        {
            //如果equipment面板不为空，那么就对面板上的信息进行更新
            equipmentPanel.SetEquipment(id);
        }
    }

    public void RemoveEquipment(DressType equipDress,int id)
    {
        if (partsEquipIdDict.Remove(equipDress))//从字典中移除
        {
            InventoryManager.Instance.AddItem(id, 1);
        }
    }
    /// <summary>
    /// 将拥有的所有装备同步到装备面板上，适合在Equipment面板初始化时调用
    /// </summary>
    public void SyncEquip()
    {
        foreach(KeyValuePair<DressType,int> pair in partsEquipIdDict)
        {
            equipmentPanel.SetEquipment(pair.Value);
        }
    }
}
