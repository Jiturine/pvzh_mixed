using System.Collections;
using System.Collections.Generic;
using static Game;
using UnityEngine;
using static GameManager;

public class Environment : MonoBehaviour
{
    protected void Start()
    {
        spriteRenderer = lines[lineIndex].GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = SpriteManager.environmentSprite[ID];
    }
    virtual public void Remove()
    {
        spriteRenderer.sprite = null;
    }

    [HideInInspector] public SpriteRenderer spriteRenderer;
    public int lineIndex;
    public Faction faction;
    public int ID;
}
