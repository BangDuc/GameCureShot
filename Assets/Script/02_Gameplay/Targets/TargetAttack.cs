using System;
using UnityEngine;

[Serializable]
public class TargetAttack : MonoBehaviour, IAffect
{
    public int damage = 1;
    public float attackSpeed = 2f;
    public float timeStartAttacking = 5f;

    private PlayerManager player;

    public void ApplyEffect()
    {
        CancelEffect();
        float speed = attackSpeed > 0f ? attackSpeed : 2f;
        float delay = timeStartAttacking >= 0f ? timeStartAttacking : 5f;
        InvokeRepeating(nameof(AttackPlayer), delay, speed);
    }

    public void AttackPlayer()
    {
        // Nếu game đang Pause hoặc Run đã dừng -> Không được tấn công
        if (Time.timeScale <= 0f) return;
        if (RunController.Instance != null && !RunController.Instance.IsRunning) return;

        if (player == null) player = PlayerManager.Instance;
        if (player != null)
        {
            int dmg = damage > 0 ? damage : 1;
            player.GetHurt(dmg);
            Debug.Log($"<color=red>[TargetAttack] Quái vật tấn công người chơi! Gây {dmg} sát thương.</color>");
        }
    }

    private void OnEnable()
    {
        player = PlayerManager.Instance;
        ApplyEffect();
    }

    private void OnDisable()
    {
        CancelEffect();
    }

    public void CancelEffect()
    {
        CancelInvoke(nameof(AttackPlayer));
    }
}
