
using UnityEngine;

public class Vampirism : Ability
{
    public Vampirism() { SetInfo(); }
    public Vampirism(Entity entity)
    {
        SetEntity(entity);
        SetInfo();
    }
    public override void Remove()
    {
        entity.DoDamageEvent -= Heal;
    }
    public override void SetEntity(Entity entity)
    {
        this.entity = entity;
        entity.DoDamageEvent += Heal;
    }
    public override void SetTempEntity(Entity entity)
    {
        this.entity = entity;
    }
    void Heal(int effectiveDamage)
    {
        if (entity == null) Debug.LogError("Entity Reference is NULL!");
        entity.Heal(effectiveDamage);
    }
}
