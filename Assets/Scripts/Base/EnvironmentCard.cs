
using System.Linq;
using static Game;
using Unity.VisualScripting;
using UnityEngine;
using static GameManager;
using System.Collections.Generic;

public class EnvironmentCard : Card
{
    public override List<Collider2D> AIApplicableColliders => ColliderManager.LineColliders;
    public override bool IsApplicableFor(Collider2D collider)
    {
        if (AllyHero.totalPoint < cost) return false;
        if (collider.CompareTag("Line"))
        {
            Line line = collider.GetComponent<Line>();
            if (line.terrain == Line.Terrain.Plain)
            {
                return true;
            }
        }
        return false;
    }
    public override void Select()
    {
        base.Select();
        foreach (Line line in lines)
        {
            if (this.IsApplicableFor(line.lineCollider))
            {
                line.ShowApplicableLine();
            }
        }
    }
    public override void CencelSelect()
    {
        base.CencelSelect();
        foreach (Line line in lines)
        {
            line.HideApplicableLine();
        }
    }
}