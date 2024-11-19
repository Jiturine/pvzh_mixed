using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskPanel : BasePanel
{
    override public void OpenPanel<T>()
    {
        active = true;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 1;
        this.name = typeof(T).ToString();
    }
}
