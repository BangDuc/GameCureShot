using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UpgradeLevelData
{
    [SerializeField] private int level;
    [SerializeField] private int price;
    [SerializeField] private float holdTime;
    [SerializeField] private float cooldownTime;
    [SerializeField] private float healthCostPercent;
    [SerializeField] private float doubleMoneyChance;

    public int Level => level;
    public int Price => price;
    public float HoldTime => holdTime;
    public float CooldownTime => cooldownTime;
    public float HealthCostPercent => healthCostPercent;
    public float DoubleMoneyChance => doubleMoneyChance;
}

[CreateAssetMenu(fileName = "CameraUpgradeData", menuName = "Photo/Camera Upgrade Data")]
public class CameraUpgradeData : ScriptableObject
{
    [SerializeField] private string upgradeId;
    [SerializeField] private string upgradeName;
    [SerializeField] private List<UpgradeLevelData> levels = new List<UpgradeLevelData>();

    public string UpgradeId => upgradeId;
    public string UpgradeName => upgradeName;
    public List<UpgradeLevelData> Levels => levels;
}