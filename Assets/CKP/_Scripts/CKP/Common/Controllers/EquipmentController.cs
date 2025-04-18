using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace DFDJ
{
    /// <summary>
    /// 设备管理器
    /// </summary>
    public class EquipmentController : BaseController
    {
        /// <summary>
        /// 当前所在最佳视角的设备
        /// </summary>
        private BaseEquipment currentBestViewEquipment;
        public EquipmentController(GameFacade gameFacade) : base(gameFacade)
        {

        }
        /// <summary>
        /// 存放所有设备的字典
        /// </summary>
        private Dictionary<string, BaseEquipment> allEquipmentDict = new Dictionary<string, BaseEquipment>();
        private BaseEquipment[] equipments;
       
        public override void OnInit()
        {
            SetAllEquipmentController();
            //ReadConfig();
        }

        public void SetAllEquipmentController()
        {
            BaseEquipment[] baseEquipments = GameObject.FindObjectsOfType<BaseEquipment>();
            foreach (BaseEquipment item in baseEquipments)
            {
                if (!allEquipmentDict.ContainsKey(item.GetID()))
                {
                    allEquipmentDict.Add(item.GetID(), item);
                }
                else
                {
                    Debug.LogError(string.Format("allEquipmentDict中已经存在Key为{0}的键", item.GetID()));
                }
            }
            equipments = GameObject.FindObjectsOfType<BaseEquipment>();
        }
        /// <summary>
        /// 将设备添加到字典中
        /// </summary>
        /// <param name="baseEquipment"></param>
        public void AddBaseEquipment(BaseEquipment baseEquipment)
        {
            if (!allEquipmentDict.ContainsKey(baseEquipment.GetID()))
            {
                allEquipmentDict.Add(baseEquipment.GetID(), baseEquipment);
            }
            else
            {
                Debug.LogError(string.Format("allEquipmentDict中已经存在Key为{0}的键", baseEquipment.GetID()));
            }
        }
        /// <summary>
        /// 根据ID查找设备
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseEquipment GetBaseEquipmentForID(string id)
        {
            BaseEquipment baseEquipment;
            if (!allEquipmentDict.TryGetValue(id, out baseEquipment))
            {
                Debug.LogError(string.Format("allEquipmentDict中不经存在Key为{0}的键", id));
            }
            return baseEquipment;
        }
        /// <summary>
        /// 根据设备ID，前往该设备的最佳视角位置
        /// </summary>
        /// <param name="id"></param>
        /// <param name="moveTrans"></param>
        public void GoToEquipmentBestViewPosForID(string id, Transform moveTrans)
        {
            if (currentBestViewEquipment != null)
            {
                currentBestViewEquipment.isAtBestViewPos = false;
            }
            BaseEquipment baseEquipment = GetBaseEquipmentForID(id);
            baseEquipment.GoToBestViewPos(moveTrans);

        }
        /// <summary>
        /// 重置所有isAtBestViewPos
        /// </summary>
        public void ResetAll_isAtBestViewPos()
        {
            foreach (BaseEquipment item in allEquipmentDict.Values)
            {
                if (item.isAtBestViewPos)
                {
                    item.isAtBestViewPos = false;
                }
            }

        }
        /// <summary>
        /// 根据设备所在房间，获取设备
        /// </summary>
        /// <param name="equipmentRoom"></param>
        /// <returns></returns>
        public List<BaseEquipment> GetBaseEquipmentForEquipmentRoom(EquipmentRoom equipmentRoom)
        {
            List<BaseEquipment> baseEquipmentList = new List<BaseEquipment>();
            foreach (var item in allEquipmentDict.Values)
            {
                if (item.equipmentRoom == equipmentRoom)
                {
                    baseEquipmentList.Add(item);
                }
            }
            return baseEquipmentList;
        }
        /// <summary>
        ///获取所有设备
        /// </summary>
        /// <param name="equipmentRoom"></param>
        /// <returns></returns>
        public List<BaseEquipment> GetAllBaseEquipment()
        {
            List<BaseEquipment> baseEquipmentList = new List<BaseEquipment>();
            foreach (var item in allEquipmentDict.Values)
            {
                baseEquipmentList.Add(item);
            }
            return baseEquipmentList;
        }



        public override void OnUpdate()
        {

        }
        public override void OnDestory()
        {

        }
    }
}