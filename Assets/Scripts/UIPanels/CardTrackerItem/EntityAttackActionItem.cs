using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Game;

public class EntityAttackActionItem : HistoryActionItem
{
    public void SetInfo(CardTracker.EntityAttackAction entityAttackAction)
    {
        if (entityAttackAction.attackerInfo.Faction == Faction.Plant)
        {
            backgroundImage.sprite = SpriteManager.plantHandCardsSprite;
        }
        else
        {
            backgroundImage.sprite = SpriteManager.zombieHandCardsSprite;
        }
        attackerItem.image.sprite = SpriteManager.cardSprite[entityAttackAction.attackerInfo.entityID];
        attackerItem.healthText.text = entityAttackAction.attackerInfo.health.ToString();
        attackerItem.atkText.text = entityAttackAction.attackerInfo.atk.ToString();
        if (entityAttackAction.targetInfos != null)
        {
            foreach (var targetInfo in entityAttackAction.targetInfos)
            {
                var targetItem = Instantiate(entityItemPrefab, targetContent).GetComponent<EntityItem>();
                if (targetInfo.entityID == 20000) //是墓碑
                {
                    targetItem.image.sprite = SpriteManager.gravestoneSprites[0];
                    targetItem.healthImage.enabled = false;
                    targetItem.atkImage.enabled = false;
                }
                else
                {
                    targetItem.image.sprite = SpriteManager.cardSprite[targetInfo.entityID];
                    targetItem.healthText.text = targetInfo.health.ToString();
                    targetItem.atkText.text = targetInfo.atk.ToString();
                }

            }
        }
        if (entityAttackAction.targetHeroInfo != null)
        {
            var targetHeroItem = Instantiate(heroItemPrefab, targetContent).GetComponent<HeroItem>();
            targetHeroItem.healthText.text = entityAttackAction.targetHeroInfo.health.ToString();
            if (entityAttackAction.targetHeroInfo.faction == Faction.Plant)
            {
                targetHeroItem.heroIcon.sprite = SpriteManager.plantIcon;
            }
            else
            {
                targetHeroItem.heroIcon.sprite = SpriteManager.zombieIcon;
            }
        }
    }
    public EntityItem attackerItem;
    public List<EntityItem> targetItems;
    public GameObject heroItemPrefab;
    public GameObject entityItemPrefab;
    public Transform targetContent;
}
