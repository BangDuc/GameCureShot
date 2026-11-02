using System;
using UnityEngine;
using Assets.Script.Core.GamePlay.Camera;
using Bang.Lib.Singleton;

public class CameraManager : Singleton<CameraManager>
{
    [Header("Current State")]
    [SerializeField] private CameraState state = CameraState.Ready;

    [Header("Stats (from Upgrade)")]
    [SerializeField] private float cooldownTime = 2.0f;
    [SerializeField] private float healthPercent = 0.05f;
    [SerializeField] private float MoneyPercent = 10f;

    [Header("Runtime Timers")]
    [SerializeField] private float cooldownTimer = 0f;

    [SerializeField] private PhotoManager photoManager;
    [Header("SubClass")]
    [SerializeField] private CameraMoving cameraMoving;
    [SerializeField] private InGamePhotoCapture cameraCapture;

    public CameraState State => state;
    public float CooldownTime => cooldownTime;
    public float HealthPercent => healthPercent;
    public float MoneyPercentVal => MoneyPercent;
    public float CooldownTimer => cooldownTimer;

    private void Start()
    {
        if (cameraCapture == null)
        {
            cameraCapture = GetComponentInChildren<InGamePhotoCapture>(true);
            if (cameraCapture == null)
            {
                cameraCapture = FindAnyObjectByType<InGamePhotoCapture>();
            }
        }
    }

    private void Update()
    {
        UpdateCamera();
    }

    public void UpdateCamera()
    {
        if (cooldownTimer <= 0) 
        {
            cooldownTimer = 0f;
            state = CameraState.Ready;
            return;
        }

        cooldownTimer = Mathf.Max(0f, cooldownTimer - Time.deltaTime);
        state = CameraState.Cooldown;
    }

    public bool CanCapture()
    {
        // 1. Không cho chụp nếu Run đã kết thúc hoặc chưa bắt đầu
        if (RunController.Instance != null && !RunController.Instance.IsRunning)
        {
            return false;
        }

        // 2. Không cho chụp nếu camera đang hồi chiêu
        return state == CameraState.Ready && cooldownTimer <= 0f;
    }

    public void Capture()
    {
        if (!CanCapture()) return;

        if (cameraCapture != null)
        {
            cameraCapture.Chup();
        }
        else
        {
            var cap = FindAnyObjectByType<InGamePhotoCapture>();
            if (cap != null)
            {
                cap.Chup();
            }
        }

        cooldownTimer = cooldownTime;
        state = CameraState.Cooldown;
    }

    public void ApplyUpgrade(UpgradeCamera upgradeData)
    {
        if (upgradeData == null) return;
        cooldownTime = upgradeData.CooldownTime;
        healthPercent = upgradeData.HealthPercent;
        MoneyPercent = upgradeData.MoneyPercent;
    }
}
