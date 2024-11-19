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
        // string joinCode = await RelayManager.Instance.CreateRelayAsync();
        // Debug.Log($"Join Code: {joinCode}");
        NetworkManager.Singleton.StartHost();
        gameMode = GameMode.Online;
        gameState = GameState.SelectCard;
        SceneManager.LoadScene("Select Card");
    }
    public async void JoinGame()
    {
        // string joinCode = inputField.text;
        // if (joinCode == "") return;
        // await RelayManager.Instance.JoinRelayAsync(joinCode);
        NetworkManager.Singleton.StartClient();
        gameMode = GameMode.Online;
        gameState = GameState.SelectCard;
        SceneManager.LoadScene("Select Card");
    }
}
