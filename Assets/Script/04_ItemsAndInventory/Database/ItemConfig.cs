using System;
using System.Collections.Generic;

[Serializable]
public class ItemConfig
{
    public string itemId;
    public string itemName;
    public string description;
    public string iconPath;
}

[Serializable]
public class ItemConfigData
{
    public List<ItemConfig> items =
        new List<ItemConfig>();

}