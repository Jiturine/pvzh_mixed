using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumoWrestlerCard : EntityCard
{
    new void Start()
    {
        base.Start();
        abilities = new List<Ability>(){
            new Gravestone()
        };
    }
}