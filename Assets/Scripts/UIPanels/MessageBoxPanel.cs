using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageBoxPanel : BasePanel
{
    public void ShowMessage(string message, Vector3 position)
    {
        messageText.text = message;
        transform.position = position;
    }
    public TextMeshProUGUI messageText;
}
