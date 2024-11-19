using System.Collections;
using System.Collections.Generic;
using static Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleFieldSceneManager : MonoBehaviour
{
    public static BattleFieldSceneManager Instance
    {
        get; set;
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        UIManager.Instance.ExistPanels.Clear();
        var battleFieldPanel = UIManager.Instance.OpenPanel<BattleFieldPanel>();
        var handCardsPanel = UIManager.Instance.OpenPanel<HandCardsPanel>();
        GameManager.Instance.LoadBattleFieldScene(battleFieldPanel, handCardsPanel);
        AudioManager.Instance.PlayBGM("Tutorial Grasswalk");
    }
}
