
using UnityEngine;

public class Vampirism : Ability
{
    public Vampirism() { SetInfo(); }
    public Vampirism(Entity entity)
    {
        SetEntity(entity);
        SetInfo();
    }
    public override void SetEntity(Entity entity)
    {
        this.entity = entity;
        entity.DoDamageEvent += Heal;
    }
    void Heal(int effectiveDamage)
    {
        if (entity == null) Debug.LogError("Entity Reference is NULL!");
        entity.Heal(effectiveDamage);
    }
    public override void SetInfo()
    {
        name = "吸血";
        description = "当这名队友造成伤害时，为自己恢复等量生命值";
        ID = 3;
    }
}
