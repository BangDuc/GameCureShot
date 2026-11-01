using Core.Interact;
using System.Linq;
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

        //RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, 0f, interactableLayer);

        GameObject hitGame = GetTopInteractableObject(mouseWorldPos, interactableLayer);

        IInteractable target = hitGame != null ? hitGame.GetComponent<IInteractable>() : null;

        if (target != currentHovered)
        {
            currentHovered?.OnHoverExit();
            currentHovered = target;
            currentHovered?.OnHoverEnter();
        }
    }

    public GameObject GetTopInteractableObject(Vector2 worldPos, LayerMask layerMask)
    {
        // 1. Bắn tia xuyên qua tất cả các collider tại vị trí trỏ chuột
        RaycastHit2D[] hits = Physics2D.RaycastAll(worldPos, Vector2.zero, Mathf.Infinity, layerMask);
        if (hits.Length == 0) return null;

        // 2. Lấy danh sách kèm theo SpriteRenderer tương ứng
        var hitObjects = hits
            .Select(h => new
            {
                Collider = h.collider,
                Renderer = h.collider.GetComponentInParent<SpriteRenderer>()
            })
            .Where(x => x.Renderer != null)
            .OrderByDescending(x => SortingLayer.GetLayerValueFromID(x.Renderer.sortingLayerID)) // Layer cao hơn ưu tiên
            .ThenByDescending(x => x.Renderer.sortingOrder) // Sorting Order lớn hơn ưu tiên
            .ToList();

        // 3. Trả về GameObject của vật thể hiển thị trên cùng
        return hitObjects.Count > 0 ? hitObjects[0].Collider.gameObject : hits[0].collider.gameObject;
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
