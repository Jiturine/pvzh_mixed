using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UIManager();
            }
            return _instance;
        }
    }
    private Transform _uiRoot;
    public Transform UIRoot
    {
        get
        {
            if (_uiRoot == null)
            {
                if (GameObject.Find("Canvas"))
                {
                    _uiRoot = GameObject.Find("Canvas").transform;
                }
                else
                {
                    _uiRoot = new GameObject("Canvas").transform;
                }
            }
            return _uiRoot;
        }
    }
    public List<BasePanel> _existPanels;
    public List<BasePanel> ExistPanels
    {
        get
        {
            if (_existPanels == null)
            {
                _existPanels = new List<BasePanel>();
            }
            return _existPanels;
        }
    }
    private Dictionary<string, GameObject> _prefabDict;
    public Dictionary<string, GameObject> PrefabDict
    {
        get
        {
            if (_prefabDict == null)
            {
                _prefabDict = new Dictionary<string, GameObject>();
            }
            return _prefabDict;
        }
    }
    public T OpenPanel<T>() where T : BasePanel
    {
        //若已加载，控制显隐
        T panel = ExistPanels.Find(panel => panel is T) as T;
        if (panel != null)
        {
            if (panel.active)
            {
                Debug.LogError("界面已打开");
                return null;
            }
            else
            {
                panel.OpenPanel<T>();
                return panel;
            }
        }
        //若未加载，加载prefab
        string name = typeof(T).ToString();
        if (!PrefabDict.TryGetValue(name, out GameObject panelPrefab))
        {
            panelPrefab = Resources.Load<GameObject>("Prefabs/UIPanels/" + typeof(T).ToString());
            PrefabDict.Add(name, panelPrefab);
        }
        //加载界面
        panel = GameObject.Instantiate(panelPrefab, UIRoot, false).GetComponent<BasePanel>() as T;
        ExistPanels.Add(panel);
        panel.active = true;
        return panel;
    }
    public T TryOpenPanel<T>() where T : BasePanel
    {
        T panel = ExistPanels.Find(panel => panel is T) as T;
        if (panel != null)
        {
            if (!panel.active)
            {
                panel.OpenPanel<T>();
            }
            return panel;
        }
        //若未加载，加载prefab
        string name = typeof(T).ToString();
        if (!PrefabDict.TryGetValue(name, out GameObject panelPrefab))
        {
            panelPrefab = Resources.Load<GameObject>("Prefabs/UIPanels/" + typeof(T).ToString());
            PrefabDict.Add(name, panelPrefab);
        }
        //加载界面
        panel = GameObject.Instantiate(panelPrefab, UIRoot, false).GetComponent<BasePanel>() as T;
        ExistPanels.Add(panel);
        panel.active = true;
        return panel;
    }
    public void ClosePanel<T>() where T : BasePanel
    {
        BasePanel panel = ExistPanels.Find(panel => panel is T);
        if (panel == null)
        {
            Debug.LogError("界面未打开");
        }
        if (!panel.active)
        {
            Debug.LogError("界面未打开");
        }
        panel.ClosePanel<T>();
    }
    public bool TryClosePanel<T>() where T : BasePanel
    {
        BasePanel panel = ExistPanels.Find(panel => panel is T);
        if (panel == null)
        {
            return false;
        }
        if (!panel.active)
        {
            return false;
        }
        panel.ClosePanel<T>();
        return true;
    }
    public void TogglePanel<T>() where T : BasePanel
    {
        BasePanel panel = ExistPanels.Find(panel => panel is T);
        if (panel != null)
        {
            if (panel.active)
            {
                panel.ClosePanel<T>();
            }
            else
            {
                panel.OpenPanel<T>();
            }
        }
        else
        {
            string name = typeof(T).ToString();
            //未加载，加载prefab
            if (!PrefabDict.TryGetValue(name, out GameObject panelPrefab))
            {
                panelPrefab = Resources.Load<GameObject>("Prefabs/UIPanels/" + typeof(T).ToString());
                PrefabDict.Add(name, panelPrefab);
            }
            //加载界面
            panel = GameObject.Instantiate(panelPrefab, UIRoot, false).GetComponent<BasePanel>();
            ExistPanels.Add(panel);
            panel.active = true;
        }
    }
    public T GetPanel<T>() where T : BasePanel
    {
        T panel = ExistPanels.Find(panel => panel is T) as T;
        if (panel != null && panel.active)
        {
            return panel;
        }
        else
        {
            Debug.LogError("未找到面板");
            return null;
        }
    }
    public void PlayButtonClickSFX()
    {
        AudioManager.Instance.PlaySFX("ButtonClick");
    }
    public void PlayTapSFX()
    {
        AudioManager.Instance.PlaySFX("Tap1");
    }
}
