using System;
using System.Collections.Generic;
using UnityEngine;

public class PhotoManager : MonoBehaviour
{
    public static PhotoManager Instance { get; private set; }

    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private List<PhotoTarget> activeTargets = new List<PhotoTarget>();

    // Sự kiện toàn cục để các hệ thống khác (như Mission) lắng nghe
    public static event Action<PhotoResult> OnPhotoTaken;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterTarget(PhotoTarget target)
    {
        if (target != null && !activeTargets.Contains(target))
        {
            activeTargets.Add(target);
        }
    }

    public void UnregisterTarget(PhotoTarget target)
    {
        if (target != null && activeTargets.Contains(target))
        {
            activeTargets.Remove(target);
        }
    }

    public PhotoResult TakePhoto()
    {
        // Tìm mục tiêu hợp lệ đang nằm trong tầm ngắm (ví dụ lấy mục tiêu gần nhất hoặc mục tiêu đầu tiên)
        PhotoTarget detectedTarget = GetTargetInFocus();

        PhotoResult result = new PhotoResult();

        if (detectedTarget != null && detectedTarget.Data != null)
        {
            PhotoTargetData targetData = detectedTarget.Data;

            // Kiểm tra tỉ lệ x2 tiền thưởng từ CameraManager
            bool isDouble = UnityEngine.Random.value < (cameraManager != null ? cameraManager.DoubleMoneyChance : 0f);
            int finalReward = isDouble ? targetData.BaseReward * 2 : targetData.BaseReward;

            result.success = true;
            result.target = targetData;
            result.moneyEarned = finalReward;
            result.isDoubleMoney = isDouble;

            // Kích hoạt logic phản hồi sau khi bị chụp của mục tiêu
            detectedTarget.OnPhotographed();
        }
        else
        {
            result.success = false;
            result.target = null;
            result.moneyEarned = 0;
            result.isDoubleMoney = false;
        }

        // Bắn sự kiện ra ngoài cho Mission/Economy xử lý
        OnPhotoTaken?.Invoke(result);

        return result;
    }

    private PhotoTarget GetTargetInFocus()
    {
        // Logic kiểm tra mục tiêu nào đang nằm trong khung hình camera (Raycast / Viewport / Overlap)
        if (activeTargets.Count > 0)
        {
            return activeTargets[0]; // Trả về mục tiêu đầu tiên để test mẫu
        }
        return null;
    }
}