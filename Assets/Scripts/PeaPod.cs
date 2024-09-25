using static GameManager;

public class PeaPod : Entity
{
    private TurnStartHandler turnStartHandler;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        turnStartHandler = new TurnStartHandler(OnTurnStart);
        OnTurnStartEvent += turnStartHandler;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }
    public void OnTurnStart()
    {
        Atk++;
        Health++;
    }
    public override void Die()
    {
        base.Die();
        OnTurnStartEvent -= turnStartHandler;
    }
}
