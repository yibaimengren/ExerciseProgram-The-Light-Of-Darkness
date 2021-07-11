using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType {
    BabyWolf,
    NormalWolf,
    BossWolf
}

public class EnemyBase : MonoBehaviour
{
    private Animator animator;
    private CharacterController characterController;
    private float timer;
    private Vector3 birthPos;
    private Vector3 walkDirection;
    private bool isMove = false;
    private bool followTarget = false;
    private GameObject fadeTextPrefab;
    private Transform uiRootT;
    private GameObject effectPrefab;

    public EnemyType enemyType = EnemyType.BabyWolf;
    public float statyTime = 2.0f;
    public float walkDistance = 20;//相对于出生点的可移动半径
    public float moveSpeed = 2;
    public int Hp = 30;
    public int expValue = 30;
    //攻击
    public float attack_MaxDistance = 10;
    public float attack_MinDistance = 1;
    public Transform attackTarget;
    public int attackPower = 15;
    public float attack_rate = 1f;//攻击频率

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        birthPos = transform.position;
        fadeTextPrefab = Resources.Load<GameObject>("FadeText");
        uiRootT = GameObject.FindWithTag("Canvas").transform;
        effectPrefab = Resources.Load<GameObject>("Range Hit_Effect");
    }

    void Update()
    {
        if (Hp <= 0)
            return;

        timer += Time.deltaTime;
        if(timer >= statyTime && !followTarget)
        {
            timer = 0;
            int option = Random.Range(0, 2);
            if (option == 0)
            {
                animator.SetBool("Walk", true);
                isMove = true;
                SetWalkDirection();

            }
            else
            {
                animator.SetBool("Walk", false);
                isMove = false;
            } 
        }

        if (isMove && Hp >0)
        {
            characterController.SimpleMove(walkDirection * moveSpeed);
        }

        AutoAttack();
    }

    private void SetWalkDirection()
    {
        //设置随机移动方向
  loop: float degree = Random.Range(0, 360);
        walkDirection = Quaternion.Euler(0, degree, 0) * transform.forward; 
        //验证此随机值是否在移动范围内
        Vector3 targetPos = transform.position + walkDirection * moveSpeed;
        float currentDistance = Vector3.Distance(birthPos, transform.position);
        float moveAfterDistance = Vector3.Distance(birthPos, targetPos);
        if (moveAfterDistance > walkDistance && moveAfterDistance>currentDistance)//如果超出了移动范围并且往远方向走则重新计算方向
            goto loop;

        transform.LookAt(targetPos);       
    }
    /// <summary>
    /// 受到攻击
    /// </summary>
    /// <param name="value"></param>
    public int GetDamage(int value)
    {
        if (Hp <= 0)
            return 0;

        animator.SetBool("Walk", false);
        isMove = false;
        Instantiate(effectPrefab, transform);
        Hp -= value;
        FadeText fadeText = Instantiate(fadeTextPrefab, uiRootT).GetComponent<FadeText>();
        fadeText.Initial(transform, "-" + value, Color.red, 5f);

        if (Hp <= 0)
        {
            animator.SetTrigger("Death");
            if(isEnter)
                CursorManager.Instance.SetNormalCursor();

            if (enemyType == EnemyType.BabyWolf)
                BabyWolfSpawn.Instance.BabyWolfDead();

            QuestManager.Instance.babyWolfKilled++;

            Destroy(this.gameObject, 2);
            return expValue;
        }
        else
        {
            //随机选择受伤动画播放，多元一些
            int d = Random.Range(0, 2);
            if(d==0)
                animator.SetTrigger("Damage1");
            else
                animator.SetTrigger("Damage2");

            followTarget = true;
            attackTarget = PlayerStatus.Instance.transform;//将攻击目标设为玩家
            return 0;
        }
           
    }

    private bool isEnter = false;
    void OnMouseEnter()
    {
        CursorManager.Instance.SetAttackCursor();
        isEnter = true;
    }

    void OnMouseExit()
    {
        CursorManager.Instance.SetNormalCursor();
        isEnter = false;
    }


    void AutoAttack()
    {
        if (attackTarget == null)
            return;
        
        if(Vector3.Distance(transform.position,attackTarget.position) <= attack_MinDistance && timer >= attack_rate)//如果处于攻击范围内且达到了攻击频率时间
        {
            timer = 0;
            transform.LookAt(attackTarget.position);

            int d = Random.Range(0, 2);
            if (d == 0)
                animator.SetTrigger("Attack1");
            else
                animator.SetTrigger("Attack2");

            int damage = attackPower - PlayerStatus.Instance.GetDefenceValue();//计算伤害值
            if (damage < 0)
                damage = 0;
            if (PlayerStatus.Instance.GetDamage(damage))//如果攻击让角色死亡了
            {
                isMove = false;
                followTarget = false;
                attackTarget = null;
            }
        }else if(Vector3.Distance(transform.position, attackTarget.position) <= attack_MaxDistance)//如果处于跟踪距离内
        {
            animator.SetBool("Walk", true);
            transform.LookAt(attackTarget.position);
            walkDirection = (attackTarget.position - transform.position).normalized;//设置移动方向
            isMove = true;
        }
        else //如果角色跑到了此敌人攻击范围外，就取消跟踪
        {
            isMove = false;
            followTarget = false;
            attackTarget = null;
        }

    }
}
