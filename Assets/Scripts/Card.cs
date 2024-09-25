using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;
using Image = UnityEngine.UI.Image;

public class Card : MonoBehaviour
{
    // Start is called before the first frame update
    protected void Start()
    {
        isSelectable = true;
        isSelected = false;
        entityPrefab = CardDictionary.card[ID];
        content = transform.Find("content").GetComponent<Image>();
        costUI = GetComponent<CostUI>();

    }
    // Update is called once per frame
    protected void Update()
    {
        if (costUI == null) return;
        if (isSelected)
        {
            content.color = Color.yellow;
        }
        else
        {
            content.color = Color.white;
        }
        costUI.costText.text = cost.ToString();
    }
    public Entity CreateEntity(Transform transform)
    {
        return Instantiate(CardDictionary.entity[ID], transform.position, Quaternion.identity, transform).GetComponent<Entity>();
    }
    public enum Tag
    {

    }
    public enum Type
    {
        Entity,
        Strategy,
        Environment
    }
    public enum Location
    {
        InCardLibrary,
        InDeck,
        InHandCards
    }
    public Faction faction;
    public List<Tag> tags;
    public Type type;
    public Location location;
    public bool isSelectable;
    virtual public bool IsApplicableFor(Collider2D collider)
    {
        if (AllyHero.totalPoint < selectedCard.cost) return false;
        if (collider.name != "myPos" && collider.name != "enemyPos") return false;
        if (type == Type.Entity)
        {
            Line line = collider.GetComponentInParent<Line>();
            return line.GetFaction(collider.name) == faction && line.GetEntity(collider.name).Count == 0;
        }
        if (type == Type.Strategy)
        {
            Line line = collider.GetComponentInParent<Line>();
            return line.GetFaction(collider.name) == OpponentHero.faction && line.GetEntity(collider.name).Count != 0;
        }
        return false;
    }
    virtual public void ApplyFor(Collider2D collider)
    {
        AllyHero.totalPoint -= cost;
        if (type == Type.Entity)
        {
            Line line = collider.GetComponentInParent<Line>();
            Entity newEntity = CreateEntity(collider.transform);
            Debug.Log(newEntity.transform.position);
            newEntity.lineIndex = line.number;
            line.GetEntity(faction).Add(newEntity);
        }
        AllyHandCards.Remove(this);
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
    protected HandCards AllyHandCards
    {
        get
        {
            if (faction == myHero.faction)
            {
                return myHandCards;
            }
            else
            {
                return enemyHandCards;
            }
        }
    }
    public int count;
    public int cost;
    public int ID;
    public bool isSelected;
    public GameObject entityPrefab;
    public Image content;
    public HealthUI healthUI;
    public AtkUI atkUI;
    public CostUI costUI;
    public void OnClick()
    {
        if ((location == Location.InCardLibrary) && isSelectable)
        {
            GameManager.myDeck.Add(this);
        }
        else if (location == Location.InDeck)
        {
            GameManager.myDeck.Remove(this);
        }
        else if (location == Location.InHandCards)
        {
            if (GameManager.selectedCard != null)
            {
                GameManager.selectedCard.isSelected = false;
            }
            isSelected = true;
            GameManager.selectedCard = this;
        }
    }
}
