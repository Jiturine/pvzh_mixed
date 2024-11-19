using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static GameManager;
using static Game;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;
using System.Threading.Tasks;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;
using UnityEngine.UIElements;

public class Entity : Interactable
{
    protected void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthRenderer = GetComponent<HealthRenderer>();
        atkRenderer = GetComponent<AtkRenderer>();
        animator = GetComponent<Animator>();
        applicableIndicator = transform.Find("applicableIndicator").GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
    }
    protected void Start()
    {
        if (!abilities.Contains<Charge>())
        {
            ReadyToAttack = false;
        }
        if (Atk != 0)
        {
            counterAttackCount = 1;
        }
    }
    protected void Update()
    {
        OnUpdate?.Invoke();
    }
    virtual public void Place()
    {
        if (!abilities.Contains<Gravestone>() && HasBattlecry)
        {
            ActionSequence.AddAction(new BattlecryAction(this));
        }
    }

    virtual public void Battlecry() { }
    virtual public void CounterAttack()
    {
        if (abilities.Contains<Gravestone>(out var gravestone) && gravestone.outOfGrave == false) return;
        counterAttackCount--;
        SetAttackAnimation();
        Timer.Register(0.5f, () =>
    {
        if (!slot.OpponentSlot.Empty)
        {
            Entity attackEntity = slot.OpponentSlot.FrontEntity;
            DoDamage(attackEntity, atk);
        }
        else
        {
            if (!abilities.Contains<Bullseye>())
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
    });
    }
    virtual public void Attack()
    {
        ReadyToAttack = false;
        SetAttackAnimation();
        Debug.Log($" {slot.OpponentSlot == null} {slot.OpponentSlot.FrontEntity == null}");
        if (!slot.OpponentSlot.Empty && slot.OpponentSlot.FrontEntity.abilities.Contains<Gravestone>(out var gravestone) && gravestone.outOfGrave == false)
        {
            return;
        }
        else
        {
            Timer.Register(0.5f, () =>
        {
            if (!slot.OpponentSlot.Empty)
            {
                Entity attackEntity = slot.OpponentSlot.FrontEntity;
                DoDamage(attackEntity, atk);
                if (attackEntity.counterAttackCount > 0 && !abilities.Contains<NoCounterAttack>())
                {
                    attackEntity.CounterAttack();
                }
            }
            else
            {
                if (!abilities.Contains<Bullseye>())
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
        if (abilities.Contains<Gravestone>(out var gravestone) && gravestone.outOfGrave == false) return 0;
        if (abilities.Contains<Armored>(out var armored))
        {
            damage -= armored.shield;
            if (damage < 0) damage = 0;
        }
        if (Health >= damage)
        {
            Health -= damage;
            OnTakeDamageEvent?.Invoke(damage);
            return damage;
        }
        else
        {
            int tempHP = Health;
            Health = 0;
            OnTakeDamageEvent?.Invoke(tempHP);
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
    virtual public bool IsAbleToMoveTo(Collider2D collider)
    {
        Slot targetSlot = collider.GetComponentInParent<Slot>();
        //移到水路，要有两栖异能
        if (targetSlot.Line.terrain == Line.Terrain.Water && !this.abilities.Contains<Amphibious>())
        {
            return false;
        }
        //本格前后两单位交换
        if (targetSlot == this.slot)
        {
            return targetSlot.GetEntity(collider) != this && targetSlot.GetEntity(collider) != null;
        }
        //目标格空则只能移到后位
        if (targetSlot.Empty)
        {
            if (collider == targetSlot.FirstCollider) return true;
            else return false;
        }
        //目标格两位置都满则不能移
        else if (targetSlot.FirstEntity != null && targetSlot.SecondEntity != null) return false;
        //一位置空，有组队可以移
        else if (this.abilities.Contains<TeamUp>()) return true;
        //一位置空，无组队，看另一单位是否组队
        else
        {
            if (targetSlot.FirstEntity != null)
            {
                if (targetSlot.FirstEntity.abilities.Contains<TeamUp>()) return true;
                else return false;
            }
            else
            {
                if (targetSlot.SecondEntity.abilities.Contains<TeamUp>()) return true;
                else return false;
            }
        }
    }
    virtual public void MoveTo(Collider2D collider)
    {
        var originalPosition = transform.position;
        Slot targetSlot = collider.GetComponentInParent<Slot>();
        //本格前后两单位交换
        if (targetSlot == this.slot)
        {
            Entity anotherEntity = targetSlot.GetEntity(collider);
            anotherEntity.transform.SetParent(targetSlot.GetCollider(this).transform);
            this.transform.SetParent(collider.transform);
            (targetSlot.SecondEntity, targetSlot.FirstEntity) = (targetSlot.FirstEntity, targetSlot.SecondEntity);
        }
        else
        {
            if (targetSlot.Empty)
            {
                this.slot.RemoveEntity(this);
                this.slot = targetSlot;
                this.transform.SetParent(collider.transform);
                targetSlot.FirstEntity = this;
            }
            else if (targetSlot.FirstEntity != null)
            {
                if (collider == targetSlot.FirstCollider)
                {
                    targetSlot.SecondEntity = targetSlot.FirstEntity;
                    targetSlot.SecondEntity.transform.SetParent(targetSlot.SecondCollider.transform);
                    this.slot.RemoveEntity(this);
                    this.slot = targetSlot;
                    this.transform.SetParent(collider.transform);
                    targetSlot.FirstEntity = this;
                }
                else
                {
                    this.slot.RemoveEntity(this);
                    this.slot = targetSlot;
                    this.transform.SetParent(collider.transform);
                    targetSlot.SecondEntity = this;
                }
            }
            else
            {
                if (collider == targetSlot.SecondCollider)
                {
                    targetSlot.FirstEntity = targetSlot.SecondEntity;
                    targetSlot.FirstEntity.transform.SetParent(targetSlot.FirstCollider.transform);
                    this.slot.RemoveEntity(this);
                    this.slot = targetSlot;
                    this.transform.SetParent(collider.transform);
                    targetSlot.SecondEntity = this;
                }
                else
                {
                    this.slot.RemoveEntity(this);
                    this.slot = targetSlot;
                    this.transform.SetParent(collider.transform);
                    targetSlot.FirstEntity = this;
                }
            }
        }
        Debug.Log($"original position:{originalPosition} transform.position : {transform.position}");
        transform.position = originalPosition;
        StartCoroutine(MoveTowards(Pos.transform.position));
    }
    public bool IsDying => Health == 0;
    virtual public void Die()
    {
        Exit();
    }

    virtual public void Bounce()
    {
        Exit();
        GameManager.Instance.GetHandCards(faction).Add(ID);
    }
    virtual public void Exit()
    {
        slot.RemoveEntity(this);
        foreach (Ability ability in abilities)
        {
            ability.Remove();
        }
        Destroy(gameObject);
    }
    override public void OnPointerUp()
    {
        if (SelectedCard != null) return;
        if (abilities.Contains<Gravestone>(out var gravestone) && !gravestone.outOfGrave)
        {
            return;
        }
        if (Game.State is MyTurnState && faction == myHero.faction && ReadyToAttack)
        {
            ActionSequence.AddAction(new AttackAction(this));
        }
    }

    override public void OnPointerEnter()
    {
        var panel = UIManager.Instance.TryOpenPanel<TooltipPanel>();
        panel.ShowEntity(this);
        if (applicableIndicator.enabled)
        {
            applicableIndicator.color = Color.green;
        }
    }

    override public void OnPointerExit()
    {
        UIManager.Instance.TryClosePanel<TooltipPanel>();
        if (applicableIndicator.enabled)
        {
            applicableIndicator.color = Color.white;
        }
    }

    protected void SetAttackAnimation()
    {
        if (faction == myHero.faction)
        {
            animator.Play("AttackUp");
        }
        else
        {
            animator.Play("AttackDown");
        }
    }

    protected Hero OpponentHero => (faction == myHero.faction) ? enemyHero : myHero;
    protected Hero AllyHero => (faction == myHero.faction) ? myHero : enemyHero;
    [HideInInspector] public bool readyToAttack;
    public bool ReadyToAttack
    {
        get => readyToAttack;
        set
        {
            readyToAttack = value;
            if (readyToAttack && !(abilities.Contains<Gravestone>(out var gravestone) && gravestone.outOfGrave == false))
            {
                material.SetInt("_Enable", 1);
            }
            else
            {
                material.SetInt("_Enable", 0);
            }
        }
    }
    public int health;
    public int maxHealth;
    public int Health
    {
        get => health;
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
        get => atk;
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
            if (atk == 0)
            {
                atkRenderer.HideAtk();
            }
            else
            {
                atkRenderer.ShowAtk();
            }
            if (abilities.Contains<Gravestone>(out var gravestone) && gravestone.outOfGrave == false)
            {
                atkRenderer.HideAtk();
                healthRenderer.HideHealth();
            }
            atkRenderer.atkText.text = atk.ToString();
            if (atk != 0) atkRenderer.AtkShake();
        }
    }
    public void SetInfo()
    {
        ID = CardDictionary.entityID[GetType().Name];
        faction = (ID / 10000 == 1) ? Faction.Plant : Faction.Zombie;
        name = CardDictionary.cardInfo[ID].name;
        tags = CardDictionary.cardInfo[ID].tags;
        health = CardDictionary.cardInfo[ID].health;
        maxHealth = health;
        atk = CardDictionary.cardInfo[ID].atk;
        maxAtk = atk;
        healthRenderer.healthText.text = health.ToString();
        atkRenderer.atkText.text = atk.ToString();
        abilities = new List<Ability>();
        foreach (var abilityName in CardDictionary.cardInfo[ID].abilities)
        {
            abilities.Add(System.Activator.CreateInstance(System.Type.GetType(abilityName), this) as Ability);
        }
        if (atk == 0 || abilities.Contains<Gravestone>(out var gravestone) && gravestone.outOfGrave == false)
        {
            atkRenderer.HideAtk();
        }
    }
    public void SetInfo(EntityCard entityCard)
    {
        ID = entityCard.ID;
        faction = (ID / 10000 == 1) ? Faction.Plant : Faction.Zombie;
        name = entityCard.name;
        tags = entityCard.tags;
        health = entityCard.health;
        maxHealth = health;
        atk = entityCard.atk;
        maxAtk = atk;
        healthRenderer.healthText.text = health.ToString();
        atkRenderer.atkText.text = atk.ToString();
        abilities = entityCard.abilities;
        foreach (Ability ability in abilities)
        {
            ability.SetEntity(this);
        }
        if (atk == 0)
        {
            atkRenderer.HideAtk();
        }
    }
    public void SetTempInfo(EntityCard entityCard)
    {
        ID = entityCard.ID;
        faction = (ID / 10000 == 1) ? Faction.Plant : Faction.Zombie;
        name = entityCard.name;
        tags = entityCard.tags;
        health = entityCard.health;
        maxHealth = health;
        atk = entityCard.atk;
        maxAtk = atk;
        healthRenderer.healthText.text = health.ToString();
        atkRenderer.atkText.text = atk.ToString();
        abilities = entityCard.abilities;
        foreach (Ability ability in abilities)
        {
            ability.SetTempEntity(this);
        }
        if (atk == 0)
        {
            atkRenderer.HideAtk();
        }
    }

    private IEnumerator MoveTowards(Vector3 targetPosition)
    {
        while (transform.position != targetPosition)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, 8 * Time.deltaTime);
            yield return null;
        }
    }
    public void ShowApplicableEntity()
    {
        applicableIndicator.enabled = true;
        applicableIndicator.color = Color.white;
    }
    public void HideApplicableEntity()
    {
        applicableIndicator.enabled = false;
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
    public Position Pos => slot.GetPosition(this);
    public Line Line => slot.Line;
    public List<Ability> abilities;
    public event DoDamageHandler DoDamageEvent;
    public delegate void DoDamageHandler(int effectiveDamage);
    public event Action OnUpdate;
    public event Action<int> OnTakeDamageEvent;
    public string className;
    public new string name;
    virtual public bool HasBattlecry => false;
    public Material material;
    public SpriteRenderer applicableIndicator;
}
