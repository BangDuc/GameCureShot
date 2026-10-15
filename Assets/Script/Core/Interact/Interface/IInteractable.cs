namespace Core.Interact
{
    public interface IInteractable
    {
        bool Interact();
        
        void OnHoverEnter();
        void OnHoverExit();
        bool CanInteract();
    }
}