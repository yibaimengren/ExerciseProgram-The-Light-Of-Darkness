using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyWolfSpawn : MonoBehaviour
{
    static private BabyWolfSpawn _instance;
    static public BabyWolfSpawn Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;
        StartCoroutine(CreateBabyWolf());
    }



    public float interval = 1.0f;
    public int maxWolfCount = 5;
    public float SpawnRadius = 3;
    public GameObject babyWolfPrefab;
    

    private int currentCount = 0;

    /// <summary>
    /// 当前小狼数量减一
    /// </summary>
    public void BabyWolfDead()
    {
        currentCount--;
        StartCoroutine(CreateBabyWolf());
    }
    /// <summary>
    /// 生成狼
    /// </summary>
    private IEnumerator CreateBabyWolf()
    {
        while(currentCount < maxWolfCount)
        {
            Transform trans = Instantiate(babyWolfPrefab, transform).GetComponent<Transform>();
            float x = Random.Range(trans.position.x - 3, trans.position.x + 3);
            float z = Random.Range(trans.position.z - 3, trans.position.z + 3);
            trans.position = new Vector3(x, trans.position.y, z);
            currentCount++;
            yield return new WaitForSeconds(interval);         
        }
    }
}
