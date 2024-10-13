using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CostUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        costText = transform.Find("costText").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public TextMeshProUGUI costText;
}
