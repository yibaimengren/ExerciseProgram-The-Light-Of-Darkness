using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillItem : MonoBehaviour
{
    [System.NonSerialized]
    public SkillInfo skill;
    public Image icon;
    public Text skillName;
    public Text applyType;
    public Text description;
    public Text MP;
    public GameObject mask;

    public void SetSkillItemById(int id)
    {
        skill = SkillsInfo.Instance.GetSkillInfoById(id);
        icon.sprite = Resources.Load<Sprite>("Icon/" + skill.icon_name);
        skillName.text = skill.name;
         
        switch (skill.applyType)
        {
            case ApplyType.Passive:
                applyType.text = "增益";
                break;
            case ApplyType.Buff:
                 applyType.text = "增强";
                break;
            case ApplyType.SingleTarget:
                applyType.text = "单个目标";
                break;
            case ApplyType.MultiTarget:
                applyType.text = "多个目标";
                break;

        }
        description.text = skill.des;
        MP.text = skill.mp.ToString();
    }

    public void UpdateMask(int level)
    {
       // print(skill.level);
        if (skill.level <= level)
            mask.SetActive(false);
        else
            mask.SetActive(true);
    }
}
