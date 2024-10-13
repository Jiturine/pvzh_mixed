using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    private static DeckManager _instance;
    public static DeckManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DeckManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("DeckManager");
                    _instance = singletonObject.AddComponent<DeckManager>();
                }
            }
            return _instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        ReadDeckInfo();
    }

    // Update is called once per frame
    void Update()
    {

    }
    static void ReadDeckInfo()
    { }

}
