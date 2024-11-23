using System.Collections;
using System.Collections.Generic;
using static Game;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleFieldPanel : BasePanel
{
    new void Start()
    {
        base.Start();
        endTurnButton.onClick.AddListener(UIManager.Instance.PlayButtonClickSFX);
        endTurnButton.onClick.AddListener(EndTurn);
        optionsButton.onClick.AddListener(UIManager.Instance.PlayButtonClickSFX);
        optionsButton.onClick.AddListener(UIManager.Instance.TogglePanel<OptionsPanel>);
        cardTrackerButton.onClick.AddListener(UIManager.Instance.PlayButtonClickSFX);
        cardTrackerButton.onClick.AddListener(UIManager.Instance.TogglePanel<CardTrackerPanel>);
        if (GameManager.tempFaction == Faction.Plant)
        {
            myTotalPointImage.sprite = SpriteManager.plantCostSprite;
            enemyTotalPointImage.sprite = SpriteManager.zombieCostSprite;
        }
        else
        {
            myTotalPointImage.sprite = SpriteManager.zombieCostSprite;
            enemyTotalPointImage.sprite = SpriteManager.plantCostSprite;
        }
    }
    void Update()
    {
        myTotalPointText.text = myHero.totalPoint.ToString();
        enemyTotalPointText.text = enemyHero.totalPoint.ToString();
        currentTurnText.text = $"当前回合：{currentTurn}";
    }
    public void EndTurn()
    {
        if (Game.State is MyTurnState)
        {
            ActionSequence.AddAction(new EndTurnAction(myHero.faction));
        }
    }
    public TextMeshProUGUI myTotalPointText;
    public TextMeshProUGUI enemyTotalPointText;
    public Image myTotalPointImage;
    public Image enemyTotalPointImage;
    public TextMeshProUGUI currentTurnText;
    public TextMeshProUGUI myHeroEndTurnText;
    public TextMeshProUGUI enemyHeroEndTurnText;
    public Hero myHero;
    public Hero enemyHero;
    public Deck myDeck;
    public Deck enemyDeck;
    public HandCards enemyHandCards;
    public Button endTurnButton;
    public Button optionsButton;
    public Button cardTrackerButton;
}
