using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 屏幕Ui标识鼠标处理事件接口
/// </summary>
public interface IScreenIconHandle
{
    /// <summary>
    /// 鼠标点击
    /// </summary>
    void OnMouseClickIcon();
    /// <summary>
    /// 鼠标进入
    /// </summary>
    void OnMouseEnterIcon();
    /// <summary>
    /// 鼠标移出
    /// </summary>
    void OnMouseExitIcon();
}
