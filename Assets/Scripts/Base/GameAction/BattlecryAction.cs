using JetBrains.Annotations;
using UnityEngine;
using static Game;
using Unity.Netcode;

public class BattlecryAction : GameAction
{
    public Entity entity;
    public BattlecryAction(Entity entity)
    {
        this.entity = entity;
        time = 1f;
    }
    public BattlecryAction(params int[] args)
    {
        Faction faction = (Faction)args[0];
        int lineIndex = args[1];
        int posID = args[2];
        entity = lines[lineIndex].GetSlot(faction).positions[posID].entity;
        time = 1f;
    }
    public override void Apply()
    {
        if (entity == null)
        {
            ended = true;
            return;
        }
        base.Apply();
        entity.Battlecry();
    }
    public override int[] ToTransportArgs()
    {
        return new int[] { (int)entity.faction, entity.slot.lineIndex, entity.slot.GetPosID(entity) };
    }
}