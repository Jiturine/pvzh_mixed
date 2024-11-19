using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthenticationManager : PersistentSingleton<AuthenticationManager>
{
    void Start()
    {
        LoginAnonymously();
    }
    public async void LoginAnonymously()
    {
        await Authentication.Login();
    }
}