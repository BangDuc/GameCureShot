using UnityEngine;
using Core.Interact;
using DG.Tweening;

[RequireComponent(typeof(PolygonCollider2D))]
public class ItemInteract_DOTween : MonoBehaviour, IInteractable
{
    public CabinetTransition DOTween_Transition;
    protected Collider2D col;

    protected virtual void Awake()
    {
        DOTween_Transition=GetComponentInParent<CabinetTransition>();
        col = GetComponent<Collider2D>();
    }

    public virtual bool CanInteract()
    {
       
        return true;
    }

    public virtual bool Interact()
    {
        DOTween_Transition.Toggle();
        
        return true;
    }

    public void OnHoverEnter()
    {
      
    }

    public void OnHoverExit()
    {
        
    }

    
}
