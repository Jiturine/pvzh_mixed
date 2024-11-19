using Unity.VisualScripting;
using UnityEngine;

public class Ability
{
    public Entity entity;
    public EntityCard card;
    public Ability() { SetInfo(); }
    public Ability(Entity entity)
    {
        SetEntity(entity);
        SetInfo();
    }
    public Ability(EntityCard card)
    {
        SetCard(card);
        SetInfo();
    }
    virtual public void SetEntity(Entity entity)
    {
        this.entity = entity;
    }
    virtual public void SetCard(EntityCard card)
    {
        this.card = card;
    }
    virtual public void SetTempEntity(Entity entity)
    {
        SetEntity(entity);
    }
    virtual public void Remove()
    {

    }
    public void SetInfo()
    {
        ID = CardDictionary.abilityID[GetType().Name];
        name = CardDictionary.ability[ID].name;
        description = CardDictionary.ability[ID].description;
    }
    public string name;
    public string description;
    public int ID;
}
