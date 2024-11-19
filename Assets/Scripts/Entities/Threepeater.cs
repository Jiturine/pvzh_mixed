using static GameManager;
using System.Linq;
using UnityEngine;
using static Game;
using System.Threading.Tasks;

public class Threepeater : Entity
{
    public override void Attack()
    {
        ReadyToAttack = false;
        SetAttackAnimation();
        if (!slot.OpponentSlot.Empty && slot.OpponentSlot.FrontEntity.abilities.Contains<Gravestone>(out var gravestone) && gravestone.outOfGrave == false)
        {
            return;
        }
        else
        {
            Timer.Register(0.5f, () =>
        {
            int leftDamage = atk, rightDamage = atk, selfDamage = atk;
            if (slot.lineIndex > 0)
            {
                Slot leftSlot = lines[slot.lineIndex - 1].GetOpponentSlot(faction);
                if (!leftSlot.Empty)
                {
                    Entity attackEntity = leftSlot.FrontEntity;
                    DoDamage(attackEntity, atk);
                    leftDamage = 0;
                }
            }
            else
            {
                leftDamage = 0;
            }
            if (slot.lineIndex < 4)
            {
                Slot rightSlot = lines[slot.lineIndex + 1].GetOpponentSlot(faction);
                if (!rightSlot.Empty)
                {
                    Entity attackEntity = rightSlot.FrontEntity;
                    DoDamage(attackEntity, atk);
                    rightDamage = 0;
                }
            }
            else
            {
                rightDamage = 0;
            }
            if (!slot.OpponentSlot.Empty)
            {
                Entity attackEntity = slot.OpponentSlot.FrontEntity;
                DoDamage(attackEntity, atk);
                selfDamage = 0;
                if (attackEntity.counterAttackCount > 0 && !abilities.Contains<NoCounterAttack>())
                {
                    attackEntity.CounterAttack();
                }

                int heroDamage = leftDamage + rightDamage + selfDamage;
                if (heroDamage > 0)
                {
                    if (!abilities.Contains<Bullseye>())
                    {
                        int increaseShield = Random.Range(1, 4);
                        if (increaseShield + OpponentHero.Shield >= 10)
                        {
                            OpponentHero.Shield = 0;
                        }
                        else
                        {
                            OpponentHero.Shield += increaseShield;
                            DoDamage(OpponentHero, heroDamage);
                        }
                    }
                    else
                    {
                        DoDamage(OpponentHero, heroDamage);
                    }
                }
            }
        });
        }
    }
    public override void CounterAttack()
    {
        if (abilities.Contains<Gravestone>(out var gravestone) && gravestone.outOfGrave == false) return;
        counterAttackCount--;
        SetAttackAnimation();
        Timer.Register(0.5f, () =>
    {
        int leftDamage = atk, rightDamage = atk, selfDamage = atk;
        if (slot.lineIndex > 0)
        {
            Slot leftSlot = lines[slot.lineIndex - 1].GetOpponentSlot(faction);
            if (!leftSlot.Empty)
            {
                Entity attackEntity = leftSlot.FrontEntity;
                DoDamage(attackEntity, atk);
                leftDamage = 0;
            }
        }
        else
        {
            leftDamage = 0;
        }
        if (slot.lineIndex < 4)
        {
            Slot rightSlot = lines[slot.lineIndex + 1].GetOpponentSlot(faction);
            if (!rightSlot.Empty)
            {
                Entity attackEntity = rightSlot.FrontEntity;
                DoDamage(attackEntity, atk);
                rightDamage = 0;
            }
        }
        else
        {
            rightDamage = 0;
        }
        if (!slot.OpponentSlot.Empty)
        {
            Entity attackEntity = slot.OpponentSlot.FrontEntity;
            DoDamage(attackEntity, atk);
            selfDamage = 0;
        }

        int heroDamage = leftDamage + rightDamage + selfDamage;
        if (heroDamage > 0)
        {
            if (!abilities.Contains<Bullseye>())
            {
                int increaseShield = Random.Range(1, 4);
                if (increaseShield + OpponentHero.Shield >= 10)
                {
                    OpponentHero.Shield = 0;
                }
                else
                {
                    OpponentHero.Shield += increaseShield;
                    DoDamage(OpponentHero, heroDamage);
                }
            }
            else
            {
                DoDamage(OpponentHero, heroDamage);
            }
        }
    });
    }
}
