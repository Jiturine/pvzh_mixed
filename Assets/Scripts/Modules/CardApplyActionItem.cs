using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static Game;

public class CardApplyActionItem : HistoryActionItem
{
    public void SetInfo(CardTracker.CardApplyAction cardApplyAction)
    {
        if (cardApplyAction.Faction == Faction.Plant)
        {
            backgroundImage.sprite = SpriteManager.plantHandCardsSprite;
        }
        else
        {
            backgroundImage.sprite = SpriteManager.zombieHandCardsSprite;
        }
        Card card = Instantiate(CardDictionary.card[cardApplyAction.cardID], cardContent).GetComponent<Card>();
        card.SetInfo();
        card.transform.localScale *= 0.67f;
        if (cardApplyAction.targetType == CardTracker.CardApplyAction.TargetType.Any)
        {
            Destroy(targetContent.gameObject);
            Destroy(arrow.gameObject);
        }
        else if (cardApplyAction.targetType == CardTracker.CardApplyAction.TargetType.Entity)
        {
            var targetItem = Instantiate(entityItemPrefab, targetContent).GetComponent<EntityItem>();
            Position position = ColliderManager.colliders[cardApplyAction.colliderID].GetComponent<Position>();
            targetItem.image.sprite = SpriteManager.cardSprite[position.entity.ID];
            targetItem.healthText.text = position.entity.health.ToString();
            targetItem.atkText.text = position.entity.atk.ToString();
        }
        else if (cardApplyAction.targetType == CardTracker.CardApplyAction.TargetType.Position)
        {
            var positionItem = Instantiate(positonItemPrefab, targetContent).GetComponent<PositionItem>();
            positionItem.positionText.text = $"{cardApplyAction.colliderID / 10 % 10}路";
        }
    }
    public Transform cardContent;
    public List<EntityItem> targetItems;
    public GameObject entityItemPrefab;
    public GameObject positonItemPrefab;
    public Transform targetContent;
    public Transform arrow;
}
