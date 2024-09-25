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
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CardLibrary>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("CardLibrary");
                    _instance = singletonObject.AddComponent<CardLibrary>();
                }
            }
            return _instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        cardList = new List<Card>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    static public List<Card> cardList;
    static public void Add(int ID)
    {
        GameObject newCardObject = Instantiate(CardDictionary.card[ID], Instance.contentTransform);
        Card newCard = newCardObject.GetComponent<Card>();
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
