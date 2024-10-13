using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GameManager;

public class SpikeweedCard : EnvironmentCard
{
    public override void ApplyFor(Collider2D collider)
    {
        Line line = collider.GetComponent<Line>();
        if (line.TryGetComponent<Environment>(out var preEnvironment))
        {
            preEnvironment.Remove();
        }
        Environment environment = line.AddComponent<Spikeweed>();
        environment.lineIndex = line.index;
        environment.ID = ID;
        base.ApplyFor(collider);
    }
}
