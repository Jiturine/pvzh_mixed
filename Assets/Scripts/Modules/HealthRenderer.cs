using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthRenderer : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        healthText = transform.Find("healthText").GetComponent<TextMeshPro>();
        healthSprite = transform.Find("healthSprite").GetComponent<SpriteRenderer>();
        animator = transform.Find("healthSprite").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

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
        animator.SetBool("Health Shake", true);
        GameManager.playingAnimationCounter++;
        Timer.Register(0.2f, () =>
    {
        animator.SetBool("Health Shake", false);
        GameManager.playingAnimationCounter--;
    });
    }

    public TextMeshPro healthText;
    public SpriteRenderer healthSprite;
    public Animator animator;
}
