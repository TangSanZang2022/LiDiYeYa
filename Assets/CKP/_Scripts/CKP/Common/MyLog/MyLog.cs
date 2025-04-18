using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 打印
/// </summary>
public static class MyLog 
{
    [System.Diagnostics.Conditional("ENABLE_LOG")]
    public static void Log(object message)
    {
        UnityEngine.Debug.Log(message);
    }
}
