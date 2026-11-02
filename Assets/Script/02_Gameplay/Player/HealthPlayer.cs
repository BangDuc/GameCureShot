using UnityEngine;

public class HealthPlayer : MonoBehaviour
{
   public int maxHealth;

    public int currentHealth;

    public void Heal(int heal_hp)
    {
        if (currentHealth >= maxHealth) return;

        currentHealth = Mathf.Clamp(currentHealth += heal_hp, 0, maxHealth);
    }

    public void HealthDeduct(int d_hp)
    {
        if (!CanHealthDeduct(d_hp)) return;
        currentHealth -= d_hp;
    }

    public bool CanHealthDeduct(int d_hp)
    {
        if (currentHealth < d_hp) return false;
        return true;
    }
}
