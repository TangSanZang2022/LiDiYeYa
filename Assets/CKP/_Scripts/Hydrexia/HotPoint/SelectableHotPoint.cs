using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LiDi.CKP
{
    /// <summary>
    /// 可选中的热点
    /// </summary>
    public class SelectableHotPoint : BaseHotPoint
    {

        /// <summary>
        /// 鼠标按下
        /// </summary>
        protected override void OnMouseDown()
        {
            IsSelected = !IsSelected;

        }
    }
}