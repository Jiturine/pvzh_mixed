public class Charge : Ability
{
    public Charge()
    {
        SetInfo();
    }
    public Charge(Entity entity)
    {
        SetInfo();
        SetEntity(entity);
    }
    public override void SetEntity(Entity entity)
    {
        base.SetEntity(entity);
        entity.ReadyToAttack = true;
    }
}
