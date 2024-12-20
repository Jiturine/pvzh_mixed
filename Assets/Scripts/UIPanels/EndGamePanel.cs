using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGamePanel : BasePanel
{
    public TextMeshProUGUI winOrLoseText;
    public Button back;
    new void Start()
    {
        back.onClick.AddListener(GameManager.Instance.OnEndGameMenuBtnClick);
        back.onClick.AddListener(UIManager.Instance.PlayButtonClickSFX);
    }
}
