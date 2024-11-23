using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static Game;

public class GravestoneEntity : Entity
{
    new void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        applicableIndicator = transform.Find("applicableIndicator").GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
        material.SetInt("_Enable", 0);
        spriteIndex = Random.Range(0, 10);
        spriteRenderer.sprite = SpriteManager.gravestoneSprites[spriteIndex];
        GameManager.AddTurnStartEvent(Faction.Zombie, GetOutOfGrave);
    }
    public void GetOutOfGrave()
    {
        Destroy(gameObject);
        hiddenEntity.gameObject.SetActive(true);
        GameManager.Instance.PlaceEntity(hiddenEntity, Pos.collider);
        GameManager.RemoveTurnStartEvent(Faction.Zombie, GetOutOfGrave);
    }
    public Entity hiddenEntity;
    public int spriteIndex;
    public override int TakeDamage(int damage) => 0;
    public override void OnPointerUp() { }
    public override bool ReadyToAttack => false;
    public override int Atk => int.MaxValue;
    public override int Health => int.MaxValue;
    public override void Place() { }
    public override void Attack() { }
    public override void CounterAttack() { }
    public void SetInfo(EntityCard entityCard, Collider2D collider)
    {
        faction = Faction.Zombie;
        ID = 20000;
        hiddenEntity = Instantiate(CardDictionary.entity[entityCard.ID], collider.transform.position, Quaternion.identity, collider.transform).GetComponent<Entity>();
        hiddenEntity.SetInfo(entityCard);
        hiddenEntity.gameObject.SetActive(false);
    }
    public override void Exit()
    {
        slot.RemoveEntity(this);
        Destroy(gameObject);
        if (hiddenEntity != null)
        {
            Destroy(hiddenEntity);
        }
    }
}
