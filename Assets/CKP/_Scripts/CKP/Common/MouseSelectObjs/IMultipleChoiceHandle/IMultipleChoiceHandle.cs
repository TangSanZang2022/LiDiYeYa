using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MouseSelectObjs
{
    /// <summary>
    /// 被鼠标框选之后做出反应的接口
    /// </summary>
    public interface IMultipleChoiceHandle
    {
        /// <summary>
        /// 正在被选中，但是鼠标还没有抬起
        /// </summary>
        void SelectingHandle();
        /// <summary>
        /// 鼠标按键抬起后被选中之后的响应
        /// </summary>
        void SelectedHandle();
        /// <summary>
        /// 操作结束后的响应
        /// </summary>
        void EndHandle();
    }
}
