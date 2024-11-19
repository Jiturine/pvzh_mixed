using System.Collections;
using System.Collections.Generic;
using static Game;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HandCardsPanel : BasePanel
{
    new void Start()
    {
        base.Start();
        this.GetComponent<Canvas>().worldCamera = Camera.main;
        if (GameManager.tempFaction == Faction.Plant)
        {
            handCardsImage.sprite = SpriteManager.plantHandCardsSprite;
        }
        else
        {
            handCardsImage.sprite = SpriteManager.zombieHandCardsSprite;
        }
    }
    public HandCards myHandCards;
    public Image handCardsImage;
}
