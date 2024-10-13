using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static GameManager;

public class RollingStone : StrategyCard
{
    new void Start()
    {
        base.Start();
        if (gameState == UIState.GamePlay)
        {
            AIApplicableColliders = ColliderManager.colliders.Where(kvp => kvp.Key / 100 == 1).Select(kvp => kvp.Value).ToList();
        }
    }
    new void Update()
    {
        base.Update();
    }
    public override bool IsApplicableFor(Collider2D collider)
    {
        if (AllyHero.totalPoint < cost) return false;
        if (collider.CompareTag("Slot"))
        {
            Slot slot = collider.GetComponentInParent<Slot>();
            if (slot.faction == faction) return false;
            Entity entity = slot.GetEntity(collider);
            if (entity != null)
            {
                if (entity.atk <= 2) return true;
                else return false;
            }
            else return false;
        }
        else return false;
    }
    public override void ApplyFor(Collider2D collider)
    {
        Slot slot = collider.GetComponentInParent<Slot>();
        slot.GetEntity(collider).Die();
        base.ApplyFor(collider);
    }
}

