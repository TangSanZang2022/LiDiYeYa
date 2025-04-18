using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 内存池的类型
/// </summary>
public enum MemoryPoolType
{
    /// <summary>
    /// 默认的
    /// </summary>
    None,
    /// <summary>
    /// 世界UI
    /// </summary>
    WorldUIIcon,
    /// <summary>
    /// 警报灯
    /// </summary>
    AlarmLight,
    /// <summary>
    /// 报警详情UI
    /// </summary>
    AlarmUI,
    /// <summary>
    /// 游客
    /// </summary>
    Passenger
}
