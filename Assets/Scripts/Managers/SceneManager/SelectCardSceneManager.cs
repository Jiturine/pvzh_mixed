using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SelectCardSceneManager : MonoBehaviour
{
    void Start()
    {
        UIManager.Instance.ExistPanels.Clear();
        var selectCardPanel = UIManager.Instance.OpenPanel<SelectCardPanel>();
        GameManager.Instance.LoadSelectCardScene(selectCardPanel);
        AudioManager.Instance.PlayBGM("PVZHdeckmusic");
    }
}
