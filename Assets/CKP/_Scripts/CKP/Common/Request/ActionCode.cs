using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 根据响应类型Request在Unity中实际要做什么的响应
/// </summary>
public enum ActionCode
{
    /// <summary>
    /// 默认为空
    /// </summary>
    None=0,
    /// <summary>
    /// 更新皮带状态
    /// </summary>
    UpdateBelt=1,
    /// <summary>
    /// 更新摄像头状态
    /// </summary>
    UpdateMonitor =2,
    /// <summary>
    /// 更新物体位置
    /// </summary>
    UpdateObjPos =3,
    /// <summary>
    /// 更新检验台状态
    /// </summary>
    UpdateTestBench =4,
    /// <summary>
    /// 更新自助通道
    /// </summary>
    UpdateAutomatedChannels =5,
    /// <summary>
    /// 更新旅客
    /// </summary>
    UpdatePassenger =6
}
