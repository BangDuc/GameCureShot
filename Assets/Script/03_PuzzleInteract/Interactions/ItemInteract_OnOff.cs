using Core.Interact;
using UnityEngine;

public class ItemInteract_OnOff : MonoBehaviour, IInteractable
{
    [SerializeField]GameObject OnOffGameObject;
    public bool CanInteract()
    {
        return true;
    }

    public bool Interact()
    {

        if (OnOffGameObject != null)
        {
            
            bool currentStatus = OnOffGameObject.activeSelf;
            OnOffGameObject.SetActive(!currentStatus);
        }

        return true;
    }

    public void OnHoverEnter()
    {

    }

    public void OnHoverExit()
    {

    }

    
}
