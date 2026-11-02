using Bang.Lib.Singleton;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGamePhotoCapture : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Image previewUI; 
    [SerializeField] private PhotoManager photoManager;
    public static event Action<PhotoResult> OnPhotoTaken;

    private void Awake()
    {
        if (cam == null) cam = Camera.main;
    }

    public Sprite CaptureToSprite(int width = 512, int height = 512)
    {
        if (cam == null) cam = Camera.main;
        if (cam == null) return null;

        // 1. Dùng RenderTexture kích thước nhỏ (512x512)
        RenderTexture rt = RenderTexture.GetTemporary(width, height, 16);
        cam.targetTexture = rt;
        cam.Render();

        RenderTexture.active = rt;
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        texture.Apply();

        // 2. Dọn dẹp RenderTexture
        cam.targetTexture = null;
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(rt);

        // 3. Tạo Sprite từ Texture
        Sprite photoSprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f)
        );

        if (previewUI != null) previewUI.sprite = photoSprite;
        return photoSprite;
    }

    public PhotoResult Chup()
    {
        if (Time.timeScale <= 0f || (RunController.Instance != null && !RunController.Instance.IsRunning))
        {
            Debug.LogWarning("[InGamePhotoCapture] Không thể chụp khi game đang Pause hoặc Run không hoạt động.");
            return null;
        }

        Sprite sprite = CaptureToSprite();

        if (photoManager != null)
        {
            if (photoManager.librarySpriteCapture == null)
            {
                photoManager.librarySpriteCapture = new List<Sprite>();
            }
            if (sprite != null)
            {
                photoManager.librarySpriteCapture.Add(sprite);
            }
        }

        // Xác định Target hợp lệ trong khung hình
        TargetController targetHit = DetectTargetInView();
        PhotoResult result = new PhotoResult();
        result.photoThumbnail = sprite;

        if (targetHit != null && targetHit.Data != null)
        {
            result.success = true;
            result.target = targetHit.Data;
            result.moneyEarned = targetHit.Data.MoneyReward;
            targetHit.OnCaptured();
            Debug.Log($"<color=green>[InGamePhotoCapture] Chụp trúng Target: {targetHit.Data.TargetName} (+${result.moneyEarned})</color>");
        }
        else
        {
            result.success = false;
            result.target = null;
            result.moneyEarned = 0;
            Debug.Log("<color=yellow>[InGamePhotoCapture] Chụp ảnh không có Target nào trong khung hình.</color>");
        }

        // Gửi kết quả về RunController để xử lý Run Health, Money và số ảnh
        if (RunController.Instance != null)
        {
            RunController.Instance.OnPhotoCaptured(result);
        }

        OnPhotoTaken?.Invoke(result);
        return result;
    }

    public TargetController DetectTargetInView()
    {
        Camera activeCamera = cam != null ? cam : Camera.main;
        if (activeCamera == null) return null;

        TargetController[] allTargets = FindObjectsByType<TargetController>(FindObjectsSortMode.None);
        if (allTargets == null || allTargets.Length == 0) return null;

        TargetController bestTarget = null;
        float closestDistanceToCenter = float.MaxValue;
        Vector2 viewportCenter = new Vector2(0.5f, 0.5f);

        foreach (var target in allTargets)
        {
            if (target == null || !target.gameObject.activeInHierarchy || !target.enabled)
                continue;

            Vector3 vp = activeCamera.WorldToViewportPoint(target.transform.position);

            bool inViewport = vp.z > 0f && vp.x >= 0f && vp.x <= 1f && vp.y >= 0f && vp.y <= 1f;

            if (!inViewport)
            {
                var r = target.GetComponentInChildren<Renderer>();
                if (r != null)
                {
                    Vector3 minVp = activeCamera.WorldToViewportPoint(r.bounds.min);
                    Vector3 maxVp = activeCamera.WorldToViewportPoint(r.bounds.max);
                    if (minVp.z > 0f && maxVp.x >= 0f && minVp.x <= 1f && maxVp.y >= 0f && minVp.y <= 1f)
                    {
                        inViewport = true;
                        vp = (minVp + maxVp) * 0.5f;
                    }
                }
            }

            if (inViewport)
            {
                float dist = Vector2.Distance(new Vector2(vp.x, vp.y), viewportCenter);
                if (dist < closestDistanceToCenter)
                {
                    closestDistanceToCenter = dist;
                    bestTarget = target;
                }
            }
        }

        return bestTarget;
    }
}
