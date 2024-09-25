using static GameManager;
using UnityEngine;

public class SunFlower : Entity
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
        AllyHero.totalPoint++;
    }
    public override void Die()
    {
        base.Die();
        OnTurnStartEvent -= turnStartHandler;
    }
}
