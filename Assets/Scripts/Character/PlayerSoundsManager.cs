using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundsManager : MonoBehaviour
{
    static private PlayerSoundsManager _instance;
    static public PlayerSoundsManager Instance
    {
        get { return _instance; }
    }

    private AudioSource audioSource;

    public AudioClip hit;
    public AudioClip attack;
    public AudioClip levelUp;
    public AudioClip heal;
    // Start is called before the first frame update
    void Start()
    {
        _instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(SoundType soundType)
    {
        switch(soundType)
        {
            case SoundType.Hit:
                audioSource.clip = hit;
                break;
            case SoundType.Attack:
                audioSource.clip = attack;
                break;
            case SoundType.LevelUp:
                audioSource.clip = levelUp;
                break;
            case SoundType.Heal:
                audioSource.clip = heal;
                break;
        }
        
        audioSource.Play();
    }



    
}

public enum SoundType
{
    Hit,
    Attack,
    LevelUp,
    Heal
}
