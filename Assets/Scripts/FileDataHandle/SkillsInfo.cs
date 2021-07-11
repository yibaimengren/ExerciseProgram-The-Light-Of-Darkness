using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsInfo : MonoBehaviour
{
    static public SkillsInfo Instance;

    private Dictionary<int, SkillInfo> skillInfoDict = new Dictionary<int, SkillInfo>();

    public int[] swordmanIdList;
    public int[] magicianIdList;
    void Awake()
    {
        Instance = this;
        InitSkillInfoDict();
    }
    /// <summary>
    /// 从文件中读取技能信息并存储到字典中
    /// </summary>
    private void InitSkillInfoDict()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("SkillsInfoList");
        string[] lines = textAsset.text.Split('\n');//每一行是一个技能信息
        for(int i = 1; i < lines.Length; i++)
        {
            string[] items = lines[i].Split(','); 
            SkillInfo skill = new SkillInfo();
            skill.id =int.Parse(items[0]);
            skill.name = items[1];
            skill.icon_name = items[2];
            skill.des = items[3];
            skill.applyType = (ApplyType)System.Enum.Parse(typeof(ApplyType), items[4]);
            skill.applyProperty = (ApplyProperty)System.Enum.Parse(typeof(ApplyProperty), items[5]);
            skill.applyValue = int.Parse(items[6]);
            skill.applyTime = int.Parse(items[7]);
            skill.mp = int.Parse(items[8]);
            skill.coldTime = int.Parse(items[9]);
            skill.applicableRole = (ApplicableRole)System.Enum.Parse(typeof(ApplicableRole), items[10]);
            skill.level = int.Parse(items[11]);
            skill.releaseType = (ReleaseType)System.Enum.Parse(typeof(ReleaseType), items[12]);
            skill.distance = float.Parse(items[13]);
            skill.efx_name = items[14];
            skill.aniname = items[15];
            skill.anitime = float.Parse(items[16]);

            skillInfoDict.Add(skill.id, skill);

        }

    }

    public SkillInfo GetSkillInfoById(int id)
    {
        SkillInfo skill;
        skillInfoDict.TryGetValue(id, out skill);
        return skill;

    }
}

//适用角色类型
public enum ApplicableRole
{
    Swordman,
    Magician
}
//作用类型
public enum ApplyType
{
    Passive,//增益
    Buff,
    SingleTarget,
    MultiTarget
}
//作用属性
public enum ApplyProperty
{
    Attack,
    Def,
    Speed,
    AttackSpeed,
    HP,
    MP
}
//释放类型
public enum ReleaseType
{
    Self,
    Enemy,
    Position
}

public class SkillInfo
{
    public int id;
    public string name;
    public string icon_name;
    public string des;
    public ApplyType applyType;//增益/增值/单个目标/多个目标
    public ApplyProperty applyProperty;//hp/mp/速度/攻击/防御···
    public int applyValue;
    public int applyTime;
    public int mp;
    public int coldTime;
    public ApplicableRole applicableRole;//适用职业
    public int level;
    public ReleaseType releaseType;//释放对象
    public float distance = 0;
    public string efx_name;
    public string aniname;
    public float anitime = 0;
}
