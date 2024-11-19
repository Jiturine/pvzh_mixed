using System;
using UnityEngine;
using Random = UnityEngine.Random;

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
        entity.atkRenderer.HideAtk();
        entity.healthRenderer.HideHealth();
        entity.spriteRenderer.sprite = SpriteManager.gravestoneSprites[Random.Range(0, 10)];
        GameManager.AddTurnStartEvent(entity.faction, GetOutOfGrave);
    }
    public override void SetTempEntity(Entity entity)
    {
        this.entity = entity;
    }
    public void GetOutOfGrave()
    {
        entity.atkRenderer.ShowAtk();
        entity.healthRenderer.ShowHealth();
        entity.spriteRenderer.sprite = SpriteManager.cardSprite[entity.ID];
        ActionSequence.AddAction(new BattlecryAction(entity));
        GameManager.RemoveTurnStartEvent(entity.faction, GetOutOfGrave);
        outOfGrave = true;
    }
    public bool outOfGrave;
}