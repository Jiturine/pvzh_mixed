using JetBrains.Annotations;
using UnityEngine;
using static Game;
using Unity.Netcode;
using System;

public class MoveAction : GameAction
{
    Entity entity;
    Collider2D collider;
    public MoveAction(Entity entity, Collider2D collider)
    {
        this.entity = entity;
        this.collider = collider;
        time = 1f;
    }
    public MoveAction(params int[] args)
    {
        Faction faction = (Faction)args[0];
        int lineIndex = args[1];
        int posID = args[2];
        entity = lines[lineIndex].GetSlot(faction).positions[posID].entity;
        int colliderID = args[3];
        collider = ColliderManager.colliders[colliderID];
        time = 1f;
    }
    public override void Apply()
    {
        if (entity == null) return;
        entity.MoveTo(collider);
        GameManager.Instance.OnMoveEntityComplete();
    }
    public override int[] ToTransportArgs()
    {
        return new int[] { (int)entity.faction, entity.slot.lineIndex, entity.slot.GetPosID(entity), ColliderManager.colliderID[collider] };
    }
}