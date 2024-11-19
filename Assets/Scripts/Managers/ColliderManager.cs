using System.Collections;
using System.Collections.Generic;
using static Game;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;
using System.Linq;

public class ColliderManager : MonoBehaviour
{
    void Start()
    {
        colliders = new Dictionary<int, Collider2D>();
        colliderID = new Dictionary<Collider2D, int>();
        for (int i = 1; i <= 5; i++)
        {
            Line line = GameObject.Find($"Line{i}").GetComponent<Line>();
            Slot mySlot = line.transform.Find("My Slot").GetComponent<Slot>();
            Slot enemySlot = line.transform.Find("Enemy Slot").GetComponent<Slot>();
            Collider2D myFirstCollider = mySlot.transform.Find("First Pos").GetComponent<Collider2D>();
            Collider2D mySecondCollider = mySlot.transform.Find("Second Pos").GetComponent<Collider2D>();
            Collider2D enemyFirstCollider = enemySlot.transform.Find("First Pos").GetComponent<Collider2D>();
            Collider2D enemySecondCollider = enemySlot.transform.Find("Second Pos").GetComponent<Collider2D>();
            if (gameMode == GameMode.Online)
            {
                if (GameManager.Instance.IsServer)
                {
                    colliders.Add(100 + i * 10 + 1, myFirstCollider);
                    colliderID.Add(myFirstCollider, 100 + i * 10 + 1);

                    colliders.Add(100 + i * 10 + 2, mySecondCollider);
                    colliderID.Add(mySecondCollider, 100 + i * 10 + 2);

                    colliders.Add(200 + i * 10 + 1, enemyFirstCollider);
                    colliderID.Add(enemyFirstCollider, 200 + i * 10 + 1);

                    colliders.Add(200 + i * 10 + 2, enemySecondCollider);
                    colliderID.Add(enemySecondCollider, 200 + i * 10 + 2);
                }
                else
                {
                    colliders.Add(200 + i * 10 + 1, myFirstCollider);
                    colliderID.Add(myFirstCollider, 200 + i * 10 + 1);

                    colliders.Add(200 + i * 10 + 2, mySecondCollider);
                    colliderID.Add(mySecondCollider, 200 + i * 10 + 2);

                    colliders.Add(100 + i * 10 + 1, enemyFirstCollider);
                    colliderID.Add(enemyFirstCollider, 100 + i * 10 + 1);

                    colliders.Add(100 + i * 10 + 2, enemySecondCollider);
                    colliderID.Add(enemySecondCollider, 100 + i * 10 + 2);
                }
            }
            else
            {
                colliders.Add(100 + i * 10 + 1, myFirstCollider);
                colliderID.Add(myFirstCollider, 100 + i * 10 + 1);

                colliders.Add(100 + i * 10 + 2, mySecondCollider);
                colliderID.Add(mySecondCollider, 100 + i * 10 + 2);

                colliders.Add(200 + i * 10 + 1, enemyFirstCollider);
                colliderID.Add(enemyFirstCollider, 200 + i * 10 + 1);

                colliders.Add(200 + i * 10 + 2, enemySecondCollider);
                colliderID.Add(enemySecondCollider, 200 + i * 10 + 2);
            }
            colliders.Add(300 + i, line.lineCollider);
            colliderID.Add(line.lineCollider, 300 + i);
        }
    }
    static public Dictionary<int, Collider2D> colliders;
    static public Dictionary<Collider2D, int> colliderID;
    static public int GetColliderPos(Collider2D collider)
    {
        return colliderID[collider] % 10 - 1;
    }
    static public List<Collider2D> EnemyColliders => colliders.Where(kvp => kvp.Key / 100 == 2).Select(kvp => kvp.Value).ToList();
    static public List<Collider2D> MyColliders => colliders.Where(kvp => kvp.Key / 100 == 1).Select(kvp => kvp.Value).ToList();
    static public List<Collider2D> LineColliders => colliders.Where(kvp => kvp.Key / 100 == 3).Select(kvp => kvp.Value).ToList();
    static public List<Collider2D> AllColliders => colliders.Select(kvp => kvp.Value).ToList();
}
