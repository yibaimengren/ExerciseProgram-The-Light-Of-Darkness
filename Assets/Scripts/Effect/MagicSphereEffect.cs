using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSphereEffect : MonoBehaviour
{
    public int damage = 1;

    private List<EnemyBase> enemyList = new List<EnemyBase>();
    void OnTriggerEnter(Collider other)
    {
        EnemyBase enemyBase = other.gameObject.GetComponent<EnemyBase>();
        if (enemyBase)
        {
            int index = enemyList.IndexOf(enemyBase);

            if (index == -1)
            {
                enemyList.Add(enemyBase);
                int exp = enemyBase.GetDamage(damage);
                if (exp != 0)//如果获得了经验，表示打死了敌人
                {
                    enemyBase = null;
                    PlayerStatus.Instance.AddExperiment(exp);
                }
            }
        }        
    }
}
