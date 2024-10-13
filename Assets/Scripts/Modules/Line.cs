using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static GameManager;

public class Line : MonoBehaviour, IPointerClickHandler
{
    void Awake()
    {
        mySlot = transform.Find("My Slot").GetComponent<Slot>();
        enemySlot = transform.Find("Enemy Slot").GetComponent<Slot>();
        lineCollider = GetComponent<BoxCollider2D>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnPointerClick(PointerEventData eventData)
    {
    }
    public Slot GetSlot(Faction faction)
    {
        if (faction == myHero.faction)
        {
            return mySlot;
        }
        else
        {
            return enemySlot;
        }
    }
    public Slot GetOpponentSlot(Faction faction)
    {
        if (faction == myHero.faction)
        {
            return enemySlot;
        }
        else
        {
            return mySlot;
        }
    }
    public void EndTurn() { OnEndTurnEvent?.Invoke(); }
    public void OnEntityEnter(Entity entity) { OnEntityEnterEvent?.Invoke(entity); }
    public void OnEntityLeave(Entity entity) { OnEntityLeaveEvent?.Invoke(entity); }

    public int index;
    public Slot mySlot;
    public Slot enemySlot;
    public BoxCollider2D lineCollider;

    public enum LineTerrain
    {
        Highland,
        Plain,
        Water
    }
    public LineTerrain terrain;
    public Environment environment;
    public delegate void EndTurnHandler();
    public event EndTurnHandler OnEndTurnEvent;
    public delegate void EntityEnterHandler(Entity entity);
    public event EntityEnterHandler OnEntityEnterEvent;
    public delegate void EntityLeaveHandler(Entity entity);
    public event EntityEnterHandler OnEntityLeaveEvent;
}
