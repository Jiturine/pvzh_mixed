using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.VisualScripting;
using UnityEngine;

public class RelayManager : PersistentSingleton<RelayManager>
{
    public async Task<string> CreateRelayAsync()
    {
        string joinCode = "0";
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(1); //最多1个client
            joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartHost();
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
            var messageBoxPanel = UIManager.Instance.TryOpenPanel<MessageBoxPanel>();
            messageBoxPanel.ShowMessage($"连接错误：{e}", 3f);
        }
        Debug.Log("Join Code: " + joinCode);
        this.joinCode = joinCode;
        return joinCode;
    }
    public async Task JoinRelayAsync(string joinCode)
    {
        try
        {
            Debug.Log("Joining Relay with: " + joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartClient();
            GameManager.clientName = UserManager.Instance.currentUserInfo.name;
        }
        catch (RelayServiceException e)
        {
            var messageBoxPanel = UIManager.Instance.TryOpenPanel<MessageBoxPanel>();
            messageBoxPanel.ShowMessage($"连接错误：{e}", 3f);
            Debug.Log(e);
        }
    }
    public string joinCode;
}
