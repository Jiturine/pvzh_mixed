using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HelpPanel : BasePanel
{
    public Button back;
    new void Start()
    {
        base.Start();
        back.onClick.AddListener(BackToMainMenu);
    }
    private void BackToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
