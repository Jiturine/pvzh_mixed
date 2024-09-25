using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class BerryBlast : Card
{
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }
    public override void ApplyFor(Collider2D collider)
    {
        AllyHero.totalPoint -= cost;
        Line line = collider.GetComponentInParent<Line>();
        line.GetEntity(OpponentHero.faction)[0].TakeDamage(3, null);
        AllyHandCards.Remove(this);
    }
}
