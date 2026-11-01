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

    [SerializeField] CameraMoving cameraMoving;

    [SerializeField] InGamePhotoCapture cameraCapture;
    public CameraState State => state;



    private void Update()
    {
        UpdateCamera();
    }

    public void UpdateCamera()
    {
        if (cooldownTimer == 0) 
        {
            state = CameraState.Ready;
            return;
        }

        if (cooldownTimer > 0)
        {
            cooldownTimer=Math.Clamp(cooldownTimer-= Time.deltaTime,0,999);
            state = CameraState.Cooldown;
            return;
        }

    }

    public bool CanCapture()
    {
        return state == CameraState.Ready;
    }
    

    public void Capture()
    {
        if (!CanCapture()) return;
        cameraCapture.Chup();
        cooldownTimer = cooldownTime;
    }

    public void ApplyUpgrade(UpgradeCamera upgradeData)
    {
        cooldownTime = upgradeData.CooldownTime;
        healthPercent = upgradeData.HealthPercent;
        MoneyPercent = upgradeData.MoneyPercent;
    }
}