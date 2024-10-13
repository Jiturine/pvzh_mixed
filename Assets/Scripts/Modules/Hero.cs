using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class Hero : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        healthUI = GetComponent<HealthUI>();
        shieldImage = transform.Find("shieldImage").GetComponent<Image>();
        health = 20;
        maxHealth = 20;
        Shield = 0;
        endTurn = false;
    }

    // Update is called once per frame
    void Update()
    {
        healthUI.healthText.text = health.ToString();
    }
    public int TakeDamage(int damage)
    {
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
            shieldImage.sprite = SpriteManager.shieldSprite[shield];
        }
    }
    public int totalPoint;
    public bool endTurn;
    public int turnOrder;
    public HealthUI healthUI;
    public Image shieldImage;
    public Faction faction;
}

