// 當定義RELEASE時, 原本使用Debug.xxx的函式將會失去作用
// 提升RELEASE版本的效率
#if RELEASE

using System.Diagnostics;
using UnityEngine;

public static class Debug
{
    [Conditional("RELEASE")]
    public static void Break() { }

    [Conditional("RELEASE")]
    public static void ClearDeveloperConsole() { }

    [Conditional("RELEASE")]
    public static void DebugBreak() { }

    [Conditional("RELEASE")]
    public static void DrawLine(Vector3 start, Vector3 end) { }

    [Conditional("RELEASE")]
    public static void DrawLine(Vector3 start, Vector3 end, Color color) { }

    [Conditional("RELEASE")]
    public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration) { }

    [Conditional("RELEASE")]
    public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration, bool depthTest) { }

    [Conditional("RELEASE")]
    public static void DrawRay(Vector3 start, Vector3 dir) { }

    [Conditional("RELEASE")]
    public static void DrawRay(Vector3 start, Vector3 dir, Color color) { }

    [Conditional("RELEASE")]
    public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration) { }

    [Conditional("RELEASE")]
    public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration, bool depthTest) { }

    [Conditional("RELEASE")]
    public static void Log(object message) { }

    [Conditional("RELEASE")]
    public static void Log(object message, UnityEngine.Object context) { }

    [Conditional("RELEASE")]
    public static void LogError(object message) { }

    [Conditional("RELEASE")]
    public static void LogError(object message, UnityEngine.Object context) { }

    [Conditional("RELEASE")]
    public static void LogException(System.Exception exception) { }

    [Conditional("RELEASE")]
    public static void LogException(System.Exception exception, UnityEngine.Object context) { }

    [Conditional("RELEASE")]
    public static void LogWarning(object message) { }

    [Conditional("RELEASE")]
    public static void LogWarning(object message, UnityEngine.Object context) { }
}

#endif