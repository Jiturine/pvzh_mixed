using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Services.Lobbies;
using UnityEngine;
using UnityEngine.UI;

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
        quickJoin.onClick.AddListener(UIManager.Instance.PlayButtonClickSFX);
        quickJoin.onClick.AddListener(QuickJoin);
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
    public async void QuickJoin()
    {
        await LobbyManager.Instance.QuickJoinLobbyAsync();
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
            Debug.LogError(e);
        }
    }
    private float refreshTimer;
}
