using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Zookeeper : Entity
{
    public override void Place()
    {
        base.Place();
        GameManager.OnApplyCardEvent += OnApplyCard;
    }
    void OnApplyCard(Card card)
    {
        if (card.tags.Contains(Game.Tag.Pet))
        {
            if (card is EntityCard entityCard && entityCard.createdEntity == this)
            {
                return;
            }
            var entities = Game.GetEntities(faction).Where(entity => entity.tags.Contains(Game.Tag.Pet)).ToList();
            foreach (Entity entity in entities)
            {
                entity.Atk++;
            }
        }
    }
    public override void Exit()
    {
        base.Exit();
        GameManager.OnApplyCardEvent -= OnApplyCard;
    }
}
