using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        healthText = transform.Find("healthText").GetComponent<TextMeshProUGUI>();
        healthImage = transform.Find("healthImage").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public TextMeshProUGUI healthText;
    public Image healthImage;
}
