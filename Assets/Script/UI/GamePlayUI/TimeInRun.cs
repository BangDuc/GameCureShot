using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TimeInRun : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI TimeUI;
     

    
    void Update()
    {
        TimeUI.text = ConvertTime(RunController.Instance.GetRemainingTime());
    }

    public string ConvertTime(float time)
    {
        if(time <= 0) return TimeUI.text;
        int min = (int)time / 60;
        
        return (min +" : "+ "00");
    }
}
