using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GameManager;

public class OnionRings : Entity
{
    public override void Place()
    {
        base.Place();
        foreach (Card card in GameManager.Instance.GetHandCards(faction).cardList)
        {
            if (card is EntityCard entityCard)
            {
                entityCard.maxAtk = 4;
                entityCard.maxHealth = 4;
                entityCard.Atk = 4;
                entityCard.Health = 4;
            }
        }
    }
}
