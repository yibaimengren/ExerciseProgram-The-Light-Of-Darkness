using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShopItem : MonoBehaviour
{
    private PlayerStatus playerStatus;
    private WeaponShopPanel shopPanel;
    private ObjectInfo objInfo;
    private Image icon;
    private Text detail;
    private AudioSource audioSource;
    void Awake()
    {
        playerStatus = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>();
        icon = transform.Find("icon").GetComponent<Image>();
        detail = transform.Find("detail").GetComponent<Text>();
        audioSource = this.GetComponent<AudioSource>();
    }

    public void SetItemById(int id,WeaponShopPanel weaponShopPanel)
    {
        shopPanel = weaponShopPanel;
        objInfo = ObjectsInfo.Instance.GetObjectInfoById(id);
        icon.sprite = Resources.Load<Sprite>("Icon/"+objInfo.icon_name);
        detail.text = objInfo.name + "\n";
        string str = "";
        if (objInfo.attack > 0)
            str = "+伤害 " + objInfo.attack;
        else if (objInfo.def > 0)
            str = "+防御 " + objInfo.def;
        else if (objInfo.speed > 0)
            str = "+速度 " + objInfo.speed;
        detail.text += str+"\n";
        detail.text += objInfo.price_buy;
    }

    public void BuyItem()
    {
       
        if(playerStatus.coin >= objInfo.price_buy)
        {
            InventoryManager.Instance.AddItem(objInfo.id, 1);//添加到背包中
            playerStatus.coin -= objInfo.price_buy;//扣钱
            shopPanel.UpdateCoinText(); //更新金币显示
            audioSource.Play();
        }
      
    }

}
