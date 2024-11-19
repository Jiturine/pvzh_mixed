using static GameManager;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using static Game;

public class TorchWood : Entity
{
    new void Update()
    {
        base.Update();
        if (slot != null)
        {
            if (slot.SecondEntity == this && slot.FirstEntity != null && slot.FirstEntity.tags.Contains(Tag.Pea))
            {
                if (buffEntity != slot.FirstEntity)
                {
                    buffEntity = slot.FirstEntity;
                    buffEntity.Atk += 2;
                }
            }
            else if (buffEntity != null)
            {
                if (buffEntity.Atk < 2)
                {
                    buffEntity.Atk = 0;
                }
                else
                {
                    buffEntity.Atk -= 2;
                }
                buffEntity = null;
            }
        }
    }
    public override void Exit()
    {
        base.Exit();
        if (buffEntity != null)
        {
            if (buffEntity.Atk < 2)
            {
                buffEntity.Atk = 0;
            }
            else
            {
                buffEntity.Atk -= 2;
            }
        }
    }
    private Entity buffEntity;
}
