using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UpgradeCamera
{
    [SerializeField] private float cooldownTime;
    [SerializeField] private float healthPercent;
    [SerializeField] private float moneyPercent;

    public float CooldownTime { get { return cooldownTime; } set { cooldownTime = value; } }

    public float HealthPercent { get { return healthPercent; } set { healthPercent = value; } }

    public float MoneyPercent { get { return moneyPercent; } set { moneyPercent = value; } }


}