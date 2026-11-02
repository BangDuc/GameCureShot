using System;
using Bang.Lib.Singleton;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMoving : Singleton<CameraMoving>
{
    [Header("Tốc độ & Vùng mép chuột")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float edgeBorderPercent = 0.05f;
    [SerializeField] private bool enableEdgeScrolling = true;

    [Header("Giới hạn bối cảnh (Background Bounds)")]
    [SerializeField] private BackgroundBounds backgroundBounds;

    [SerializeField] private GetAxis_Input getAxis;

    [SerializeField] private CinemachineCamera[] allCameras;

    [SerializeField] private CinemachineCamera activeCam;
    [SerializeField] private float minX, maxX, minY, maxY;

    [SerializeField] private float camHalfHeight;
    [SerializeField] private float camHalfWidth;

    [SerializeField] public bool isZoomMode;
    private Transform zoomTargetProxy;

    public BackgroundBounds BackgroundBounds => backgroundBounds;
    public CinemachineCamera ActiveCam => activeCam;
    public float MinX => minX;
    public float MaxX => maxX;
    public float MinY => minY;
    public float MaxY => maxY;
    public float CamHalfHeight => camHalfHeight;
    public float CamHalfWidth => camHalfWidth;

    private void Start()
    {
        isZoomMode = false;

        if (getAxis == null)
        {
            getAxis = GetComponent<GetAxis_Input>();
            if (getAxis == null) getAxis = GetComponentInChildren<GetAxis_Input>(true);
            if (getAxis == null) getAxis = FindFirstObjectByType<GetAxis_Input>();
        }

        if (allCameras == null || allCameras.Length == 0)
        {
            allCameras = FindObjectsByType<CinemachineCamera>(FindObjectsSortMode.None);
        }

        // Mặc định chọn Gameplay Camera ngay từ đầu (allCameras[0])
        if (allCameras != null && allCameras.Length > 0)
        {
            activeCam = allCameras[0];
        }

        if (backgroundBounds == null)
        {
            backgroundBounds = GetComponent<BackgroundBounds>();
            if (backgroundBounds == null) backgroundBounds = FindFirstObjectByType<BackgroundBounds>();
        }

        if (backgroundBounds != null)
        {
            backgroundBounds.CalculateBounds();
        }

        CalculateBounds();
        UpdateActiveCamera();

        if (zoomTargetProxy == null)
        {
            GameObject proxyGo = new GameObject("[Camera_Zoom_Target]");
            zoomTargetProxy = proxyGo.transform;
        }

        Debug.Log($"<color=cyan>[CameraMoving] Khởi tạo hoàn tất! ActiveCam: {(activeCam != null ? activeCam.name : "null")}, Bounds: X[{minX:F1}..{maxX:F1}] Y[{minY:F1}..{maxY:F1}], Speed: {speed}</color>");
    }

    [ContextMenu("Run Self Test")]
    public bool RunSelfTest()
    {
        UpdateActiveCamera();
        if (activeCam == null)
        {
            Debug.LogError("[CameraMoving Test] FAIL: activeCam is null!");
            return false;
        }

        CalculateBounds();
        Vector3 initialPos = transform.position;
        Vector3 testPos = initialPos + new Vector3(1f, 0f, 0f);
        testPos.x = Mathf.Clamp(testPos.x, minX, maxX);
        testPos.y = Mathf.Clamp(testPos.y, minY, maxY);
        transform.position = testPos;

        bool moveable = (transform.position != initialPos || Mathf.Approximately(minX, maxX));
        transform.position = initialPos;

        Debug.Log($"<color=green>[CameraMoving Test] SUCCESS: Camera active={activeCam.name}, Bounds=X[{minX:F1}..{maxX:F1}] Y[{minY:F1}..{maxY:F1}], CanMove={moveable}</color>");
        return true;
    }

    private void Update()
    {
        if (Time.timeScale <= 0f) return;

        if (activeCam == null)
        {
            UpdateActiveCamera();
        }

        if (isZoomMode) return;
        Moving();
    }

    public void UpdateActiveCamera()
    {
        if (allCameras == null || allCameras.Length == 0)
        {
            allCameras = FindObjectsByType<CinemachineCamera>(FindObjectsSortMode.None);
        }

        if (allCameras == null || allCameras.Length == 0) return;

        CinemachineCamera foundLive = null;
        foreach (var cam in allCameras)
        {
            if (cam != null && cam.IsLive)
            {
                foundLive = cam;
                break;
            }
        }

        CinemachineCamera target = foundLive != null ? foundLive : (activeCam != null ? activeCam : allCameras[0]);

        if (target != null && target != activeCam)
        {
            activeCam = target;
            CalculateBounds();
        }
        else if (activeCam == null && target != null)
        {
            activeCam = target;
            CalculateBounds();
        }
    }

    public void CalculateBounds()
    {
        if (activeCam == null)
        {
            if (allCameras != null && allCameras.Length > 0)
            {
                activeCam = allCameras[0];
            }
            else
            {
                return;
            }
        }

        if (backgroundBounds == null)
        {
            backgroundBounds = GetComponent<BackgroundBounds>();
            if (backgroundBounds == null) backgroundBounds = GetComponentInChildren<BackgroundBounds>(true);
            if (backgroundBounds == null) backgroundBounds = FindFirstObjectByType<BackgroundBounds>();
        }

        if (backgroundBounds == null) return;

        backgroundBounds.CalculateBounds();

        Camera cam = Camera.main;
        float aspect = (cam != null && cam.pixelHeight > 0)
            ? cam.aspect
            : ((float)Screen.width / Mathf.Max(1, Screen.height));

        camHalfHeight = activeCam.Lens.OrthographicSize;
        camHalfWidth = camHalfHeight * aspect;

        float bgWidth = backgroundBounds.maxX - backgroundBounds.minX;
        float bgHeight = backgroundBounds.maxY - backgroundBounds.minY;

        if (bgWidth >= camHalfWidth * 2f)
        {
            minX = backgroundBounds.minX + camHalfWidth;
            maxX = backgroundBounds.maxX - camHalfWidth;
        }
        else
        {
            float midX = (backgroundBounds.minX + backgroundBounds.maxX) * 0.5f;
            minX = midX;
            maxX = midX;
        }

        if (bgHeight >= camHalfHeight * 2f)
        {
            minY = backgroundBounds.minY + camHalfHeight;
            maxY = backgroundBounds.maxY - camHalfHeight;
        }
        else
        {
            float midY = (backgroundBounds.minY + backgroundBounds.maxY) * 0.5f;
            minY = midY;
            maxY = midY;
        }

        if (minX > maxX) { float temp = minX; minX = maxX; maxX = temp; }
        if (minY > maxY) { float temp = minY; minY = maxY; maxY = temp; }
    }

    public void SetBackgroundBounds(BackgroundBounds newBounds, Vector3? newCameraPos = null)
    {
        if (newBounds == null) return;
        backgroundBounds = newBounds;
        newBounds.CalculateBounds();
        CalculateBounds();

        if (newCameraPos.HasValue)
        {
            Vector3 pos = newCameraPos.Value;
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
            pos.z = transform.position.z;
            transform.position = pos;
        }
        else
        {
            Vector3 clampedPos = transform.position;
            clampedPos.x = Mathf.Clamp(clampedPos.x, minX, maxX);
            clampedPos.y = Mathf.Clamp(clampedPos.y, minY, maxY);
            transform.position = clampedPos;
        }
    }

    public void PanToPosition(Vector3 targetPos, float duration = 0.5f, Action onComplete = null)
    {
        targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);
        targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);
        targetPos.z = transform.position.z;

        transform.DOMove(targetPos, duration).SetEase(Ease.InOutQuad).OnComplete(() => onComplete?.Invoke());
    }

    public void Moving()
    {
        if (Time.timeScale <= 0f) return;

        if (activeCam == null)
        {
            UpdateActiveCamera();
            if (activeCam == null) return;
        }

        Vector2 inputDir = Vector2.zero;

        // 1. Nhận input từ GetAxis_Input nếu có
        if (getAxis != null && getAxis.axisInput != Vector2.zero)
        {
            inputDir = getAxis.axisInput;
        }

        // 2. Direct Keyboard fallback (WASD / Mũi tên)
        if (inputDir == Vector2.zero)
        {
            float h = 0f;
            float v = 0f;

            if (Keyboard.current != null)
            {
                if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) h -= 1f;
                if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) h += 1f;
                if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) v -= 1f;
                if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) v += 1f;
            }

            if (Mathf.Approximately(h, 0f) && Mathf.Approximately(v, 0f))
            {
                try
                {
                    h = Input.GetAxisRaw("Horizontal");
                    v = Input.GetAxisRaw("Vertical");
                }
                catch {}
            }

            inputDir = new Vector2(h, v);
        }

        // 3. Rê chuột ra mép màn hình (Edge scrolling)
        if (enableEdgeScrolling && inputDir == Vector2.zero)
        {
            Vector2 mousePos = Input.mousePosition;
            if (mousePos.x >= 0 && mousePos.x <= Screen.width && mousePos.y >= 0 && mousePos.y <= Screen.height)
            {
                float borderX = Screen.width * edgeBorderPercent;
                float borderY = Screen.height * edgeBorderPercent;

                float edgeH = 0f;
                float edgeV = 0f;

                if (mousePos.x <= borderX) edgeH = -1f;
                else if (mousePos.x >= Screen.width - borderX) edgeH = 1f;

                if (mousePos.y <= borderY) edgeV = -1f;
                else if (mousePos.y >= Screen.height - borderY) edgeV = 1f;

                inputDir = new Vector2(edgeH, edgeV);
            }
        }

        if (inputDir.sqrMagnitude > 1f)
        {
            inputDir.Normalize();
        }

        if (inputDir != Vector2.zero)
        {
            Vector2 movePos = inputDir * speed * Time.deltaTime;
            Vector3 clampedPos = transform.position + new Vector3(movePos.x, movePos.y, 0f);
            clampedPos.x = Mathf.Clamp(clampedPos.x, minX, maxX);
            clampedPos.y = Mathf.Clamp(clampedPos.y, minY, maxY);
            transform.position = clampedPos;
        }
    }

    public void ChangeCamera(CameraIndex index)
    {
        var cameraNew = GetCamera(index);
        if (cameraNew == null) return;
        if (cameraNew == activeCam) return;

        if (activeCam != null)
        {
            int tempPriority = activeCam.Priority;
            activeCam.Priority = cameraNew.Priority;
            cameraNew.Priority = tempPriority;
        }

        activeCam = cameraNew;
        CalculateBounds();
    }

    public CinemachineCamera GetCamera(CameraIndex camIndex)
    {
        int idx = (int)camIndex;
        if (allCameras != null && idx >= 0 && idx < allCameras.Length)
        {
            return allCameras[idx];
        }

        Debug.LogWarning($"Index {idx} vượt quá kích thước mảng allCameras!");
        return null;
    }

    [ContextMenu("Test PhotoMode")]
    public void ChangeCameraPhotoMode()
    {
        if (isZoomMode) isZoomMode = false;
        ChangeCamera(CameraIndex.PhotoMode);
    }

    [ContextMenu("Test GameplayMode")]
    public void ChangeCameraGamePlayMode()
    {
        if (isZoomMode) isZoomMode = false;
        ChangeCamera(CameraIndex.Gameplay);
    }

    public void ChangeZoomMode(Transform Zoom_object)
    {
        isZoomMode = true;

        if (zoomTargetProxy == null)
        {
            GameObject proxyGo = new GameObject("[Camera_Zoom_Target]");
            zoomTargetProxy = proxyGo.transform;
        }

        zoomTargetProxy.position = new Vector3(Zoom_object.position.x, Zoom_object.position.y, -10f);

        ChangeCamera(CameraIndex.ZoomMode);
        if (activeCam != null)
        {
            activeCam.Follow = zoomTargetProxy;
            activeCam.LookAt = zoomTargetProxy;
        }
    }
}

public enum CameraIndex
{
    Gameplay = 0,
    PhotoMode = 1,
    ZoomMode = 2
}
