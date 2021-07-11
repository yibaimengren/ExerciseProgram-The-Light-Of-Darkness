using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class StartPanel : MonoBehaviour
{
    private Image pressKey;
    private GameObject startGame;
    public float speed = 0.1f;

    void Start()
    {
        pressKey = this.transform.Find("PressAnykeyToStart").GetComponent<Image>();
        startGame = this.transform.Find("startGame").gameObject;
    }

    void Update()
    {
        if (pressKey != null)
        {
            float a = pressKey.color.a - speed * Time.deltaTime;

            if (a <= 0 || a >= 1)
                speed *= -1;
            pressKey.color = new Color(pressKey.color.r, pressKey.color.b, pressKey.color.g,a);

            if (Input.anyKey)
            {
                OnPressAnyKey();
            }
        }
     
    }

    public void OnPressAnyKey()
    {
        startGame.SetActive(true);
        pressKey.gameObject.SetActive(false);
    }
    /// <summary>
    /// 开始新游戏，加载选择角色场景
    /// </summary>
    public void OnNewGame()
    {
        startGame.transform.Find("newGame").GetComponent<AudioSource>().Play();
        PlayerPrefs.SetInt("LoadGame", 0);//在新场景进行验证，是否要读取存档，0表示不读取
        SceneManager.LoadScene(1);
    }
    /// <summary>
    /// 加载保存过的游戏，进入游戏场景
    /// </summary>
    public void OnLoadGame()
    {
        startGame.transform.Find("loadGame").GetComponent<AudioSource>().Play();
        PlayerPrefs.SetInt("LoadGame", 1);//在新场景进行验证，是否要读取存档，1表示读取

        SceneManager.LoadScene(2);
    }
    
}
