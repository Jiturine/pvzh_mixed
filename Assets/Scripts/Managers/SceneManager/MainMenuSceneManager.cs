using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class MainMenuSceneManager : NetworkBehaviour
{
    void Start()
    {
        UIManager.Instance.ExistPanels.Clear();
        UIManager.Instance.OpenPanel<MainMenuPanel>();
        AudioManager.Instance.PlayBGM("PVZHmain");
        GameManager.Instance.LoadMainMenuScene();
        Debug.Log(GetIP());
    }
    void GetInputIP(string userInput)
    {
        NetworkManager.GetComponent<UnityTransport>().ConnectionData.Address = userInput;
    }
    void GetInputPort(string userInput)
    {
        NetworkManager.GetComponent<UnityTransport>().ConnectionData.Port = ushort.Parse(userInput);
    }
    string GetIP()
    {
        string ipv4 = "";
        foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
        {
            NetworkInterfaceType _type1 = NetworkInterfaceType.Wireless80211;
            NetworkInterfaceType _type2 = NetworkInterfaceType.Ethernet;
            if ((item.NetworkInterfaceType == _type1 || item.NetworkInterfaceType == _type2) && item.OperationalStatus == OperationalStatus.Up)
            {
                foreach (var ip in item.GetIPProperties().UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        ipv4 = ip.Address.ToString();
                    }
                }
            }
        }
        return ipv4;
    }
}
