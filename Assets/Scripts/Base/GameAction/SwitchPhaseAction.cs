using JetBrains.Annotations;
using UnityEngine;
using static Game;

public class SwitchPhaseAction : GameAction
{
    public SwitchPhaseAction()
    {
        time = 0.5f;
    }
    public SwitchPhaseAction(int[] args)
    {
        time = 0.5f;
    }
    public override void Apply()
    {
        base.Apply();
        GameManager.Instance.SwitchPhase();
    }
}