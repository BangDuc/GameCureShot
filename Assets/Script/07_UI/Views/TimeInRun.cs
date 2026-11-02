using System;
using TMPro;
using UnityEngine;

public class TimeInRun : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TimeUI;

    private void Awake()
    {
        if (TimeUI == null)
        {
            TimeUI = GetComponent<TextMeshProUGUI>();
        }
    }

    private void Update()
    {
        if (TimeUI == null) return;

        if (RunController.Instance != null)
        {
            TimeUI.text = ConvertTime(RunController.Instance.GetRemainingTime());
        }
    }

    public string ConvertTime(float time)
    {
        if (time <= 0f) return "00:00";
        int totalSeconds = Mathf.FloorToInt(time);
        int min = totalSeconds / 60;
        int sec = totalSeconds % 60;
        return $"{min:00}:{sec:00}";
    }
}
