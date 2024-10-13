using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;
using static GameManager;

public class SpriteManager
{
    static public Dictionary<int, Sprite> cardSprite;
    static public Dictionary<int, Sprite> environmentSprite;
    static public Sprite[] shieldSprite;
    static public Sprite gravestoneSprite;
    static public Sprite[] plantTooltipSprites;
    static public Sprite[] zombieTooltipSprites;
    static public Sprite plantHandCardsSprite;
    static public Sprite zombieHandCardsSprite;
    static public Dictionary<int, Sprite> abilityIcons;
    static public Sprite defaultAtkIcon;
    static public Sprite defaultHealthIcon;
    static public void Init()
    {
        cardSprite = new Dictionary<int, Sprite>();
        foreach (var kvp in CardDictionary.cardInfo)
        {
            cardSprite[kvp.Key] = Resources.Load<Sprite>("Sprites/CardSprites/" + kvp.Value.name);
        }
        shieldSprite = new Sprite[11];
        for (int i = 0; i <= 10; i++)
        {
            shieldSprite[i] = Resources.Load<Sprite>($"Sprites/shield/shield{i}");
        }
        abilityIcons = new Dictionary<int, Sprite>
        {
            [4] = Resources.Load<Sprite>("Sprites/AbilityIcons/precise"),
            [6] = Resources.Load<Sprite>("Sprites/AbilityIcons/armor")
        };
        defaultAtkIcon = Resources.Load<Sprite>("Sprites/atk");
        defaultHealthIcon = Resources.Load<Sprite>("Sprites/health");
        environmentSprite = new Dictionary<int, Sprite>
        {
            [10007] = Resources.Load<Sprite>("Sprites/Environments/spikeweed")
        };
        gravestoneSprite = Resources.Load<Sprite>("Sprites/Gravestone");
        plantTooltipSprites = Resources.LoadAll<Sprite>("Sprites/plant_tooltip");
        zombieTooltipSprites = Resources.LoadAll<Sprite>("Sprites/zombie_tooltip");
        plantHandCardsSprite = Resources.Load<Sprite>("Sprites/plant_handcards_background");
        zombieHandCardsSprite = Resources.Load<Sprite>("Sprites/zombie_handcards_background");
    }
    static public Sprite[] GetTooltipSprites(Faction faction)
    {
        if (faction == Faction.Plant)
        {
            return plantTooltipSprites;
        }
        else
        {
            return zombieTooltipSprites;
        }
    }
}

