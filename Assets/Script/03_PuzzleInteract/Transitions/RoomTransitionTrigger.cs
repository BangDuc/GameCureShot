using Core.Interact;
using UnityEngine;

public class RoomTransitionTrigger : MonoBehaviour, IInteractable
{
    [Header("Phòng Đích (Target Room)")]
    [SerializeField] private RoomId targetRoom;
    [SerializeField] private bool immediateTransition = false;
    [SerializeField] private string hoverDescription = "Đi sang phòng khác";

    [Header("Hiệu Ứng Rê Chuột (Hover FX)")]
    [SerializeField] private Transform visualTransform;
    [SerializeField] private Vector3 hoverScale = new Vector3(1.08f, 1.08f, 1f);
    private Vector3 originalScale = Vector3.one;

    public RoomId TargetRoom => targetRoom;
    public string HoverDescription => hoverDescription;

    private void Awake()
    {
        if (visualTransform == null) visualTransform = transform;
        originalScale = visualTransform.localScale;
    }

    public void Setup(RoomId room, bool immediate = false)
    {
        targetRoom = room;
        immediateTransition = immediate;
    }

    public bool CanInteract()
    {
        return RoomManager.Instance != null &&
               !RoomManager.Instance.IsTransitioning &&
               RoomManager.Instance.CurrentRoom != targetRoom;
    }

    public bool Interact()
    {
        if (!CanInteract()) return false;

        Debug.Log($"[RoomTransitionTrigger] Kích hoạt chuyển sang {targetRoom}");
        RoomManager.Instance.SwitchRoom(targetRoom, immediateTransition);
        return true;
    }

    public void OnHoverEnter()
    {
        if (visualTransform != null)
        {
            visualTransform.localScale = Vector3.Scale(originalScale, hoverScale);
        }
    }

    public void OnHoverExit()
    {
        if (visualTransform != null)
        {
            visualTransform.localScale = originalScale;
        }
    }
}
