using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GameManager;

public class TeamMascot : Entity
{
    new void Start()
    {
        base.Start();
        if (slot != null)
        {
            OnTurnStartEvent += OnTurnStart;
        }
    }
    public void OnTurnStart()
    {
        foreach (Line line in lines)
        {
            Slot slot = line.GetSlot(faction);
            for (int i = 0; i < 2; i++)
            {
                Entity entity = slot.entities[i];
                if (entity != null && entity.tags.Any(tag => tag == Tag.Athlete))
                {
                    entity.Atk++;
                    entity.Health++;
                }
            }
        }
    }
    public override void Die()
    {
        base.Die();
        OnTurnStartEvent -= OnTurnStart;
    }
}
