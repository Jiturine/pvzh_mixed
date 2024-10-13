using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Conehead : Entity
{
    new void Start()
    {
        base.Start();
        if (slot != null)
        {
            if (!abilities.Any(ability => ability is Armored))
            {
                abilities.Add(new Armored(1));
            }
        }
    }
}
