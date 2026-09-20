using Core.Interact;
using UnityEngine;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
