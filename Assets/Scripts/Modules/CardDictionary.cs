using System.Collections.Generic;
using UnityEngine;
using Newtonsoft;
using System.IO;
using Newtonsoft.Json;
using Unity.VisualScripting;
using static GameManager;

public class CardDictionary
{
    static public Dictionary<int, CardInfo> cardInfo;
    static public Dictionary<int, GameObject> card;
    static public Dictionary<int, GameObject> entity;
    static public Dictionary<Tag, string> tagName;
    static public void Init()
    {
        cardInfo = FileManager.LoadData<Dictionary<int, CardInfo>>("CardInfo");
        card = new Dictionary<int, GameObject>();
        foreach (var kvp in cardInfo)
        {
            card[kvp.Key] = Resources.Load<GameObject>("Prefabs/Cards/" + kvp.Value.name + "_card");
        }
        entity = new Dictionary<int, GameObject>();
        foreach (var kvp in cardInfo)
        {
            if (kvp.Value.isEntity)
            {
                entity[kvp.Key] = Resources.Load<GameObject>("Prefabs/Entities/" + kvp.Value.name);
            }
        }
        tagName = FileManager.LoadData<Dictionary<Tag, string>>("Tags");
    }
    public class CardInfo
    {
        public string name;
        public string description;
        public bool isEntity;
    }
}


