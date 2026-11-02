using System.Collections.Generic;
using UnityEngine;

public class ItemEffectLibrary : MonoBehaviour
{
    public List<ItemEffectContainer> effects;

    public Dictionary<string, ItemEffectContainer> dic_effects;

    private void Awake()
    {
        InitDictionaryEffects();
    }

    public void InitDictionaryEffects()
    {
        dic_effects = new Dictionary<string, ItemEffectContainer>();

        foreach (var effect in effects)
        {
            effect.InitEffect();
            dic_effects.Add(effect.ItemId, effect);
        }

    }
    public ItemEffectContainer GetEffects(string id)
    {
        if (!dic_effects.ContainsKey(id)) return null;
        return dic_effects[id];

    }
}
