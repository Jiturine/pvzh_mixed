using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RenameUserPanel : BasePanel
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
        Cancel.onClick.AddListener(UIManager.Instance.TogglePanel<RenameUserPanel>);
    }
    public void SubmitName()
    {
        string name = inputField.text;
        if (name == "") return;
        var switchUserPanel = UIManager.Instance.GetPanel<SwitchUserPanel>();
        var userItem = switchUserPanel.userItemList.Find(userItem => userItem.userNameText.text == switchUserPanel.currentUserName);
        if (userItem != null)
        {
            userItem.userNameText.text = name;
        }
        switchUserPanel.currentUserName = name;
        UserManager.Instance.RenameUser(name);
        UIManager.Instance.TogglePanel<RenameUserPanel>();
    }
}
