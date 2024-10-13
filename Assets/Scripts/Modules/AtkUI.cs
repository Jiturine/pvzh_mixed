using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AtkUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        atkText = transform.Find("atkText").GetComponent<TextMeshProUGUI>();
        atkImage = transform.Find("atkImage").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void HideAtk()
    {
        atkText.enabled = false;
        atkImage.enabled = false;
    }
    public void ShowAtk()
    {
        atkText.enabled = true;
        atkImage.enabled = true;
    }
    public TextMeshProUGUI atkText;
    public Image atkImage;
}
