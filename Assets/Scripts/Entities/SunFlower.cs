using static GameManager;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SunFlower : Entity
{
    public override void Place()
    {
        base.Place();
        OnTurnStartEvent += OnTurnStart;
    }
    public void OnTurnStart()
    {
        AllyHero.totalPoint++;
    }
    public override void Exit()
    {
        base.Exit();
        OnTurnStartEvent -= OnTurnStart;
    }
}
