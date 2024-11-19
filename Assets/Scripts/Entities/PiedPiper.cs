using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game;

public class PiedPiper : Entity
{
    public override bool HasBattlecry => true;
    override public void Battlecry()
    {
        foreach (var entity in slot.OpponentSlot.Entities)
        {
            entity.Atk--;
            entity.Health--;
        }
    }
}
