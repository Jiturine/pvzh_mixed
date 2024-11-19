using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuPanel : BasePanel
{
    public Button startOffline;
    public Button startOnline;
    public Button hall;
    public Button options;
    public Button exit;
    public Button help;
    public Button switchUser;
    public TextMeshProUGUI userNameText;
    new void Start()
    {
        base.Start();
        startOffline.onClick.AddListener(GameManager.Instance.OnStartOfflineBtnClick);
        startOnline.onClick.AddListener(UIManager.Instance.TogglePanel<OnlineGamePanel>);
        startOnline.onClick.AddListener(UIManager.Instance.PlayButtonClickSFX);
        hall.onClick.AddListener(UIManager.Instance.PlayButtonClickSFX);
        hall.onClick.AddListener(UIManager.Instance.TogglePanel<HallPanel>);
        options.onClick.AddListener(UIManager.Instance.TogglePanel<OptionsPanel>);
        exit.onClick.AddListener(GameManager.Instance.Quit);
        switchUser.onClick.AddListener(UIManager.Instance.TogglePanel<SwitchUserPanel>);
        userNameText.text = UserManager.Instance.currentUserInfo.name;
        help.onClick.AddListener(LoadHelpScene);
    }

    void Update()
    {
        userNameText.text = UserManager.Instance.currentUserInfo.name;
    }
    private void LoadHelpScene()
    {
        SceneManager.LoadScene("Help");
    }
}
