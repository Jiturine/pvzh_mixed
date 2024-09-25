using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class PeaShooter : Entity
{
    new void Start()
    {
        base.Start();
        faction = Faction.Plant;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }
}
