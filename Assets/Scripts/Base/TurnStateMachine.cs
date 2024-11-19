using System;
using System.Collections.Generic;

public class TurnStateMachine
{
    public TurnStateMachine()
    {
        stateDict = new Dictionary<Type, BaseTurnState>();
    }
    public BaseTurnState currentState;
    public Dictionary<Type, BaseTurnState> stateDict;
    private void AddState<T>() where T : BaseTurnState
    {
        if (!stateDict.ContainsKey(typeof(T)))
        {
            stateDict.Add(typeof(T), Activator.CreateInstance<T>());
        }
    }
    public void SwitchState<T>() where T : BaseTurnState
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        if (!stateDict.ContainsKey(typeof(T)))
        {
            AddState<T>();
        }
        currentState = stateDict[typeof(T)];
        currentState.OnEnter();
    }
    public void OnUpdate()
    {
        currentState.OnUpdate();
    }
}