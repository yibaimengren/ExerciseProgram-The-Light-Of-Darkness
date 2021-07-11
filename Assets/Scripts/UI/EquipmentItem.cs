using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipmentItem : MonoBehaviour,IPointerClickHandler
{
    public int equipId = 2001;
    public DressType equipType;
    private Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }
    /// <summary>
    /// 修改显示的装备
    /// </summary>
    public void ChangeShowEquipment(int id)
    {
        equipId = id;
        ObjectInfo info = ObjectsInfo.Instance.GetObjectInfoById(id);
        image.sprite = Resources.Load<Sprite>("Icon/" + info.icon_name);
        equipType = info.dressType;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            PlayerEquipment.Instance.RemoveEquipment(equipType, equipId);
            Destroy(this.gameObject);
        }
    }
}
