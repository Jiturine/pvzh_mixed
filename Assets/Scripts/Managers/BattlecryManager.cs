using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlecryManager : MonoBehaviour
{
    public static BattlecryManager Instance
    {
        get; set;
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        battlecryEntities = new Queue<Entity>();
    }

    // Update is called once per frame
    void Update()
    {
        if (battlecryEntities.Count != 0 && GameManager.playingAnimationCounter == 0 && !isBattlecrying)
        {
            Entity entity = battlecryEntities.Dequeue();
            entity.Battlecry();
        }
    }
    static public void Add(Entity entity)
    {
        Instance.battlecryEntities.Enqueue(entity);
    }
    public Queue<Entity> battlecryEntities;
    public bool isBattlecrying;
}
