using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static GameManager;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

[Serializable]
public class Entity : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    protected void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthRenderer = GetComponent<HealthRenderer>();
        atkRenderer = GetComponent<AtkRenderer>();
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    protected void Start()
    {
        if (abilities == null) abilities = new List<Ability>();
        healthRenderer.healthText.text = Health.ToString();
        atkRenderer.atkText.text = Atk.ToString();
        if (Atk == 0 || abilities.Any(ability => ability is Gravestone gravestone && gravestone.outOfGrave == false))
        {
            atkRenderer.HideAtk();
        }
        else
        {
            atkRenderer.ShowAtk();
            counterAttackCount = 1;
        }
    }

    // Update is called once per frame
    protected void Update()
    {
        OnUpdate?.Invoke();
    }

    public void Battlecry()
    {
        BattlecryManager.Instance.isBattlecrying = true;
        BattlecryEvent?.Invoke();
    }

    virtual public void CounterAttack()
    {
        if (abilities.Any(ability => ability is Gravestone gravestone && gravestone.outOfGrave == false)) return;
        counterAttackCount--;
        SetAttackAnimation(true);
        Timer.Register(0.5f, () =>
    {
        if (!slot.OpponentSlot.Empty)
        {
            Entity attackEntity = slot.OpponentSlot.FrontEntity;
            DoDamage(attackEntity, atk);
        }
        else
        {
            if (!abilities.Any(ability => ability is Bullseye))
            {
                int increaseShield = Random.Range(1, 4);
                if (increaseShield + OpponentHero.Shield >= 10)
                {
                    OpponentHero.Shield = 0;
                }
                else
                {
                    OpponentHero.Shield += increaseShield;
                    DoDamage(OpponentHero, atk);
                }
            }
            else
            {
                DoDamage(OpponentHero, atk);
            }
        }
        SetAttackAnimation(false);
    });
    }
    virtual public void Attack()
    {
        ReadyToAttack = false;
        SetAttackAnimation(true);
        if (!slot.OpponentSlot.Empty && slot.OpponentSlot.FrontEntity.abilities.Any(ability => ability is Gravestone gravestone && gravestone.outOfGrave == false))
        {
            Timer.Register(0.5f, () =>
            {
                SetAttackAnimation(false);
            });
        }
        else
        {
            Timer.Register(0.5f, () =>
        {
            if (!slot.OpponentSlot.Empty)
            {
                Entity attackEntity = slot.OpponentSlot.FrontEntity;
                DoDamage(attackEntity, atk);
                if (attackEntity.counterAttackCount > 0 && !abilities.Any(ability => ability is NoCounterAttack))
                {
                    attackEntity.CounterAttack();
                }
            }
            else
            {
                if (!abilities.Any(ability => ability is Bullseye))
                {
                    int increaseShield = Random.Range(1, 4);
                    if (increaseShield + OpponentHero.Shield >= 10)
                    {
                        OpponentHero.Shield = 0;
                    }
                    else
                    {
                        OpponentHero.Shield += increaseShield;
                        DoDamage(OpponentHero, atk);
                    }
                }
                else
                {
                    DoDamage(OpponentHero, atk);
                }
            }
            SetAttackAnimation(false);
        });
        }
    }
    virtual public void DoDamage(Entity entity, int atk)
    {
        int effectiveDamage = entity.TakeDamage(atk);
        DoDamageEvent?.Invoke(effectiveDamage);
    }
    virtual public void DoDamage(Hero hero, int atk)
    {
        int effectiveDamage = hero.TakeDamage(atk);
        DoDamageEvent?.Invoke(effectiveDamage);
    }
    virtual public int TakeDamage(int damage)
    {
        if (abilities.Any(ability => ability is Gravestone gravestone && gravestone.outOfGrave == false)) return 0;
        if (abilities.Any(ability => ability is Armored))
        {
            Armored armored = (Armored)abilities.Find(ability => ability is Armored);
            damage -= armored.shield;
            if (damage < 0) damage = 0;
        }
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
    virtual public void Heal(int hp)
    {
        if (Health == 0) return;
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
        slot.RemoveEntity(this);
        Destroy(gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (abilities.Any(ability => ability is Gravestone gravestone && gravestone.outOfGrave == false))
        {
            return;
        }
        if (turnPhase == TurnPhase.MyTurn && faction == myHero.faction && ReadyToAttack)
        {
            if (gameMode == GameMode.Online)
            {
                int posID = (slot.FirstEntity == this) ? 0 : 1;
                GameManager.Instance.EntityAttackServerRpc(faction, slot.lineIndex, posID);
                GameManager.Instance.SwitchPhaseServerRpc();
            }
            else
            {
                Attack();
                GameManager.Instance.SwitchPhase();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Tooltip.Instance.ShowEntity(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.Instance.gameObject.SetActive(false);
    }

    protected void SetAttackAnimation(bool trigger)
    {
        if (trigger)
        {
            playingAnimationCounter++;
        }
        else
        {
            playingAnimationCounter--;
        }
        if (faction == myHero.faction)
        {
            animator.SetBool("AttackUp", trigger);
        }
        else
        {
            animator.SetBool("AttackDown", trigger);
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
    [HideInInspector] public bool readyToAttack;
    public bool ReadyToAttack
    {
        get { return readyToAttack; }
        set
        {
            readyToAttack = value;
            if (readyToAttack && !abilities.Any(ability => ability is Gravestone gravestone && gravestone.outOfGrave == false))
            {
                spriteRenderer.color = Color.green;
            }
            else
            {
                spriteRenderer.color = Color.white;
            }
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
            healthRenderer.healthText.text = health.ToString();
            if (health != 0) healthRenderer.HealthShake();
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
            else if (atk == maxAtk)
            {
                atkRenderer.atkText.color = Color.white;
            }
            else if (atk > maxAtk)
            {
                atkRenderer.atkText.color = new Color(0.67f, 1.0f, 0.67f); //green
            }
            if (atk == 0 || abilities.Any(ability => ability is Gravestone gravestone && gravestone.outOfGrave == false))
            {
                atkRenderer.HideAtk();
            }
            else
            {
                atkRenderer.ShowAtk();
            }
            atkRenderer.atkText.text = atk.ToString();
            atkRenderer.AtkShake();
        }
    }

    public int ID;

    [HideInInspector] public int counterAttackCount;
    [HideInInspector] public HealthRenderer healthRenderer;
    [HideInInspector] public AtkRenderer atkRenderer;
    [HideInInspector] public Animator animator;
    public Faction faction;
    public List<Tag> tags;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Slot slot;
    public List<Ability> abilities;
    public event DoDamageHandler DoDamageEvent;
    public delegate void DoDamageHandler(int effectiveDamage);
    public event BattlecryHandler BattlecryEvent;
    public delegate void BattlecryHandler();
    public event UpdateHandler OnUpdate;
    public delegate void UpdateHandler();
    public new string name;
}
