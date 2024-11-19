using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public Transform contentTransform;
    public List<Card> cardList;
    public void Add(int cardID)
    {
        Card card = Instantiate(CardDictionary.card[cardID], contentTransform).GetComponent<Card>();
        card.SetInfo();
        card.location = Card.Location.InDeck;
        cardList.Add(card);
    }
    public void Remove(Card card)
    {
        cardList.Remove(card);
        Destroy(card.gameObject);
    }
    public void Clear()
    {
        foreach (Card card in cardList)
        {
            Destroy(card.gameObject);
        }
        cardList.Clear();
    }
}
