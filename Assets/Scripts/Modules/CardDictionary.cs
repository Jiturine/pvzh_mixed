using System.Collections.Generic;
using UnityEngine;
using Newtonsoft;
using System.IO;
using Newtonsoft.Json;
using Unity.VisualScripting;
using static GameManager;
using UnityEngine.UIElements;
using static Game;

public class CardDictionary
{
    static public Dictionary<int, CardInfo> cardInfo;
    static public Dictionary<int, GameObject> card;
    static public Dictionary<string, int> cardID;
    static public Dictionary<int, GameObject> entity;
    static public Dictionary<string, int> entityID;
    static public Dictionary<Tag, string> tagName;
    static public Dictionary<int, AbilityInfo> ability;
    static public Dictionary<string, int> abilityID;
    [RuntimeInitializeOnLoadMethod]
    static public void Init()
    {
        cardInfo = FileManager.LoadData<Dictionary<int, CardInfo>>("CardInfo");
        card = new Dictionary<int, GameObject>();
        cardID = new Dictionary<string, int>();
        foreach (var kvp in cardInfo)
        {
            cardID[kvp.Value.className] = kvp.Key;
            card[kvp.Key] = Resources.Load<GameObject>("Prefabs/Cards/" + kvp.Value.className + "_card");
        }
        entity = new Dictionary<int, GameObject>();
        entityID = new Dictionary<string, int>();
        foreach (var kvp in cardInfo)
        {
            if (kvp.Value.isEntity)
            {
                entityID[kvp.Value.className] = kvp.Key;
                entity[kvp.Key] = Resources.Load<GameObject>("Prefabs/Entities/" + kvp.Value.className);
            }
        }
        tagName = FileManager.LoadData<Dictionary<Tag, string>>("Tags");
        ability = FileManager.LoadData<Dictionary<int, AbilityInfo>>("Abilities");
        abilityID = new Dictionary<string, int>();
        foreach (var kvp in ability)
        {
            abilityID[kvp.Value.className] = kvp.Key;
        }
    }
    public class CardInfo
    {
        public string className;
        public string name;
        public List<Tag> tags;
        public int cost;
        public int health;
        public int atk;
        public List<string> abilities;
        public string description;
        public bool isEntity;
    }
    public class AbilityInfo
    {
        public string className;
        public string name;
        public string description;
    }
}


