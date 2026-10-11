using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemData
{
    public string itemId;
    public string itemName;
    public Sprite icon;
    public string description;
    public List<IItemEffect> effects;
    public List<IPassiveItemEffect> passiveEffects;

}
