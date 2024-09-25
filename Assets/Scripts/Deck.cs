using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
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
    public List<Card> cardList;
    public void Add(Card card)
    {
        if (GameManager.gameState == GameManager.UIState.SelectCard)
        {
            Card newCard = Instantiate(CardDictionary.card[card.ID], contentTransform).GetComponent<Card>();
            newCard.location = Card.Location.InDeck;
            cardList.Add(newCard);
        }
        else if (GameManager.gameState == GameManager.UIState.GamePlay)
        {
            Card newCard = Instantiate(CardDictionary.card[card.ID], contentTransform).GetComponent<Card>();
            newCard.location = Card.Location.InDeck;
            cardList.Add(newCard);
        }
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
