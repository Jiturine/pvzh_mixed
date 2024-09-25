
public class Vampirism : Ability
{
    Entity entity;
    public Vampirism(Entity _entity)
    {
        entity = _entity;
        entity.DoDamageEvent += Heal;
    }
    void Heal(int effectiveDamage)
    {
        entity.Heal(effectiveDamage);
    }
}
