using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 内存池基类
/// </summary>
public class BaseMemoryPool
{
    /// <summary>
    /// 存放多余物体的内存池，一种内存池只存在一个
    /// </summary>
    public Transform MemoryPool
    {
        get;
        private set;
    }
    /// <summary>
    /// 内存池的类型
    /// </summary>
    public MemoryPoolType poolType
    {
        get;
        private set;
    }
    /// <summary>
    /// 在内存池中的物体
    /// </summary>
    private List<BaseMemoryObj> baseMemoryObjs = new List<BaseMemoryObj>();
    /// <summary>
    /// 构造方法
    /// </summary>
    public BaseMemoryPool(string memoryName, MemoryPoolType poolType)
    {
        if (MemoryPool == null)
        {
            MemoryPool = new GameObject(memoryName).transform;//创建内存池父物体
            MemoryPool.gameObject.SetActive(false);//隐藏
            this.poolType = poolType;
        }
    }
    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void Init()
    {

    }
    /// <summary>
    /// 获取存放所有物体的列表
    /// </summary>
    /// <returns></returns>
    protected List<BaseMemoryObj> GetbaseMemoryObjs()
    {
        return baseMemoryObjs;
    }
    /// <summary>
    /// 将此进入内存池的物体放入列表
    /// </summary>
    /// <param name="baseMemoryObj">进入内存池的物体</param>
    public void AddbaseMemoryObjs(BaseMemoryObj baseMemoryObj)
    {
        baseMemoryObjs.Add(baseMemoryObj);
    }
    /// <summary>
    /// 将不在内存池中的物体移出列表
    /// </summary>
    /// <param name="baseMemoryObj">要移出的物体</param>
    public void RemovebaseMemoryObjs(BaseMemoryObj baseMemoryObj)
    {

        if (baseMemoryObjs.Contains(baseMemoryObj))
        {
            baseMemoryObjs.Remove(baseMemoryObj); 
        }
    }
    /// <summary>
    /// 从内存池中得到物体
    /// </summary>
    /// <param name="baseMemoryObj"></param>
    /// <returns></returns>
    public BaseMemoryObj GetMemoryObj(MemoryPoolObjType memoryPoolObjType)
    {
        BaseMemoryObj baseMemoryObj=null;
        for (int i = 0; i < baseMemoryObjs.Count; i++)
        {
            if (baseMemoryObjs[i].objType== memoryPoolObjType)
            {
                baseMemoryObj=baseMemoryObjs[i];
                baseMemoryObjs.RemoveAt(i);
                break;
            }
        }
        return baseMemoryObj;
    }

}
