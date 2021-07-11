using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HeroType
{
    Magician,
    Swordman
}
public class PlayerStatus : MonoBehaviour
{
    static private PlayerStatus _instance;
    static public PlayerStatus Instance
    {
        get
        {
            return _instance;
        }
    }


    private Animator animator;

    public string playerName = "默认昵称";
    public HeroType heroType = HeroType.Magician;
    public int level = 1;
    private int maxHp = 200;
    private int hp_current = 100;
    private int maxMp = 200;
    private int mp_current = 100;
    private int expValue = 0;
    public int coin = 200;
    public float HpPercent
    {
        get
        {
            return (float)hp_current / maxMp;
        }
    }

    public float MpPercent
    {
        get
        {
            return (float)mp_current / maxMp;
        }
    }

    public int Exp
    {
        get { return expValue; }
    }

    public bool IsDeath
    {
        get
        {
            if (hp_current <= 0)
                return true;
            else
                return false;
        }
    }

    private bool allowAttack = true;

    public bool AllowAttack
    {
        get { return allowAttack; }
        set
        {
            allowAttack = value;
            if (mainPanel)
            {
                //如果为false，就要把快捷栏中的技能图标给禁用掉
                //如果为true，就要把快捷栏中的技能图标启用           
                mainPanel.SetAllShotcutActive(value);          
            }

        }
    }

    public int attack = 20;
    public int attack_plus = 0;
    public float attackRate = 1;
    public float attackBuffTime = 0;//当前攻击力buff持续的时间
    public int defense = 20;
    public int defense_plus = 0;
    public float defenseRate = 1;
    public float defenseBuffTime = 0;//当前防御力buff持续时间
    public int speed = 20;
    public int speed_plus = 0;
    public float speedRate = 1;
    public float speedBuffTime = 0;// 当前速度buff持续时间

    public int point_remain = 0;
    [System.NonSerialized]
    public MainPanel mainPanel;
    public bool isAim = false; //是否处于瞄准状态
     //特效
    public GameObject levelUpPrefab;
    private GameObject damageEffetPrefab;
    private GameObject healEffectPrefab;
    private GameObject blueEffectPrefab;

    void Awake()
    {
        _instance = this;
        playerName = PlayerPrefs.GetString("PlayerName");
        animator = GetComponent<Animator>();
        levelUpPrefab = Resources.Load<GameObject>("Efx_LvUp");
        damageEffetPrefab = Resources.Load<GameObject>("Effect_Slash");
        healEffectPrefab = Resources.Load<GameObject>("Effect_Heal");
        blueEffectPrefab = Resources.Load<GameObject>("Effect_BlueHeal");
    }

    void Update()
    {
        if(attackBuffTime > 0)
        {
            attackBuffTime -= Time.deltaTime;
            if (attackBuffTime <= 0)
                attackRate = 1;
        }

        if (speedBuffTime > 0)
        {
            speedBuffTime -= Time.deltaTime;
            if (speedRate <= 0)
                speedRate = 1;
        }

        if (defenseBuffTime > 0)
        {
            defenseBuffTime -= Time.deltaTime;
            if (defenseRate <= 0)
                defenseRate = 1;
        }

    }
    /// <summary>
    /// 设置攻击力buff
    /// </summary>
    /// <param name="buffvalue"></param>
    /// <param name="continueTime"></param>
    public void SetAttackBuff(float buffRate,float continueTime)
    {
        attackRate = buffRate;
        attackBuffTime = continueTime;
    }
    /// <summary>
    /// 设置速度值
    /// </summary>
    /// <param name="buffRate"></param>
    /// <param name="continueTime"></param>
    public void SetSpeedBuff(float buffRate,float continueTime)
    {
        speedRate = buffRate;
        speedBuffTime = continueTime;
    }

    public void SetDefenseBuff(float buffRate, float continueTime)
    {
        defenseRate = buffRate;
        defenseBuffTime = continueTime;
    }

    public int GetAttackValue()
    {
        return (int)((attack + attack_plus)*attackRate);
    }

    public int GetDefenceValue()
    {
        return (int)((defense + defense_plus)*defenseRate);
    }

    public int GetSpeedValue()
    {
        return (int)((speed + speed_plus)*speedRate);
    }

    public void AddAttack(int count=1)
    {
        if (count > point_remain)
            count = point_remain;    
        
        attack_plus += count;
        point_remain -= count;

    }

    public void AddDefense(int count = 1)
    {
        if (count > point_remain)
            count = point_remain;

        defense_plus += count;
        point_remain -= count;

    }

    public void AddSpeed(int count = 1)
    {
        if (count > point_remain)
            count = point_remain;

        speed_plus += count;
        point_remain -= count;

    }

    public void AddAttackByEquipment(int strength)
    {
        attack_plus += strength;
        if (attack_plus < 0)
            attack_plus = 0;
    }

    public void AddDefenseByEquipment(int strength)
    {
        defense_plus += strength;
        if (defense_plus < 0)
            defense_plus = 0;
    }

    public void AddSpeedByEquipment(int strength)
    {
        speed_plus += strength;
        if (speed_plus < 0)
            speed_plus = 0;
    }
    /// <summary>
    /// 加血值
    /// </summary>
    /// <param name="value"></param>
    public void AddHP(int value)
    {
        if (value <= 0)
            return;

        //播放效果
        Instantiate(healEffectPrefab, transform);
        PlayerSoundsManager.Instance.PlaySound(SoundType.Heal);
        //增加血量
        hp_current += value;
        if (hp_current > maxHp)
            hp_current = maxHp;
    }

    public bool UseMp(int mp)
    {
        if (mp <= mp_current)
        {
            mp_current -= mp;
            return true;
        }
        else return false;

    }
    /// <summary>
    /// 加魔法值
    /// </summary>
    /// <param name="value"></param>
    public void AddMP(int value)
    {
        if (value <= 0)
            return;
        //播放效果
        Instantiate(blueEffectPrefab, transform);
        PlayerSoundsManager.Instance.PlaySound(SoundType.Heal);
        //增加魔法值
        mp_current += value;
        if (mp_current > maxMp)
            mp_current = maxMp;
    }
    /// <summary>
    /// 添加经验值
    /// </summary>
    /// <param name="addition"></param>
    public void AddExperiment(int addition)
    {
        int total = expValue + addition; 
        if(total >= 100 * level * level)//100 * level * level是每一级升级所需经验
        {            
            total = 100 * level * level;//如果升级了，那经验就归为上一级升级所需经验值
            level++;
            PlayerSoundsManager.Instance.PlaySound(SoundType.LevelUp);
            Instantiate(levelUpPrefab, transform);
    }
expValue = total;

        //更新经验条的显示
        int upgradeExperiment = 100 * level * level;
        int lastUpgradeExperiment = 100 * (level - 1) * (level - 1);
        float fillmout = (float)(expValue - lastUpgradeExperiment) / (upgradeExperiment - lastUpgradeExperiment);
        string text = expValue + "/" + upgradeExperiment;
        ExperimentBar.Instance.UpdateExperimentBarShow(fillmout, text);
    }

    /// <summary>
    /// 玩家受伤
    /// </summary>
    public bool GetDamage(int damage)
    {
        if (hp_current <= 0)
            return true;

        Instantiate(damageEffetPrefab, transform);

        PlayerSoundsManager.Instance.PlaySound(SoundType.Hit);
        hp_current -= damage;
        if (hp_current <= 0)
        {
            hp_current = 0;
            animator.SetTrigger("Death");
            return true;
        }
        else
        {
            animator.SetTrigger("Damage");
            return false;
        }
    }
}
