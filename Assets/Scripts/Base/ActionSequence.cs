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
    static public void AddInstantAction(GameAction gameAction)
    {
        if (gameMode == Game.GameMode.Online)
        {
            GameManager.Instance.AddInstantActionServerRpc(GameAction.typeDict[gameAction.GetType().ToString()], gameAction.ToTransportArgs());
        }
        else
        {
            actionSequence.AddFirst(gameAction);
        }
    }
    static public void ApplyAction()
    {
        currentAction = actionSequence.First.Value;
        actionSequence.Remove(currentAction);
        currentAction.Apply();
    }
    static public bool HasAction<T>() where T : GameAction
    {
        return actionSequence.Any(action => action is T) || currentAction is T;
    }
    static public bool Empty()
    {
        return !actionSequence.Any() && currentAction == null;
    }
    static public void Update()
    {
        if (!locked)
        {
            if (currentAction != null)
            {
                currentAction.Update();
                if (currentAction.ended)
                {
                    currentAction = null;
                }
            }
            if (currentAction == null && actionSequence.Any())
            {
                ApplyAction();
            }
        }
    }
    static public void Lock() => locked = true;
    static public void Unlock() => locked = false;
    static public GameAction currentAction;
    static public bool locked;
}
