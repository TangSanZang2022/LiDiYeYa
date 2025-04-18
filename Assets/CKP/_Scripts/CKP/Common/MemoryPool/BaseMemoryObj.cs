using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 需要放入内存池的物体
/// </summary>
public class BaseMemoryObj : MonoBehaviour
{
    /// <summary>
    /// 此物体对应的内存池
    /// </summary>
    private BaseMemoryPool memoryPool;
    /// <summary>
    /// 此物体类型
    /// </summary>
    public MemoryPoolObjType objType;
    // Start is called before the first frame update
    void Start()
    {

    }

    /// <summary>
    /// 初始化，出内存池的时候调用
    /// </summary>
    protected virtual void Init(object[] obj = null)
    {

    }
    /// <summary>
    /// 出内存池
    /// </summary>
    public virtual void OutPool(object[] obj = null)
    {
        transform.parent = null;
        Init(obj);

        //memoryPool.RemovebaseMemoryObjs(this);
    }
    /// <summary>
    /// 进入内存池
    /// </summary>
    public virtual void InPool()
    {
        if (memoryPool == null)
        {
            memoryPool = GameFacade.Instance.GetMemoryPool(objType);
        }

        transform.parent = memoryPool.MemoryPool;
        memoryPool.AddbaseMemoryObjs(this);//将此物体放入内存池的列表中


    }


}
