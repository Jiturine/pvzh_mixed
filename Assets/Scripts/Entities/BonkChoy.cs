using static GameManager;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class BonkChoy : Entity
{
    public override void Place()
    {
        base.Place();
        Atk++;
        atkBuffing = true;
        OnTurnStartEvent += OnTurnStart;
    }
    public void OnTurnStart()
    {
        Atk--;
        atkBuffing = false;
        OnTurnStartEvent -= OnTurnStart;
    }
    public override void Exit()
    {
        base.Exit();
        if (atkBuffing)
        {
            OnTurnStartEvent -= OnTurnStart;
        }
    }
    private bool atkBuffing;
}
