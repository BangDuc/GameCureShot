using UnityEngine;

public class ItemEffectContainer : MonoBehaviour
{
    [SerializeField] private string itemId;

    public string ItemId => itemId;

    public IItemEffect[] Effects { get; private set; }
    public IPassiveItemEffect[] PassiveEffects { get; private set; }

    
    public void ApplyEffect(ItemUseContext context)
    {
        if (Effects == null || Effects.Length == 0)
        {
            Debug.LogWarning($"[ItemEffectContainer] Không tìm thấy IItemEffect nào trên item: {itemId}");
            return;
        }
        foreach (var effect in Effects) 
        {
            effect.Apply(context);
        }
    }

    public void InitEffect()
    {
        Effects = GetComponents<IItemEffect>();

        PassiveEffects = GetComponents<IPassiveItemEffect>();
    }
    
}