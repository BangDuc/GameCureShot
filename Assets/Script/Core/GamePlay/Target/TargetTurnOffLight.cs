using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]

public class TargetTurnOffLight : MonoBehaviour, IAffect
{
    [SerializeField]public List<LightManager> LightsTurnOff;
    [SerializeField] public float timeToStart;
    [SerializeField] public float Duration;

    public void ApplyEffect()
    {        
        TurnOffLights();
    }

    public void CancelEffect()
    {
        
        TurnOnLights();
    }

    public void TurnOffLights()
    {
        if (LightsTurnOff == null) return;
        foreach (var lights in LightsTurnOff) {
            lights.OffAllLight();
        }
    }

    public void TurnOnLights()
    {
        if (LightsTurnOff == null) return;
        foreach (var lights in LightsTurnOff)
        {
            lights.OnAllLight();
        }
    }

    private void Start()
    {
        Invoke("ApplyEffect", timeToStart);
        Invoke("CancelEffect", Duration);
    }
}
