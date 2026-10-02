using Assets.Script.Core.GamePlay.Camera;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PhotoTargetData", menuName = "Photo/Target Data")]
public class PhotoTargetData : ScriptableObject
{
    [SerializeField] private string targetId;
    [SerializeField] private string targetName;
    [SerializeField] private PhotoTargetType targetType;
    [SerializeField] private int baseReward;

    public string TargetId => targetId;
    public string TargetName => targetName;
    public PhotoTargetType TargetType => targetType;
    public int BaseReward => baseReward;
}

public class PhotoResult
{
    public bool success;
    public int moneyEarned;
    public PhotoTargetData target;
    public bool isDoubleMoney;
}

public class PhotoTarget : MonoBehaviour
{
    [SerializeField] private PhotoTargetData data;

    public PhotoTargetData Data => data;

    private void Start()
    {
        // Tự động đăng ký mục tiêu khi xuất hiện nếu cần
        if (PhotoManager.Instance != null && data != null)
        {
            PhotoManager.Instance.RegisterTarget(this);
        }
    }

    private void OnDestroy()
    {
        // Loại bỏ đăng ký khi bị hủy
        if (PhotoManager.Instance != null)
        {
            PhotoManager.Instance.UnregisterTarget(this);
        }
    }

    public void OnPhotographed()
    {
        // Hành vi sau khi bị chụp thành công (hiệu ứng, animation, hủy quái, v.v.)
        Debug.Log($"Mục tiêu {data.TargetName} đã bị chụp!");
    }
}