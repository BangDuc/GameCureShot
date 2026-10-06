using UnityEngine;
using UnityEngine.InputSystem;

public class CameraEdgeScroll : MonoBehaviour
{
    [Header("Tốc độ & Vùng mép chuột")]
    [SerializeField] private float panSpeed = 10f;
    [SerializeField] private float edgeBorder = 50f;
    [SerializeField] private float smoothTime = 0.2f;

    [Header("Giới hạn bối cảnh (Background Bounds)")]
    [SerializeField] private SpriteRenderer backgroundRenderer;

    private Camera cam;
    private Vector3 currentVelocity;
    private float minX, maxX, minY, maxY;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        CalculateBounds();
    }

    public void CalculateBounds()
    {
        if (backgroundRenderer == null || cam == null) return;

        Bounds bgBounds = backgroundRenderer.bounds;

        float camHalfHeight = cam.orthographicSize;
        float camHalfWidth = camHalfHeight * cam.aspect;

        minX = bgBounds.min.x + camHalfWidth;
        maxX = bgBounds.max.x - camHalfWidth;
        minY = bgBounds.min.y + camHalfHeight;
        maxY = bgBounds.max.y - camHalfHeight;

        if (minX > maxX) minX = maxX = bgBounds.center.x;
        if (minY > maxY) minY = maxY = bgBounds.center.y;
    }

    private void Update()
    {
        if (cam == null || backgroundRenderer == null || Mouse.current == null)
            return;

        Rect viewport = cam.pixelRect;

        if (viewport.width <= 0f || viewport.height <= 0f || Time.deltaTime <= 0f)
        {
            currentVelocity = Vector3.zero;
            return;
        }

        Vector2 mousePos = Mouse.current.position.ReadValue();

        if (!viewport.Contains(mousePos))
        {
            currentVelocity = Vector3.zero;
            return;
        }

        CalculateBounds();

        Vector3 moveDir = Vector3.zero;

        if (mousePos.x <= viewport.xMin + edgeBorder)
            moveDir.x = -1f;
        else if (mousePos.x >= viewport.xMax - edgeBorder)
            moveDir.x = 1f;

        if (mousePos.y <= viewport.yMin + edgeBorder)
            moveDir.y = -1f;
        else if (mousePos.y >= viewport.yMax - edgeBorder)
            moveDir.y = 1f;

        Vector3 targetPosition = transform.position
            + moveDir.normalized * (panSpeed * Time.deltaTime);

        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);
        targetPosition.z = transform.position.z;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref currentVelocity,
            smoothTime
        );
    }
}