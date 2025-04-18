using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LiDi.CKP
{
    /// <summary>
    /// 热点控制器
    /// </summary>
    public class HotPointController : BaseController
    {
        public HotPointController(GameFacade gameFacade) : base(gameFacade) { }

        /// <summary>
        /// 存放所有热点的字典
        /// </summary>
        private Dictionary<string, BaseHotPoint> allHotPointDict = new Dictionary<string, BaseHotPoint>();
       
        public override void OnInit()
        {
            base.OnInit();
            AddAllHotPoint();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnDestory()
        {
            base.OnDestory();
        }
        /// <summary>
        /// 将所有热点添加到字典中
        /// </summary>
        private void AddAllHotPoint()
        {
            BaseHotPoint[] baseHotPoints = GameObject.FindObjectsOfType<BaseHotPoint>();
            for (int i = 0; i < baseHotPoints.Length; i++)
            {
                int index = i;
                if (allHotPointDict.ContainsKey(baseHotPoints[index].id))
                {
                    Debug.LogWarning("已经存在相同的热点:"+baseHotPoints[index].id);
                }
                else
                {
                    allHotPointDict.Add(baseHotPoints[index].id, baseHotPoints[index]);
                }
                
            }
        }
        /// <summary>
        /// 将热点添加到字典中
        /// </summary>
        /// <param name="baseHotPoint"></param>
        public void AddHotPointIntoDic(BaseHotPoint baseHotPoint)
        {
            if (!allHotPointDict.ContainsKey(baseHotPoint.id))
            {
                allHotPointDict.Add(baseHotPoint.id, baseHotPoint);
            }

        }
        /// <summary>
        /// 设置热点可点击状态
        /// </summary>
        /// <param name="hotPointIdList"></param>
        /// <param name="operability"></param>
        public void SetHotPointOperability(List<string> hotPointIdList,bool operability)
        {

            foreach (string id in allHotPointDict.Keys)
            {
                string currentID = id;
                if (hotPointIdList.Contains(currentID))
                {
                    allHotPointDict[currentID].Operability = operability;
                }
            }
           
        }
        /// <summary>
        /// 设置热点可点击状态
        /// </summary>
        /// <param name="hotPointIdList"></param>
        /// <param name="hotPointId"></param>
        public void SetHotPointOperability(string hotPointId, bool operability)
        {
            allHotPointDict[hotPointId].Operability = operability;
        }


        /// <summary>
        /// 设置热点显示隐藏状态
        /// </summary>
        /// <param name="hotPointIdList"></param>
        /// <param name="activeState"></param>
        public void SetHotPointActive(List<string> hotPointIdList, bool activeState)
        {
            foreach (string id in allHotPointDict.Keys)
            {
                string currentID = id;
                if (hotPointIdList.Contains(currentID))
                {
                    allHotPointDict[currentID].gameObject.SetActive(activeState);
                }
            }
        }

        /// <summary>
        /// 设置热点显示隐藏状态
        /// </summary>
        /// <param name="hotPointIdList"></param>
        /// <param name="activeState"></param>
        public void SetHotPointActive(string hotPointId, bool activeState)
        {
            Debug.Log(hotPointId);
            allHotPointDict[hotPointId].gameObject.SetActive(activeState);
           
        }
        /// <summary>
        /// 重置所有热点到初始状态
        /// </summary>
        public void SetAllHotPointToStartState()
        {
            foreach (var item in allHotPointDict.Keys)
            {
                allHotPointDict[item].SetToStartState();
            }
        }
        /// <summary>
        /// 重置所有热点到初始状态，用于退出时调用
        /// </summary>
        public void SetAllHotPointToStartState_ForQuite()
        {
            foreach (var item in allHotPointDict.Keys)
            {
                allHotPointDict[item].SetToStartState_ForQuite();
            }
        }
        
        /// <summary>
        /// 根据ID获取热点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseHotPoint GetBaseHotPointForID(string id)
        {
            return allHotPointDict[id];
        }
    }
}