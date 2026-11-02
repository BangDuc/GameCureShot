using UnityEngine;

public class FollowTargetXY : MonoBehaviour
{
    [Header("Target & Offset")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector2 offset = Vector2.zero;

    [Header("Movement Settings")]
    [SerializeField] private bool smooth = true;
    [SerializeField] private float smoothTime = 0.2f;

    private Vector2 currentVelocity;

    // Sử dụng LateUpdate để tránh hiện tượng giật hình (jitter) khi target di chuyển trong Update
    private void LateUpdate()
    {
        if (target == null) return;

        // Vị trí mục tiêu X, Y kết hợp với offset
        Vector2 targetXY = new Vector2(target.position.x + offset.x, target.position.y + offset.y);

        if (smooth)
        {
            // Bám theo mượt mà
            Vector2 newPosXY = Vector2.SmoothDamp(
                transform.position,
                targetXY,
                ref currentVelocity,
                smoothTime
            );

            // Giữ nguyên trục Z của GameObject hiện tại
            transform.position = new Vector3(newPosXY.x, newPosXY.y, transform.position.z);
        }
        else
        {
            // Bám tức thì theo vị trí mục tiêu
            transform.position = new Vector3(targetXY.x, targetXY.y, transform.position.z);
        }
    }

    // Hàm tiện ích để đổi mục tiêu qua code khi cần
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}