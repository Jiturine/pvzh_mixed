using System.Linq;

public class Podfather : Entity
{
    public override void Place()
    {
        base.Place();
        GameManager.OnPlaceEntityEvent += OnPlaceEntity;
    }
    void OnPlaceEntity(Entity entity)
    {
        if (entity == this) return;
        if (entity.tags.Contains(Game.Tag.Pea))
        {
            entity.Health += 2;
            entity.Atk += 2;
        }
    }
    public override void Exit()
    {
        base.Exit();
        GameManager.OnPlaceEntityEvent -= OnPlaceEntity;
    }
}