using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeleteUserPanel : BasePanel
{
    public Button OK;
    public Button Cancel;
    public TextMeshProUGUI deleteUserName;
    new void Start()
    {
        base.Start();
        OK.onClick.AddListener(UIManager.Instance.PlayButtonClickSFX);
        OK.onClick.AddListener(DeleteUser);
        Cancel.onClick.AddListener(UIManager.Instance.PlayButtonClickSFX);
        Cancel.onClick.AddListener(UIManager.Instance.TogglePanel<DeleteUserPanel>);
    }
    public void SetDeleteInfo(string name)
    {
        deleteUserName.text = $"删除用户 {name}\n该操作不可撤销";
    }
    public void DeleteUser()
    {
        var switchUserPanel = UIManager.Instance.GetPanel<SwitchUserPanel>();
        UserManager.Instance.DeleteUser(switchUserPanel.currentUserName);
        switchUserPanel.RemoveUserItem(switchUserPanel.currentUserName);
        switchUserPanel.currentUserName = "";
        UIManager.Instance.TogglePanel<DeleteUserPanel>();
    }
}
