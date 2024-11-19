using JetBrains.Annotations;
using UnityEngine;
using static Game;
using Unity.Netcode;

public class ApplyCardAction : GameAction
{
    Card card;
    Collider2D collider;
    public ApplyCardAction(Card card, Collider2D collider)
    {
        this.card = card;
        this.collider = collider;
        time = 0.5f;
    }
    public ApplyCardAction(params int[] args)
    {
        Faction faction = (Faction)args[0];
        int cardIndex = args[1];
        int colliderID = args[2];
        card = GameManager.Instance.GetHandCards(faction).cardList[cardIndex];
        collider = ColliderManager.colliders[colliderID];
        time = 0.5f;
    }
    public override void Apply()
    {
        if (!card.IsApplicableFor(collider)) return;
        card.ApplyFor(collider);
        
        if (card is EntityCard)
        {
            ActionSequence.actionSequence.AddFirst(new SwitchPhaseAction());
        }
    }
    public override int[] ToTransportArgs()
    {
        return new int[] { (int)card.faction, GameManager.Instance.GetHandCards(card.faction).cardList.IndexOf(card), ColliderManager.colliderID[collider] };
    }
}