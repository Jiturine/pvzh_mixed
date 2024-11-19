using System.Collections;
using System.Collections.Generic;
using static Game;
using UnityEngine;
using static GameManager;

[CreateAssetMenu(fileName = "DeckInfo", menuName = "Deck Info")]
public class DeckInfo : ScriptableObject
{
    public List<int> myCardID;
    public List<int> enemyCardID;
    public Faction faction;
}
