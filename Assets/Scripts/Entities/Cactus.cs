using System.Collections.Generic;
using System.Linq;

public class Cactus : Entity
{
    new void Start()
    {
        base.Start();
        if (slot != null)
        {
            if (!abilities.Any(ability => ability is Bullseye))
            {
                abilities.Add(new Bullseye());
            }
        }
    }
}
