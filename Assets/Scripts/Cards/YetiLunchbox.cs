using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game;
using System.Linq;

public class YetiLunchbox : TrickCard
{
    public override List<Collider2D> AIApplicableColliders => ColliderManager.EnemyColliders;
    public override bool IsApplicableFor(Collider2D collider)
    {
        if (AllyHero.totalPoint < cost) return false;
        if (collider.CompareTag("Pos"))
        {
            Position pos = collider.GetComponent<Position>();
            if (pos.faction == faction)
            {
                return pos.entity != null;
            }
            else return false;
        }
        else return false;
    }
    public override void ApplyFor(Collider2D collider)
    {
        CardTracker.Instance.Add(new CardTracker.CardApplyAction(this, collider, CardTracker.CardApplyAction.TargetType.Entity));
        Slot slot = collider.GetComponentInParent<Slot>();
        slot.GetEntity(collider).Atk++;
        slot.GetEntity(collider).Health++;
        base.ApplyFor(collider);
    }
    public override void Select()
    {
        base.Select();
        foreach (var entity in Game.GetEntities(faction))
        {
            if (this.IsApplicableFor(entity.Pos.collider))
            {
                entity.ShowApplicableEntity();
            }
        }
    }
    public override void CencelSelect()
    {
        base.CencelSelect();
        foreach (var entity in Game.GetEntities(faction))
        {
            entity.HideApplicableEntity();
        }
    }
}
