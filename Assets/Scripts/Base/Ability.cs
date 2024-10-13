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
    virtual public void SetInfo() { }
    public string name;
    public string description;
    public int ID;
}
