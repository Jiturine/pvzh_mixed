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

    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.ExistPanels.Clear();
        UIManager.Instance.OpenPanel<MainMenuPanel>();
        if (AudioManager.Instance.PlayingBGM != "PVZHmain") AudioManager.Instance.PlayBGM("PVZHmain");
        GameManager.Instance.LoadMainMenuScene();
        // inputIPField = GameObject.Find("Input IP").GetComponent<InputField>();
        // inputPortField = GameObject.Find("Input Port").GetComponent<InputField>();
        // startOfflineButton = GameObject.Find("Start Offline").GetComponent<Button>();
        // startHostButton = GameObject.Find("Start Host").GetComponent<Button>();
        // startClientButton = GameObject.Find("Start Client").GetComponent<Button>();
        // inputIPField.onEndEdit.AddListener(GetInputIP);
        // inputPortField.onEndEdit.AddListener(GetInputPort);
        // startOfflineButton.onClick.AddListener(GameManager.Instance.OnStartOfflineBtnClick);
        // startHostButton.onClick.AddListener(GameManager.Instance.OnStartHostBtnClick);
        // startClientButton.onClick.AddListener(GameManager.Instance.OnStartClientBtnClick);
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
