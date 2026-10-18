using System;
using System.Collections.Generic;

[Serializable]
public class ItemConfig
{
    public string itemId;
    public string itemName;
    public string description;
    public string iconPath;
    public int initialUses = 1;

    public List<ItemEffectConfig> effects =
        new List<ItemEffectConfig>();

    public List<ItemEffectConfig> passiveEffects =
        new List<ItemEffectConfig>();
}

[Serializable]
public class ItemEffectConfig
{
    public string type;
    public float amount;
    public string monsterId;
}

[Serializable]
public class ItemConfigData
{
    public List<ItemConfig> items =
        new List<ItemConfig>();

}