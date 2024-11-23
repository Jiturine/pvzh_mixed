using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaitingPanel : BasePanel
{
    public TextMeshProUGUI messageText;
    public void SetMessage(string text)
    {
        messageText.text = text;
    }
}
