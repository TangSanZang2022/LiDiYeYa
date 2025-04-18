using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 处理接收的数据接口
/// </summary>
public interface IHandleReceivedMsg
{
    /// <summary>
    /// 接收的消息
    /// </summary>
    string receivedMsg { get; set; }

    /// <summary>
    /// 处理方法
    /// </summary>
     void HandleMsg();

}
