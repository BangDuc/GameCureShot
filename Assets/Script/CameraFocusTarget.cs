using UnityEngine;

public class CameraFocusTarget : MonoBehaviour
{
    [SerializeField] private int focusOrder;
    [SerializeField] private Transform focusPoint;
    [SerializeField, Min(0.1f)] private float orthographicSize = 2f;

    public int FocusOrder => focusOrder;

    public Vector3 FocusPosition =>
        focusPoint != null ? focusPoint.position : transform.position;

    public float OrthographicSize => orthographicSize;
}