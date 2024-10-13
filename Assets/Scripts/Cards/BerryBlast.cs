using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static GameManager;

public class BerryBlast : StrategyCard
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
            return slot.GetEntity(collider) != null;
        }
        else return false;
    }
    public override void ApplyFor(Collider2D collider)
    {
        Slot slot = collider.GetComponentInParent<Slot>();
        slot.GetEntity(collider).TakeDamage(3);
        base.ApplyFor(collider);
    }
}
