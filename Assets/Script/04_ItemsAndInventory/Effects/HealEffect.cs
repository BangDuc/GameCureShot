using UnityEngine;

public class HealEffect : MonoBehaviour, IItemEffect
{
    [SerializeField, Min(1)] private int healAmount = 15;

    public bool Apply(ItemUseContext context)
    {
        //if (context == null || context.runController == null)
        //    return false;

        //RunController controller = context.runController;

        //int healthBefore = controller.CurrentHealth;

        //controller.Heal(healAmount);

        //return controller.CurrentHealth > healthBefore;

        Debug.Log($"đã hồi {healAmount} máu");
        return true;
    }
}