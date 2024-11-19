using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;
using UnityEngine.UI;

public class CardLibrary : MonoBehaviour
{
    public Transform contentTransform;
    private static CardLibrary _instance;
    public static CardLibrary Instance
    {
        get; set;
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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

    }
    static public List<Card> cardList;
    static public void Add(int ID)
    {
        if (cardList == null) cardList = new List<Card>();
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
            if (card != null) Destroy(card.gameObject);
        }
        cardList.Clear();
    }
}
