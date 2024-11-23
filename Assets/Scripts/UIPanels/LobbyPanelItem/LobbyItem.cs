using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyItem : MonoBehaviour
{
    public Button button;
    public Image backgroundImage;
    public TextMeshProUGUI lobbyNameText;
    public Lobby lobby;
    void Start()
    {
        button.onClick.AddListener(SelectUser);
    }
    public void SelectUser()
    {
        var hallPanel = UIManager.Instance.GetPanel<HallPanel>();
        var preItem = hallPanel.lobbyItemList.Find(lobbyItem => lobbyItem.lobby.Id == hallPanel.selectedLobbyID);
        if (preItem != null)
        {
            preItem.backgroundImage.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        }
        hallPanel.selectedLobbyID = lobby.Id;
        backgroundImage.color = selectedColor;
    }
    public void UpdateDetails(Lobby lobby)
    {
        lobbyNameText.text = lobby.Name;
        this.lobby = lobby;
    }
    public void Init(Lobby lobby)
    {
        UpdateDetails(lobby);
    }
    static private Color selectedColor = new Color(20.0f / 255, 180.0f / 255, 15.0f / 255, 1.0f);
}
