using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieYeti : Entity
{

    public override bool HasBattlecry => true;
    public override void Place()
    {
        base.Place();
        OnTakeDamageEvent += OnTakeDamage;
    }
    public override void Battlecry()
    {
        GameManager.Instance.GetHandCards(faction).Add(20016); //增加一张雪人午餐盒
    }
    void OnTakeDamage(int damage)
    {
        if (Health != 0)
        {
            Bounce();
        }
    }
    public override void Exit()
    {
        base.Exit();
        OnTakeDamageEvent -= OnTakeDamage;
    }
}
