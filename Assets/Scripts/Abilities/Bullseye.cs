using UnityEngine;

public class Bullseye : Ability
{
    public Bullseye() { SetInfo(); }
    public Bullseye(Entity entity)
    {
        SetEntity(entity);
        SetInfo();
    }
    public Bullseye(EntityCard card)
    {
        SetCard(card);
        SetInfo();
    }
    public override void Remove()
    {
        if (card != null) card.atkUI.atkImage.sprite = SpriteManager.defaultAtkIcon;
        if (entity != null) entity.atkRenderer.atkSprite.sprite = SpriteManager.defaultAtkIcon;
    }
    public override void SetEntity(Entity entity)
    {
        base.SetEntity(entity);
        entity.atkRenderer.atkSprite.sprite = SpriteManager.abilityIcons[ID];
    }
    public override void SetCard(EntityCard card)
    {
        base.SetCard(card);
        card.atkUI.atkImage.sprite = SpriteManager.abilityIcons[ID];
    }
}
