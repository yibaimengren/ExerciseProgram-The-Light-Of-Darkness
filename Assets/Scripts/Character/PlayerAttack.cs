using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackDistance = 5;
    public int damagePower = 10;

    public EnemyBase attackTarget;
    private PlayerDir dir;
    private Animator animator;
    private Dictionary<string, GameObject> skillEffectDict = new Dictionary<string, GameObject>();
    private PlayerDir playerDir;

    private float attack_rate = 1f;
    private float timer = 0;
    void Start()
    {
        animator = GetComponent<Animator>();
        dir = GetComponent<PlayerDir>();
        playerDir = GetComponent<PlayerDir>();
    }
    void Update()
    {
        if (PlayerStatus.Instance.IsDeath)
            return;

        Attack();
        timer += Time.deltaTime;

        if (multiTargetSkill != null && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            Physics.Raycast(ray, out raycastHit);

            if(raycastHit.collider)
                StartCoroutine("UseMultTargetSkill", raycastHit.point);
        }

        if(attackTarget != null)
        {
            transform.LookAt(attackTarget.transform.position);
        }
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0) && PlayerStatus.Instance.AllowAttack)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit) && raycastHit.collider.tag=="Enemy")//如果在敌人身上点击左键
            {
                attackTarget = raycastHit.collider.GetComponent<EnemyBase>();                
            }
        }

        if (attackTarget == null)
            return;

        if(Vector3.Distance(transform.position,attackTarget.transform.position) > attackDistance)//如果在攻击技能释放范围外
        {
            dir.targetPos = attackTarget.transform.position;
        }
        else if(timer >= attack_rate && PlayerStatus.Instance.AllowAttack ) //如果在攻击技能释放范围内且时间达到攻击时间间隔
        {
            dir.targetPos = transform.position;
            transform.LookAt(attackTarget.transform.position);
            if (singleTargetSkill != null) // 有待释放的技能
            {
                print(singleTargetSkill);
                StartCoroutine("SkillForSingleTarget", singleTargetSkill);
            }
            else if(!PlayerStatus.Instance.isAim)//没有待释放的技能
            {
                timer = 0;
                dir.targetPos = transform.position;
                animator.SetTrigger("Attack1");
                PlayerSoundsManager.Instance.PlaySound(SoundType.Attack);
                int exp = attackTarget.GetDamage(PlayerStatus.Instance.GetAttackValue());
                if (exp != 0)//如果获得了经验，表示打死了敌人
                {
                    attackTarget = null;
                    PlayerStatus.Instance.AddExperiment(exp);
                }
            }
            
        }
    }

    public bool UseSkill(SkillInfo skill)
    {
        //检测MP够不够，够就释放技能。不够就返回false
        //同时还需要处于未使用其他技能的时刻
        if (PlayerStatus.Instance.AllowAttack && PlayerStatus.Instance.UseMp(skill.mp))
        {
            //使用技能
            switch (skill.applyType)
            {
                case ApplyType.Passive://如果是增益技能
                    SkillForPassive(skill);
                    break;
                case ApplyType.Buff://如果是Buff技能
                    SkillForBuff(skill);
                    break;
                case ApplyType.SingleTarget://如果是单目标技能
                    StartCoroutine("SkillForSingleTarget", skill);
                    break;
                case ApplyType.MultiTarget://如果是多目标技能
                    PrepareSkillForMultiTarget(skill);
                    break;
            }


            PlayerStatus.Instance.AllowAttack = true;
            return true;
        }
        else return false;
    }
    /// <summary>
    /// 获取技能效果的prefab
    /// </summary>
    /// <returns></returns>
    private GameObject GetSkillEffectPrefab(SkillInfo skill)
    {
        GameObject effectPrefab;
        skillEffectDict.TryGetValue(skill.efx_name, out effectPrefab);//先从字典里查找
        if (effectPrefab == null)//如果字典里没有，那么就读取资源然后存入字典中
        {
            effectPrefab = Resources.Load<GameObject>("Skill/" + skill.efx_name);
            skillEffectDict.Add(skill.efx_name, effectPrefab);
        }
        return effectPrefab;
    }

    private IEnumerator InstantiateAndSetAnimation(SkillInfo skill)
    {     
        //播放动画
        animator.SetTrigger(skill.aniname);
        //等待动画播放完毕
        yield return new WaitForSeconds(skill.anitime);
        timer = 0;

        Instantiate(GetSkillEffectPrefab(skill), transform);

        PlayerStatus.Instance.AllowAttack = true;
    }
    /// <summary>
    /// 使用增益技能
    /// </summary>
    /// <param name="skill"></param>
    /// <returns></returns>
    private void SkillForPassive(SkillInfo skill)
    {
        StartCoroutine("InstantiateAndSetAnimation", skill);

        if (skill.applyProperty == ApplyProperty.HP)
            PlayerStatus.Instance.AddHP(skill.applyValue);
        else if(skill.applyProperty == ApplyProperty.MP)
            PlayerStatus.Instance.AddMP(skill.applyValue);

    }

    private void SkillForBuff(SkillInfo skill)
    {
        StartCoroutine("InstantiateAndSetAnimation", skill);

        switch (skill.applyProperty)
        {
            case ApplyProperty.Attack://如果是攻击力buff
                PlayerStatus.Instance.SetAttackBuff(skill.applyValue / 100, skill.applyTime);
                break;
            case ApplyProperty.Speed://如果是速度buff
                PlayerStatus.Instance.SetSpeedBuff(skill.applyValue / 100, skill.applyTime);
                break;
            case ApplyProperty.Def://如果是防御力buff
                PlayerStatus.Instance.SetDefenseBuff(skill.applyValue / 100, skill.applyTime);
                break;
            case ApplyProperty.AttackSpeed://如果是攻击速度buff、
                attack_rate /= (skill.applyValue / 100);
                break;
        }
    }

    /// <summary>
    /// 释放单个目标技能
    /// </summary>
    private SkillInfo singleTargetSkill;
    private Transform skillTarget;
    private IEnumerator SkillForSingleTarget(SkillInfo skill)
    {

        if(attackTarget == null)//如果当前没有攻击目标，则留给下一次攻击
        {
            PlayerStatus.Instance.isAim = true;
            CursorManager.Instance.SetAimCursor();
            singleTargetSkill = skill;
            multiTargetSkill = null;
        }
        else//如果当前有攻击目标，则直接攻击
        {
            singleTargetSkill = null;
            timer = 0;
            skillTarget = attackTarget.transform;
            //播放动画
            animator.SetTrigger(skill.aniname);
            //等待动画播放完毕
            yield return new WaitForSeconds(skill.anitime);
            timer = 0;
            //实例化特效
            if(skillTarget != null)
            {
                PlayerSoundsManager.Instance.PlaySound(SoundType.Attack);
                Instantiate(GetSkillEffectPrefab(skill), skillTarget);
                //计算伤害
                //print(attackTarget);
                int exp = attackTarget.GetDamage(skill.applyValue); ;
                if (exp != 0)//如果获得了经验，表示打死了敌人
                {
                    attackTarget = null;
                    PlayerStatus.Instance.AddExperiment(exp);
                }

                skillTarget = null;
            }

            PlayerStatus.Instance.AllowAttack = true;
            PlayerStatus.Instance.isAim = false;
            singleTargetSkill = null;
            CursorManager.Instance.SetNormalCursor();
            timer = 0;
        }
    }
    public SkillInfo multiTargetSkill;//待释放的多目标技能

    private void PrepareSkillForMultiTarget(SkillInfo skill)
    {
        PlayerStatus.Instance.isAim = true;
        CursorManager.Instance.SetAimCursor();
        multiTargetSkill = skill;
        singleTargetSkill = null;    
    }
    /// <summary>
    /// 使用多目标技能
    /// </summary>
    public IEnumerator UseMultTargetSkill(Vector3 releasePos)
    {
        SkillInfo skill = multiTargetSkill;
        multiTargetSkill = null;
        //播放动画
        animator.SetTrigger(skill.aniname);
        
        //等待动画播放完毕
        yield return new WaitForSeconds(skill.anitime);
        timer = 0;

        GameObject go = Instantiate(GetSkillEffectPrefab(skill), releasePos, Quaternion.identity);

        MagicSphereEffect magicSphereEffect = go.GetComponent<MagicSphereEffect>();
        magicSphereEffect.damage = skill.applyValue;
      
        PlayerStatus.Instance.AllowAttack = true;
        PlayerStatus.Instance.isAim = false;
        multiTargetSkill = null;
        CursorManager.Instance.SetNormalCursor();
        timer = 0;
    }

}
