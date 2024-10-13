using System;
using static GameManager;

public class SkyShooter : Entity
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        if (slot != null)
        {
            Line line = lines[slot.lineIndex];
            if (line.terrain == Line.LineTerrain.Highland)
            {
                Atk += 2;
                Health += 2;
            }
        }
    }
}
