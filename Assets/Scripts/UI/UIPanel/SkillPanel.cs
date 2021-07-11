using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPanel : BasePanel
{
    private GameObject skillItemPrefab;
    private PlayerStatus playerStatus;

    public Transform itemParent;
    new void Awake()
    {
        base.Awake();
        skillItemPrefab = Resources.Load<GameObject>("SkillItem");
        playerStatus = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>();
        Initial();
        UpdateSkillItemsMask();
    }

    public override void OnResume()
    {
        base.OnResume();
        UpdateSkillItemsMask();
    }

    public void OnClose() {
        UIManager.Instance.PopPanel();
    
    }

    public void AddSkill(int id)
    {
        GameObject go = Instantiate(skillItemPrefab,itemParent);
        go.GetComponent<SkillItem>().SetSkillItemById(id);
    }

    private void Initial()
    {
        int[] skillsId;
        switch (playerStatus.heroType)
        {
            case HeroType.Swordman:
                skillsId = SkillsInfo.Instance.swordmanIdList;
                break;
            case HeroType.Magician:
                skillsId = SkillsInfo.Instance.magicianIdList;
                break;
            default:
                skillsId = new int[0];
                break;
        }
        
        for(int i = 0; i < skillsId.Length; i++)
        {
            //print("id=" + skillsId[i]);
            AddSkill(skillsId[i]);
        }
    } 
    /// <summary>
    /// 刷新所有技能的遮罩
    /// </summary>
    private void UpdateSkillItemsMask()
    {
        SkillItem[] skillItems = itemParent.GetComponentsInChildren<SkillItem>();
        foreach(SkillItem item in skillItems)
        {
            item.UpdateMask(playerStatus.level);
        }
    }

}
