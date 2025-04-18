using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 能够放进内存池的物体的类型
/// </summary>
public enum MemoryPoolObjType
{
    /// <summary>
    ///默认类型
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
    /// 旅客
    /// </summary>
    Passenger,

}
