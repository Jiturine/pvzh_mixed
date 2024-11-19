using static Game;
public class EndTurnState : BaseTurnState
{
    public override void OnEnter()
    {
        GameManager.Instance.OnEndTurn();
        foreach (Line line in lines)
        {
            line.EndTurn();
        }
    }

    public override void OnExit()
    {
        myHero.endTurn = false;
        enemyHero.endTurn = false;
    }

    public override void OnUpdate()
    {

    }
}