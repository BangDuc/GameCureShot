using Core.Interact;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interact_Manager : MonoBehaviour
{
    [SerializeField] private LayerMask interactableLayer;
    private Camera mainCamera;
    private IInteractable currentHovered;
    
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        HandleHover();

        HandleClick();
    }

    private void HandleHover()
    {
        if (Mouse.current == null) return;

        // Bắn Raycast 2D từ tọa độ chuột
        Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, 0f, interactableLayer);

        IInteractable target = hit.collider != null ? hit.collider.GetComponent<IInteractable>() : null;

        if (target != currentHovered)
        {
            currentHovered?.OnHoverExit();
            currentHovered = target;
            currentHovered?.OnHoverEnter();
        }
    }
    private void HandleClick()
    {
        if (Mouse.current == null || !Mouse.current.leftButton.wasPressedThisFrame) return;

        if (currentHovered != null)
        {
            Interact(currentHovered);
        }
    }
    public bool Interact(IInteractable target)
    {
        return target != null && target.Interact();
    }

}
