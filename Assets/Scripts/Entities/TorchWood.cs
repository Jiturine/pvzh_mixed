using static GameManager;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TorchWood : Entity
{
    new void Start()
    {
        base.Start();
        if (slot != null)
        {
            if (!abilities.Any(ability => ability is TeamUp))
            {
                abilities.Add(new TeamUp());
            }
        }
    }
    new void Update()
    {
        base.Update();
        if (slot != null)
        {
            if (slot.SecondEntity == this && slot.FirstEntity != null && slot.FirstEntity.tags.Any(tag => tag == Tag.Pea))
            {
                if (buffEntity != slot.FirstEntity)
                {
                    buffEntity = slot.FirstEntity;
                    buffEntity.Atk += 2;
                }
            }
            else if (buffEntity != null)
            {
                buffEntity.Atk -= 2;
                buffEntity = null;
            }
        }
    }
    public override void Die()
    {
        base.Die();
        if (buffEntity != null)
        {
            buffEntity.Atk -= 2;
        }
    }
    private Entity buffEntity;
}
