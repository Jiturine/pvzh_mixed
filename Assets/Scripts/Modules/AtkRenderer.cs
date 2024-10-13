using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AtkRenderer : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        atkText = transform.Find("atkText").GetComponent<TextMeshPro>();
        atkSprite = transform.Find("atkSprite").GetComponent<SpriteRenderer>();
        animator = transform.Find("atkSprite").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void HideAtk()
    {
        atkText.enabled = false;
        atkSprite.enabled = false;
    }
    public void ShowAtk()
    {
        atkText.enabled = true;
        atkSprite.enabled = true;
    }
    public void AtkShake()
    {
        animator.SetBool("Atk Shake", true);
        GameManager.playingAnimationCounter++;
        Timer.Register(0.2f, () =>
    {
        animator.SetBool("Atk Shake", false);
        GameManager.playingAnimationCounter--;
    });
    }

    public TextMeshPro atkText;
    public SpriteRenderer atkSprite;
    public Animator animator;
}
