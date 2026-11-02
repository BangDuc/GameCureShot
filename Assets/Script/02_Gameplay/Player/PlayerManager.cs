using Bang.Lib.Singleton;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] public MoneyPlayer moneyPlayer;
    [SerializeField] public HealthPlayer healthPlayer;

    public void GetHurt(int damage)
    {
        if (damage <= 0) return;

        // Nếu game đang Pause hoặc Run không chạy -> Bỏ qua sát thương
        if (Time.timeScale <= 0f) return;
        if (RunController.Instance != null && !RunController.Instance.IsRunning) return;

        if (RunController.Instance != null && RunController.Instance.IsRunning)
        {
            RunController.Instance.TakeDamage(damage);
        }
        else
        {
            if (healthPlayer != null)
            {
                healthPlayer.HealthDeduct(damage);
            }
            EventBus.Publish("OnPlayerGetHurt");
        }
    }
}
