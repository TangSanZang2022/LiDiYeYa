using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LiDi.CKP
{
    /// <summary>
    /// 测量压力表控制器
    /// </summary>
    public class MeterController : BaseController
    {
        public MeterController(GameFacade gameFacade) : base(gameFacade)
        { }
        /// <summary>
        /// 所有压力表列表
        /// </summary>
        private List<BaseMeter> baseMeterMetersList = new List<BaseMeter>();

        public override void OnInit()
        {
            base.OnInit();
            AddAllMeterToList();
        }

        private void AddAllMeterToList()
        {
            baseMeterMetersList.AddRange(GameObject.FindObjectsOfType<BaseMeter>());
        }

        /// <summary>
        /// 根据压力表ID获取压力表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseMeter GetPressureMeterForID(string id)
        {
           return baseMeterMetersList.Find((m) => m.ID == id);
        }

        /// <summary>
        /// 根据压力表ID获取压力表读数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public float GetPressureMeterReadingValueForID(string id)
        {
           // Debug.Log(id);
            return baseMeterMetersList.Find((m) => m.ID == id).Get_readingValue();
        }
        /// <summary>
        /// 设置压力表到目标值
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public void SetPressureMeterToTargetReadingValue(string id, float value)
        {
            baseMeterMetersList.Find((m) => m.ID == id).SetToTargetValue(value);
        }
    }
}