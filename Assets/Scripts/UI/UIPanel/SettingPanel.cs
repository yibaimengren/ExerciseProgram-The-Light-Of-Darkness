using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanel : BasePanel
{
    public void ContinueGame()
    {
        UIManager.Instance.PopPanel();
    }

    public void Exit()
    {
        PlayerPrefs.SetInt("PlayerLevel", PlayerStatus.Instance.level);//存储等级
        PlayerPrefs.SetString("PlayerName", PlayerStatus.Instance.playerName);//存储名字
        PlayerPrefs.SetInt("SelectCharacterIndex",GameManager.Instance.characterIndex);//存储职业
        //存储角色位置坐标
        PlayerPrefs.SetFloat("posX", PlayerStatus.Instance.transform.position.x);
        PlayerPrefs.SetFloat("posY", PlayerStatus.Instance.transform.position.y);
        PlayerPrefs.SetFloat("posZ", PlayerStatus.Instance.transform.position.z);
        
        Application.Quit();
    }
}
