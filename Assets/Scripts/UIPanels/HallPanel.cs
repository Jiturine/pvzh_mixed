using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Services.Lobbies;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Game;

public class HallPanel : BasePanel
{
    public Transform lobbyListContent;
    public Button createLobby;
    public Button joinLobby;
    public Button quickJoin;
    public Button cancel;
    public List<LobbyItem> lobbyItemList;
    public GameObject lobbyItemPrefab;
    public string selectedLobbyID;
    new void Start()
    {
        base.Start();
        createLobby.onClick.AddListener(UIManager.Instance.PlayButtonClickSFX);
        createLobby.onClick.AddListener(UIManager.Instance.TogglePanel<CreateLobbyPanel>);
        joinLobby.onClick.AddListener(UIManager.Instance.PlayButtonClickSFX);
        joinLobby.onClick.AddListener(JoinLobbyAsync);
        quickJoin.onClick.AddListener(UIManager.Instance.PlayButtonClickSFX);
        quickJoin.onClick.AddListener(QuickJoinLobbyAsync);
        cancel.onClick.AddListener(UIManager.Instance.PlayButtonClickSFX);
        cancel.onClick.AddListener(UIManager.Instance.TogglePanel<HallPanel>);
        lobbyItemList = new List<LobbyItem>();
        refreshTimer = Time.time;
    }
    void Update()
    {
        if (refreshTimer < Time.time)
        {
            FetchLobbiesAsync();
        }
    }
    public async void QuickJoinLobbyAsync()
    {
        var waitingPanel = UIManager.Instance.TryOpenPanel<WaitingPanel>();
        waitingPanel.SetMessage("正在尝试快速加入，请耐心等待");
        await LobbyManager.Instance.QuickJoinLobbyAsync();
        gameMode = GameMode.Online;
        gameState = GameState.SelectCard;
        SceneManager.LoadScene("Select Card");
        UIManager.Instance.ClosePanel<WaitingPanel>();
    }

    public async void JoinLobbyAsync()
    {
        var waitingPanel = UIManager.Instance.TryOpenPanel<WaitingPanel>();
        waitingPanel.SetMessage("正在尝试加入房间，请耐心等待");
        await LobbyManager.Instance.JoinLobbyAsync(selectedLobbyID);
        gameMode = GameMode.Online;
        gameState = GameState.SelectCard;
        SceneManager.LoadScene("Select Card");
        UIManager.Instance.ClosePanel<WaitingPanel>();
    }

    private async void FetchLobbiesAsync()
    {
        try
        {
            refreshTimer = Time.time + 5f;
            // 获取所有房间
            var allLobbies = await LobbyManager.Instance.SearchLobbyAsync();
            //无房间时显示：
            if (!lobbyItemList.Any())
            {
                Debug.Log("No available lobby");
            }
            // _noLobbiesText.SetActive(!_currentLobbySpawns.Any());

            // 销毁本次刷新后不存在的房间
            // 排除自己创建的房间
            var lobbyIds = allLobbies.Where(lobby => lobby.HostId != Authentication.PlayerId).Select(lobby => lobby.Id);
            if (lobbyItemList.Any())
            {
                var notActiveLobbyItems = lobbyItemList.Where(lobbyItem => !lobbyIds.Contains(lobbyItem.lobby.Id)).ToList();

                foreach (var lobbyItem in notActiveLobbyItems)
                {
                    Destroy(lobbyItem.gameObject);
                    lobbyItemList.Remove(lobbyItem);
                }
            }
            // 更新或生成房间
            foreach (var lobby in allLobbies)
            {
                var currentLobbyItem = lobbyItemList.FirstOrDefault(lobbyItem => lobbyItem.lobby.Id == lobby.Id);
                if (currentLobbyItem != null)
                {
                    currentLobbyItem.UpdateDetails(lobby);
                }
                else
                {
                    var lobbyItem = Instantiate(lobbyItemPrefab, lobbyListContent).GetComponent<LobbyItem>();
                    lobbyItem.Init(lobby);
                    lobbyItemList.Add(lobbyItem);
                }
            }
        }
        catch (LobbyServiceException e)
        {
            var messageBoxPanel = UIManager.Instance.TryOpenPanel<MessageBoxPanel>();
            messageBoxPanel.ShowMessage($"连接错误：{e}", 3f);
            Debug.LogError(e);
        }
    }
    private float refreshTimer;
}
