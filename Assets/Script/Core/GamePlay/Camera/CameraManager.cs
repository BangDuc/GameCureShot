using System;
using UnityEngine;
using Assets.Script.Core.GamePlay.Camera;

public class CameraManager : MonoBehaviour
{

    
    [Header("Current State")]
    [SerializeField] private CameraState state = CameraState.Ready;

    [Header("Stats (from Upgrade)")]
    [SerializeField] private float holdTime = 1.0f;
    [SerializeField] private float cooldownTime = 2.0f;
    [SerializeField] private float healthCostPercent = 0.05f;
    [SerializeField] private float doubleMoneyChance = 0.1f;

    [Header("Runtime Timers")]
    [SerializeField] private float currentHoldTimer = 0f;
    [SerializeField] private float cooldownTimer = 0f;

    //[SerializeField] private PhotoManager photoManager;

    public CameraState State => state;
    public float DoubleMoneyChance => doubleMoneyChance;

    private void Update()
    {
        UpdateCamera();
    }

    public void UpdateCamera()
    {
        
    }

    public bool CanCapture()
    {
        return state == CameraState.Ready;
    }

    public void StartCapture()
    {
        if (!CanCapture()) return;

        currentHoldTimer = 0f;
        state = CameraState.Holding;
    }

    public void CancelCapture()
    {
        if (state == CameraState.Holding)
        {
            currentHoldTimer = 0f;
            state = CameraState.Ready;
        }
    }

    public void Capture()
    {
        
    }

    //public void ApplyUpgrade(UpgradeLevelData levelData)
    //{
    //    holdTime = levelData.HoldTime;
    //    cooldownTime = levelData.CooldownTime;
    //    healthCostPercent = levelData.HealthCostPercent;
    //    doubleMoneyChance = levelData.DoubleMoneyChance;
    //}
}