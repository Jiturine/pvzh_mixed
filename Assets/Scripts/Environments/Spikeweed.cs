using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class Spikeweed : Environment
{
    // Start is called before the first frame update
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
        if (!slot.Empty)
        {
            foreach (Entity entity in slot.entities)
            {
                if (entity != null)
                {
                    entity.TakeDamage(2);
                }
            }
            GameManager.Instance.CheckDieEntity();
        }
    }

}
