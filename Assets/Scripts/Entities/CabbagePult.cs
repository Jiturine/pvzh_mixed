using System;
using static Game;

public class CabbagePult : Entity
{
    public override void Place()
    {
        base.Place();
        Line line = lines[slot.lineIndex];
        if (line.terrain == Line.Terrain.Highland)
        {
            Atk++;
            Health++;
        }
    }
}
