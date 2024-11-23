using System;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class GameAction
{
    public float time;
    private float timer;
    public bool ended;
    virtual public void Apply()
    {
        timer = time;
    }
    virtual public void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0) ended = true;
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
        SwitchPhaseAction,
        MoveAction
    }
    static public Dictionary<Type, string> stringDict = new Dictionary<Type, string>()
    {
        [Type.ApplyCardAction] = "ApplyCardAction",
        [Type.AttackAction] = "AttackAction",
        [Type.BattlecryAction] = "BattlecryAction",
        [Type.DrawCardAction] = "DrawCardAction",
        [Type.EndTurnAction] = "EndTurnAction",
        [Type.SwitchPhaseAction] = "SwitchPhaseAction",
        [Type.MoveAction] = "MoveAction",
    };
    static public Dictionary<string, Type> typeDict = new Dictionary<string, Type>()
    {
        ["ApplyCardAction"] = Type.ApplyCardAction,
        ["AttackAction"] = Type.AttackAction,
        ["BattlecryAction"] = Type.BattlecryAction,
        ["DrawCardAction"] = Type.DrawCardAction,
        ["EndTurnAction"] = Type.EndTurnAction,
        ["SwitchPhaseAction"] = Type.SwitchPhaseAction,
        ["MoveAction"] = Type.MoveAction
    };
}