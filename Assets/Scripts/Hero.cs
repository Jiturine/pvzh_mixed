using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class Hero : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        health = 20;
        maxHealth = 20;
        shield = 0;
        endTurn = false;
        healthUI = GetComponent<HealthUI>();
    }

    // Update is called once per frame
    void Update()
    {
        healthUI.healthText.text = health.ToString();
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health < 0)
        {
            health = 0;
        }
    }
    public int health;
    public int maxHealth;
    public int shield;
    public int totalPoint;
    public bool endTurn;
    public int turnOrder;
    public HealthUI healthUI;
    public Faction faction;
}

