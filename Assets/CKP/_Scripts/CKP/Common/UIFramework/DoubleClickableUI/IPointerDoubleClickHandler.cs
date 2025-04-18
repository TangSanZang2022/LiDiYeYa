using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UIFramework
{
    /// <summary>
    /// 双击UI接口
    /// </summary>
    public interface IPointerDoubleClickHandler
    { /// <summary>
      /// 鼠标双击事件
      /// </summary>
        void OnPointDoubleClickHandler(PointerEventData eventDat);
       
    }
}