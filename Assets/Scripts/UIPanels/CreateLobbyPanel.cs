using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Game;

public class CreateLobbyPanel : BasePanel
{
    public Button OK;
    public Button cancel;
    public TMP_InputField inputField;
    new void Start()
    {
        base.Start();
        OK.onClick.AddListener(UIManager.Instance.PlayButtonClickSFX);
        OK.onClick.AddListener(CreateLobbyAsync);
        cancel.onClick.AddListener(UIManager.Instance.PlayButtonClickSFX);
        cancel.onClick.AddListener(UIManager.Instance.TogglePanel<CreateLobbyPanel>);
    }
    public async void CreateLobbyAsync()
    {
        var waitingPanel = UIManager.Instance.TryOpenPanel<WaitingPanel>();
        waitingPanel.SetMessage("正在创建房间，请耐心等待");
        string name = inputField.text;
        await LobbyManager.Instance.CreateLobbyAsync(name);
        inputField.text = "";
        gameMode = GameMode.Online;
        gameState = GameState.SelectCard;
        SceneManager.LoadScene("Select Card");
        UIManager.Instance.ClosePanel<WaitingPanel>();
        UIManager.Instance.TogglePanel<CreateLobbyPanel>();
    }
}
