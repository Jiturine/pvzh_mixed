using System;
using System.Collections;
using System.Collections.Generic;
using static Game;
using UnityEngine;
using UnityEngine.EventSystems;
using static GameManager;

public class Line : Interactable
{
    void Start()
    {
        applicableIndicator.enabled = false;
    }
    public Slot GetSlot(Faction faction) => (faction == myHero.faction) ? mySlot : enemySlot;
    public Slot GetOpponentSlot(Faction faction) => (faction == myHero.faction) ? enemySlot : mySlot;
    public void EndTurn()
    {
        OnEndTurnEvent?.Invoke();
        if (!mySlot.Empty)
        {
            if (mySlot.FirstEntity != null && mySlot.FirstEntity.Atk != 0)
            {
                mySlot.FirstEntity.ReadyToAttack = true;
                mySlot.FirstEntity.counterAttackCount = 1;
            }
            if (mySlot.SecondEntity != null && mySlot.SecondEntity.Atk != 0)
            {
                mySlot.SecondEntity.ReadyToAttack = true;
                mySlot.SecondEntity.counterAttackCount = 1;
            }
        }
        if (!enemySlot.Empty)
        {
            if (enemySlot.FirstEntity != null && enemySlot.FirstEntity.Atk != 0)
            {
                enemySlot.FirstEntity.ReadyToAttack = true;
                enemySlot.FirstEntity.counterAttackCount = 1;
            }
            if (enemySlot.SecondEntity != null && enemySlot.SecondEntity.Atk != 0)
            {
                enemySlot.SecondEntity.ReadyToAttack = true;
                enemySlot.SecondEntity.counterAttackCount = 1;
            }
        }
    }
    public void OnEntityEnter(Entity entity) { OnEntityEnterEvent?.Invoke(entity); }
    public void OnEntityLeave(Entity entity) { OnEntityLeaveEvent?.Invoke(entity); }

    public void ShowApplicableLine()
    {
        applicableIndicator.enabled = true;
        applicableIndicator.color = Color.white;
    }
    public void HideApplicableLine()
    {
        applicableIndicator.enabled = false;
    }

    override public void OnPointerEnter()
    {
        if (applicableIndicator.enabled)
        {
            applicableIndicator.color = Color.green;
        }
    }

    override public void OnPointerExit()
    {
        if (applicableIndicator.enabled)
        {
            applicableIndicator.color = Color.white;
        }
    }
    public override void OnPointerUp()
    {
        if (Game.State is MyTurnState && SelectedCard != null && SelectedCard.IsApplicableFor(lineCollider))
        {
            ActionSequence.AddAction(new ApplyCardAction(SelectedCard, lineCollider));
        }
    }

    public int index;
    public Slot mySlot;
    public Slot enemySlot;
    public BoxCollider2D lineCollider;

    public enum Terrain
    {
        Highland,
        Plain,
        Water
    }
    public Terrain terrain;
    public Environment environment;
    public SpriteRenderer applicableIndicator;
    public event Action OnEndTurnEvent;
    public event Action<Entity> OnEntityEnterEvent;
    public event Action<Entity> OnEntityLeaveEvent;
}
