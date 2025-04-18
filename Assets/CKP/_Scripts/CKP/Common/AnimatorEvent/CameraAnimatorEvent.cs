using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DFDJ
{
    /// <summary>
    /// 动画事件
    /// </summary>
    public class CameraAnimatorEvent : MonoBehaviour
    {
        /// <summary>
        /// 动画完成之后事件
        /// </summary>
        public void AniEndEvent()
        {
            IAnimEvent animEndEvent = GetComponentInChildren<IAnimEvent>();
            if (animEndEvent!=null)
            {
                animEndEvent.AniEndEvent();
            }

        }
    }
}
