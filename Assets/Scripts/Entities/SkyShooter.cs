using System;
using static Game;

public class SkyShooter : Entity
{
    public override void Place()
    {
        base.Place();
        if (Line.terrain == Line.Terrain.Highland)
        {
            Atk += 2;
            Health += 2;
        }
    }
}
