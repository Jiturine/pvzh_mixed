using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class OptionsMenu : MonoBehaviour
{
    public static OptionsMenu Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        BackToMainMenuButton = Instance.transform.Find("Button").GetComponent<Button>();
        volumeSlider = Instance.transform.Find("Volume Slider").GetComponent<Slider>();
        BackToMainMenuButton.onClick.AddListener(GameManager.Instance.OnEndGameMenuBtnClick);
        volumeSlider.onValueChanged.AddListener(SetVolume);
        volumeSlider.value = AudioListener.volume;
        Instance.gameObject.SetActive(false);
    }

    void Update()
    {

    }
    public void ToggleMenu()
    {
        Instance.gameObject.SetActive(!Instance.gameObject.activeSelf);
    }
    public void SetVolume(float value)
    {
        AudioListener.volume = value;
    }
    public Button BackToMainMenuButton;
    public Slider volumeSlider;
}
