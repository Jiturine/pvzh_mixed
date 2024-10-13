using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using static GameManager;

public class EntityCard : Card
{
    protected new void Start()
    {
        base.Start();
        abilities = new List<Ability>();
        healthUI = GetComponent<HealthUI>();
        atkUI = GetComponent<AtkUI>();
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
        if (gameState == UIState.GamePlay) AIApplicableColliders = ColliderManager.colliders.Where(kvp => kvp.Key / 100 == 2).Select(kvp => kvp.Value).ToList();
    }
    protected new void Update()
    {
        base.Update();
        if (selectedCard == this)
        {
            if (curEntity != null)
            {
                curEntity.transform.position = TranslateScreenToWorld(Input.mousePosition);
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
            if (atk == 0)
            {
                atkUI.HideAtk();
            }
            else
            {
                atkUI.ShowAtk();
            }
            atkUI.atkText.text = atk.ToString();
        }
    }
    [HideInInspector] public GameObject curEntity;
    virtual public Entity CreateEntity(Transform transform)
    {
        Entity entity = Instantiate(CardDictionary.entity[ID], transform.position, Quaternion.identity, transform).GetComponent<Entity>();
        entity.abilities = new List<Ability>();
        foreach (Ability ability in abilities)
        {
            entity.abilities.Add(ability);
            ability.SetEntity(entity);
        }
        entity.atk = atk;
        entity.health = health;
        entity.name = name;
        return entity;
    }
    public override bool IsApplicableFor(Collider2D collider)
    {
        if (AllyHero.totalPoint < cost) return false;
        if (collider.CompareTag("Slot"))
        {
            Slot slot = collider.GetComponentInParent<Slot>();
            if (lines[slot.lineIndex].terrain == Line.LineTerrain.Water && !abilities.Any(ability => ability is Amphibious))
            {
                return false;
            }
            if (slot.faction == faction)
            {
                if (!abilities.Any(ability => ability is TeamUp))
                {
                    if (slot.Empty)
                    {
                        if (collider == slot.firstCollider) return true;
                        else return false;
                    }
                    else if (slot.FirstEntity != null && slot.SecondEntity != null) return false;
                    else if (slot.FirstEntity != null)
                    {
                        if (slot.FirstEntity.abilities.Any(ability => ability is TeamUp)) return true;
                        else return false;
                    }
                    else
                    {
                        if (slot.SecondEntity.abilities.Any(ability => ability is TeamUp)) return true;
                        else return false;
                    }
                }
                else
                {
                    if (slot.FirstEntity != null && slot.SecondEntity != null) return false;
                    else if (slot.Empty && collider == slot.secondCollider) return false;
                    else return true;
                }
            }
        }
        return false;
    }
    public override void ApplyFor(Collider2D collider)
    {
        Slot slot = collider.GetComponentInParent<Slot>();
        Entity newEntity = CreateEntity(collider.transform);
        newEntity.slot = slot;
        if (slot.Empty)
        {
            slot.FirstEntity = newEntity;
        }
        else if (slot.FirstEntity != null)
        {
            if (collider == slot.firstCollider)
            {
                slot.SecondEntity = slot.FirstEntity;
                slot.SecondEntity.transform.SetParent(slot.secondCollider.transform, false);
                slot.FirstEntity = newEntity;
            }
            else
            {
                slot.SecondEntity = newEntity;
            }
        }
        else
        {
            if (collider == slot.firstCollider)
            {
                slot.FirstEntity = newEntity;
            }
            else
            {
                slot.FirstEntity = slot.SecondEntity;
                slot.FirstEntity.transform.SetParent(slot.firstCollider.transform, false);
                slot.SecondEntity = newEntity;
            }
        }
        base.ApplyFor(collider);
    }
    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (location == Location.InHandCards)
        {
            curEntity = Instantiate(CardDictionary.entity[ID]);
            curEntity.GetComponent<Animator>().enabled = false;
            curEntity.GetComponent<Collider2D>().enabled = false;
            curEntity.transform.position = TranslateScreenToWorld(eventData.position);
            if (selectedCard != null)
            {
                if (selectedCard is EntityCard entityCard)
                {
                    Destroy(entityCard.curEntity);
                }
                selectedCard.isSelected = false;
            }
            selectedCard = this;
            isSelected = true;
        }
    }
    public override void OnDrag(PointerEventData eventData)
    {
        if (location == Location.InHandCards)
        {
            if (curEntity == null)
            {
                return;
            }
            curEntity.transform.position = TranslateScreenToWorld(eventData.position);
        }
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (location == Location.InHandCards)
        {
            if (curEntity == null)
            {
                return;
            }
            if (turnPhase == TurnPhase.MyTurn)
            {
                Collider2D[] hitColliders = Physics2D.OverlapPointAll(TranslateScreenToWorld(eventData.position));
                foreach (var collider in hitColliders)
                {
                    if (IsApplicableFor(collider))
                    {
                        if (gameMode == GameMode.Online)
                        {
                            GameManager.Instance.ApplyCardServerRpc(myHero.faction, myHandCards.cardList.IndexOf(this), ColliderManager.colliderID[collider]);
                            if (!needToWait) GameManager.Instance.SwitchPhaseServerRpc();
                        }
                        else
                        {
                            ApplyFor(collider);
                            if (!needToWait) GameManager.Instance.SwitchPhase();
                        }
                    }
                }
            }
            selectedCard = null;
            isSelected = false;
            Destroy(curEntity);
        }
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        if ((location == Location.InCardLibrary) && isSelectable)
        {
            myDeck.Add(this);
        }
        else if (location == Location.InDeck)
        {
            myDeck.Remove(this);
        }
        else if (location == Location.InHandCards)
        {
            if (selectedCard != null && selectedCard is EntityCard entityCard)
            {
                Destroy(entityCard.curEntity);
            }
            if (selectedCard != this)
            {
                if (selectedCard != null)
                {
                    selectedCard.isSelected = false;
                }
                selectedCard = this;
                isSelected = true;
                curEntity = Instantiate(CardDictionary.entity[ID]);
                curEntity.GetComponent<Animator>().enabled = false;
                curEntity.GetComponent<Collider2D>().enabled = false;
                curEntity.transform.position = TranslateScreenToWorld(eventData.position);
            }
            else
            {
                selectedCard = null;
                isSelected = false;
            }
        }
    }
}
