using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Game;
using static GameManager;

public class Position : Interactable
{
    void Start()
    {
        applicableIndicator.enabled = false;
    }
    public void ShowApplicablePositon()
    {
        applicableIndicator.enabled = true;
        applicableIndicator.color = Color.white;
    }
    public void HideApplicablePosition()
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
        if (Game.State is MyTurnState && SelectedCard != null && SelectedCard.IsApplicableFor(collider))
        {
            ActionSequence.AddAction(new ApplyCardAction(SelectedCard, collider));
        }
    }
    public Faction faction;
    public Entity entity;
    public new Collider2D collider;
    public Slot slot;
    public SpriteRenderer applicableIndicator;
}
