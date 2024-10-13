using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WallNut : Entity
{
    new void Start()
    {
        base.Start();
        if (slot != null)
        {
            if (!abilities.Any(ability => ability is TeamUp))
            {
                abilities.Add(new TeamUp());
            }
        }
    }
}
