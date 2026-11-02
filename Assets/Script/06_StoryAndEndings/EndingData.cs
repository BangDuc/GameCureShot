using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EndingData
{
    public EndingId endingId;
    public string endingType;
    public string endingTitle;
    public string description;
    public string imagePath;

    [NonSerialized]
    public Sprite endingImage;

    [NonSerialized]
    public EndingRule endingRule;
}

[Serializable]
public class EndingConfigData
{
    public List<EndingData> endings = new();
}