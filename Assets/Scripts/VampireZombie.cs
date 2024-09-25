using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireZombie : Entity
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        abilities.Add(new NoCounterAttack());
        abilities.Add(new Vampirism(this));
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }
}
