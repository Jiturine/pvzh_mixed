using System.Linq;
using static Game;
using UnityEngine;
using static GameManager;
using System.Collections.Generic;
public class VitaminZ : TrickCard
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
        slot.GetEntity(collider).Atk += 3;
        slot.GetEntity(collider).Health += 3;
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