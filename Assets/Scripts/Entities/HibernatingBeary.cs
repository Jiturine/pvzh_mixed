using UnityEngine;

public class HibernatingBeary : Entity
{
    new void Start()
    {
        base.Start();
        spriteRenderer.sprite = AsleepSprite;
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
            spriteRenderer.sprite = AwakenSprite;
        }
    }
    public override void Exit()
    {
        base.Exit();
        OnTakeDamageEvent -= OnTakeDamage;
    }
    public static Sprite asleepSprite;
    public static Sprite AsleepSprite
    {
        get
        {
            if (asleepSprite == null)
            {
                asleepSprite = Resources.Load<Sprite>("Sprites/Entity/HibernatingBeary/HibernatingBeary_asleep");
            }
            return asleepSprite;
        }
    }
    public static Sprite awakenSprite;
    public static Sprite AwakenSprite
    {
        get
        {
            if (awakenSprite == null)
            {
                awakenSprite = Resources.Load<Sprite>("Sprites/Entity/HibernatingBeary/HibernatingBeary");
            }
            return awakenSprite;
        }
    }
    public bool asleep;
}