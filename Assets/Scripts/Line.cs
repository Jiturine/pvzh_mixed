using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class Line : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        myEntity = new List<Entity>();
        enemyEntity = new List<Entity>();
        myCollider = transform.Find("myPos").GetComponent<BoxCollider2D>();
        enemyCollider = transform.Find("enemyPos").GetComponent<BoxCollider2D>();
        lineCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void RemoveEntity(Entity entity)
    {
        if (myEntity.Contains(entity))
        {
            myEntity.Remove(entity);
        }
        else if (enemyEntity.Contains(entity))
        {
            enemyEntity.Remove(entity);
        }
    }
    public List<Entity> GetEntity(string name)
    {
        if (name == "myPos")
        {
            return myEntity;
        }
        else if (name == "enemyPos")
        {
            return enemyEntity;
        }
        return null;
    }
    public List<Entity> GetEntity(Faction faction)
    {
        if (faction == myHero.faction)
        {
            return myEntity;
        }
        else
        {
            return enemyEntity;
        }
    }
    public Faction GetFaction(string name)
    {
        Debug.Log(name);
        Debug.Log(myHeroFaction);
        if (name == "myPos")
        {
            return myHero.faction;
        }
        else
        {
            return enemyHero.faction;
        }
    }
    public List<Entity> myEntity;
    public List<Entity> enemyEntity;
    public int number;
    public BoxCollider2D myCollider;
    public BoxCollider2D enemyCollider;
    public BoxCollider2D lineCollider;

    public enum Terrain
    {
        Highland,
        Plain,
        Water
    }
    public Terrain terrain;
}
