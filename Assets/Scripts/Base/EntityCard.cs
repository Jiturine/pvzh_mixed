using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static Game;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using static GameManager;
using UnityEditor.Timeline;

public class EntityCard : Card
{
    protected new void Awake()
    {
        base.Awake();
        healthUI = GetComponent<HealthUI>();
        atkUI = GetComponent<AtkUI>();
    }
    protected new void Start()
    {
        base.Start();
        healthUI.healthText.text = Health.ToString();
        atkUI.atkText.text = Atk.ToString();
        if (Atk == 0)
        {
            atkUI.HideAtk();
        }
        else
        {
            atkUI.ShowAtk();
        }
    }
    public override List<Collider2D> AIApplicableColliders => ColliderManager.EnemyColliders;
    protected new void Update()
    {
        base.Update();
        if (SelectedCard == this)
        {
            if (curEntity != null)
            {
                curEntity.transform.position = Input.mousePosition.TranslateScreenToWorld();
            }
        }
    }

    [HideInInspector] public HealthUI healthUI;
    [HideInInspector] public AtkUI atkUI;
    public List<Ability> abilities;
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
                atkUI.atkText.color = new Color(1.0f, 0.67f, 0.67f); // pink
            }
            else if (health == maxHealth)
            {
                atkUI.atkText.color = Color.white;
            }
            else if (health > maxHealth)
            {
                atkUI.atkText.color = new Color(0.67f, 1.0f, 0.67f); //green
            }
            atkUI.atkText.text = atk.ToString();
            if (atk == 0)
            {
                atkUI.HideAtk();
            }
            else
            {
                atkUI.ShowAtk();
            }
        }
    }
    [HideInInspector] public GameObject curEntity;
    public override void SetInfo()
    {
        base.SetInfo();
        Health = CardDictionary.cardInfo[ID].health;
        Atk = CardDictionary.cardInfo[ID].atk;
        abilities = new List<Ability>();
        foreach (var abilityName in CardDictionary.cardInfo[ID].abilities)
        {
            Ability ability = System.Activator.CreateInstance(System.Type.GetType(abilityName)) as Ability;
            ability.SetCard(this);
            abilities.Add(ability);
        }
    }
    virtual public Entity CreateEntity(Transform transform)
    {
        Entity entity = Instantiate(CardDictionary.entity[ID], transform.position, Quaternion.identity, transform).GetComponent<Entity>();
        entity.SetInfo(this);
        return entity;
    }
    public override bool IsApplicableFor(Collider2D collider)
    {
        if (AllyHero.totalPoint < cost) return false;
        if (collider.CompareTag("Pos"))
        {
            Slot slot = collider.GetComponentInParent<Slot>();
            if (lines[slot.lineIndex].terrain == Line.Terrain.Water && !abilities.Contains<Amphibious>())
            {
                return false;
            }
            if (slot.Faction == faction)
            {
                if (!abilities.Contains<TeamUp>())
                {
                    if (slot.Empty)
                    {
                        if (collider == slot.FirstCollider) return true;
                        else return false;
                    }
                    else if (slot.FirstEntity != null && slot.SecondEntity != null) return false;
                    else if (slot.FirstEntity != null)
                    {
                        if (slot.FirstEntity.abilities.Contains<TeamUp>()) return true;
                        else return false;
                    }
                    else
                    {
                        if (slot.SecondEntity.abilities.Contains<TeamUp>()) return true;
                        else return false;
                    }
                }
                else
                {
                    if (slot.FirstEntity != null && slot.SecondEntity != null) return false;
                    else if (slot.Empty && collider == slot.SecondCollider) return false;
                    else return true;
                }
            }
        }
        return false;
    }
    public override void ApplyFor(Collider2D collider)
    {
        createdEntity = CreateEntity(collider.transform);
        GameManager.Instance.PlaceEntity(createdEntity, collider);
        base.ApplyFor(collider);
    }
    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (location == Location.InHandCards)
        {
            CreateTempEntity(eventData.position);
            SelectedCard = this;
        }
    }
    public override void OnDrag(PointerEventData eventData)
    {
        if (location == Location.InHandCards)
        {
            if (curEntity == null) return;
            curEntity.transform.position = eventData.position.TranslateScreenToWorld();
        }
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (location == Location.InHandCards)
        {
            if (curEntity != null) Destroy(curEntity);
        }
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        if ((location == Location.InCardLibrary) && isSelectable)
        {
            myDeck.Add(this.ID);
        }
        else if (location == Location.InDeck)
        {
            myDeck.Remove(this);
        }
        else if (location == Location.InHandCards)
        {
            if (SelectedCard != this)
            {
                SelectedCard = this;
                CreateTempEntity(eventData.position);
            }
        }
    }
    public override void Select()
    {
        base.Select();
        foreach (var position in Game.GetPositions(faction))
        {
            if (this.IsApplicableFor(position.collider))
            {
                position.ShowApplicablePositon();
            }
        }
    }
    public override void CencelSelect()
    {
        base.CencelSelect();
        foreach (var position in Game.GetPositions(faction))
        {
            position.HideApplicablePosition();
        }
    }
    private void CreateTempEntity(Vector3 position)
    {
        curEntity = Instantiate(CardDictionary.entity[ID]);
        Entity entity = curEntity.GetComponent<Entity>();
        entity.SetTempInfo(this);
        curEntity.GetComponent<Animator>().enabled = false;
        curEntity.GetComponent<Collider2D>().enabled = false;
        curEntity.transform.position = position.TranslateScreenToWorld();
    }
    public Entity createdEntity;
}
