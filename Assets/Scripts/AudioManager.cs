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
    public AudioClip[] sceneMusic;
    public AudioSource audioSource;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        sceneMusic = new AudioClip[5];
        sceneMusic[0] = Resources.Load<AudioClip>("AudioClip/Faster");
        sceneMusic[1] = Resources.Load<AudioClip>("AudioClip/Look up at the Sky");
    }
}
