using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGameMenu : MonoBehaviour
{
    private static EndGameMenu _instance;
    public static EndGameMenu Instance
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
        text = transform.Find("Text").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public TextMeshProUGUI text;
}
