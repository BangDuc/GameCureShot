using System;
using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    [Serializable]
    public enum ShrinkSide
    {
        Right, // Giảm Right về 0 (thanh máu cố định bên trái, rút từ phải về)
        Left   // Giảm Left về 0 (thanh máu cố định bên phải, rút từ trái về)
    }

    [Header("Cấu hình")]
    [SerializeField] private RectTransform healthBarRect;
    [SerializeField] private ShrinkSide shrinkSide = ShrinkSide.Right;

    [Tooltip("Khoảng cách tối đa khi đầy máu (ví dụ 450)")]
    [SerializeField] private float maxOffset = 450f;
    [Tooltip("Khoảng cách khi hết máu (thường là 0)")]
    [SerializeField] private float minOffset = 0f;

    /// <summary>
    /// Cập nhật thanh máu tự động theo tỷ lệ máu
    /// </summary>
    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        float t = Mathf.Clamp01(currentHealth / maxHealth);
        // t = 1 (đầy máu) -> 450; t = 0 (hết máu) -> 0
        float currentOffset = Mathf.Lerp(minOffset, maxOffset, t);

        ApplyOffset(currentOffset);
    }

    /// <summary>
    /// Gán trực tiếp giá trị Inspector [450 -> 0]
    /// </summary>
    public void ApplyOffset(float value)
    {
        if (shrinkSide == ShrinkSide.Right)
        {
            // Right tương ứng với -offsetMax.x
            Vector2 offsetMax = healthBarRect.offsetMax;
            offsetMax.x = -value;
            healthBarRect.offsetMax = offsetMax;
        }
        else // ShrinkSide.Left
        {
            // Left tương ứng với offsetMin.x
            Vector2 offsetMin = healthBarRect.offsetMin;
            offsetMin.x = value;
            healthBarRect.offsetMin = offsetMin;
        }
    }

    private void Update()
    {
        UpdateHealth(PlayerManager.Instance.healthPlayer.currentHealth, PlayerManager.Instance.healthPlayer.maxHealth);
    }

}