using static GameManager;
using System.Linq;
using UnityEngine;
using static Game;
using System.Threading.Tasks;
using System.Collections.Generic;

public class Threepeater : Entity
{
    public override void Attack()
    {
        ReadyToAttack = false;
        SetAttackAnimation();
        Timer.Register(0.5f, () =>
    {
        List<Entity> allTargetEntities = new List<Entity>();
        int leftDamage = atk, rightDamage = atk, selfDamage = atk;
        if (slot.lineIndex > 0)
        {
            Slot leftSlot = lines[slot.lineIndex - 1].GetOpponentSlot(faction);
            if (!leftSlot.Empty)
            {
                allTargetEntities.Add(leftSlot.FrontEntity);
                DoDamage(leftSlot.FrontEntity, atk);
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
                allTargetEntities.Add(rightSlot.FrontEntity);
                DoDamage(rightSlot.FrontEntity, atk);
                rightDamage = 0;
            }
        }
        else
        {
            rightDamage = 0;
        }
        if (!slot.OpponentSlot.Empty)
        {
            Entity targetEntity = slot.OpponentSlot.FrontEntity;
            allTargetEntities.Add(targetEntity);
            DoDamage(targetEntity, atk);
            selfDamage = 0;
            if (targetEntity.counterAttackCount > 0 && !abilities.Contains<NoCounterAttack>())
            {
                targetEntity.CounterAttack();
            }
        }
        int heroDamage = leftDamage + rightDamage + selfDamage;
        if (heroDamage > 0)
        {
            if (!abilities.Contains<Bullseye>())
            {
                DoDamage(OpponentHero, atk, bullseye: false);
            }
            else
            {
                DoDamage(OpponentHero, atk, bullseye: true);
            }
            CardTracker.Instance.Add(new CardTracker.EntityAttackAction(this, OpponentHero, allTargetEntities.ToArray()));
        }
        else
        {
            CardTracker.Instance.Add(new CardTracker.EntityAttackAction(this, null, allTargetEntities.ToArray()));
        }
    });

    }
    public override void CounterAttack()
    {
        counterAttackCount--;
        SetAttackAnimation();
        Timer.Register(0.5f, () =>
    {
        List<Entity> allTargetEntities = new List<Entity>();
        int leftDamage = atk, rightDamage = atk, selfDamage = atk;
        if (slot.lineIndex > 0)
        {
            Slot leftSlot = lines[slot.lineIndex - 1].GetOpponentSlot(faction);
            if (!leftSlot.Empty)
            {
                allTargetEntities.Add(leftSlot.FrontEntity);
                DoDamage(leftSlot.FrontEntity, atk);
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
                allTargetEntities.Add(rightSlot.FrontEntity);
                DoDamage(rightSlot.FrontEntity, atk);
                rightDamage = 0;
            }
        }
        else
        {
            rightDamage = 0;
        }
        if (!slot.OpponentSlot.Empty)
        {
            Entity targetEntity = slot.OpponentSlot.FrontEntity;
            allTargetEntities.Add(targetEntity);
            DoDamage(targetEntity, atk);
            selfDamage = 0;
        }

        int heroDamage = leftDamage + rightDamage + selfDamage;
        if (heroDamage > 0)
        {
            if (!abilities.Contains<Bullseye>())
            {
                DoDamage(OpponentHero, atk, bullseye: false);
            }
            else
            {
                DoDamage(OpponentHero, atk, bullseye: true);
            }
            CardTracker.Instance.Add(new CardTracker.EntityAttackAction(this, OpponentHero, allTargetEntities.ToArray()));
        }
        else
        {
            CardTracker.Instance.Add(new CardTracker.EntityAttackAction(this, null, allTargetEntities.ToArray()));
        }
    });
    }
}
