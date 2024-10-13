using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using static GameManager;

public class Slot : MonoBehaviour
{
    public Faction faction;
    public Entity[] entities;
    public Collider2D firstCollider;
    public Collider2D secondCollider;
    public int lineIndex;
    public Entity FirstEntity
    {
        get
        {
            return entities[0];
        }
        set
        {
            entities[0] = value;
        }
    }
    public Entity SecondEntity
    {
        get
        {
            return entities[1];
        }
        set
        {
            entities[1] = value;
        }
    }
    public Entity FrontEntity
    {
        get
        {
            if (entities[1] != null)
            {
                return entities[1];
            }
            else if (entities[0] != null)
            {
                return entities[0];
            }
            else
            {
                return null;
            }
        }

    }
    public Slot OpponentSlot
    {
        get
        {
            if (faction == myHero.faction)
            {
                return lines[lineIndex].enemySlot;
            }
            else
            {
                return lines[lineIndex].mySlot;
            }
        }
    }
    public void RemoveEntity(Entity entity)
    {
        if (entities[0] == entity)
        {
            entities[0] = null;
        }
        else if (entities[1] == entity)
        {
            entities[1] = null;
        }
        else
        {
            Debug.LogError("Entity Remove Failed!");
        }
    }
    public Entity GetEntity(Collider2D collider)
    {
        if (collider == firstCollider)
        {
            return FirstEntity;
        }
        else if (collider == secondCollider)
        {
            return SecondEntity;
        }
        else
        {
            Debug.LogError("Cannot Get Entity!");
            return null;
        }
    }
    public Collider2D GetCollider(Entity entity)
    {
        if (entity == FirstEntity)
        {
            return firstCollider;
        }
        else if (entity == SecondEntity)
        {
            return secondCollider;
        }
        else
        {
            Debug.LogError("Cannot Get Collider!");
            return null;
        }
    }
    public void SetEntity(Collider2D collider, Entity entity)
    {
        if (collider == firstCollider)
        {
            FirstEntity = entity;
        }
        else if (collider == secondCollider)
        {
            SecondEntity = entity;
        }
    }
    public bool Empty
    {
        get
        {
            return entities[0] == null && entities[1] == null;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        entities = new Entity[2];
        firstCollider = transform.Find("First Pos").GetComponent<Collider2D>();
        secondCollider = transform.Find("Second Pos").GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
