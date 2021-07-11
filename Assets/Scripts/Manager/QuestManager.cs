using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    static private QuestManager _instance;
    static public QuestManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public int babyWolfKilled = 0;

    void Awake()
    {
        _instance = this;
    }
}
