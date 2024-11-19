using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateLobbyPanel : BasePanel
{
    public Button OK;
    public Button cancel;
    public TMP_InputField inputField;
    new void Start()
    {
        base.Start();
        OK.onClick.AddListener(UIManager.Instance.PlayButtonClickSFX);
        OK.onClick.AddListener(CreateLobbyAsync);
        cancel.onClick.AddListener(UIManager.Instance.PlayButtonClickSFX);
        cancel.onClick.AddListener(UIManager.Instance.TogglePanel<CreateLobbyPanel>);
    }
    public async void CreateLobbyAsync()
    {
        string name = inputField.text;
        await LobbyManager.Instance.CreateLobbyAsync(name);
        inputField.text = "";
        UIManager.Instance.TogglePanel<CreateLobbyPanel>();
    }
}
