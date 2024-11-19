using System.Collections;
using System.Collections.Generic;
using static Game;
using UnityEngine;
using UnityEngine.TextCore;
using static GameManager;

public class SpriteManager
{
    static public Dictionary<int, Sprite> cardSprite;
    static public Dictionary<int, Sprite> environmentSprite;
    static public Sprite[] gravestoneSprites;
    static public Sprite[] plantTooltipSprites;
    static public Sprite[] zombieTooltipSprites;
    static public Sprite[] plantHeroShieldSprites;
    static public Sprite[] zombieHeroShieldSprites;
    static public Sprite plantHeroEmptyShieldSprite;
    static public Sprite plantHeroExistShieldSprite;
    static public Sprite zombieHeroEmptyShieldSprite;
    static public Sprite zombieHeroExistShieldSprite;
    static public Sprite plantHandCardsSprite;
    static public Sprite zombieHandCardsSprite;
    static public Sprite plantCostSprite;
    static public Sprite zombieCostSprite;
    static public Dictionary<int, Sprite> abilityIcons;
    static public Sprite defaultAtkIcon;
    static public Sprite defaultHealthIcon;
    [RuntimeInitializeOnLoadMethod]
    static public void Init()
    {
        cardSprite = new Dictionary<int, Sprite>();
        foreach (var kvp in CardDictionary.cardInfo)
        {
            cardSprite[kvp.Key] = Resources.Load<Sprite>("Sprites/CardSprites/" + kvp.Value.className);
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
        gravestoneSprites = Resources.LoadAll<Sprite>("Sprites/Gravestone/Gravestones");
        plantTooltipSprites = Resources.LoadAll<Sprite>("Sprites/plant_tooltip");
        zombieTooltipSprites = Resources.LoadAll<Sprite>("Sprites/zombie_tooltip");
        plantCostSprite = Resources.Load<Sprite>("Sprites/Base/plant_cost");
        zombieCostSprite = Resources.Load<Sprite>("Sprites/Base/zombie_cost");
        plantHandCardsSprite = Resources.Load<Sprite>("Sprites/plant_handcards_background");
        zombieHandCardsSprite = Resources.Load<Sprite>("Sprites/zombie_handcards_background");
        plantHeroEmptyShieldSprite = Resources.Load<Sprite>("Sprites/shield/plant_shield/empty_shield");
        plantHeroExistShieldSprite = Resources.Load<Sprite>("Sprites/shield/plant_shield/exist_shield");
        zombieHeroEmptyShieldSprite = Resources.Load<Sprite>("Sprites/shield/zombie_shield/empty_shield");
        zombieHeroExistShieldSprite = Resources.Load<Sprite>("Sprites/shield/zombie_shield/exist_shield");
        plantHeroShieldSprites = new Sprite[11];
        zombieHeroShieldSprites = new Sprite[11];
        for (int i = 0; i < 11; i++)
        {
            plantHeroShieldSprites[i] = Resources.Load<Sprite>($"Sprites/shield/plant_shield/plant_shield_{i}");
            zombieHeroShieldSprites[i] = Resources.Load<Sprite>($"Sprites/shield/zombie_shield/zombie_shield_{i}");
        }
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

