using Assets.Script.Core.GamePlay;
using Bang.Lib.Singleton;
using System;
using UnityEngine;

public class RunController : Singleton<RunController>
{
    
    [SerializeField] private float remainingTime;
    [SerializeField] private float elapsedTime;
    [SerializeField] private int runHealth;
    [SerializeField] private bool isRunning = false;
    [SerializeField] private StarterPotionState starterPotionState;


    public event Action<int> OnDamageTaken;
    public event Action OnHealthDepleted;

 
    private void FixedUpdate()
    {
        if (isRunning)
        {
            UpdateRun();
        }
    }

    public void KhoiTao()
    {
        remainingTime = 500;
        elapsedTime = 0;
        runHealth = 100;
        isRunning = true;
    }
    [ContextMenu ("Test")]
    public void StartRun()
    {
        KhoiTao();
    }

    private void UpdateRun()
    {
        remainingTime -= Time.fixedDeltaTime;
        elapsedTime += Time.fixedDeltaTime;

        //Debug.Log("remainingTime = " + remainingTime + " elapsedTime= " + elapsedTime);
    }

    public RunResult FinishRun()
    {
        // TODO: Xử lý kết thúc run thành công và trả về kết quả
        return null;
    }

    public void FailRun()
    {
        if (runHealth <= 0)
        {
            Debug.Log("Thua Game Pause");
            Pause();
        }
        
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0) return;
        OnDamageTaken?.Invoke(damage);
        runHealth -= damage;

    }

    public void Pause()
    {
        isRunning = false;
    }

    public void Resume()
    {
        isRunning= true;
    }

    public void Heal(int percent)
    {
        OnHealthDepleted?.Invoke();
        runHealth += percent;
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }
    public float GetRemainingTime()
    {
        return remainingTime;
    }
}