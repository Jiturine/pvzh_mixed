using JetBrains.Annotations;
using UnityEngine;
using static Game;
using Unity.Netcode;
using UnityEditor.Rendering.BuiltIn.ShaderGraph;

public class EndTurnAction : GameAction
{
    public Faction faction;
    public EndTurnAction(Faction faction)
    {
        this.faction = faction;
        time = 0.5f;
    }
    public EndTurnAction(params int[] args)
    {
        this.faction = (Faction)args[0];
        time = 0.5f;
    }
    public override void Apply()
    {
        base.Apply();
        GameManager.Instance.EndTurn(faction);
    }
    public override int[] ToTransportArgs()
    {
        return new int[] { (int)faction };
    }
}