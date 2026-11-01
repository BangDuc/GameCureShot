using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventBus
//chỉ dùng cho nghe và phản ứng chứ không nên làm dùng để thay đổi logic hay thay đổi data
//class muốn là Subscriber hay Publisher phải thông qua interface cấm dùng lung tung trong code
{
    public static Dictionary<string, Action> Events = new();
    public static void Subscribe(string eventName, Action callback)
    {
        if (!Events.ContainsKey(eventName))
            Events[eventName] = callback;
        else
            Events[eventName] += callback;
    }

    public static void Unsubscribe(string eventName, Action callback)
    {
        if (Events.ContainsKey(eventName))
            Events[eventName] -= callback;
    }

    public static void Publish(string eventName)
    {
        if (Events.ContainsKey(eventName))
            Events[eventName]?.Invoke();
        //Debug.Log(eventName);
    }
}