using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static GameManager;

public class FunDeadRaiser : StrategyCard
{
    new void Start()
    {
        base.Start();
        if (gameState == UIState.GamePlay)
        {
            AIApplicableColliders = ColliderManager.colliders.Select(kvp => kvp.Value).ToList();
        }
    }
    public override bool IsApplicableFor(Collider2D collider)
    {
        if (AllyHero.totalPoint < cost) return false;
        else return true;
    }
    public override void ApplyFor(Collider2D collider)
    {
        for (int i = 0; i < 2; i++)
            GameManager.Instance.GetHandCards(faction).DrawFrom(GameManager.Instance.GetDeck(faction));
        base.ApplyFor(collider);
    }
}
