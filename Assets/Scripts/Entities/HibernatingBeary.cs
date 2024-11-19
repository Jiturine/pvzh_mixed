using UnityEngine;

public class HibernatingBeary : Entity
{
    new void Start()
    {
        base.Start();
        if (asleepSprite == null)
        {
            asleepSprite = Resources.Load<Sprite>("Sprites/CardSprites/HibernatingBeary/HibernatingBeary_sleep");
        }
        spriteRenderer.sprite = asleepSprite;
        asleep = true;
    }
    public override void Place()
    {
        base.Place();
        OnTakeDamageEvent += OnTakeDamage;
    }
    public void OnTakeDamage(int damage)
    {
        Atk += 4;
        if (asleep)
        {
            asleep = false;
            spriteRenderer.sprite = SpriteManager.cardSprite[ID];
        }
    }
    public override void Exit()
    {
        base.Exit();
        OnTakeDamageEvent -= OnTakeDamage;
    }
    public static Sprite asleepSprite;
    public bool asleep;
}