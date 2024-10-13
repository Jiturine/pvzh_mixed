using System;
using static GameManager;

public class PeaPod : Entity
{
    // Start is called before the first frame update
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
        Atk++;
        Health++;
    }
    public override void Die()
    {
        base.Die();
        OnTurnStartEvent -= OnTurnStart;
    }
}
