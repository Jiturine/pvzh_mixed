using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Game;

public class OptionsPanel : BasePanel
{
    new void Start()
    {
        base.Start();
        backButton.onClick.AddListener(UIManager.Instance.TogglePanel<OptionsPanel>);
        backButton.onClick.AddListener(UIManager.Instance.PlayButtonClickSFX);
        backToMainMenuButton.onClick.AddListener(UIManager.Instance.PlayButtonClickSFX);
        backToMainMenuButton.onClick.AddListener(BackToMainMenu);
        BGMVolumeSlider.onValueChanged.AddListener(SetBGMVolume);
        BGMVolumeSlider.value = AudioManager.Instance.BGMVolume;
        SFXVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        SFXVolumeSlider.value = AudioManager.Instance.SFXVolume;
        fullScreen.onValueChanged.AddListener(SetFullScreen);
        fullScreen.isOn = Screen.fullScreen;
    }
    void Update()
    {
        if (gameState == GameState.MainMenu)
        {
            backToMainMenuButton.gameObject.SetActive(false);
        }
        else
        {
            backToMainMenuButton.gameObject.SetActive(true);
        }
    }
    public void ToggleMenu()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
    public void SetBGMVolume(float value)
    {
        AudioManager.Instance.BGMVolume = value;
    }
    public void SetSFXVolume(float value)
    {
        AudioManager.Instance.SFXVolume = value;
    }
    public void SetFullScreen(bool value)
    {
        Screen.fullScreen = value;
        UIManager.Instance.PlayButtonClickSFX();
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public Button backButton;
    public Button backToMainMenuButton;
    public Slider BGMVolumeSlider;
    public Slider SFXVolumeSlider;
    public Toggle fullScreen;
}
