using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetail : MonoBehaviour
{
    private Text text;
    // Start is called before the first frame update
    void Awake()
    {
        text = transform.Find("Text").GetComponent<Text>();
    }

    public void ShowDetailInfo(int id)
    {
        ObjectInfo info =  ObjectsInfo.Instance.GetObjectInfoById(id);
        if(info.type == ObjectType.Drug)
        {
            text.text = "名称：" + info.name + "\n"
                   + "类型：" + info.type.ToString() + "\n"
                   + "血量：" + info.hp + "\n"
                   + "魔法：" + info.mp + "\n"
                   + "出售价：" + info.price_sell + "\n"
                   + "购买价：" + info.price_buy + "\n";
        }else if(info.type == ObjectType.Equip)
        {
            text.text = "名称：" + info.name + "\n"
                  + "类型：" + info.type.ToString() + "\n"
                  + "攻击力：" + info.attack + "\n"
                  + "防御力：" + info.def + "\n"
                  +"速度：" +info.speed+"\n"
                  +"职业："+info.applicationType+"\n"
                  + "出售价：" + info.price_sell + "\n"
                  + "购买价：" + info.price_buy + "\n";
        }
       
    }
}
