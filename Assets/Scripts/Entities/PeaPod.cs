using System;
using static GameManager;

public class PeaPod : Entity
{
    public override void Place()
    {
        base.Place();
        OnTurnStartEvent += OnTurnStart;
    }
    public void OnTurnStart()
    {
        Atk++;
        Health++;
    }
    public override void Exit()
    {
        base.Exit();
        OnTurnStartEvent -= OnTurnStart;
    }
}
