using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class CatLady : Entity
{
    public override void Place()
    {
        base.Place();
        increaseAtk = 0;
        GameManager.OnApplyCardEvent += OnApplyCard;
        GameManager.OnEndTurnEvent += OnEndTurn;
    }
    void OnApplyCard(Card card)
    {
        if (card.tags.Contains(Game.Tag.Pet))
        {
            //如果是创建自身的卡，不加攻击力
            if (card is EntityCard entityCard && entityCard.createdEntity == this)
            {
                return;
            }
            Atk += 3;
            //每轮首次加攻击力时得以攻击
            if (increaseAtk == 0)
            {
                ReadyToAttack = true;
                counterAttackCount = 1;
            }
            increaseAtk += 3;
        }
    }
    void OnEndTurn()
    {
        if (Atk < increaseAtk)
        {
            Atk = 0;
        }
        else
        {
            Atk -= increaseAtk;
        }
        increaseAtk = 0;
    }
    public override void Exit()
    {
        base.Exit();
        GameManager.OnApplyCardEvent -= OnApplyCard;
        GameManager.OnEndTurnEvent -= OnEndTurn;
    }
    private int increaseAtk;
}
