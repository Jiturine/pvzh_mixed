using System;
using static GameManager;

public class CabbagePult : Entity
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
                Atk++;
                Health++;
            }
        }
    }
}
