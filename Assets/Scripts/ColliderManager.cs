using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColliderManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public static int GetColliderID(Collider2D collider)
    {
        if (collider.name == "myPos")
        {
            Line line = collider.GetComponentInParent<Line>();
            if (GameManager.Instance.IsServer)
            {
                return line.number;
            }
            else
            {
                return line.number + 5;
            }
        }
        else if (collider.name == "enemyPos")
        {
            Line line = collider.GetComponentInParent<Line>();
            if (GameManager.Instance.IsServer)
            {
                return line.number + 5;
            }
            else
            {
                return line.number;
            }
        }
        else
        {
            return -1;
        }
    }
    public static Collider2D GetCollider(int colliderID)
    {
        if (GameManager.Instance.IsServer)
        {
            if (colliderID >= 0 && colliderID < 5)
            {
                return GameManager.line[colliderID].transform.Find("myPos").GetComponent<Collider2D>();
            }
            else if (colliderID >= 5 && colliderID < 10)
            {
                return GameManager.line[colliderID - 5].transform.Find("enemyPos").GetComponent<Collider2D>();
            }
            else
            {
                return null;
            }
        }
        else
        {
            if (colliderID >= 0 && colliderID < 5)
            {
                return GameManager.line[colliderID].transform.Find("enemyPos").GetComponent<Collider2D>();
            }
            else if (colliderID >= 5 && colliderID < 10)
            {
                return GameManager.line[colliderID - 5].transform.Find("myPos").GetComponent<Collider2D>();
            }
            else
            {
                return null;
            }
        }
    }
}
