using System;
using UnityEngine;

[Serializable]
public class TargetData : IIdentifiable
{
    [SerializeField] private string targetId;
    [SerializeField] private string targetName;
    [SerializeField] private TargetType targetType;
    [SerializeField] private int moneyReward;
    [SerializeField] private string spritePath;
    [SerializeField] private string behaviorType;

    public string TargetId { get => targetId; set => targetId = value; }
    public string TargetName { get => targetName; set => targetName = value; }
    public TargetType TargetType { get => targetType; set => targetType = value; }
    public int MoneyReward { get => moneyReward; set => moneyReward = value; }
    public string SpritePath { get => spritePath; set => spritePath = value; }
    public string BehaviorType { get => behaviorType; set => behaviorType = value; }

    public string Id => targetId;

    public TargetData() { }

    public TargetData(string id, string name, TargetType type, int reward, string spritePath = "", string behaviorType = "")
    {
        this.targetId = id;
        this.targetName = name;
        this.targetType = type;
        this.moneyReward = reward;
        this.spritePath = spritePath;
        this.behaviorType = behaviorType;
    }
}

public enum TargetType
{
    Interest,
    Monster
}
