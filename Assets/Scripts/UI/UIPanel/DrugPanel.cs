using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrugPanel : BasePanel
{
    public GameObject okWindow;
    public GameObject[] buyButtons;
    public Text coinText;
    private GameObject okButton;
    private int buyTargetId = 0;//要购买的物品的ID
    private int priceTotal = 0;
    private InputField numInput;
    private PlayerStatus playerStatus;
    [System.NonSerialized]
    public Potion_Npc potion_Npc;

    new void Awake()
    {
        base.Awake();
        playerStatus = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>();
        numInput = okWindow.transform.Find("InputField").GetComponent<InputField>();
        okButton = okWindow.transform.Find("OKButton").gameObject;
    }

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

    public void SelectItem(int id)
    {
        buyTargetId = id;
        okWindow.SetActive(true);
        BuyButtonSetActive(false);
        numInput.text = "";
    }

    public void BuyItem()
    {
        InventoryManager.Instance.AddItem(buyTargetId, int.Parse(numInput.text));
        okWindow.SetActive(false);
        BuyButtonSetActive(true);

        //扣钱
        playerStatus.coin -= priceTotal;
        //Debug.Log("我有多少钱：" + playerStatus.coin);

        coinText.text = "剩余金币：" + playerStatus.coin;
    }

    public void OnClose()
    {
        UIManager.Instance.PopPanel();
        potion_Npc.isOpen = false;
    }
    /// <summary>
    /// 当玩家修改购买数值时，检测价钱总额是否超过了拥有的钱
    /// </summary>
    public void OnNumChange(string str)
    {
        //根据输入数量计算总额
        if (str == "")
            str = "0";
        int num = int.Parse(str);
        priceTotal = num * ObjectsInfo.Instance.GetObjectInfoById(buyTargetId).price_buy;
        //如果玩家钱不够，就不能点OK
        if (priceTotal > playerStatus.coin)
            okButton.SetActive(false);
        else
            okButton.SetActive(true);
    }

    public void BuyButtonSetActive(bool active)
    {
        foreach(GameObject go in buyButtons)
        {
            go.SetActive(active);
        }
    }

    public void OnCloseOkWindow()
    {
        BuyButtonSetActive(true);
        okWindow.SetActive(false);
        coinText.text = "剩余金币：" + playerStatus.coin;
    }
}
