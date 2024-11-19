using System;
using static Game;

public class ChimneySweep : Entity
{
    public override void Place()
    {
        base.Place();
        if (Line.terrain == Line.Terrain.Highland)
        {
            Atk++;
            Health++;
        }
    }
}
