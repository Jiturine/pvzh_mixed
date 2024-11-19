using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game;

public class Spikeweed : Environment
{
    new void Start()
    {
        base.Start();
        lines[lineIndex].OnEndTurnEvent += DoDamage;
    }
    public override void Remove()
    {
        base.Remove();
        lines[lineIndex].OnEndTurnEvent -= DoDamage;
    }
    public void DoDamage()
    {
        Slot slot = lines[lineIndex].GetOpponentSlot(faction);
        foreach (Entity entity in slot.Entities)
        {
            entity.TakeDamage(2);
        }
        GameManager.Instance.CheckDieEntity();
    }
}

