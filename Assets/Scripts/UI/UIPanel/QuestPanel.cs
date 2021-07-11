using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class QuestPanel : BasePanel
{
    [NonSerialized]
    public Bar_NPC bar_Npc;
    public int killTarget = 10;

    public Text text;
    public GameObject acceptButton;
    public GameObject cancelButton;
    public Image okButton;

    private PlayerStatus playerStatus;
    private bool isAccept = false;

    void Start()
    {
        playerStatus = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>();
    }
    public override void OnEnter()
    {
        base.OnEnter();
        if (QuestManager.Instance.babyWolfKilled < 10) //如果杀的狼不够就不能领取奖励
        {
            okButton.GetComponent<Button>().interactable = false;
            Color color = okButton.color;
            okButton.color = new Color(color.r, color.b, color.g, 0.5f);
        }
        else//如果杀的敌人够了就可以领取奖励
        {
            okButton.GetComponent<Button>().interactable = true;
            Color color = okButton.color;
            okButton.color = new Color(color.r, color.b, color.g, 1f);
        }

        if (isAccept)
        {
            text.text = "任务：\n你已经杀死了" + QuestManager.Instance.babyWolfKilled + "\\只野狼\n\n奖励:\n1000金币";
        }
           
    }
    public void OnClickCancel()
    {
        UIManager.Instance.PopPanel();
        bar_Npc.isOpen = false;
    }

    public void OnClickAccept()
    {
        text.text = "任务：\n你已经杀死了" + QuestManager.Instance.babyWolfKilled + "\\10只野狼\n\n奖励:\n1000金币";
        acceptButton.SetActive(false);
        cancelButton.SetActive(false);
        okButton.gameObject.SetActive(true);
        isAccept = true;
    }

    /// <summary>
    /// 点击OK按钮，领取奖励
    /// </summary>
    public void OnClickOK()
    {
        playerStatus.coin += 1000;
        QuestManager.Instance.babyWolfKilled -= killTarget;
        text.text = "任务：\n杀死10只野狼\n\n奖励:\n1000金币";
        acceptButton.SetActive(true);
        cancelButton.SetActive(true);
        okButton.gameObject.SetActive(false);
        OnClickCancel();
    }
}
