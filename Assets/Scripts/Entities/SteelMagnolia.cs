using System.Collections.Generic;
using static Game;
public class SteelMagnolia : Entity
{
    public override void Place()
    {
        base.Place();
        List<Entity> entities = new List<Entity>();
        if (slot.lineIndex > 0)
        {
            Slot leftSlot = lines[slot.lineIndex - 1].GetSlot(faction);
            if (leftSlot.FirstEntity != null) entities.Add(leftSlot.FirstEntity);
            if (leftSlot.SecondEntity != null) entities.Add(leftSlot.SecondEntity);
        }
        if (slot.lineIndex < 4)
        {
            Slot rightSlot = lines[slot.lineIndex + 1].GetSlot(faction);
            if (rightSlot.FirstEntity != null) entities.Add(rightSlot.FirstEntity);
            if (rightSlot.SecondEntity != null) entities.Add(rightSlot.SecondEntity);
        }
        Slot _slot = lines[slot.lineIndex].GetSlot(faction);
        if (_slot.FirstEntity != null) entities.Add(_slot.FirstEntity);
        if (_slot.SecondEntity != null) entities.Add(_slot.SecondEntity);
        foreach (Entity entity in entities)
        {
            entity.Health += 2;
        }
    }
}