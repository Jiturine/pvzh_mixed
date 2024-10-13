using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class Environment : MonoBehaviour
{
    // Start is called before the first frame update
    protected void Start()
    {
        spriteRenderer = lines[lineIndex].GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = SpriteManager.environmentSprite[ID];
    }

    // Update is called once per frame
    void Update()
    {

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
