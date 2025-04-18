using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DFDJ
{
    /// <summary>
    /// 动画参数
    /// </summary>
    public class AnimatorParameter
    {
        #region//相机动画参数
        /// <summary>
        /// 相机漫游动画开始Trigger
        /// </summary>
        public const string StartMove = "StartMove";
        /// <summary>
        /// 中途退出
        /// </summary>
        public const string QuitMidway = "QuitMidway";

        /// <summary>
        /// 厂区相机动画名称
        /// </summary>
        public const string FactoryCameraManYou = "ManYou";
       
        /// <summary>
        /// 重型装配相机动画名称
        /// </summary>
        public const string HeavyEquipmentCamreaManYou = "ManYou";
        #endregion
    }
}
