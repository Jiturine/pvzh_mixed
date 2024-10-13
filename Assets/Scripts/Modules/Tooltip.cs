using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
using static GameManager;

public class Tooltip : MonoBehaviour
{
    private static Tooltip _instance;
    public static Tooltip Instance
    {
        get; set;
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            entityImage = transform.Find("Tooltip_up").transform.Find("Image").GetComponent<Image>();
            text = transform.Find("Tooltip_down").transform.Find("Text").GetComponentInChildren<TextMeshProUGUI>();
            tooltip_up = transform.Find("Tooltip_up").GetComponent<Image>();
            tooltip_down = transform.Find("Tooltip_down").GetComponent<Image>();
            rectTransform = GetComponent<RectTransform>();
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        FixPosition();
        if (targetObject == null)
        {
            gameObject.SetActive(false);
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
        if (card.tags.Count != 0)
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
            if (entityCard.abilities.Count != 0)
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
            if (_entityCard.abilities.Count != 0)
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
        if (entity.abilities.Any(ability => ability is Gravestone gravestone && gravestone.outOfGrave == false))
        {
            entityImage.sprite = null;
            text.text = "???";
        }
        else
        {
            entityImage.sprite = SpriteManager.cardSprite[entity.ID];
            text.text = $"{entity.name}\n";
            if (entity.tags.Count != 0)
            {
                text.text += "<color=grey>- ";
                foreach (Tag tag in entity.tags)
                {
                    text.text += CardDictionary.tagName[tag] + " ";
                }
                text.text += "-</color>\n";
            }
            if (entity.abilities.Count != 0)
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
            if (entity.abilities.Count != 0)
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
