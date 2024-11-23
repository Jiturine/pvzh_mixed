using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using static Game;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static GameManager;
using Image = UnityEngine.UI.Image;
using TMPro;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    protected void Awake()
    {
        content = transform.Find("mask").transform.Find("content").GetComponent<Image>();
        animator = GetComponent<Animator>();
        costUI = GetComponent<CostUI>();
        cardCountText = transform.Find("countText").GetComponent<TextMeshProUGUI>();
    }
    protected void Start()
    {
        isSelectable = true;
        isSelected = false;
        if (location != Location.InCardLibrary)
        {
            cardCountText.enabled = false;
        }
    }
    protected void Update()
    {
        if (costUI != null)
        {
            costUI.costText.text = cost.ToString();
        }
        if (location == Location.InCardLibrary)
        {
            cardCountText.text = $"x {Count}";
            if (Count == 0)
            {
                content.color = Color.gray;
                isSelectable = false;
            }
            else if (!isSelectable)
            {
                content.color = Color.white;
                isSelectable = true;
            }
        }
        if (gameState == GameState.GamePlay)
        {
            content.color = isSelected ? Color.grey : Color.white;
        }
    }
    public enum Type
    {
        Entity,
        Trick,
        Environment
    }
    public enum Location
    {
        InCardLibrary,
        InDeck,
        InHandCards
    }
    public string className;
    public new string name;
    public Faction faction;
    public List<Tag> tags;
    public Location location;
    [HideInInspector] public bool isSelectable;
    virtual public bool IsApplicableFor(Collider2D collider)
    {
        if (AllyHero.totalPoint < cost) return false;
        return false;
    }
    virtual public void ApplyFor(Collider2D collider)
    {
        AllyHero.totalPoint -= cost;
        if (faction == enemyHero.faction && !(this is EntityCard entityCard && entityCard.abilities.Contains<Gravestone>()))
        {
            Card tempCard = Instantiate(gameObject, enemyHero.transform.position, Quaternion.identity, enemyHero.transform).GetComponent<Card>();
            tempCard.SetInfo();
            tempCard.animator.SetBool("Card Disappear", true);
            Destroy(tempCard.gameObject, 1.25f);
        }
        GameManager.Instance.OnApplyCard(this);
        AllyHandCards.Remove(this);
    }
    protected Hero OpponentHero => (faction == myHero.faction) ? enemyHero : myHero;
    protected Hero AllyHero => (faction == myHero.faction) ? myHero : enemyHero;
    protected HandCards AllyHandCards => (faction == myHero.faction) ? myHandCards : enemyHandCards;
    public int cost;
    public int ID;
    [HideInInspector] public bool isSelected;
    [HideInInspector] public Image content;
    [HideInInspector] public CostUI costUI;
    [HideInInspector] public Animator animator;
    [HideInInspector] public TextMeshProUGUI cardCountText;
    [HideInInspector] public bool needToWait;

    virtual public void OnBeginDrag(PointerEventData eventData)
    {
        if (location == Location.InHandCards)
        {
            SelectedCard = this;
        }
    }
    virtual public void OnDrag(PointerEventData eventData)
    {

    }
    virtual public void OnEndDrag(PointerEventData eventData)
    {

    }

    virtual public void OnPointerClick(PointerEventData eventData)
    {
        if ((location == Location.InCardLibrary) && isSelectable)
        {
            Count--;
            myDeck.Add(this.ID);
        }
        else if (location == Location.InDeck)
        {
            Count++;
            myDeck.Remove(this);
        }
        else if (location == Location.InHandCards)
        {
            if (SelectedCard != this)
            {
                SelectedCard = this;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        var tooltipPanel = UIManager.Instance.TryOpenPanel<TooltipPanel>();
        tooltipPanel.ShowCard(this);
        transform.localScale *= 1.1f;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale *= 10f / 11f;
        UIManager.Instance.TryClosePanel<TooltipPanel>();
    }
    virtual public void SetInfo()
    {
        string cardName = GetType().Name;
        string trueName;
        if (cardName.EndsWith("Card"))
        {
            trueName = cardName.Substring(0, cardName.Length - 4);
        }
        else
        {
            trueName = cardName;
        }
        ID = CardDictionary.cardID[trueName];
        faction = (ID / 10000 == 1) ? Faction.Plant : Faction.Zombie;
        name = CardDictionary.cardInfo[ID].name;
        tags = CardDictionary.cardInfo[ID].tags;
        cost = CardDictionary.cardInfo[ID].cost;
    }
    virtual public void Select()
    {
        isSelected = true;
    }
    virtual public void CencelSelect()
    {
        isSelected = false;
    }
    [HideInInspector] virtual public List<Collider2D> AIApplicableColliders { get; }
    public int Count
    {
        get => CardLibrary.cardCountDict[ID];
        set => CardLibrary.cardCountDict[ID] = value;
    }
}
