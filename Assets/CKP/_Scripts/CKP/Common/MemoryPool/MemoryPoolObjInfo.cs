using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
/// <summary>
/// 能够放入内存池的物体
/// </summary>
public class MemoryPoolObjInfo : ISerializationCallbackReceiver
{
    [NonSerialized]
    /// <summary>
    /// 物体类型
    /// </summary>
    public MemoryPoolObjType objType;
    [NonSerialized]
    /// <summary>
    /// 此类型的物体对应哪个类型的内存池
    /// </summary>
    public MemoryPoolType poolType;
    /// <summary>
    /// 类型string，反序列化之后要转换为MemoryPoolObjType类型
    /// </summary>
    public string prefabTypeStr;
    /// <summary>
    /// 预制体存放在Resources下的位置
    /// </summary>
    public string prefabPath;
    /// <summary>
    /// 类型string，反序列化之后要转换为MemoryPoolType类型
    /// </summary>
    public string memoryPoolTypeStr;
    /// <summary>
    /// 在将json转换为对象的时候执行
    /// </summary>
    public void OnAfterDeserialize()
    {
        objType = (MemoryPoolObjType)Enum.Parse(typeof(MemoryPoolObjType), prefabTypeStr);
        poolType = (MemoryPoolType)Enum.Parse(typeof(MemoryPoolType), memoryPoolTypeStr);
    }

    public void OnBeforeSerialize()
    {

    }
}
[Serializable]
/// <summary>
/// 解析MemoryPoolObjConfig 配置Json文件用到
/// </summary>
public class MemoryPoolObjInfoList
{
    public List<MemoryPoolObjInfo> memoryPoolObjConfigList;
}
