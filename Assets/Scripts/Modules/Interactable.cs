using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements.Experimental;

public class Interactable : MonoBehaviour
{
    public bool isPointerOver;
    public bool IsPointerOver
    {
        get => isPointerOver;
        set
        {
            isPointerOver = value;
            if (isPointerOver == true)
            {
                OnPointerEnter();
            }
            else
            {
                OnPointerExit();
            }
        }
    }
    virtual public void OnPointerEnter()
    {

    }
    virtual public void OnPointerExit()
    {

    }
    virtual public void OnPointerClick()
    {

    }
    virtual public void OnPointerUp()
    {

    }
}