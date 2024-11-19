using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Game;

public class SelectCardPanel : BasePanel
{
    public Button plantHeroButton;
    public Button zombieHeroButton;
    public Button startGameButton;
    public Button getReadyButton;
    public Image plantHeroIcon;
    public Image zombieHeroIcon;
    public Deck myDeck;
    new void Start()
    {
        base.Start();
        plantHeroButton.onClick.AddListener(GameManager.Instance.OnSwitchHeroBtnClick);
        zombieHeroButton.onClick.AddListener(GameManager.Instance.OnSwitchHeroBtnClick);
        if (gameMode == GameMode.Online)
        {
            getReadyButton.onClick.AddListener(GameManager.Instance.OnGetReadyBtnClick);
        }
        else
        {
            getReadyButton.gameObject.SetActive(false);
        }
        startGameButton.onClick.AddListener(GameManager.Instance.OnStartGameBtnClick);

    }
    void Update()
    {
        if (GameManager.tempFaction == Faction.Plant)
        {
            plantHeroIcon.color = Color.white;
            zombieHeroIcon.color = Color.grey;
        }
        else
        {
            plantHeroIcon.color = Color.grey;
            zombieHeroIcon.color = Color.white;
        }
    }
}
