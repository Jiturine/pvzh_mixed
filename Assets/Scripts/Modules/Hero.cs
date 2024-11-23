using System.Collections;
using System.Collections.Generic;
using static Game;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class Hero : MonoBehaviour
{
    void Start()
    {
        healthUI = GetComponent<HealthUI>();
        health = 20;
        maxHealth = 20;
        Shield = 0;
        ShieldCount = 3;
        EndTurn = false;
        totalPoint = 1;
    }
    void Update()
    {
        healthUI.healthText.text = health.ToString();
    }
    public int TakeDamage(int damage, bool bullseye = false)
    {
        if (!bullseye && ShieldCount > 0)
        {
            int increaseShield = Random.Range(1, 4);
            if (increaseShield + Shield >= 10)
            {
                Shield = 0;
                ShieldCount--;
                return 0;
            }
            else
            {
                Shield += increaseShield;
                if (Health >= damage)
                {
                    Health -= damage;
                    return damage;
                }
                else
                {
                    int tempHP = Health;
                    Health = 0;
                    return tempHP;
                }
            }
        }
        else if (Health >= damage)
        {
            Health -= damage;
            return damage;
        }
        else
        {
            int tempHP = Health;
            Health = 0;
            return tempHP;
        }
    }
    public int health;
    public int maxHealth;
    public int Health
    {
        get { return health; }
        set
        {
            health = value;
            if (health < maxHealth)
            {
                healthUI.healthText.color = new Color(1.0f, 0.67f, 0.67f); // pink
            }
            else if (health == maxHealth)
            {
                healthUI.healthText.color = Color.white;
            }
            else if (health > maxHealth)
            {
                healthUI.healthText.color = new Color(0.67f, 1.0f, 0.67f); //green
            }
            healthUI.healthText.text = health.ToString();
        }
    }
    public int shield;
    public int Shield
    {
        get { return shield; }
        set
        {
            shield = value;
            if (faction == Faction.Plant)
            {
                shieldImage.sprite = SpriteManager.plantHeroShieldSprites[value];
            }
            else
            {
                shieldImage.sprite = SpriteManager.zombieHeroShieldSprites[value];
            }
        }
    }
    public int totalPoint;
    [HideInInspector] public bool endTurn;
    public bool EndTurn
    {
        get => endTurn;
        set
        {
            endTurn = value;
            var battleFieldPanel = UIManager.Instance.GetPanel<BattleFieldPanel>();
            if (faction == myHero.faction)
            {
                battleFieldPanel.myHeroEndTurnText.enabled = endTurn;
            }
            else
            {
                battleFieldPanel.enemyHeroEndTurnText.enabled = endTurn;
            }
        }
    }
    [HideInInspector] public int turnOrder;
    public HealthUI healthUI;
    public Image shieldImage;
    public Image[] smallShield;
    public int shieldCount;
    public int ShieldCount
    {
        get => shieldCount;
        set
        {
            shieldCount = value;
            if (shieldCount == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    smallShield[i].enabled = false;
                }
                shieldImage.enabled = false;
            }
            else if (faction == Faction.Plant)
            {
                for (int i = 0; i < 3 - shieldCount; i++)
                {
                    smallShield[i].sprite = SpriteManager.plantHeroEmptyShieldSprite;
                }
                for (int i = 3 - shieldCount; i < 3; i++)
                {
                    smallShield[i].sprite = SpriteManager.plantHeroExistShieldSprite;
                }
            }
            else
            {
                for (int i = 0; i < 3 - shieldCount; i++)
                {
                    smallShield[i].sprite = SpriteManager.zombieHeroEmptyShieldSprite;
                }
                for (int i = 3 - shieldCount; i < 3; i++)
                {
                    smallShield[i].sprite = SpriteManager.zombieHeroExistShieldSprite;
                }
            }
        }
    }
    public Faction faction;
}

