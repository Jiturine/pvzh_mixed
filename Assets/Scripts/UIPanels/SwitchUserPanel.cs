using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwitchUserPanel : BasePanel
{
    public Button Rename;
    public Button Delete;
    public Button Cancel;
    public Button OK;
    public Button CreateANewUser;
    public GameObject userItemPrefab;
    public List<UserItem> userItemList;
    public Transform userNameContent;
    [HideInInspector] public string currentUserName;
    new void Start()
    {
        base.Start();
        currentUserName = "";
        foreach (var userInfo in UserManager.Instance.userInfos)
        {
            GameObject newUserItemObj = Instantiate(userItemPrefab, userNameContent);
            newUserItemObj.GetComponentInChildren<TextMeshProUGUI>().text = userInfo.name;
            UserItem newUserItem = newUserItemObj.GetComponent<UserItem>();
            userItemList.Add(newUserItem);
            if (userInfo.name == UserManager.Instance.defaultUserName)
            {
                newUserItem.SelectUser();
            }
        }
        Rename.onClick.AddListener(UIManager.Instance.TogglePanel<RenameUserPanel>);
        Rename.onClick.AddListener(UIManager.Instance.PlayButtonClickSFX);
        Cancel.onClick.AddListener(UIManager.Instance.TogglePanel<SwitchUserPanel>);
        Cancel.onClick.AddListener(UIManager.Instance.PlayButtonClickSFX);
        OK.onClick.AddListener(Apply);
        OK.onClick.AddListener(UIManager.Instance.PlayButtonClickSFX);
        CreateANewUser.onClick.AddListener(UIManager.Instance.TogglePanel<CreateUserPanel>);
        CreateANewUser.onClick.AddListener(UIManager.Instance.PlayButtonClickSFX);
        Delete.onClick.AddListener(ConfirmDelete);
        Delete.onClick.AddListener(UIManager.Instance.PlayButtonClickSFX);
    }
    void Update()
    {
        if (currentUserName == "")
        {
            Rename.interactable = false;
            Delete.interactable = false;
        }
        else
        {
            Rename.interactable = true;
            Delete.interactable = true;
        }
    }
    public void AddUserItem(string name)
    {
        GameObject newUserItemObj = Instantiate(userItemPrefab, userNameContent);
        UserItem newUserItem = newUserItemObj.GetComponent<UserItem>();
        newUserItem.userNameText.text = name;
        userItemList.Add(newUserItem);
        newUserItem.SelectUser();
    }
    public void RemoveUserItem(string name)
    {
        UserItem userItem = userItemList.Find(userItem => userItem.userNameText.text == name);
        if (userItem != null)
        {
            userItemList.Remove(userItem);
            Destroy(userItem.gameObject);
        }
    }
    public void Apply()
    {
        Debug.Log(currentUserName + " " + UserManager.Instance.currentUserInfo.name);
        if (currentUserName != UserManager.Instance.currentUserInfo.name)
        {
            if (currentUserName == "")
            {
                UserManager.Instance.defaultUserName = "";
                UserManager.Instance.currentUserInfo = new UserManager.UserInfo("Player");
            }
            else
            {
                UserManager.Instance.defaultUserName = currentUserName;
                UserManager.Instance.SelectUser(currentUserName);
            }
        }
        UIManager.Instance.TogglePanel<SwitchUserPanel>();
    }
    public void ConfirmDelete()
    {
        var deleteUserPanel = UIManager.Instance.OpenPanel<DeleteUserPanel>();
        deleteUserPanel.SetDeleteInfo(currentUserName);
    }
}
