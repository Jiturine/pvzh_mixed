using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static Game;
using Unity.VisualScripting;
using UnityEngine;
using static GameManager;

public class LocustSwarm : TrickCard
{
    public override List<Collider2D> AIApplicableColliders => ColliderManager.MyColliders;
    public override bool IsApplicableFor(Collider2D collider)
    {
        if (AllyHero.totalPoint < cost) return false;
        if (collider.CompareTag("Pos"))
        {
            Position pos = collider.GetComponent<Position>();
            if (pos.faction == faction) return false;
            if (pos.entity != null)
            {
                return true;
            }
            else return false;
        }
        else return false;
    }
    public override void ApplyFor(Collider2D collider)
    {
        CardTracker.Instance.Add(new CardTracker.CardApplyAction(this, collider, CardTracker.CardApplyAction.TargetType.Entity));
        Position pos = collider.GetComponent<Position>();
        pos.entity.Die();
        base.ApplyFor(collider);
    }
    public override void Select()
    {
        base.Select();
        foreach (var entity in Game.GetOpponentEntities(faction))
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
        foreach (var entity in Game.GetOpponentEntities(faction))
        {
            entity.HideApplicableEntity();
        }
    }
}

