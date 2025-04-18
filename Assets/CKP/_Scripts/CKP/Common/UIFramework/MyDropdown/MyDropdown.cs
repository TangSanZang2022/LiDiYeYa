using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
/// <summary>
/// 自定义Dropdown类
/// </summary>
public class MyDropdown : Dropdown
{
    /// <summary>
    /// 创建遮挡器的时候调用
    /// </summary>
    public UnityAction OnCreateBlocker;
    /// <summary>
    /// 删除遮挡器的时候调用
    /// </summary>
    public UnityAction OnDestroyBlocker;

   
    protected override GameObject CreateBlocker(Canvas rootCanvas)
    {
        if (OnCreateBlocker!=null)
        {
            OnCreateBlocker();
        }
        return base.CreateBlocker(rootCanvas);
    }

    protected override void DestroyBlocker(GameObject blocker)
    {
        if (OnDestroyBlocker!=null)
        {
            OnDestroyBlocker();
        }
        base.DestroyBlocker(blocker);
    }
}
