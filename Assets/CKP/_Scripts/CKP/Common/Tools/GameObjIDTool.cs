using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DFDJ
{
    /// <summary>
    /// 物体ID
    /// </summary>
    public class GameObjIDTool 
    {
        #region//显示隐藏物体ID
        /// <summary>
        /// 建筑物屏幕图标父物体ID
        /// </summary>
        public const string BuildingScreenIconCanvas = "BuildingScreenIconCanvas";
        /// <summary>
        /// 设备屏幕Icon
        /// </summary>
        public const string EquipmentScreenIconCanvas = "EquipmentScreenIconCanvas";
        /// <summary>
        /// 外框特效
        /// </summary>
        public const string WaiKuangTeXiao = "WaiKuangTeXiao";
        #endregion

        #region//相机位置ID
        /// <summary>
        /// 相机开始位置
        /// </summary>
        public const string CamStartPos = "CamStartPos";
        /// <summary>
        /// 园区总览相机位置
        /// </summary>
        public const string PandectCamPos = "PandectCamPos";
        /// <summary>
        /// 重型装配车间相机位置
        /// </summary>
        public const string HeavyEquipmentBulidingCamPos = "HeavyEquipmentBulidingCamPos";
        /// <summary>
        /// 厂区漫游起点位置
        /// </summary>
        public const string FactoryRoamtPos = "FactoryRoamtPos";
        /// <summary>
        /// 重型装配车间固定视角开始位置
        /// </summary>
        public const string HeavyEquipmentRoamtPos = "HeavyEquipmentRoamtPos";
        /// <summary>
        /// 选择厂区自由视角还是固定镜头时相机位置
        /// </summary>
        public const string FactorySelectFixedAngleFreeCameraCamPos = "FactorySelectFixedAngleFreeCameraCamPos";
        /// <summary>
        /// 重型装配车间选择自由视角和固定视角漫游时相机位置
        /// </summary>
        public const string HeavyEquipmentSelectFixedAngleFreeCameraCamPos = "HeavyEquipmentSelectFixedAngleFreeCameraCamPos";
        /// <summary>
        /// FPS 状态下相机位置，即FPSController子物体
        /// </summary>
        public const string FPSCamPos = "FPSCamPos";
        #endregion

        #region//FPS,第一人称控制器位置
        /// <summary>
        /// 厂区漫游FPS开始位置
        /// </summary>
        public const string FactoryFPSStartPos = "FactoryFPSStartPos";
        /// <summary>
        /// 重型装配车间FPS漫游开始位置
        /// </summary>
        public const string HeavyEquipmentFPSStartPos = "HeavyEquipmentFPSStartPos";

        #endregion
    }
}
