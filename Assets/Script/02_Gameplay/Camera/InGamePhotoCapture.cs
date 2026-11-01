using Bang.Lib.Singleton;
using System;
using UnityEngine;
using UnityEngine.UI;

public class InGamePhotoCapture : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Image previewUI; 
    [SerializeField] PhotoManager photoManager;
    public static event Action<PhotoResult> OnPhotoTaken;


    public Sprite CaptureToSprite(int width = 512, int height = 512)
    {
        // 1. Dùng RenderTexture kích thước nhỏ (512x512 là đủ nét cho UI/Object)
        RenderTexture rt = RenderTexture.GetTemporary(width, height, 16);
        cam.targetTexture = rt;
        cam.Render();

        RenderTexture.active = rt;
        // Dùng RGB565 hoặc RGBA32 để tiết kiệm RAM
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        texture.Apply();

        // 2. Dọn dẹp RenderTexture
        cam.targetTexture = null;
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(rt);

        // 3. Tạo Sprite từ Texture để hiển thị thẳng lên màn hình
        Sprite photoSprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f)
        );

        if (previewUI != null) previewUI.sprite = photoSprite;
        return photoSprite;
    }
    public void Chup()
    {
        Sprite sprite = CaptureToSprite();
        photoManager.librarySpriteCapture.Add(sprite);

    }
}