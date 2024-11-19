using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyManager : PersistentSingleton<LobbyManager>
{
    public async Task CreateLobbyAsync(string lobbyName)
    {
        string joinCode = await RelayManager.Instance.CreateRelayAsync();
        var options = new CreateLobbyOptions
        {
            Data = new Dictionary<string, DataObject>
            {
                [JoinCodeKey] = new DataObject(DataObject.VisibilityOptions.Member, joinCode)
            }
        };
        currentLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, 2, options);
        Heartbeat();
        PeriodicallyRefreshLobby();
    }
    public async Task QuickJoinLobbyAsync()
    {
        try
        {
            currentLobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            if (currentLobby != null)
            {
                string joinCode = currentLobby.Data[JoinCodeKey].Value;
                await RelayManager.Instance.JoinRelayAsync(joinCode);
            }

        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
    private async void Heartbeat()
    {
        heartBeatSource = new CancellationTokenSource();
        while (!heartBeatSource.IsCancellationRequested && currentLobby != null)
        {
            await Lobbies.Instance.SendHeartbeatPingAsync(currentLobby.Id);
            await Task.Delay(15 * 1000); //每15s Heartbeat 1次
        }
    }
    private async void PeriodicallyRefreshLobby()
    {
        updateLobbySource = new CancellationTokenSource();
        await Task.Delay(1 * 1000);
        while (!updateLobbySource.IsCancellationRequested && currentLobby != null)
        {
            currentLobby = await Lobbies.Instance.GetLobbyAsync(currentLobby.Id);
            await Task.Delay(1 * 1000);
        }
    }

    public async Task LeaveLobbyAsync()
    {
        heartBeatSource?.Cancel();
        updateLobbySource?.Cancel();
        if (currentLobby != null)
        {
            try
            {
                if (currentLobby.HostId == Authentication.PlayerId)
                {
                    await Lobbies.Instance.DeleteLobbyAsync(currentLobby.Id);
                }
                else
                {
                    await Lobbies.Instance.RemovePlayerAsync(currentLobby.Id, Authentication.PlayerId);
                }
                currentLobby = null;
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }

    public async Task<List<Lobby>> SearchLobbyAsync()
    {
        var options = new QueryLobbiesOptions
        {
            Count = 15,

            Filters = new List<QueryFilter> {
                new(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT),
                new(QueryFilter.FieldOptions.IsLocked, "0", QueryFilter.OpOptions.EQ)
            }
        };
        var allLobbies = await Lobbies.Instance.QueryLobbiesAsync(options);
        return allLobbies.Results;
    }

    public Lobby currentLobby;
    private CancellationTokenSource heartBeatSource;
    private CancellationTokenSource updateLobbySource;

    static public string JoinCodeKey = "JoinCode";
}