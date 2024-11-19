using System;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class GameAction
{
    public float time;
    virtual public void Apply()
    {
        Debug.LogError("未定义基类！");
    }
    virtual public int[] ToTransportArgs()
    {
        return null;
    }
    public enum Type
    {
        ApplyCardAction,
        AttackAction,
        BattlecryAction,
        DrawCardAction,
        EndTurnAction,
        SwitchPhaseAction
    }
    static public Dictionary<Type, string> stringDict = new Dictionary<Type, string>()
    {
        [Type.ApplyCardAction] = "ApplyCardAction",
        [Type.AttackAction] = "AttackAction",
        [Type.BattlecryAction] = "BattlecryAction",
        [Type.DrawCardAction] = "DrawCardAction",
        [Type.EndTurnAction] = "EndTurnAction",
        [Type.SwitchPhaseAction] = "SwitchPhaseAction"
    };
    static public Dictionary<string, Type> typeDict = new Dictionary<string, Type>()
    {
        ["ApplyCardAction"] = Type.ApplyCardAction,
        ["AttackAction"] = Type.AttackAction,
        ["BattlecryAction"] = Type.BattlecryAction,
        ["DrawCardAction"] = Type.DrawCardAction,
        ["EndTurnAction"] = Type.EndTurnAction,
        ["SwitchPhaseAction"] = Type.SwitchPhaseAction
    };
}