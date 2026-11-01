using System;
using UnityEngine;
[Serializable]
public class TargetAttack : MonoBehaviour, IAffect
{

    public int damage;
    public float attackSpeed;

    public float timeStartAttacking;

    PlayerManager player;
    public void ApplyEffect()
    {
        InvokeRepeating("AttackPlayer", timeStartAttacking, attackSpeed);
    }


    public void AttackPlayer()
    {
        player.GetHurt(damage);
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
