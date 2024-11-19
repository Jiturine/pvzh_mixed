using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : NetworkBehaviour
{
    public static SceneLoader Instance
    {
        get; set;
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // public IEnumerator LoadScene(string name)
    // {
    //     animator = GameObject.Find("Mask Image").GetComponent<Animator>();
    //     animator.SetBool("FadeIn", false);
    //     animator.SetBool("FadeOut", true);
    //     AsyncOperation async = SceneManager.LoadSceneAsync(name);
    //     async.allowSceneActivation = false;
    //     yield return new WaitForSeconds(1f);
    //     async.allowSceneActivation = true;
    //     async.completed += OnLoadedScene;
    // }
    // private void OnLoadedScene(AsyncOperation async)
    // {
    //     animator = GameObject.Find("Mask Image").GetComponent<Animator>();
    //     animator.SetBool("FadeOut", false);
    //     animator.SetBool("FadeIn", true);
    //     StopAllCoroutines();
    // }
    public async void LoadSceneAsync(string name)
    {
        readyClientNumber = 0;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);
        asyncLoad.allowSceneActivation = false; // 等待所有客户端加载完成后再激活场景
        bool first = true;
        // 等待场景加载进度
        while (!asyncLoad.isDone)
        {
            // 如果所有客户端加载完毕，激活场景
            if (asyncLoad.progress >= 0.9f)
            {
                if (first)
                {
                    if (!IsServer)
                    {
                        UpdateStateServerRpc();
                    }
                    else
                    {
                        readyClientNumber++;
                    }
                    first = false;
                }
                if (ableToContinue)
                {
                    asyncLoad.allowSceneActivation = true;
                }
                if (readyClientNumber >= 2 && IsServer)
                {
                    MessageClientRpc();
                }
            }
            await Task.Yield(); // 等待一帧
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void UpdateStateServerRpc()
    {
        readyClientNumber++;
    }
    [ClientRpc]
    public void MessageClientRpc()
    {
        ableToContinue = true;
    }
    public int readyClientNumber;
    public bool ableToContinue;
}
