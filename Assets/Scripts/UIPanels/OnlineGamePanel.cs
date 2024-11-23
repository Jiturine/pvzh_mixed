using System.Collections;
using System.Collections.Generic;
using static Game;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnlineGamePanel : BasePanel
{
    public Button createGame;
    public Button joinGame;
    public Button cancel;
    public TMP_InputField inputField;
    new void Start()
    {
        base.Start();
        cancel.onClick.AddListener(UIManager.Instance.PlayButtonClickSFX);
        cancel.onClick.AddListener(UIManager.Instance.TogglePanel<OnlineGamePanel>);
        createGame.onClick.AddListener(UIManager.Instance.PlayButtonClickSFX);
        createGame.onClick.AddListener(CreateGame);
        joinGame.onClick.AddListener(UIManager.Instance.PlayButtonClickSFX);
        joinGame.onClick.AddListener(JoinGame);
    }
    public async void CreateGame()
    {
        var waitingPanel = UIManager.Instance.TryOpenPanel<WaitingPanel>();
        waitingPanel.SetMessage("正在创建对局，请耐心等待");
        string joinCode = await RelayManager.Instance.CreateRelayAsync();
        UIManager.Instance.ClosePanel<WaitingPanel>();
        Debug.Log($"Join Code: {joinCode}");
        gameMode = GameMode.Online;
        gameState = GameState.SelectCard;
        SceneManager.LoadScene("Select Card");
    }
    public async void JoinGame()
    {
        string joinCode = inputField.text;
        if (joinCode == "") return;
        var waitingPanel = UIManager.Instance.TryOpenPanel<WaitingPanel>();
        waitingPanel.SetMessage("正在加入对局，请耐心等待");
        await RelayManager.Instance.JoinRelayAsync(joinCode);
        UIManager.Instance.ClosePanel<WaitingPanel>();
        gameMode = GameMode.Online;
        gameState = GameState.SelectCard;
        SceneManager.LoadScene("Select Card");
    }
}
