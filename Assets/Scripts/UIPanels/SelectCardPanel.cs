using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;
using static Game;

public class SelectCardPanel : BasePanel
{
    public Button plantHeroButton;
    public Button zombieHeroButton;
    public Button startGameButton;
    public Button toggleReadyButton;
    public Image plantHeroIcon;
    public Image zombieHeroIcon;
    public Image cardLibraryBackground;
    public Image deckBackground;
    public Deck myDeck;
    public TextMeshProUGUI deckCounterText;
    public TextMeshProUGUI toggleReadyText;
    public TextMeshProUGUI joinCodeText;
    public TextMeshProUGUI myNameText;
    public TextMeshProUGUI enemyNameText;
    new void Start()
    {
        base.Start();
        Faction = GameManager.tempFaction;
        plantHeroButton.onClick.AddListener(GameManager.Instance.OnSwitchHeroBtnClick);
        plantHeroButton.onClick.AddListener(UIManager.Instance.PlayButtonClickSFX);
        zombieHeroButton.onClick.AddListener(GameManager.Instance.OnSwitchHeroBtnClick);
        zombieHeroButton.onClick.AddListener(UIManager.Instance.PlayButtonClickSFX);
        startGameButton.onClick.AddListener(GameManager.Instance.OnStartGameBtnClick);
        if (gameMode == GameMode.Online)
        {
            toggleReadyButton.onClick.AddListener(ToggleReady);
            if (!GameManager.Instance.IsServer)
            {
                startGameButton.gameObject.SetActive(false);
            }
        }
        else
        {
            toggleReadyButton.gameObject.SetActive(false);
        }
    }
    void Update()
    {
        if (Faction != GameManager.tempFaction)
        {
            Faction = GameManager.tempFaction;
        }
        deckCounterText.text = $"{myDeck.cardList.Count}/40";
        if (myDeck.cardList.Count != 40)
        {
            deckCounterText.color = Color.red;
        }
        else
        {
            deckCounterText.color = Color.black;
        }
        if (GameManager.Instance.IsServer)
        {
            myNameText.text = GameManager.hostName;
            enemyNameText.text = GameManager.clientName;
        }
        else
        {
            myNameText.text = GameManager.clientName;
            enemyNameText.text = GameManager.hostName;
        }
    }
    void ToggleReady()
    {
        UIManager.Instance.PlayButtonClickSFX();
        GameManager.Instance.OnToggleReadyBtnClick();
        if (GameManager.isReady)
        {
            toggleReadyText.text = "取消准备";
        }
        else
        {
            toggleReadyText.text = "准备";
        }
    }
    public Faction faction;
    public Faction Faction
    {
        get => faction;
        set
        {
            faction = value;
            if (faction == Faction.Plant)
            {
                plantHeroIcon.color = Color.white;
                zombieHeroIcon.color = Color.grey;
                cardLibraryBackground.sprite = SpriteManager.plantHandCardsSprite;
                deckBackground.sprite = SpriteManager.plantHandCardsSprite;
            }
            else
            {
                plantHeroIcon.color = Color.grey;
                zombieHeroIcon.color = Color.white;
                cardLibraryBackground.sprite = SpriteManager.zombieHandCardsSprite;
                deckBackground.sprite = SpriteManager.zombieHandCardsSprite;
            }
        }
    }
}
