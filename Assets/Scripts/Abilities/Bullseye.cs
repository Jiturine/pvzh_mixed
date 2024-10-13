using UnityEngine;

public class Bullseye : Ability
{
    public Bullseye()
    {
        SetInfo();
    }
    public Bullseye(EntityCard card)
    {
        SetInfo();
        SetCard(card);
    }
    public Bullseye(Entity entity)
    {
        SetInfo();
        SetEntity(entity);
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
    ~Bullseye()
    {
        if (card != null) card.atkUI.atkImage.sprite = SpriteManager.defaultAtkIcon;
        if (entity != null) entity.atkRenderer.atkSprite.sprite = SpriteManager.defaultAtkIcon;
    }

    public override void SetInfo()
    {
        name = "必中";
        description = "这名队友攻击对方英雄时，不会填补其超能格挡槽";
        ID = 4;
    }
}
