using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using static Game;

public class Gravestone : Ability
{
    public Gravestone()
    {
        SetInfo();
    }
    public Gravestone(Entity entity)
    {
        SetInfo();
        SetEntity(entity);
    }
    public override void SetEntity(Entity entity)
    {
        this.entity = entity;
    }
    public override void SetTempEntity(Entity entity)
    {
        this.entity = entity;
    }
    static public GameObject gravestoneEntityPrefab;
    static public GameObject GravestoneEntityPrefab
    {
        get
        {
            if (gravestoneEntityPrefab == null)
            {
                gravestoneEntityPrefab = Resources.Load<GameObject>("Prefabs/Entities/GravestoneEntity");
            }
            return gravestoneEntityPrefab;
        }
    }
}