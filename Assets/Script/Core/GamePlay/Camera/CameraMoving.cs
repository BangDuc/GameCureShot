using Bang.Lib.Singleton;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMoving : Singleton<CameraMoving>
{
    [Header("Tốc độ & Vùng mép chuột")]
    [SerializeField] private float speed = 10f;

    [Header("Giới hạn bối cảnh (Background Bounds)")]
    [SerializeField] private BackgroundBounds backgroundBounds;

    [SerializeField] GetAxis_Input getAxis;

    [SerializeField] private CinemachineCamera[] allCameras;

    [SerializeField]private CinemachineCamera activeCam;
    [SerializeField] private float minX, maxX, minY, maxY;

    [SerializeField] float camHalfHeight;
    [SerializeField] float camHalfWidth;


    [SerializeField]public bool isZoomMode;
    private Transform zoomTargetProxy;
    private void Start()
    {
        isZoomMode = false;
        UpdateActiveCamera();

        GameObject proxyGo = new GameObject("[Camera_Zoom_Target]");
        zoomTargetProxy = proxyGo.transform;
    }

    private void Update()
    {
        if (isZoomMode) return;
        Moving();
    }
    private void UpdateActiveCamera()
    {
        if (allCameras == null || allCameras.Length == 0) return;

        
        foreach (var cam in allCameras)
        {
            if (cam != null && cam.IsLive && cam != activeCam)
            {
                activeCam = cam;
                CalculateBounds(); 
                break;
            }
        }
    }


    public void CalculateBounds()
    {
        if (activeCam == null || Camera.main == null || backgroundBounds == null) return;

        camHalfHeight = activeCam.Lens.OrthographicSize;
        camHalfWidth = camHalfHeight * Camera.main.aspect; 

        minX = backgroundBounds.minX + camHalfWidth;
        maxX = backgroundBounds.maxX - camHalfWidth;
        minY = backgroundBounds.minY + camHalfHeight;
        maxY = backgroundBounds.maxY - camHalfHeight;

        
        if (minX > maxX) { float temp = minX; minX = maxX; maxX = temp; }
        if (minY > maxY) { float temp = minY; minY = maxY; maxY = temp; }
    }

    public void Moving()
    {
        if (getAxis == null || activeCam == null) return;

        Vector2 movePos = getAxis.axisInput * speed * Time.deltaTime;

        Vector3 clampedPos = transform.position + new Vector3(movePos.x, movePos.y, 0f);
        clampedPos.x = Mathf.Clamp(clampedPos.x, minX, maxX);
        clampedPos.y = Mathf.Clamp(clampedPos.y, minY, maxY);
        transform.position = clampedPos;
    }
    
    public void ChangeCamera (CameraIndex index)
    {
        var cameraNew = GetCamera(index);
        if (cameraNew == null) return;
        if (cameraNew == activeCam) return;

        int tempPriority = activeCam.Priority;
        
        activeCam.Priority = cameraNew.Priority;

        cameraNew.Priority = tempPriority;

        activeCam = cameraNew;

        CalculateBounds();
    }
    public CinemachineCamera GetCamera(CameraIndex camIndex)
    {
        int idx = (int)camIndex;
        if (idx >= 0 && idx < allCameras.Length)
        {
            return allCameras[idx];
        }

        Debug.LogWarning($"Index {idx} vượt quá kích thước mảng allCameras!");
        return null;
    }


    [ContextMenu("Test")]
    public void ChangeCameraPhotoMode()
    {
        if(isZoomMode) isZoomMode = false;

        ChangeCamera(CameraIndex.PhotoMode);
    }

    [ContextMenu("Test2")]
    public void ChangeCameraGamePlayMode()
    {
        if (isZoomMode) isZoomMode = false;
        ChangeCamera(CameraIndex.Gameplay);
    }

    //transform position Z phải luôn là -10
    public void ChangeZoomMode(Transform Zoom_object)
    {
        isZoomMode = true;

        
        zoomTargetProxy.position = new Vector3(Zoom_object.position.x, Zoom_object.position.y, -10f);

        ChangeCamera(CameraIndex.ZoomMode);
        activeCam.Follow = zoomTargetProxy;
        activeCam.LookAt = zoomTargetProxy;


    }

}

public enum CameraIndex
{
    Gameplay = 0,
    PhotoMode = 1,

    ZoomMode = 2
    
}
