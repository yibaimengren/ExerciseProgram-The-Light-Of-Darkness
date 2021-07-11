using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterCreation : MonoBehaviour
{
    private GameObject[] characterArray;
    private int currentIndex;
    public InputField input;
    void Start()
    {
        GameObject[] prefabs = Resources.LoadAll<GameObject>("Character/No_Animation");
        characterArray = new GameObject[prefabs.Length];
        for(int i = 0; i < prefabs.Length; i++)
        {
            characterArray[i] = Instantiate(prefabs[i],transform.position,transform.rotation);
            characterArray[i].SetActive(false);
        }
        characterArray[0].SetActive(true);
    }

    private void UpdateShowCharacter()
    {
        for (int i = 0; i < characterArray.Length; i++)
        {
            characterArray[i].SetActive(false);
            if (i == currentIndex)
                characterArray[i].SetActive(true);
        }
    }

    public void OnClickNextButton()
    {
        currentIndex++;
        currentIndex %= characterArray.Length;
        UpdateShowCharacter();
    }

    public void OnClickPriorButton()
    {
        currentIndex--;
        if (currentIndex == -1)
            currentIndex = characterArray.Length - 1;
        UpdateShowCharacter();
    }

    public void OnClickOKButton()
    {
        //存储角色和名字
        PlayerPrefs.SetInt("SelectCharacterIndex", currentIndex);
        PlayerPrefs.SetString("PlayerName", input.text);
        PlayerPrefs.SetInt("LoadGame", 0);//0表示创建新游戏,1表示加载存档游戏
        //加载新场景

        SceneManager.LoadScene(2);
    }

}
