using System;
using UnityEngine;

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
        entity.spriteRenderer.sprite = SpriteManager.gravestoneSprite;
        GameManager.AddTurnBeginEvent(entity.faction, GetOutOfGrave);
    }
    public void GetOutOfGrave()
    {
        entity.atkRenderer.ShowAtk();
        entity.healthRenderer.ShowHealth();
        entity.spriteRenderer.sprite = SpriteManager.cardSprite[entity.ID];
        BattlecryManager.Add(entity);
        GameManager.RemoveTurnBeginEvent(entity.faction, GetOutOfGrave);
        outOfGrave = true;
    }
    public override void SetInfo()
    {
        name = "墓碑";
        description = "直到下一次我方行动前不会显现";
        ID = 5;
    }
    public bool outOfGrave;
}