using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get; set;
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Init()
    {
        audioSource = GetComponent<AudioSource>();
        sceneMusic = new AudioClip[10];
        sceneMusic[0] = Resources.Load<AudioClip>("AudioClip/PVZHmain");
        sceneMusic[1] = Resources.Load<AudioClip>("AudioClip/PVZHdeckmusic");
        sceneMusic[2] = Resources.Load<AudioClip>("AudioClip/Tutorial Grasswalk");
        sceneMusic[3] = Resources.Load<AudioClip>("AudioClip/GameWon");
        sceneMusic[4] = Resources.Load<AudioClip>("AudioClip/Lost");
    }
    public void Play(int index)
    {
        if (audioSource.clip != null) audioSource.Stop();
        audioSource.clip = sceneMusic[index];
        audioSource.Play();
        switch (index)
        {
            case 0:
            case 1:
            case 2:
                audioSource.loop = true;
                break;
            case 3:
            case 4:
                audioSource.loop = false;
                break;
        }
    }
    public AudioClip[] sceneMusic;
    public AudioSource audioSource;
}
