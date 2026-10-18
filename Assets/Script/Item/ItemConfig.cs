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
}

[Serializable]
public class ItemConfigData
{
    public List<ItemConfig> items = new();
}