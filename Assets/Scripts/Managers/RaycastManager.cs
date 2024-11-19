using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.U2D.Sprites;
using UnityEngine;
using static GameManager;

public class RaycastManager : MonoBehaviour
{
    void Awake()
    {
        isPointerOverObjects = new List<Interactable>();
    }
    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D[] hitColliders = Physics2D.OverlapPointAll(mousePosition);
        List<Interactable> currentIsPointerOverObjects = new List<Interactable>();
        foreach (var collider in hitColliders)
        {
            if (collider.TryGetComponent<Interactable>(out var interactable))
            {
                currentIsPointerOverObjects.Add(interactable);
                if (!interactable.IsPointerOver)
                {
                    interactable.IsPointerOver = true;
                    isPointerOverObjects.Add(interactable);
                }
            }
        }
        List<Interactable> pointerExitObjects = isPointerOverObjects.Where(obj => !currentIsPointerOverObjects.Contains(obj)).ToList();
        foreach (var obj in pointerExitObjects)
        {
            obj.IsPointerOver = false;
            isPointerOverObjects.Remove(obj);
        }
        if (Input.GetMouseButtonDown(0))
        {
            foreach (var obj in isPointerOverObjects)
            {
                obj.OnPointerClick();
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            foreach (var obj in isPointerOverObjects)
            {
                obj.OnPointerUp();
            }
            if (SelectedCard != null) SelectedCard = null;
        }
    }
    private List<Interactable> isPointerOverObjects;
}
