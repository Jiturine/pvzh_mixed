using static Game;

public class DrawCardState : BaseTurnState
{
    public override void OnEnter()
    {
        currentTurn++;
        myHero.totalPoint = currentTurn;
        enemyHero.totalPoint = currentTurn;
        GameManager.hasDrawnCard = false;
        if (gameMode == GameMode.Offline)
        {
            EnemyAI.Instance.hasApplicableCard = true;
            EnemyAI.Instance.hasReadyToAttackEntity = true;
        }
        GameManager.Instance.OnTurnStart();

        int drawCardCount = (currentTurn == 1) ? 5 : 1;
        if ((gameMode == GameMode.Online && GameManager.Instance.IsServer) || gameMode == GameMode.Offline)
        {
            if (!GameManager.hasDrawnCard)
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
                ActionSequence.AddAction(new SwitchPhaseAction());
                GameManager.hasDrawnCard = true;
            }
        }
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {
    }
}