using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TargetTurnOffLight : MonoBehaviour, IAffect
{
    [SerializeField] public List<LightManager> LightsTurnOff;
    [SerializeField] public float timeToStart = 5f;
    [SerializeField] public float Duration = 10f;

    public void ApplyEffect()
    {
        TurnOffLights();
    }

    public void CancelEffect()
    {
        CancelInvoke(nameof(ApplyEffect));
        CancelInvoke(nameof(CancelEffect));
        TurnOnLights();
    }

    public void TurnOffLights()
    {
        // Nếu game đang Pause hoặc Run không chạy -> Bỏ qua
        if (Time.timeScale <= 0f) return;
        if (RunController.Instance != null && !RunController.Instance.IsRunning) return;

        // Kiểm tra xem người chơi có buff miễn nhiễm 1 lần tắt đèn hay không
        if (RunController.Instance != null && RunController.Instance.Buffs != null && RunController.Instance.Buffs.hasLightImmunity)
        {
            RunController.Instance.Buffs.hasLightImmunity = false;
            Debug.Log("<color=yellow>[TargetTurnOffLight] Bùa Hộ Mệnh đã kích hoạt! Miễn nhiễm 1 lần quái vật The Shy tắt đèn.</color>");
            return;
        }

        if (LightsTurnOff == null || LightsTurnOff.Count == 0)
        {
            LightsTurnOff = new List<LightManager>(FindObjectsByType<LightManager>(FindObjectsSortMode.None));
        }

        foreach (var lights in LightsTurnOff)
        {
            if (lights != null) lights.OffAllLight();
        }
    }

    public void TurnOnLights()
    {
        if (LightsTurnOff == null) return;
        foreach (var lights in LightsTurnOff)
        {
            if (lights != null) lights.OnAllLight();
        }
    }

    private void OnEnable()
    {
        CancelInvoke(nameof(ApplyEffect));
        CancelInvoke(nameof(CancelEffect));
        float start = timeToStart >= 0f ? timeToStart : 5f;
        float dur = Duration > 0f ? Duration : 10f;
        Invoke(nameof(ApplyEffect), start);
        Invoke(nameof(CancelEffect), start + dur);
    }

    private void OnDisable()
    {
        CancelEffect();
    }
}
