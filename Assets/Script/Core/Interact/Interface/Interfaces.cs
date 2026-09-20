namespace Interfaces
{
    public interface IInteractable
    {
        bool Interact();

        // Bổ sung để hỗ trợ Game Feel & Cursor phản hồi
        void OnHoverEnter();
        void OnHoverExit();
        bool CanInteract();
    }
}