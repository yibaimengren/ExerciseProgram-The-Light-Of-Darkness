using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusPanel : BasePanel
{
    private PlayerStatus playerStatus;

    public Text detailText;//显示攻击力、速度等具体数值
    public Text surplusPoint;

    public GameObject attackAdd;
    public GameObject defenseAdd;
    public GameObject speedAdd;

    new void Awake()
    {
        base.Awake();
        playerStatus = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>();
    }

    void UpdateInfo()//刷新信息
    {
        detailText.text = (playerStatus.attack + playerStatus.attack_plus) + "\n\n"
                        + (playerStatus.defense + playerStatus.defense_plus) + "\n\n"
                        + (playerStatus.speed + playerStatus.speed_plus);

        surplusPoint.text = "剩余点数：" + playerStatus.point_remain;

        if(playerStatus.point_remain <= 0)
        {
            attackAdd.SetActive(false);
            defenseAdd.SetActive(false);
            speedAdd.SetActive(false);
        }
        else
        {
            attackAdd.SetActive(true);
            defenseAdd.SetActive(true);
            speedAdd.SetActive(true);
        }
    }

    public override void  OnEnter()
    {
        base.OnEnter();
        UpdateInfo();
    }

    public override void OnResume()
    {
        base.OnResume();
        UpdateInfo();
    }

    public void OnClose()
    {
        UIManager.Instance.PopPanel();
    }

    public void AddAttackPoint()
    {
        playerStatus.AddAttack();
        UpdateInfo();
    }

    public void AddDefensePoint()
    {

        playerStatus.AddDefense();
        UpdateInfo();
    }

    public void AddSpeedPoint()
    {
        playerStatus.AddSpeed();
        UpdateInfo();
    }
}
