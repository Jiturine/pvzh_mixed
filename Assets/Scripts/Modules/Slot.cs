using System.Collections;
using System.Collections.Generic;
using System.Data;
using static Game;
using UnityEngine;
using static GameManager;
using System.Linq;

public class Slot : MonoBehaviour
{
    public Faction faction;
    public Faction Faction
    {
        get => faction;
        set
        {
            faction = value;
            positions[0].faction = value;
            positions[1].faction = value;
        }
    }
    public Position[] positions;
    public Collider2D FirstCollider
    {
        get => positions[0].collider;
        set => positions[0].collider = value;
    }
    public Collider2D SecondCollider
    {
        get => positions[1].collider;
        set => positions[1].collider = value;
    }
    public List<Collider2D> Colliders => positions.Select(position => position.collider).ToList();
    public int lineIndex;
    public Entity FirstEntity
    {
        get => positions[0].entity;
        set => positions[0].entity = value;
    }
    public Entity SecondEntity
    {
        get => positions[1].entity;
        set => positions[1].entity = value;
    }
    public List<Entity> Entities => positions.Select(position => position.entity).Where(entity => entity != null).ToList();
    public Entity FrontEntity => (SecondEntity != null) ? SecondEntity : (FirstEntity != null) ? FirstEntity : null;
    public Slot OpponentSlot => (faction == myHero.faction) ? lines[lineIndex].enemySlot : lines[lineIndex].mySlot;
    public Line Line => lines[lineIndex];
    public void RemoveEntity(Entity entity)
    {
        if (FirstEntity == entity)
        {
            FirstEntity = null;
        }
        else if (SecondEntity == entity)
        {
            SecondEntity = null;
        }
        else
        {
            Debug.LogError("Entity Remove Failed!");
        }
    }
    public int GetPosID(Entity entity)
    {
        if (FirstEntity == entity)
        {
            return 0;
        }
        else if (SecondEntity == entity)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }
    public Entity GetEntity(Collider2D collider)
    {
        if (collider == FirstCollider)
        {
            return FirstEntity;
        }
        else if (collider == SecondCollider)
        {
            return SecondEntity;
        }
        else
        {
            Debug.LogError("Cannot Get Entity!");
            return null;
        }
    }
    public Position GetPosition(Entity entity)
    {
        if (entity == FirstEntity)
        {
            return positions[0];
        }
        else if (entity == SecondEntity)
        {
            return positions[1];
        }
        else return null;
    }
    public Collider2D GetCollider(Entity entity)
    {
        if (entity == FirstEntity)
        {
            return FirstCollider;
        }
        else if (entity == SecondEntity)
        {
            return SecondCollider;
        }
        else
        {
            Debug.LogError("Cannot Get Collider!");
            return null;
        }
    }
    public bool Empty => FirstEntity == null && SecondEntity == null;
}
