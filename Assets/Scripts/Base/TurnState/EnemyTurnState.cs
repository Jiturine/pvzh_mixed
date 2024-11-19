public class EnemyTurnState : BaseTurnState
{
    public override void OnEnter()
    {
        var turnPhasePanel = UIManager.Instance.TryOpenPanel<TurnPhasePanel>();
        turnPhasePanel.ShowTurnPhase("对方行动");
        GameManager.Instance.OnEnemyHeroTurnStart();
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {

    }
}