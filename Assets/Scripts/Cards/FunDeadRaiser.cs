using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static Game;
using Unity.VisualScripting;
using UnityEngine;
using static GameManager;

public class FunDeadRaiser : TrickCard
{
    public override List<Collider2D> AIApplicableColliders => ColliderManager.AllColliders;
    public override bool IsApplicableFor(Collider2D collider)
    {
        if (AllyHero.totalPoint < cost) return false;
        else return true;
    }
    public override void ApplyFor(Collider2D collider)
    {
        CardTracker.Instance.Add(new CardTracker.CardApplyAction(this, collider, CardTracker.CardApplyAction.TargetType.Any));
        for (int i = 0; i < 2; i++)
        {
            ActionSequence.actionSequence.AddFirst(new DrawCardAction(faction));
        }
        base.ApplyFor(collider);
    }
}
