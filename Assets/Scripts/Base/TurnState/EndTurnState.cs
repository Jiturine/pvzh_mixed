using static Game;
public class EndTurnState : BaseTurnState
{
    private float duration = 2f;
    public bool ended;
    public override void OnEnter()
    {
        var turnPhasePanel = UIManager.Instance.TryOpenPanel<TurnPhasePanel>();
        turnPhasePanel.ShowTurnPhase("回合结束");
        GameManager.Instance.OnEndTurn();
        foreach (Line line in lines)
        {
            line.EndTurn();
        }
        Timer.Register(duration, () => ended = true);
    }

    public override void OnExit()
    {
        myHero.EndTurn = false;
        enemyHero.EndTurn = false;
        ended = false;
    }

    public override void OnUpdate()
    {

    }
}