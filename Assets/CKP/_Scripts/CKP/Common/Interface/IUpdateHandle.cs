using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 /// <summary>
 ///  更新接口
 /// </summary>
public interface IUpdateHandle
{
    /// <summary>
    ///  更新操作
    /// </summary>
    void  UpdateHandle(object data);
    /// <summary>
    /// 还原为初始状态
    /// </summary>
    void ReductionNormalState();

     
}
