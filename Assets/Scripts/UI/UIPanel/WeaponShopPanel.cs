using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShopPanel : BasePanel
{
    [System.NonSerialized]
    public Weapon_NPC weapon_Npc;
    public Text coinText;
    public Transform listParentT;
    private PlayerStatus playerStatus;
    private GameObject itemPrefab;

    public override void OnEnter()
    {
        base.OnEnter();
        coinText.text = "剩余金币：" + playerStatus.coin;       
    }

    public override void OnResume()
    {
        base.OnResume();
        coinText.text = "剩余金币：" + playerStatus.coin;
    }

    new void Awake()
    {
        base.Awake();
        playerStatus = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>();
        itemPrefab = Resources.Load<GameObject>("WeaponShopItem");
        InitiallItem();
    }

    public void OnClose()
    {
        UIManager.Instance.PopPanel();
        weapon_Npc.isOpen = false;
    }

    public void InitiallItem()
    {
        for(int i = 2001;i<= 2022; i++)
        {
            WeaponShopItem item = Instantiate(itemPrefab, listParentT).GetComponent<WeaponShopItem>();
            item.SetItemById(i,this);
        }
    }

    public void UpdateCoinText()
    {
        coinText.text = "剩余金币：" + playerStatus.coin;
    }

}
