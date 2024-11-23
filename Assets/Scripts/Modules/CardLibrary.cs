using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;
using UnityEngine.UI;

public class CardLibrary : Singleton<CardLibrary>
{
    public Transform contentTransform;
    static public Dictionary<int, int> cardCountDict;
    static public List<Card> cardList;
    static public void Init()
    {
        cardList = new List<Card>();
        cardCountDict = new Dictionary<int, int>();
        foreach (var kvp in CardDictionary.card)
        {
            cardCountDict.Add(kvp.Key, 4);
        }
    }
    static public void Add(int ID)
    {
        GameObject newCardObject = Instantiate(CardDictionary.card[ID], Instance.contentTransform);
        Card newCard = newCardObject.GetComponent<Card>();
        newCard.SetInfo();
        newCard.location = Card.Location.InCardLibrary;
        cardList.Add(newCard);
    }
    static public void LoadPlant()
    {
        int count = CardDictionary.card.Count(pair => pair.Key > 10000 && pair.Key < 20000);
        for (int i = 1; i <= count; i++)
        {
            Add(10000 + i);
        }
    }
    static public void LoadZombie()
    {
        int count = CardDictionary.card.Count(pair => pair.Key > 20000 && pair.Key < 30000);
        for (int i = 1; i <= count; i++)
        {
            Add(20000 + i);
        }
    }
    static public void Clear()
    {
        foreach (Card card in cardList)
        {
            Destroy(card.gameObject);
        }
        cardList.Clear();
    }
}
