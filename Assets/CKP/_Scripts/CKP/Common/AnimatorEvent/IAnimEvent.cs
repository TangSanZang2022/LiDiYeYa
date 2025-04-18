using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DFDJ
{
    /// <summary>
    /// 结束动画事件接口
    /// </summary>
    public interface IAnimEvent
    {
        /// <summary>
        /// 是否已经添加的事件
        /// </summary>
        bool IsAddEvent { get; set; }
        /// <summary>
        /// 动画开始时事件
        /// </summary>
        void AniStartEvent();
        /// <summary>
        /// 中途任意时刻退出
        /// </summary>
        void QuitAniMidway();
        /// <summary>
        /// 动画结束事件
        /// </summary>
        void AniEndEvent();



    }
}
