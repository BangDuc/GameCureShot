using Core.Interact;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemInteract_Base : MonoBehaviour, IInteractable
{

    
    public bool CanInteract()
    {
        Debug.Log("CanInteract " + this.gameObject);
        return true;
    }

    public bool Interact()
    {
        Debug.Log("Click "+ this.gameObject);
        return true;
    }

    public void OnHoverEnter()
    {
        Debug.Log("OnHoverEnter " + this.gameObject);
    }

    public void OnHoverExit()
    {
        Debug.Log("OnHoverExit " + this.gameObject);
    }

   
}
