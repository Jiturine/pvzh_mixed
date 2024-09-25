using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class BattleFieldSceneManager : MonoBehaviour
{
    public Text turnPhaseText;
    public Text myTotalPointText;
    public Text enemyTotalPointText;
    public Button endTurnButton;
    public Button backToMainMenuButton;
    // Start is called before the first frame update
    void Start()
    {
        turnPhaseText = GameObject.Find("Turn Phase").GetComponent<Text>();
        myTotalPointText = GameObject.Find("My Total Point Text").GetComponent<Text>();
        enemyTotalPointText = GameObject.Find("Enemy Total Point Text").GetComponent<Text>();
        endTurnButton = GameObject.Find("End Turn").GetComponent<Button>();
        backToMainMenuButton = GameObject.Find("Back To Main Menu").GetComponent<Button>();
        endTurnButton.onClick.AddListener(GameManager.Instance.OnEndTurnBtnClick);
        backToMainMenuButton.onClick.AddListener(GameManager.Instance.OnBackToMainMenuBtnClick);
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
        turnPhaseText.text = GameManager.turnPhase.ToString();
        myTotalPointText.text = GameManager.myHero.totalPoint.ToString();
        enemyTotalPointText.text = GameManager.enemyHero.totalPoint.ToString();
    }
    bool firstLoad;
}
