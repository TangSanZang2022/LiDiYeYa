using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 只在编译器下打印
/// </summary>
public class Debug : UnityEngine.Debug
{
#if !UNITY_EDITOR
    public static void Log(string str){}
#endif
}
