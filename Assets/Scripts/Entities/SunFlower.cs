using static GameManager;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SunFlower : Entity
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        if (slot != null)
        {
            if (!abilities.Any(ability => ability is TeamUp))
            {
                abilities.Add(new TeamUp());
            }
            OnTurnStartEvent += OnTurnStart;
        }
    }
    public void OnTurnStart()
    {
        AllyHero.totalPoint++;
    }
    public override void Die()
    {
        base.Die();
        OnTurnStartEvent -= OnTurnStart;
    }
}
