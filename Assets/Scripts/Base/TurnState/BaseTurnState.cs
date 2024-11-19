public abstract class BaseTurnState
{
    public enum TurnState
    {
        DrawCard, MyTurn, EnemyTurn, EndTurn
    }
    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnExit();
}