using UnityEngine;
using System;

public class GameClock : MonoBehaviour
{
    [SerializeField] RunController controller;
    [SerializeField]int currentHour;
    [SerializeField] int startHour;
    [SerializeField] float realSecondsPerHour;
    public Action<int> OnHourChanged;
    private void FixedUpdate()
    {
        UpdateTime(controller.GetElapsedTime());
    }
    public void StartClock(int hourStart)
    {
        startHour = hourStart;
        currentHour = startHour;
    }

    public void UpdateTime(float totalElapsedTime)
    {
        if (realSecondsPerHour <= 0) return;
        int hoursPassed = Mathf.FloorToInt(totalElapsedTime / realSecondsPerHour);
        int newHour = (startHour + hoursPassed) % 24;
        if (newHour != currentHour)
        {
            currentHour = newHour;
            OnHourChanged?.Invoke(currentHour);
        }
    }
    public int GetCurrentHour()
    {
        return currentHour;
    }
    public bool IsHour(int hour)
    {
        return currentHour == hour;
    }

    public bool IsTimeInRange(int start, int end)
    {
        if(currentHour < start) return false;
        if(currentHour > end) return false;
        return true;
    }





}
