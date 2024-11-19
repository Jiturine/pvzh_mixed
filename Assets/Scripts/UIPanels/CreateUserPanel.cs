using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateUserPanel : BasePanel
{
    public Button OK;
    public Button Cancel;
    public TMP_InputField inputField;
    new void Start()
    {
        base.Start();
        OK.onClick.AddListener(UIManager.Instance.PlayButtonClickSFX);
        OK.onClick.AddListener(SubmitName);
        Cancel.onClick.AddListener(UIManager.Instance.PlayButtonClickSFX);
        Cancel.onClick.AddListener(UIManager.Instance.TogglePanel<CreateUserPanel>);
    }
    public void SubmitName()
    {
        string name = inputField.text;
        UserManager.Instance.CreateUser(name);
        UserManager.Instance.SelectUser(name);
        var switchUserPanel = UIManager.Instance.GetPanel<SwitchUserPanel>();
        if (switchUserPanel != null)
        {
            switchUserPanel.AddUserItem(name);
        }
        inputField.text = "";
        UIManager.Instance.TogglePanel<CreateUserPanel>();
    }
}
