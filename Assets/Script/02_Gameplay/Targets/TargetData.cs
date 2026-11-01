using System;
using UnityEngine;

[Serializable]
public class TargetData: IIdentifiable
{
    [SerializeField] private string targetId;
    [SerializeField] private string targetName;
    [SerializeField] private TargetType targetType;
    [SerializeField] private int moneyReward;

    public string TargetId { get => targetId; set => targetId = value; }
    public string TargetName { get => targetName; set => targetName = value; }
    public TargetType TargetType { get => targetType; set => targetType = value; }
    public int MoneyReward { get => moneyReward; set => moneyReward = value; }

    public string Id => targetId;

    public TargetData() { }

    public TargetData(string id, string name, TargetType type, int reward)
    {
        this.targetId = id;
        this.targetName = name;
        this.targetType = type;
        this.moneyReward = reward;
    }
}

public enum TargetType
{
    Interest,
    Monster
}