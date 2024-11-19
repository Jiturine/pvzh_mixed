using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthRenderer : MonoBehaviour
{
    void Awake()
    {
        healthText = transform.Find("healthText").GetComponent<TextMeshPro>();
        healthSprite = transform.Find("healthSprite").GetComponent<SpriteRenderer>();
        animator = transform.Find("healthSprite").GetComponent<Animator>();
    }
    public void HideHealth()
    {
        healthText.enabled = false;
        healthSprite.enabled = false;
    }
    public void ShowHealth()
    {
        healthText.enabled = true;
        healthSprite.enabled = true;
    }
    public void HealthShake()
    {
        animator.Play("HealthShake");
    }

    public TextMeshPro healthText;
    public SpriteRenderer healthSprite;
    public Animator animator;
}
