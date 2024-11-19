using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using static Game;

public class ActionSequence
{
    static public LinkedList<GameAction> actionSequence;
    static public void Init()
    {
        actionSequence = new LinkedList<GameAction>();
    }
    static public void AddAction(GameAction gameAction)
    {
        if (gameMode == Game.GameMode.Online)
        {
            GameManager.Instance.AddActionServerRpc(GameAction.typeDict[gameAction.GetType().ToString()], gameAction.ToTransportArgs());
        }
        else
        {
            actionSequence.AddLast(gameAction);
        }
    }
    static public void ApplyAction()
    {
        //debug
        string res = "";
        foreach (GameAction action in actionSequence)
        {
            res += action.GetType().ToString() + " ";
        }
        Debug.Log(res);
        var gameAction = actionSequence.First.Value;
        Timer.Register(gameAction.time - 0.05f, () =>
        {
            actionSequence.Remove(gameAction);
        });
        gameAction.Apply();
        coolTimer = gameAction.time;
    }
    static public bool HasAction<T>() where T : GameAction
    {
        return actionSequence.Any(action => action is T);
    }
    static public bool Empty()
    {
        return actionSequence.Count == 0;
    }
    static public void Update()
    {
        coolTimer -= Time.deltaTime;
        if (actionSequence.Any() && coolTimer < 0)
        {
            ApplyAction();
        }
    }
    static public float coolTimer;
}
