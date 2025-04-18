using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 具有ID的数据类接口
/// </summary>
public interface IState 
{
    /// <summary>
    /// 段位编号(唯一性)
    /// </summary>
    string SubsectionNum { get; set; }
}
