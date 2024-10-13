using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CattailCard : EntityCard
{
    new void Start()
    {
        base.Start();
        abilities = new List<Ability>(){
            new Amphibious()
        };
    }
}
