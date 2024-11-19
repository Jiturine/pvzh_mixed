public class Armored : Ability
{
    public Armored()
    {
        SetInfo();
        this.shield = 1;
    }
    public Armored(EntityCard card)
    {
        SetInfo();
        SetCard(card);
        this.shield = 1;
    }
    public Armored(Entity entity)
    {
        SetInfo();
        SetEntity(entity);
        this.shield = 1;
    }
    public override void Remove()
    {
        if (card != null) card.healthUI.healthImage.sprite = SpriteManager.defaultAtkIcon;
        if (entity != null) entity.healthRenderer.healthSprite.sprite = SpriteManager.defaultAtkIcon;
    }
    public override void SetEntity(Entity entity)
    {
        base.SetEntity(entity);
        entity.healthRenderer.healthSprite.sprite = SpriteManager.abilityIcons[ID];
    }
    public override void SetCard(EntityCard card)
    {
        base.SetCard(card);
        card.healthUI.healthImage.sprite = SpriteManager.abilityIcons[ID];
    }

    public int shield;
}
