//using System;
//using System.Collections.Generic;
//using UnityEngine;

//public class PhotoManager : MonoBehaviour
//{

//    [SerializeField] private Camera photoCamera;
//    [SerializeField] private LayerMask obstacleMask;
//    [SerializeField] private CameraManager cameraManager;
//    [SerializeField] private List<PhotoTarget> activeTargets = new List<PhotoTarget>();

//    public static event Action<PhotoResult> OnPhotoTaken;

//    private void Awake()
//    {
//        if (Instance == null) Instance = this;
//        else Destroy(gameObject);

//        if (photoCamera == null) photoCamera = Camera.main;
//    }

//    public void RegisterTarget(PhotoTarget target)
//    {
//        if (target != null && !activeTargets.Contains(target))
//            activeTargets.Add(target);
//    }

//    public void UnregisterTarget(PhotoTarget target)
//    {
//        if (target != null && activeTargets.Contains(target))
//            activeTargets.Remove(target);
//    }

//    public PhotoResult TakePhoto()
//    {
//        // 1. Chụp lấy hình ảnh thực tế của khung cảnh ngay tại khoảnh khắc bấm máy
//        Sprite photoImage = PhotoCaptureHelper.CaptureFrameToSprite(photoCamera, 512, 512);

//        // 2. Kiểm tra xem có quái nào lọt vào khung hình không
//        PhotoTarget detectedTarget = GetTargetInFocus();

//        PhotoResult result = new PhotoResult();
//        result.photoThumbnail = photoImage; // Lưu ảnh vào kết quả

//        if (detectedTarget != null && detectedTarget.Data != null)
//        {
//            PhotoTargetData targetData = detectedTarget.Data;

//            // Xử lý x2 tiền thưởng
//            bool isDouble = UnityEngine.Random.value < (cameraManager != null ? cameraManager.DoubleMoneyChance : 0f);
//            int finalReward = isDouble ? targetData.BaseReward * 2 : targetData.BaseReward;

//            result.success = true;
//            result.target = targetData;
//            result.moneyEarned = finalReward;
//            result.isDoubleMoney = isDouble;

//            // Báo cho quái biết nó đã bị chụp
//            detectedTarget.OnPhotographed();
//        }
//        else
//        {
//            // Bấm máy chụp hụt (không có quái, chỉ chụp phong cảnh)
//            result.success = false;
//            result.target = null;
//            result.moneyEarned = 0;
//            result.isDoubleMoney = false;
//        }

//        // Bắn sự kiện ra ngoài (cho GalleryUI và Mission lưu lại)
//        OnPhotoTaken?.Invoke(result);

//        return result;
//    }

//    public PhotoTarget GetTargetInFocus()
//    {
//        PhotoTarget bestTarget = null;
//        float closestDistanceToCenter = float.MaxValue;
//        Vector3 cameraPos = photoCamera.transform.position;

//        foreach (var target in activeTargets)
//        {
//            if (target == null) continue;

//            Vector3 targetPos = target.transform.position;
//            Vector3 viewPos = photoCamera.WorldToViewportPoint(targetPos);

//            // Phải ở trước mặt camera
//            if (viewPos.z <= 0) continue;

//            // Phải nằm trong khung hình [0, 1]
//            if (viewPos.x < 0f || viewPos.x > 1f || viewPos.y < 0f || viewPos.y > 1f) continue;

//            // Kiểm tra vật cản chắn tầm nhìn
//            if (Physics.Linecast(cameraPos, targetPos, obstacleMask)) continue;

//            // Ưu tiên quái ở gần tâm ngắm
//            float distToCenter = Vector2.Distance(new Vector2(viewPos.x, viewPos.y), new Vector2(0.5f, 0.5f));
//            if (distToCenter < closestDistanceToCenter)
//            {
//                closestDistanceToCenter = distToCenter;
//                bestTarget = target;
//            }
//        }

//        return bestTarget;
//    }
//}