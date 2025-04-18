using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 控制器基类
/// </summary>
public class BaseController
{
    protected GameFacade facade;
    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="gameFacade"></param>
    public BaseController(GameFacade gameFacade)
    {
        this.facade = gameFacade;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void OnInit() { }
    /// <summary>
    /// 更新
    /// </summary>
    public virtual void OnUpdate() { }

    /// <summary>
    /// 删除
    /// </summary>
    public virtual void OnDestory() { }
}
