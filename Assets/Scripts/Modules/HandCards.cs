using System.Collections;
using System.Collections.Generic;
using static Game;
using UnityEngine;

public class HandCards : MonoBehaviour
{
    public Transform contentTransform;
    public void Add(Card card)
    {
        if (gameState == GameState.GamePlay)
        {
            GameObject cardPrefab = CardDictionary.card[card.ID];
            GameObject newCardObject = Instantiate(cardPrefab, contentTransform);
            Card newCard = newCardObject.GetComponent<Card>();
            newCard.SetInfo();
            newCard.location = Card.Location.InHandCards;
            if (cardList == null)
            {
                cardList = new List<Card>();
            }
            cardList.Add(newCard);
        }
    }
    public void Add(int ID)
    {
        if (gameState == GameState.GamePlay)
        {
            GameObject cardPrefab = CardDictionary.card[ID];
            GameObject newCardObject = Instantiate(cardPrefab, contentTransform);
            Card newCard = newCardObject.GetComponent<Card>();
            newCard.SetInfo();
            newCard.location = Card.Location.InHandCards;
            if (cardList == null)
            {
                cardList = new List<Card>();
            }
            cardList.Add(newCard);
        }
    }
    public void DrawFrom(Deck deck, int randomSeed)
    {
        if (deck.cardList.Count == 0) return;
        Random.InitState(randomSeed);
        int index = Random.Range(0, deck.cardList.Count);
        Card randomCard = deck.cardList[index];
        deck.cardList.RemoveAt(index);
        randomCard.location = Card.Location.InHandCards;
        randomCard.transform.SetParent(contentTransform);
        randomCard.transform.localScale = Vector3.one;
        cardList.Add(randomCard);
    }
    public List<Card> cardList;

    public void Remove(Card card)
    {
        cardList.Remove(card);
        Destroy(card.gameObject);
    }
}
