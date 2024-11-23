using static Game;

public class DrawCardState : BaseTurnState
{
    private float duration = 1f;
    public bool ended;
    public override void OnEnter()
    {
        currentTurn++;
        myHero.totalPoint = currentTurn;
        enemyHero.totalPoint = currentTurn;
        if (gameMode == GameMode.Offline)
        {
            EnemyAI.Instance.hasApplicableCard = true;
            EnemyAI.Instance.hasReadyToAttackEntity = true;
        }
        GameManager.Instance.OnTurnStart();
        var turnPhasePanel = UIManager.Instance.TryOpenPanel<TurnPhasePanel>();
        turnPhasePanel.ShowTurnPhase("抽牌阶段");
        int drawCardCount = (currentTurn == 1) ? 5 : 1;
        if ((gameMode == GameMode.Online && GameManager.Instance.IsServer) || gameMode == GameMode.Offline)
        {
            if (myHero.turnOrder == 0)
            {
                for (int i = 0; i < drawCardCount; i++)
                {
                    ActionSequence.AddAction(new DrawCardAction(myHero.faction));
                    ActionSequence.AddAction(new DrawCardAction(enemyHero.faction));
                }
            }
            else
            {
                for (int i = 0; i < drawCardCount; i++)
                {
                    ActionSequence.AddAction(new DrawCardAction(enemyHero.faction));
                    ActionSequence.AddAction(new DrawCardAction(myHero.faction));
                }
            }
        }
        Timer.Register(duration, () => ended = true);
    }

    public override void OnExit()
    {
        ended = false;
    }

    public override void OnUpdate()
    {
    }
}