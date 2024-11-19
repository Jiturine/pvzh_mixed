using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static Game;
using UnityEngine;
using static GameManager;

public class TeamMascot : Entity
{
    public override void Place()
    {
        base.Place();
        OnTurnStartEvent += OnTurnStart;
    }
    public void OnTurnStart()
    {
        foreach (Entity entity in Game.GetEntities(faction))
        {
            if (entity.tags.Contains(Tag.Athlete))
            {
                entity.Atk++;
                entity.Health++;
            }
        }
    }
    public override void Exit()
    {
        base.Exit();
        OnTurnStartEvent -= OnTurnStart;
    }
}
