using UnityEngine;

public class BackgroundBounds : MonoBehaviour
{
    public SpriteRenderer backgroundRenderer;

    [Header("Xem kết quả giới hạn biên tại đây:")]
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    void Awake()
    {
        CalculateBounds();
    }

    public void CalculateBounds()
    {
        if (backgroundRenderer != null)
        {
           
            Bounds bounds = backgroundRenderer.bounds;

            // bounds.min là góc dưới cùng bên trái (tọa độ nhỏ nhất)
            minX = bounds.min.x;
            minY = bounds.min.y;

            // bounds.max là góc trên cùng bên phải (tọa độ lớn nhất)
            maxX = bounds.max.x;
            maxY = bounds.max.y;

            // In ra Console để kiểm tra thử
          
        }
        else
        {
            Debug.Log("Thiếu Sence BG");
        }
    }

    
    void OnDrawGizmos()
    {
        if (backgroundRenderer != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(backgroundRenderer.bounds.center, backgroundRenderer.bounds.size);
        }
    }
}
