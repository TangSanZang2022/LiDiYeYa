using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
/// <summary>
/// 内存池控制器
/// </summary>
public class MemoryPoolController:BaseController
{
    //private static MemoryPoolController _instance;
    // /// <summary>
    // /// 单例模式
    // /// </summary>
    //public static MemoryPoolController Instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //        {
    //            _instance = new MemoryPoolController();
    //        }
    //        return _instance;
    //    }
    //}
    /// <summary>
    /// 构造方法
    /// </summary>
    public MemoryPoolController(GameFacade gameFacade ):base(gameFacade)
    {
        SetAllMemoryObjTypePathDict();
        CreatMemoryPools();
    }
    public override void OnInit()
    {
        base.OnInit();
    }

    /// <summary>
    /// 存放所有能够回收至内存池的物体的类型对应预制体地址的字典，用于最开始创建物体
    /// </summary>
    private Dictionary<MemoryPoolObjType, MemoryPoolObjInfo> allMemoryObjTypePathDict;
    /// <summary>
    /// 存放所有内存池的字典
    /// </summary>
    private Dictionary<MemoryPoolType, BaseMemoryPool> allMenoryPoolDict = new Dictionary<MemoryPoolType, BaseMemoryPool>();
    /// <summary>
    /// 读取Json给allMemoryObjTypePathDict字典赋值
    /// </summary>
    private void SetAllMemoryObjTypePathDict()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("MemoryPoolObjConfig");
        try
        {
            MemoryPoolObjInfoList memoryPoolObjInfoList = JsonUtility.FromJson<MemoryPoolObjInfoList>(textAsset.text);
            allMemoryObjTypePathDict = new Dictionary<MemoryPoolObjType, MemoryPoolObjInfo>(memoryPoolObjInfoList.memoryPoolObjConfigList.Count);
            foreach (MemoryPoolObjInfo info in memoryPoolObjInfoList.memoryPoolObjConfigList)
            {
                allMemoryObjTypePathDict.Add(info.objType, info);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
       
    }
   
    /// <summary>
    /// 根据配置创建内存池
    /// </summary>
    private void CreatMemoryPools()
    {
        if (allMemoryObjTypePathDict==null)
        {
            return;
        }
        foreach (MemoryPoolObjInfo item in allMemoryObjTypePathDict.Values)
        {
            if (allMenoryPoolDict.ContainsKey(item.poolType))
            {
                continue;
            }
            string memoryPoolClass = item.poolType.ToString() + "MemoryPool";
            Type type = Type.GetType(memoryPoolClass);//得到内存池的类型
            object[] args = new object[] { memoryPoolClass, item.poolType };//内存池构造方法的参数
            //通过反射来创建对应内存池对象
            BaseMemoryPool pool = type.Assembly.CreateInstance(memoryPoolClass,true,BindingFlags.Default,null, args, null,null) as BaseMemoryPool;
            allMenoryPoolDict.Add(pool.poolType,pool);
        }
    }
    /// <summary>
    /// 根据内存池类型获取内存池
    /// </summary>
    /// <param name="memoryPoolType">内存池类型</param>
    /// <returns></returns>
    private BaseMemoryPool GetMemoryPool(MemoryPoolType memoryPoolType)
    {
        return allMenoryPoolDict[memoryPoolType];
    }
    /// <summary>
    /// 根据物体类型的到内存池
    /// </summary>
    /// <param name="memoryPoolObjType">物体类型</param>
    /// <returns></returns>
    public BaseMemoryPool GetMemoryPool(MemoryPoolObjType memoryPoolObjType)
    {
        return GetMemoryPool(allMemoryObjTypePathDict[memoryPoolObjType].poolType);
    }
    /// <summary>
    /// 根据物体类型得到内存池中对应的物体
    /// </summary>
    /// <param name="memoryPoolObjType">物体类型</param>
    /// <returns></returns>
    public BaseMemoryObj GetObjForObjType(MemoryPoolObjType memoryPoolObjType)
    {
        BaseMemoryObj obj = GetMemoryPool(memoryPoolObjType).GetMemoryObj(memoryPoolObjType);
        if (obj == null)//内存池中无法获取此物体则需要创建
        {
            obj = GameObject.Instantiate(Resources.Load<BaseMemoryObj>(allMemoryObjTypePathDict[memoryPoolObjType].prefabPath));
        }
        return obj;
    }


}

