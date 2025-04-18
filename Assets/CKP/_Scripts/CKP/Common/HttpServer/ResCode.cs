using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
/// <summary>
/// 接受服务器数据解析实体
/// </summary>
public class ResCode
{
    /// <summary>
    /// 响应Code
    /// </summary>
    public string ResponseCode { get; set; }

    /// <summary>
    /// 响应信息
    /// </summary>
    public string ResponseMsg { get; set; }
}
