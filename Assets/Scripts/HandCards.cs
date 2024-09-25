using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCards : MonoBehaviour
{
    public Transform contentTransform;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Add(Card card)
    {
        if (GameManager.gameState == GameManager.UIState.GamePlay)
        {
            GameObject cardPrefab = CardDictionary.card[card.ID];
            GameObject newCardObject = Instantiate(cardPrefab, contentTransform);
            Card newCard = newCardObject.GetComponent<Card>();
            newCard.location = Card.Location.InHandCards;
            if (cardList == null)
            {
                cardList = new List<Card>();
            }
            cardList.Add(newCard);
        }
    }
    public void DrawFrom(Deck deck, int index)
    {
        if (deck.cardList.Count == 0) return;
        Card randomCard = deck.cardList[index];
        deck.cardList.RemoveAt(index);
        randomCard.location = Card.Location.InHandCards;
        randomCard.transform.SetParent(contentTransform);
        cardList.Add(randomCard);
    }
    public List<Card> cardList;

    public void Remove(Card card)
    {
        cardList.Remove(card);
        Destroy(card.gameObject);
    }
}
