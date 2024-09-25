using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthRenderer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        healthText = transform.Find("healthText").GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public TextMeshPro healthText;
}
