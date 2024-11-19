using JetBrains.Annotations;
using UnityEngine;
using static Game;
using Unity.Netcode;

public class DrawCardAction : GameAction
{
    private Faction faction;
    private int randomSeed;
    public DrawCardAction(Faction faction)
    {
        this.faction = faction;
        randomSeed = Random.Range(0, int.MaxValue);
        time = 0.1f;
    }
    public DrawCardAction(params int[] args)
    {
        this.faction = (Faction)args[0];
        randomSeed = args[1];
        time = 0.1f;
    }
    public override void Apply()
    {
        GameManager.Instance.GetHandCards(faction).DrawFrom(GameManager.Instance.GetDeck(faction), randomSeed);
    }
    public override int[] ToTransportArgs()
    {
        return new int[] { (int)faction, randomSeed };
    }
}