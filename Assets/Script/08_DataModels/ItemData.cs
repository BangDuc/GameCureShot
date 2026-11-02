using System;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class ItemData : IIdentifiable
{
    public string itemId;
    public string itemName;
    public string iconPath;
    public string description;

    
    [JsonIgnore]
    public string Id => itemId;

    
    [JsonIgnore]
    private Sprite _icon;

    [JsonIgnore]
    public Sprite Icon
    {
        get
        {
            if (_icon == null && !string.IsNullOrEmpty(iconPath))
            {
                _icon = Resources.Load<Sprite>(iconPath);
            }
            return _icon;
        }
    }
}