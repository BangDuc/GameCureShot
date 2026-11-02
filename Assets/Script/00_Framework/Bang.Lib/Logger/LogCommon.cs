using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Bang.Lib.Logger
{
    public static class LogCommon
    {
        [Conditional("UNITY_EDITOR")]
        public static void Log(object message)
        {
            Debug.Log(message);
        }

        [Conditional("UNITY_EDITOR")]
        public static void LogWarning(object message)
        {
            Debug.LogWarning(message);
        }

        [Conditional("UNITY_EDITOR")]
        public static void LogError(object message)
        {
            Debug.LogError(message);
        }

        [Conditional("UNITY_EDITOR")]
        public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 2f)
        {
            Debug.DrawLine(start, end, color, duration);
        }
    }
}