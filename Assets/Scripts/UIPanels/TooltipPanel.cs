using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
using static Game;

public class TooltipPanel : BasePanel
{
    void Update()
    {
        FixPosition();
        if (targetObject == null && active)
        {
            UIManager.Instance.ClosePanel<TooltipPanel>();
        }
    }
    public void FixPosition()
    {
        float xSize = tooltip_up.rectTransform.sizeDelta.x;
        float ySize = tooltip_up.rectTransform.sizeDelta.y + tooltip_down.rectTransform.sizeDelta.y;
        float xPos = Input.mousePosition.x;
        float yPos = Input.mousePosition.y;
        if (xPos + xSize > Screen.width)
        {
            xPos = Screen.width - xSize;
        }
        if (yPos < ySize)
        {
            yPos = ySize;
        }
        rectTransform.position = new Vector3(xPos, yPos, 0);
    }
    public void ShowCard(Card card)
    {
        gameObject.SetActive(true);
        entityImage.sprite = SpriteManager.cardSprite[card.ID];
        tooltip_up.sprite = SpriteManager.GetTooltipSprites(card.faction)[0];
        tooltip_down.sprite = SpriteManager.GetTooltipSprites(card.faction)[1];
        text.text = $"{card.name}\n";
        if (card.tags.Any())
        {
            text.text += "<color=grey>- ";
            foreach (Tag tag in card.tags)
            {
                text.text += CardDictionary.tagName[tag] + " ";
            }
            text.text += "-</color>\n";
        }
        if (card is EntityCard entityCard)
        {
            if (entityCard.abilities.Any())
            {
                foreach (var ability in entityCard.abilities)
                {
                    text.text += $"<color=red>{ability.name}</color> ";
                }
                text.text += "\n";
            }
        }
        if (CardDictionary.cardInfo[card.ID].description != string.Empty)
        {
            text.text += CardDictionary.cardInfo[card.ID].description + "\n";
        }
        targetObject = card.gameObject;
        if (card is EntityCard _entityCard)
        {
            if (_entityCard.abilities.Any())
            {
                text.text += "\n";
                foreach (var ability in _entityCard.abilities)
                {
                    text.text += $"<color=red>{ability.name}</color>:{ability.description}\n";
                }
            }
        }
        FixPosition();
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }
    public void ShowEntity(Entity entity)
    {
        gameObject.SetActive(true);
        tooltip_up.sprite = SpriteManager.GetTooltipSprites(entity.faction)[0];
        tooltip_down.sprite = SpriteManager.GetTooltipSprites(entity.faction)[1];
        if (entity.abilities.Contains<Gravestone>(out var gravestone) && gravestone.outOfGrave == false)
        {
            entityImage.sprite = null;
            text.text = "???";
        }
        else
        {
            entityImage.sprite = SpriteManager.cardSprite[entity.ID];
            text.text = $"{entity.name}\n";
            if (entity.tags.Any())
            {
                text.text += "<color=grey>- ";
                foreach (Tag tag in entity.tags)
                {
                    text.text += CardDictionary.tagName[tag] + " ";
                }
                text.text += "-</color>\n";
            }
            if (entity.abilities.Any())
            {
                foreach (var ability in entity.abilities)
                {
                    text.text += $"<color=red>{ability.name}</color> ";
                }
                text.text += "\n";
            }
            if (CardDictionary.cardInfo[entity.ID].description != "")
            {
                text.text += CardDictionary.cardInfo[entity.ID].description + "\n";
            }
            if (entity.abilities.Any())
            {
                text.text += "\n";
                foreach (var ability in entity.abilities)
                {
                    text.text += $"<color=red>{ability.name}</color>:{ability.description}\n";
                }
            }
        }
        targetObject = entity.gameObject;
        FixPosition();
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }
    public GameObject targetObject;
    public Image entityImage;
    public Image tooltip_up;
    public Image tooltip_down;
    public TextMeshProUGUI text;
    public RectTransform rectTransform;
}
