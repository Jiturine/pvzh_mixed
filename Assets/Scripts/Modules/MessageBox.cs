using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class MessageBox : MonoBehaviour
{
    public static MessageBox Instance { get; private set; }
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

    void Start()
    {
        messageText = GetComponentInChildren<TextMeshProUGUI>();
        Instance.gameObject.SetActive(false);
    }

    static public void ShowMessage(string message, Vector3 position)
    {
        Instance.gameObject.SetActive(true);
        Instance.messageText.text = message;
        Instance.transform.position = position;
    }

    static public void HideMessage()
    {
        Instance.gameObject.SetActive(false);
    }
    public TextMeshProUGUI messageText;
}
