using UnityEngine;

public class ItemEffectContainer : MonoBehaviour
{
    [SerializeField] private string itemId;

    public string ItemId => itemId;

    public IItemEffect[] Effects { get; private set; }
    public IPassiveItemEffect[] PassiveEffects { get; private set; }

    private void Awake()
    {
        Effects = GetComponents<IItemEffect>();

        PassiveEffects =
            GetComponents<IPassiveItemEffect>();
    }
}