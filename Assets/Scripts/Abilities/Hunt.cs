using UnityEngine;
using static Game;
using static GameManager;

public class Hunt : Ability
{
    public Hunt()
    {
        SetInfo();
    }
    public Hunt(Entity entity)
    {
        SetInfo();
        SetEntity(entity);
    }
    public override void Remove()
    {
        if (entity != null) OnPlaceEntityEvent -= OnPlaceEntity;
    }
    public override void SetEntity(Entity entity)
    {
        base.SetEntity(entity);
        OnPlaceEntityEvent += OnPlaceEntity;
    }
    public override void SetTempEntity(Entity entity)
    {
        this.entity = entity;
    }
    public void OnPlaceEntity(Entity newEntity)
    {
        if (newEntity.faction != entity.faction && newEntity.slot.lineIndex != entity.slot.lineIndex)
        {
            Line line = lines[newEntity.slot.lineIndex];
            Slot slot = line.GetSlot(entity.faction);
            foreach (var collider in slot.Colliders)
            {
                if (entity.IsAbleToMoveTo(collider))
                {
                    ActionSequence.actionSequence.AddFirst(new MoveAction(entity, collider));
                    break;
                }
            }
        }
    }
}
