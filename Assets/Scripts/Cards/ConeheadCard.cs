using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeheadCard : EntityCard
{
    new void Start()
    {
        base.Start();
        abilities = new List<Ability>(){
            new Armored(this, 1)
        };
    }
}
