public class MyTurnState : BaseTurnState
{
    public override void OnEnter()
    {
        var turnPhasePanel = UIManager.Instance.TryOpenPanel<TurnPhasePanel>();
        turnPhasePanel.ShowTurnPhase("我方行动");
        GameManager.Instance.OnMyHeroTurnStart();
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {

    }
}