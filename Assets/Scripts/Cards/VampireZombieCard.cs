using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireZombieCard : EntityCard
{
    new void Start()
    {
        base.Start();
        abilities = new List<Ability>(){
            new NoCounterAttack(),
            new Vampirism()
        };
    }
}
