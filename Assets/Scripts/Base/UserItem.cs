using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserItem : MonoBehaviour
{
    public Button button;
    public Image backgroundImage;
    public TextMeshProUGUI userNameText;
    void Start()
    {
        button.onClick.AddListener(SelectUser);
    }
    public void SelectUser()
    {
        var switchUserPanel = UIManager.Instance.GetPanel<SwitchUserPanel>();
        var preItem = switchUserPanel.userItemList.Find(userItem => userItem.userNameText.text == switchUserPanel.currentUserName);
        if (preItem != null)
        {
            preItem.backgroundImage.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        }
        switchUserPanel.currentUserName = userNameText.text;
        backgroundImage.color = selectedColor;
    }
    static private Color selectedColor = new Color(20.0f / 255, 180.0f / 255, 15.0f / 255, 1.0f);
}
