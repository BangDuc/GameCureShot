using System.Security.Cryptography;
using UnityEngine;

public class SceneBounds: MonoBehaviour
{
    public SpriteRenderer backgroundRenderer;

    
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    void Start()
    {
        CalculateBounds();
    }

    public void CalculateBounds()
    {
        if (backgroundRenderer != null)
        {
            // Lấy khối hộp bao quanh đối tượng trong không gian World Space
            Bounds bounds = backgroundRenderer.bounds;

            // bounds.min là góc dưới cùng bên trái (tọa độ nhỏ nhất)
            minX = bounds.min.x;
            minY = bounds.min.y;

            // bounds.max là góc trên cùng bên phải (tọa độ lớn nhất)
            maxX = bounds.max.x;
            maxY = bounds.max.y;

            // In ra Console để kiểm tra thử
            Debug.Log($"Biên BG -> Trái (minX): {minX} | Phải (maxX): {maxX} | Dưới (minY): {minY} | Trên (maxY): {maxY}");
        }
        else
        {
            Debug.LogError("Vui lòng gán SpriteRenderer của Background vào Script!");
        }
    }
}
