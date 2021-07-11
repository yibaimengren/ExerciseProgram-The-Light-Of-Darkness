using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public Transform playerPos;//玩家出生点

    public GameObject[] playerPrefabs;
    [System.NonSerialized]
    public int characterIndex = 0;

    static private GameManager _instance;
    static public GameManager Instance
    {
        get { return _instance; }
    }
    void Awake()
    {
        _instance = this;

        characterIndex = PlayerPrefs.GetInt("SelectCharacterIndex");
        GameObject go = Instantiate(playerPrefabs[characterIndex], playerPos);
        PlayerStatus playerStatue = go.GetComponent<PlayerStatus>(); 
        playerStatue.playerName = PlayerPrefs.GetString("PlayerName", "啦啦啦啦");

        if (PlayerPrefs.GetInt("LoadGame") != 0)//加载存档
        {
            playerStatue.level = PlayerPrefs.GetInt("PlayerLevel",1);
            float x = PlayerPrefs.GetFloat("posX");
            float y = PlayerPrefs.GetFloat("posY");
            float z = PlayerPrefs.GetFloat("posZ");
            playerStatue.transform.position = new Vector3(x, y, z);
        }
    }
}
