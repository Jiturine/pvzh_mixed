using JetBrains.Annotations;
using UnityEngine;
using static Game;
using Unity.Netcode;
using System;

public class AttackAction : GameAction
{
    private Entity entity;
    public AttackAction(Entity entity)
    {
        this.entity = entity;
        time = 1f;
    }
    public AttackAction(params int[] args)
    {
        Faction faction = (Faction)args[0];
        int lineIndex = args[1];
        int posID = args[2];
        Debug.Log(faction + " " + lineIndex + " " + posID);
        entity = lines[lineIndex].GetSlot(faction).positions[posID].entity;
        time = 1f;
    }
    public override void Apply()
    {
        if (entity == null || !entity.ReadyToAttack)
        {
            ended = true;
            return;
        }
        base.Apply();
        entity.Attack();
        ActionSequence.actionSequence.AddFirst(new SwitchPhaseAction());
    }
    public override int[] ToTransportArgs()
    {
        return new int[] { (int)entity.faction, entity.slot.lineIndex, entity.slot.GetPosID(entity) };
    }
}