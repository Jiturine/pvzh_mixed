using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static GameManager;
using Image = UnityEngine.UI.Image;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    protected void Start()
    {
        isSelectable = true;
        isSelected = false;
        content = transform.Find("mask").transform.Find("content").GetComponent<Image>();
        animator = GetComponent<Animator>();
        costUI = GetComponent<CostUI>();
    }
    // Update is called once per frame
    protected void Update()
    {
        if (costUI == null) return;
        if (isSelected)
        {
            content.color = Color.grey;
        }
        else
        {
            content.color = Color.white;
        }
        costUI.costText.text = cost.ToString();
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
    public new string name;
    public Faction faction;
    public List<Tag> tags;
    [HideInInspector] public Location location;
    [HideInInspector] public bool isSelectable;
    virtual public bool IsApplicableFor(Collider2D collider)
    {
        if (AllyHero.totalPoint < cost) return false;
        return false;
    }
    virtual public void ApplyFor(Collider2D collider)
    {
        AllyHero.totalPoint -= cost;
        if (faction == enemyHero.faction)
        {
            Card tempCard = Instantiate(gameObject, enemyHero.transform.position, Quaternion.identity, enemyHero.transform).GetComponent<Card>();
            tempCard.animator.SetBool("Card Disappear", true);
            Timer.Register(0.5f, () =>
        {
            Destroy(tempCard.gameObject);
        });
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
    [HideInInspector] public int count;
    public int cost;
    public int ID;
    [HideInInspector] public bool isSelected;
    [HideInInspector] public Image content;
    [HideInInspector] public CostUI costUI;
    [HideInInspector] public Animator animator;
    [HideInInspector] public bool needToWait;

    public static Vector3 TranslateScreenToWorld(Vector3 position)
    {
        Vector3 cameraTranslatePos = Camera.main.ScreenToWorldPoint(position);
        return new Vector3(cameraTranslatePos.x, cameraTranslatePos.y, 0);
    }
    virtual public void OnBeginDrag(PointerEventData eventData)
    {
        if (location == Location.InHandCards)
        {
            if (selectedCard != null)
            {
                selectedCard.isSelected = false;
            }
            selectedCard = this;
            isSelected = true;
        }
    }
    virtual public void OnDrag(PointerEventData eventData)
    {
        if (location == Location.InHandCards)
        {
        }
    }
    virtual public void OnEndDrag(PointerEventData eventData)
    {
        if (location == Location.InHandCards)
        {
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
                            if (this is EntityCard)
                            {
                                if (!needToWait) GameManager.Instance.SwitchPhaseServerRpc();
                            }
                        }
                        else
                        {
                            ApplyFor(collider);
                            if (this is EntityCard)
                            {
                                if (!needToWait) GameManager.Instance.SwitchPhase();
                            }
                        }
                    }
                }
            }
            selectedCard = null;
            isSelected = false;
        }
    }

    virtual public void OnPointerClick(PointerEventData eventData)
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
            }
            else
            {
                selectedCard = null;
                isSelected = false;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Tooltip.Instance.ShowCard(this);
        transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
        Tooltip.Instance.gameObject.SetActive(false);
    }
    [HideInInspector] public List<Collider2D> AIApplicableColliders;
}
