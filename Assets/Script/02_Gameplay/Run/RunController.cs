using Assets.Script.Core.GamePlay;
using Assets.Script.Core.GameRoot;
using Bang.Lib.Singleton;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RunController : Singleton<RunController>
{
    public const int DEFAULT_MAX_RUN_HEALTH = 100;
    public const float DEFAULT_RUN_DURATION = 500f;

    [Header("Run Timer")]
    [SerializeField] private float remainingTime = DEFAULT_RUN_DURATION;
    [SerializeField] private float elapsedTime = 0f;

    [Header("Run Health")]
    [SerializeField] private int maxRunHealth = DEFAULT_MAX_RUN_HEALTH;
    [SerializeField] private int runHealth = DEFAULT_MAX_RUN_HEALTH;
    [Tooltip("Lượng máu trừ mỗi lần chụp nếu không dùng % từ CameraManager")]
    [SerializeField] private int captureHealthCost = 10;

    [Header("Run Statistics")]
    [SerializeField] private int totalPhotosTaken = 0;
    [SerializeField] private int successfulPhotosCount = 0;
    [SerializeField] private int earnedMoney = 0;
    [SerializeField] private int totalDamageTaken = 0;

    [Header("Run State")]
    [SerializeField] private bool isRunning = false;
    [SerializeField] private bool isRunFinished = false;
    [SerializeField] private BuffState buffState;

    private readonly HashSet<string> capturedTargetIds = new HashSet<string>();
    private RunResult currentRunResult;
    private GameData gameData;

    public BuffState Buffs => buffState;
    public int CurrentHealth => runHealth;
    public int MaxRunHealth => maxRunHealth;
    public int PermanentHealth => gameData != null ? gameData.permanentHealth : 4;
    public int EarnedMoney => earnedMoney;
    public int TotalPhotosTaken => totalPhotosTaken;
    public int SuccessfulPhotosCount => successfulPhotosCount;
    public bool IsRunning => isRunning && !isRunFinished;
    public bool IsRunFinished => isRunFinished;
    public GameData GameData => gameData;
    public RunResult Result => currentRunResult;

    public event Action<int> OnDamageTaken;
    public event Action<int, int> OnHealthChanged;
    public event Action<int> OnMoneyChanged;
    public event Action<int> OnPhotosChanged;
    public event Action OnHealthDepleted;
    public event Action<RunResult> OnRunFinished;

    public void Initialize(GameData gameData)
    {
        this.gameData = gameData;
    }

    private void FixedUpdate()
    {
        if (isRunning && !isRunFinished)
        {
            UpdateRun();
        }
    }

    public void KhoiTao()
    {
        remainingTime = DEFAULT_RUN_DURATION;
        elapsedTime = 0f;
        maxRunHealth = DEFAULT_MAX_RUN_HEALTH;
        runHealth = maxRunHealth;
        totalPhotosTaken = 0;
        successfulPhotosCount = 0;
        earnedMoney = 0;
        totalDamageTaken = 0;
        capturedTargetIds.Clear();
        isRunFinished = false;
        currentRunResult = null;
        isRunning = true;
        buffState = new BuffState();

        SyncWithPlayerManager();
        OnHealthChanged?.Invoke(runHealth, maxRunHealth);
        OnMoneyChanged?.Invoke(earnedMoney);
        OnPhotosChanged?.Invoke(totalPhotosTaken);
    }

    [ContextMenu("Test Start Run")]
    public void StartRun()
    {
        KhoiTao();
    }

    private void UpdateRun()
    {
        if (!isRunning || isRunFinished) return;

        remainingTime -= Time.fixedDeltaTime;
        elapsedTime += Time.fixedDeltaTime;

        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            Debug.Log("<color=yellow>[RunController] Hết thời gian sinh tồn (500s)! Kết thúc Run.</color>");
            EndRun(failed: false);
        }
    }

    public void OnPhotoCaptured(PhotoResult result)
    {
        if (!isRunning || isRunFinished) return;

        // 1. Trừ máu Run Health khi chụp
        int cost = captureHealthCost;
        if (CameraManager.Instance != null && CameraManager.Instance.HealthPercent > 0)
        {
            cost = Mathf.Max(1, Mathf.RoundToInt(maxRunHealth * CameraManager.Instance.HealthPercent));
        }
        TakeDamage(cost);

        // 2. Cập nhật số lượng ảnh đã chụp
        totalPhotosTaken++;
        OnPhotosChanged?.Invoke(totalPhotosTaken);

        // 3. Nếu chụp trúng Target hợp lệ: cộng tiền
        if (result != null && result.success && result.target != null)
        {
            string targetId = result.target.TargetId;
            if (!string.IsNullOrEmpty(targetId) && !capturedTargetIds.Contains(targetId))
            {
                capturedTargetIds.Add(targetId);
                successfulPhotosCount++;
                int reward = result.target.MoneyReward;

                // Nếu có buff nhân đôi tiền thưởng từ Coin
                if (buffState != null && buffState.hasCoin && !buffState.coinUsed)
                {
                    buffState.coinUsed = true;
                    reward *= 2;
                    Debug.Log($"<color=yellow>[RunController] Buff Coin kích hoạt! Nhân đôi tiền thưởng: ${reward}</color>");
                }

                earnedMoney += reward;
                result.moneyEarned = reward;

                if (PlayerManager.Instance != null && PlayerManager.Instance.moneyPlayer != null)
                {
                    PlayerManager.Instance.moneyPlayer.EarnMoney(reward);
                }

                OnMoneyChanged?.Invoke(earnedMoney);
                Debug.Log($"<color=green>[RunController] Target hợp lệ '{result.target.TargetName}' (ID: {targetId}). Thưởng: +${reward}. Tổng tiền run: ${earnedMoney}</color>");
            }
            else
            {
                result.moneyEarned = 0;
                Debug.Log($"<color=cyan>[RunController] Target '{result.target?.TargetName}' đã được ghi nhận trước đó.</color>");
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0 || !isRunning || isRunFinished) return;

        // Kiểm tra buff Khiên bảo vệ (Shield)
        if (buffState != null && buffState.hasShield && !buffState.shieldUsed)
        {
            buffState.shieldUsed = true;
            buffState.hasShield = false;
            Debug.Log("<color=green>[RunController] Khiên bảo vệ (Shield) đã chặn toàn bộ sát thương!</color>");
            return;
        }

        runHealth = Mathf.Max(0, runHealth - damage);
        totalDamageTaken += damage;

        SyncWithPlayerManager();
        OnDamageTaken?.Invoke(damage);
        OnHealthChanged?.Invoke(runHealth, maxRunHealth);
        EventBus.Publish("OnPlayerGetHurt");

        if (runHealth <= 0)
        {
            OnHealthDepleted?.Invoke();
            Debug.Log("<color=red>[RunController] Hết máu sinh tồn (Run Health <= 0)! Kết thúc Run (Thất bại).</color>");
            EndRun(failed: true);
        }
    }

    public RunResult FinishRun()
    {
        return EndRun(failed: runHealth <= 0);
    }

    public RunResult EndRun(bool failed)
    {
        if (isRunFinished && currentRunResult != null)
        {
            return currentRunResult;
        }

        isRunning = false;
        isRunFinished = true;

        currentRunResult = new RunResult
        {
            day = gameData != null ? gameData.currentDay : 1,
            money = earnedMoney,
            photoTaken = totalPhotosTaken,
            requiredPhotosCompleted = successfulPhotosCount,
            damageTaken = totalDamageTaken,
            survivalTime = elapsedTime,
            failed = failed || runHealth <= 0
        };

        Debug.Log($"<color=magenta>[RunController] Run kết thúc! Ngày: {currentRunResult.day}, Tiền: ${currentRunResult.money}, Ảnh chụp: {currentRunResult.photoTaken}, Target hợp lệ: {currentRunResult.requiredPhotosCompleted}, Thất bại: {currentRunResult.failed}</color>");

        OnRunFinished?.Invoke(currentRunResult);
        return currentRunResult;
    }

    public void FailRun()
    {
        EndRun(failed: true);
    }

    public void Pause()
    {
        isRunning = false;
    }

    public void Resume()
    {
        if (!isRunFinished)
        {
            isRunning = true;
        }
    }

    public void Heal(int amount)
    {
        if (!isRunning || isRunFinished) return;
        runHealth = Mathf.Clamp(runHealth + amount, 0, maxRunHealth);
        SyncWithPlayerManager();
        OnHealthChanged?.Invoke(runHealth, maxRunHealth);
        Debug.Log($"<color=green>[RunController] Hồi máu: +{amount}. Máu sinh tồn hiện tại: {runHealth}/{maxRunHealth}</color>");
    }

    public float GetElapsedTime() => elapsedTime;
    public float GetRemainingTime() => remainingTime;

    private void SyncWithPlayerManager()
    {
        if (PlayerManager.Instance != null && PlayerManager.Instance.healthPlayer != null)
        {
            PlayerManager.Instance.healthPlayer.currentHealth = runHealth;
            PlayerManager.Instance.healthPlayer.maxHealth = maxRunHealth;
        }
    }
}
