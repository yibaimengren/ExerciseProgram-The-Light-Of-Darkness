using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : BasePanel
{
    public Text nameText;
    public Image hpImage;
    public Image mpImage;
    public CanvasGroup buttonLayoutCanvasGroup;
    public MainShotcut[] mainShotcuts;

    [System.NonSerialized]
    public PlayerStatus playerStatus;

    void Update()
    {
        UpdatePlayerState();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        playerStatus = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>();
        playerStatus.mainPanel = this;
    }

    public override void OnPause()
    {
        buttonLayoutCanvasGroup.blocksRaycasts = false;
    }

    public override void OnResume()
    {
        buttonLayoutCanvasGroup.blocksRaycasts = true;
    }

    public void OnClickStatusButton()
    {
        UIManager.Instance.PushPanel(UIPanelType.StatusPanel);
    }

    public void OnClickBagButton()
    {
        UIManager.Instance.PushPanel(UIPanelType.InventoryPanel);
    }

    public void OnClickEquipButton()
    {
        UIManager.Instance.PushPanel(UIPanelType.EquipmentPanel);
    }

    public void OnClickSkillButton()
    {
        UIManager.Instance.PushPanel(UIPanelType.SkillPanel);
    }

    public void OnClickSettingButton()
    {
        UIManager.Instance.PushPanel(UIPanelType.SettingPanel);
    }

    public void UpdatePlayerState()
    {
        nameText.text = "Lv."+playerStatus.level+"  " + playerStatus.playerName;
        hpImage.fillAmount = playerStatus.HpPercent;
        mpImage.fillAmount = playerStatus.MpPercent;
    }
    /// <summary>
    /// 启用或者禁用快捷栏功能
    /// </summary>
    /// <param name="isActive"></param>
    public void SetAllShotcutActive(bool isActive)
    {
        foreach (MainShotcut shotcut in mainShotcuts)
            shotcut.SetShotcutActive(isActive);
    }
}

