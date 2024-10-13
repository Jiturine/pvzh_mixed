using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VampireZombie : Entity
{
    new void Start()
    {
        base.Start();
        if (slot != null)
        {
            if (!abilities.Any(ability => ability is NoCounterAttack))
            {
                abilities.Add(new NoCounterAttack());
            }
            if (!abilities.Any(ability => ability is Vampirism))
            {
                abilities.Add(new Vampirism(this));
            }
        }
    }
}
