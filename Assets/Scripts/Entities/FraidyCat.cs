using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game;

public class FraidyCat : Entity
{
    public override void Place()
    {
        base.Place();
        GameManager.OnApplyCardEvent += OnApplyCard;
    }
    public void OnApplyCard(Card card)
    {
        if (card.faction == OpponentHero.faction && card is TrickCard)
        {
            List<Line> lines = new List<Line>(Game.lines);
            lines.Shuffle();
            foreach (Line line in lines)
            {
                if (line == this.Line) continue;
                foreach (var collider in line.GetSlot(faction).Colliders)
                {
                    if (this.IsAbleToMoveTo(collider))
                    {
                        ActionSequence.actionSequence.AddFirst(new MoveAction(this, collider));
                        break;
                    }
                }
            }
        }
    }
    public override void Exit()
    {
        base.Exit();
        GameManager.OnApplyCardEvent -= OnApplyCard;
    }
}
