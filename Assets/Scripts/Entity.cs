using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using static GameManager;
using Image = UnityEngine.UI.Image;

public class Entity : MonoBehaviour
{
    // Start is called before the first frame update
    protected void Start()
    {
        readyToAttack = false;
        sprite = GetComponent<SpriteRenderer>();
        healthRenderer = GetComponent<HealthRenderer>();
        atkRenderer = GetComponent<AtkRenderer>();
        abilities = new List<Ability>();
    }

    // Update is called once per frame
    protected void Update()
    {
        if (atk == 0)
        {
            atkRenderer.HideAtk();
        }
        else
        {
            atkRenderer.ShowAtk();
        }
        if (readyToAttack)
        {
            sprite.color = Color.green;
        }
        else
        {
            sprite.color = Color.white;
        }

        healthRenderer.healthText.text = health.ToString();
        atkRenderer.atkText.text = atk.ToString();
    }
    public void CounterAttack()
    {
        if (OpponentEntity != null)
        {
            OpponentEntity.TakeDamage(atk, this);
        }
    }
    public void Attack()
    {
        if (OpponentEntity != null)
        {
            int effectiveDamage = OpponentEntity.TakeDamage(atk, this);
            DoDamageEvent?.Invoke(effectiveDamage);
            if (OpponentEntity.counterAttackCount > 0 && !abilities.Any(ability => ability is NoCounterAttack))
            {
                OpponentEntity.counterAttackCount -= 1;
                OpponentEntity.CounterAttack();
            }
        }
        else
        {
            OpponentHero.TakeDamage(atk);
        }
        readyToAttack = false;
    }
    public int TakeDamage(int damage, Entity AtkSource)
    {
        if (Health >= damage)
        {
            Health -= damage;
            return damage;
        }
        else
        {
            Health = 0;
            return Health;
        }
    }
    public void Heal(int hp)
    {
        if (Health + hp > maxHealth)
        {
            Health = maxHealth;
        }
        else
        {
            Health += hp;
        }
    }
    public bool IsDying
    {
        get
        {
            return Health == 0;
        }
    }
    virtual public void Die()
    {
        line[lineIndex].RemoveEntity(this);
        Destroy(gameObject);
    }
    protected Entity OpponentEntity
    {
        get
        {
            if (faction == myHero.faction)
            {
                return line[lineIndex].enemyEntity.Count == 0 ? null : line[lineIndex].enemyEntity[0];
            }
            else
            {
                return line[lineIndex].myEntity.Count == 0 ? null : line[lineIndex].myEntity[0];
            }
        }
    }
    protected Hero OpponentHero
    {
        get
        {
            if (faction == myHero.faction)
            {
                return enemyHero;
            }
            else
            {
                return myHero;
            }
        }
    }
    protected Hero AllyHero
    {
        get
        {
            if (faction == myHero.faction)
            {
                return myHero;
            }
            else
            {
                return enemyHero;
            }
        }
    }
    public bool readyToAttack;
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
                healthRenderer.healthText.color = new Color(1.0f, 0.67f, 0.67f); // pink
            }
            else if (health == maxHealth)
            {
                healthRenderer.healthText.color = Color.white;
            }
            else if (health > maxHealth)
            {
                healthRenderer.healthText.color = new Color(0.67f, 1.0f, 0.67f); //green
            }
        }
    }
    public int atk;
    public int maxAtk;
    public int Atk
    {
        get { return atk; }
        set
        {
            atk = value;
            if (atk < maxAtk)
            {
                atkRenderer.atkText.color = new Color(1.0f, 0.67f, 0.67f); // pink
            }
            else if (health == maxHealth)
            {
                atkRenderer.atkText.color = Color.white;
            }
            else if (health > maxHealth)
            {
                atkRenderer.atkText.color = new Color(0.67f, 1.0f, 0.67f); //green
            }
        }
    }
    public int counterAttackCount;
    public HealthRenderer healthRenderer;
    public AtkRenderer atkRenderer;
    public Faction faction;
    public SpriteRenderer sprite;
    public int lineIndex;
    public List<Ability> abilities;
    public event DoDamageHandler DoDamageEvent;
    public delegate void DoDamageHandler(int effectiveDamage);
}
