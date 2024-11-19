using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BasePanel : MonoBehaviour
{
    public bool active;
    protected new string name;
    protected CanvasGroup canvasGroup;
    protected void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    virtual public void OpenPanel<T>() where T : BasePanel
    {
        active = true;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;
        this.name = typeof(T).ToString();
    }
    virtual public void ClosePanel<T>() where T : BasePanel
    {
        active = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
    }
}
