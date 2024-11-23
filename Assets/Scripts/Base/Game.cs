using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Game
{
    [RuntimeInitializeOnLoadMethod]
    static private void Init()
    {
        turnStateMachine = new TurnStateMachine();
    }
    public enum GameState
    {
        MainMenu,
        SelectCard,
        GamePlay,
        GameOver
    };
    public enum GameMode
    {
        Offline,
        Online
    }
    public enum TurnPhase
    {
        DrawCard,
        MyTurn,
        EnemyTurn,
        End
    }
    public enum Faction
    {
        Plant,
        Zombie
    }
    public enum Tag
    {
        Pea, Flower, Nut, Berry, Leafy, Animal, Root, Athlete, Pet, Professional, Gourmet
    }
    public static TurnStateMachine turnStateMachine;
    public static BaseTurnState State => turnStateMachine.currentState;
    static public int readyPlayerNumber;
    static public Line[] lines;
    static public GameState gameState;
    static public GameMode gameMode;
    static public Hero myHero;
    static public Hero enemyHero;
    static public Deck myDeck;
    static public Deck enemyDeck;
    static public HandCards myHandCards;
    static public HandCards enemyHandCards;
    static public int currentTurn;
    static public readonly int lineCount = 5;

    static public List<Entity> AllEntities
    {
        get
        {
            List<Entity> entities = new List<Entity>();
            foreach (Line line in lines)
            {
                Slot slot = line.GetSlot(Faction.Plant);
                if (slot.FirstEntity != null)
                {
                    entities.Add(slot.FirstEntity);
                }
                if (slot.SecondEntity != null)
                {
                    entities.Add(slot.SecondEntity);
                }
                slot = line.GetSlot(Faction.Zombie);
                if (slot.FirstEntity != null)
                {
                    entities.Add(slot.FirstEntity);
                }
                if (slot.SecondEntity != null)
                {
                    entities.Add(slot.SecondEntity);
                }
            }
            return entities;
        }
    }
    static public List<Entity> GetEntities(Faction faction)
    {
        List<Entity> entities = new List<Entity>();
        foreach (Line line in lines)
        {
            Slot slot = line.GetSlot(faction);
            if (slot.FirstEntity != null)
            {
                entities.Add(slot.FirstEntity);
            }
            if (slot.SecondEntity != null)
            {
                entities.Add(slot.SecondEntity);
            }
        }
        return entities;
    }
    static public List<Entity> GetOpponentEntities(Faction faction)
    {
        List<Entity> entities = new List<Entity>();
        foreach (Line line in lines)
        {
            Slot slot = line.GetOpponentSlot(faction);
            if (slot.FirstEntity != null)
            {
                entities.Add(slot.FirstEntity);
            }
            if (slot.SecondEntity != null)
            {
                entities.Add(slot.SecondEntity);
            }
        }
        return entities;
    }
    static public List<Position> GetPositions(Faction faction)
    {
        List<Position> colliders = new List<Position>();
        foreach (Line line in lines)
        {
            Slot slot = line.GetSlot(faction);
            colliders.AddRange(slot.positions);
        }
        return colliders;
    }
    static public List<Position> GetOpponentPositions(Faction faction)
    {
        List<Position> colliders = new List<Position>();
        foreach (Line line in lines)
        {
            Slot slot = line.GetOpponentSlot(faction);
            colliders.AddRange(slot.positions);
        }
        return colliders;
    }
}