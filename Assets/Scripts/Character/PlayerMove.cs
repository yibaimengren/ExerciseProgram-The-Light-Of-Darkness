using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private PlayerDir dir;
    private CharacterController characterController;
    private Animator animator;
    public float moveSpeed;
    private PlayerAttack playerAttack;
    // Start is called before the first frame update
    void Start()
    {
        dir = this.transform.GetComponent<PlayerDir>();
        playerAttack = transform.GetComponent<PlayerAttack>();
        characterController = this.transform.GetComponent<CharacterController>();
        animator = this.transform.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move(); 
    }

    public void Move()
    {
        if (PlayerStatus.Instance.IsDeath || playerAttack.multiTargetSkill != null )
            return;
        //没有达到目标、没有死亡且没有在播放受伤动画就继续走
        if (Vector3.Distance(this.transform.position, dir.targetPos) > 0.3f
            && !PlayerStatus.Instance.IsDeath
            && animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "TakeDamage2") 
        {
            Vector3 direction = dir.targetPos - this.transform.position;
            //print("速度："+ (moveSpeed + (PlayerStatus.Instance.GetSpeedValue() / 20)));
            characterController.SimpleMove(direction.normalized *(moveSpeed+(PlayerStatus.Instance.GetSpeedValue()/20)));
            animator.SetBool("Run", true);
            float speed = Mathf.Clamp(characterController.velocity.magnitude * 0.5f, 0.5f, 10);
            animator.speed = speed;         
        }
        else
        {
            animator.SetBool("Run", false);
        }

            
    }
}
