using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HelpPanel : BasePanel
{
    public Button back;
    public Button nextPage;
    public Button prevPage;
    public List<Image> images;
    new void Start()
    {
        base.Start();
        back.onClick.AddListener(BackToMainMenu);
        nextPage.onClick.AddListener(NextPage);
        prevPage.onClick.AddListener(PrevPage);
        currentPageIndex = 0;
        images[0].enabled = true;
    }
    void Update()
    {
        if (currentPageIndex == maxPageIndex && nextPage.gameObject.activeSelf)
        {
            nextPage.gameObject.SetActive(false);
        }
        else if (currentPageIndex != maxPageIndex && !nextPage.gameObject.activeSelf)
        {
            nextPage.gameObject.SetActive(true);
        }
        if (currentPageIndex == 0 && prevPage.gameObject.activeSelf)
        {
            prevPage.gameObject.SetActive(false);
        }
        else if (currentPageIndex != 0 && !prevPage.gameObject.activeSelf)
        {
            prevPage.gameObject.SetActive(true);
        }
    }
    private void BackToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    private void NextPage()
    {
        images[currentPageIndex].enabled = false;
        currentPageIndex++;
        images[currentPageIndex].enabled = true;
    }
    private void PrevPage()
    {
        images[currentPageIndex].enabled = false;
        currentPageIndex--;
        images[currentPageIndex].enabled = true;
    }
    private int currentPageIndex;
    private static int maxPageIndex = 5;

}
