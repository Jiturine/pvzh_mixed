using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft;
using Newtonsoft.Json;
using Unity.Networking.Transport;

public class UserManager : PersistentSingleton<UserManager>
{
    void Start()
    {
        Debug.Log(Application.persistentDataPath);
        userInfoPath = Path.Combine(Application.persistentDataPath, "UserInfo.json");
        defaultUserNamePath = Path.Combine(Application.persistentDataPath, "DefaultUser.json");
        if (File.Exists(defaultUserNamePath))
        {
            string json = File.ReadAllText(defaultUserNamePath);
            defaultUserName = JsonConvert.DeserializeObject<string>(json);
        }
        else
        {
            File.WriteAllText(defaultUserNamePath, "");
            defaultUserName = "";
        }
        if (File.Exists(userInfoPath))
        {
            string json = File.ReadAllText(userInfoPath);
            userInfos = JsonConvert.DeserializeObject<List<UserInfo>>(json);
            if (userInfos == null) userInfos = new List<UserInfo>();
        }
        else
        {
            File.WriteAllText(userInfoPath, "");
            userInfos = new List<UserInfo>();
        }
        UserInfo defaultUserInfo = userInfos.Find(userInfo => userInfo.name == defaultUserName);
        if (defaultUserInfo == null)
        {
            defaultUserInfo = new UserInfo("Player");
            File.WriteAllText(defaultUserNamePath, "");
            UIManager.Instance.OpenPanel<CreateUserPanel>();
        }
        currentUserInfo = defaultUserInfo;
    }
    public List<UserInfo> userInfos;
    public class UserInfo
    {
        public string name;
        public UserInfo(string name)
        {
            this.name = name;
        }
    }
    public UserInfo currentUserInfo;
    public string defaultUserName;
    private string userInfoPath;
    private string defaultUserNamePath;
    public void CreateUser(string name)
    {
        UserInfo newUserInfo = new UserInfo(name);
        userInfos.Add(newUserInfo);
        string json = JsonConvert.SerializeObject(userInfos);
        File.WriteAllText(userInfoPath, json);
    }
    public void SelectUser(string name)
    {
        currentUserInfo = userInfos.Find(userInfo => userInfo.name == name);
        string json = JsonConvert.SerializeObject(currentUserInfo.name);
        File.WriteAllText(defaultUserNamePath, json);
    }
    public void DeleteUser(string name)
    {
        var userInfo = userInfos.Find(userInfo => userInfo.name == name);
        if (userInfo != null)
        {
            userInfos.Remove(userInfo);
        }
        if (name == defaultUserName)
        {
            File.WriteAllText(defaultUserNamePath, "");
            currentUserInfo = new UserInfo("Player");
        }
        string json = JsonConvert.SerializeObject(userInfos);
        File.WriteAllText(userInfoPath, json);
    }
    public void RenameUser(string name)
    {
        currentUserInfo.name = name;
        if (name == defaultUserName)
        {
            string defaultUserNameJson = JsonConvert.SerializeObject(name);
            File.WriteAllText(defaultUserNamePath, defaultUserNameJson);
        }
        string json = JsonConvert.SerializeObject(userInfos);
        File.WriteAllText(userInfoPath, json);
    }
}
