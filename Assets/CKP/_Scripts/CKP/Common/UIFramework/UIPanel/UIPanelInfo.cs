using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
/// <summary>
/// UI面板信息类，用于反序列化
/// </summary>
public class UIPanelInfo : ISerializationCallbackReceiver
{
    [NonSerialized]
    public UIPanelType panelType;
    /// <summary>
    /// 面板类型对应的字符串，解析的时候要转换为UIPanelType枚举类型
    /// </summary>
    public string panelTypeString;
    /// <summary>
    /// 此面板预制体存放在Resources下的路径
    /// </summary>
    public string path;
    /// <summary>
    /// 在将json转换为对象的时候执行
    /// </summary>
    public void OnAfterDeserialize()
    {
        UIPanelType type = (UIPanelType)Enum.Parse(typeof(UIPanelType), panelTypeString);//将字符串转换为枚举
        panelType = type;
    }

    public void OnBeforeSerialize()
    {

    }


}
