using Core.Interact;
using UnityEngine;

public class ItemInteract_Zoom : MonoBehaviour, IInteractable
{
    public bool CanInteract()
    {
        
        return true;
    }

    public bool Interact()
    {
        
        CameraMoving.Instance.ChangeZoomMode(this.transform);
        return true;
    }

    public void OnHoverEnter()
    {
        
    }

    public void OnHoverExit()
    {
        
    }
}
