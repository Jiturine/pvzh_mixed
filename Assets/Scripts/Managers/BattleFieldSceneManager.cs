using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleFieldSceneManager : MonoBehaviour
{
    private static BattleFieldSceneManager _instance;
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
    [HideInInspector] public TextMeshProUGUI turnPhaseText;

    [HideInInspector] public TextMeshProUGUI myTotalPointText;

    [HideInInspector] public TextMeshProUGUI enemyTotalPointText;
    [HideInInspector] public TextMeshProUGUI currentTurnText;

    [HideInInspector] public Button endTurnButton;

    [HideInInspector] public Button endGameButton;

    [HideInInspector] public Animator turnPhaseAnimator;
    [HideInInspector] public Image handCardsImage;
    // Start is called before the first frame update
    void Start()
    {
        turnPhaseText = GameObject.Find("Turn Phase Indicator").GetComponentInChildren<TextMeshProUGUI>();
        currentTurnText = GameObject.Find("Current Turn Text").GetComponent<TextMeshProUGUI>();
        turnPhaseAnimator = GameObject.Find("Turn Phase Indicator").GetComponent<Animator>();
        myTotalPointText = GameObject.Find("My Total Point Text").GetComponent<TextMeshProUGUI>();
        enemyTotalPointText = GameObject.Find("Enemy Total Point Text").GetComponent<TextMeshProUGUI>();
        handCardsImage = GameObject.Find("Hand Cards").GetComponent<Image>();
        endTurnButton = GameObject.Find("End Turn").GetComponent<Button>();
        endGameButton = GameObject.Find("End Game Button").GetComponent<Button>();
        endTurnButton.onClick.AddListener(GameManager.Instance.OnEndTurnBtnClick);
        endGameButton.onClick.AddListener(GameManager.Instance.OnEndGameMenuBtnClick);
        firstLoad = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.deckInfo.enemyCardID.Count == 0) { return; }
        if (firstLoad)
        {
            GameManager.Instance.LoadBattleFieldScene();
            if (GameManager.myHero.faction == GameManager.Faction.Plant)
            {
                GameObject.Find("My Total Point").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/plant_cost");
                GameObject.Find("Enemy Total Point").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/zombie_cost");
            }
            else
            {
                GameObject.Find("My Total Point").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/zombie_cost");
                GameObject.Find("Enemy Total Point").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/plant_cost");
            }
            firstLoad = false;
        }
        myTotalPointText.text = GameManager.myHero.totalPoint.ToString();
        enemyTotalPointText.text = GameManager.enemyHero.totalPoint.ToString();
    }
    public void ShowTurnPhase(string name)
    {
        turnPhaseText.text = name;
        turnPhaseAnimator.SetBool("Turn Phase Show", true);
        Timer.Register(1f, () =>
        {
            turnPhaseAnimator.SetBool("Turn Phase Show", false);
        });
    }
    bool firstLoad;
}
