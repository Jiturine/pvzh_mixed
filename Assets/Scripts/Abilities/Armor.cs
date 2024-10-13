public class Armored : Ability
{
    public Armored(int shield)
    {
        SetInfo();
        this.shield = shield;
    }
    public Armored(EntityCard card, int shield)
    {
        SetInfo();
        SetCard(card);
        this.shield = shield;
    }
    public Armored(Entity entity, int shield)
    {
        SetInfo();
        SetEntity(entity);
        this.shield = shield;
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
    ~Armored()
    {
        if (card != null) card.healthUI.healthImage.sprite = SpriteManager.defaultAtkIcon;
        if (entity != null) entity.healthRenderer.healthSprite.sprite = SpriteManager.defaultAtkIcon;
    }
    public override void SetInfo()
    {
        name = "护甲";
        description = "受到伤害时降低伤害";
        ID = 6;
    }
    public int shield;
}
