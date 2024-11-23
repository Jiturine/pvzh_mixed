using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpSceneManager : MonoBehaviour
{
    void Start()
    {
        UIManager.Instance.ExistPanels.Clear();
        UIManager.Instance.OpenPanel<HelpPanel>();
        UIManager.Instance.OpenPanel<MaskPanel>();
        AudioManager.Instance.StopBGM();
        AudioManager.Instance.PlaySFX("Paper");
    }
}
