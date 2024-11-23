using System;
using UnityEngine;
using static GameManager;

public class PeaPod : Entity
{
    public override void Place()
    {
        base.Place();
        increaseCount = 0;
        OnTurnStartEvent += OnTurnStart;
    }
    public void OnTurnStart()
    {
        Atk++;
        Health++;
        increaseCount++;
        if (increaseCount <= 4)
        {
            spriteRenderer.sprite = PeapodSprite[increaseCount];
        }
    }
    public override void Exit()
    {
        base.Exit();
        OnTurnStartEvent -= OnTurnStart;
    }
    private int increaseCount;
    static private Sprite[] peapodSprite;
    static private Sprite[] PeapodSprite
    {
        get
        {
            if (peapodSprite == null)
            {
                peapodSprite = new Sprite[5];
                for (int i = 0; i < 5; i++)
                {
                    peapodSprite[i] = Resources.Load<Sprite>($"Sprites/Entity/PeaPod/PeaPod_{i + 1}");
                }
            }
            return peapodSprite;
        }
    }
}
