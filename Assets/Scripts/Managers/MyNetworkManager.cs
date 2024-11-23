using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MyNetworkManager : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            Debug.Log("A new client connected, id = " + id);
            GameManager.Instance.SetNameServerRpc(GameManager.Instance.IsServer, UserManager.Instance.currentUserInfo.name);
        };
        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
            Debug.Log("A new client disconnected, id = " + id);
        };
        NetworkManager.Singleton.OnServerStarted += () =>
        {
            Debug.Log("Server Start!");
        };
    }
}
