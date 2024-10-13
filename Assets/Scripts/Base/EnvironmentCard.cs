
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static GameManager;

public class EnvironmentCard : Card
{
    new void Start()
    {
        base.Start();
        if (gameState == UIState.GamePlay) AIApplicableColliders = ColliderManager.colliders.Where(kvp => kvp.Key / 100 == 3).Select(kvp => kvp.Value).ToList();
    }
    public override bool IsApplicableFor(Collider2D collider)
    {
        if (AllyHero.totalPoint < cost) return false;
        if (collider.CompareTag("Line"))
        {
            Line line = collider.GetComponent<Line>();
            if (line.terrain == Line.LineTerrain.Plain)
            {
                return true;
            }
        }
        return false;
    }
    public override void ApplyFor(Collider2D collider)
    {
        base.ApplyFor(collider);
    }
}