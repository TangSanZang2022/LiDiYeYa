using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LiDi.CKP
{

    /// <summary>
    /// 选择工具管理器
    /// </summary>
    public class ToolsController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameFacade"></param>
        public ToolsController(GameFacade gameFacade) : base(gameFacade) { }
        /// <summary>
        /// 存放所有工具的字典
        /// </summary>
        private Dictionary<string, BaseTool> allToolsDic = new Dictionary<string, BaseTool>();
        /// <summary>
        /// 当前选中的工具
        /// </summary>
        private BaseTool currentSelectedTool;
        public override void OnInit()
        {
            base.OnInit();
            AddAllToolsPoint();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }
        /// <summary>
        /// 将所有工具添加到字典中
        /// </summary>
        private void AddAllToolsPoint()
        {
            BaseTool[] baseTools = GameObject.FindObjectsOfType<BaseTool>();
            for (int i = 0; i < baseTools.Length; i++)
            {
                int index = i;
                if (allToolsDic.ContainsKey(baseTools[index].id))
                {
                    Debug.LogWarning("已经存在相同的热点:" + baseTools[index].id);
                }
                else
                {
                    allToolsDic.Add(baseTools[index].id, baseTools[index]);
                }

            }
        }
        /// <summary>
        /// 将工具添加到字典中
        /// </summary>
        /// <param name="baseTool"></param>
        public void AddToolIntoDic(BaseTool baseTool)
        {
            if (!allToolsDic.ContainsKey(baseTool.id))
            {
                allToolsDic.Add(baseTool.id, baseTool);
            }

        }
        /// <summary>
        /// 设置工具可点击状态
        /// </summary>
        /// <param name="baseToolIdList"></param>
        /// <param name="operability"></param>
        public void SetToolOperability(List<string> baseToolIdList, bool operability)
        {

            foreach (string id in allToolsDic.Keys)
            {
                string currentID = id;
                if (baseToolIdList.Contains(currentID))
                {
                    allToolsDic[currentID].Operability = operability;
                }
            }

        }
        /// <summary>
        /// 设置工具可点击状态
        /// </summary>
        /// <param name="hotPointIdList"></param>
        /// <param name="toolId"></param>
        public void SetToolOperability(string toolId, bool operability)
        {
            allToolsDic[toolId].Operability = operability;
        }


        /// <summary>
        /// 设置工具显示隐藏状态
        /// </summary>
        /// <param name="baseToolIdList"></param>
        /// <param name="activeState"></param>
        public void SetBaseToolActive(List<string> baseToolIdList, bool activeState)
        {
            foreach (string id in allToolsDic.Keys)
            {
                string currentID = id;
                if (baseToolIdList.Contains(currentID))
                {
                    allToolsDic[currentID].gameObject.SetActive(activeState);
                }
            }
        }

        /// <summary>
        /// 设置工具显示隐藏状态
        /// </summary>
        /// <param name="hotPointIdList"></param>
        /// <param name="activeState"></param>
        public bool SetBaseToolActive(string hotPointId, bool activeState)
        {
            Debug.Log(hotPointId);
            if (allToolsDic.ContainsKey(hotPointId))
            {
                allToolsDic[hotPointId].gameObject.SetActive(activeState);
                return true;
            }
            else
            {
                return false;
            }

        }
        /// <summary>
        /// 重置所有工具到初始状态
        /// </summary>
        public void SetAllBaseToolToStartState()
        {
            foreach (var item in allToolsDic.Keys)
            {
                allToolsDic[item].ResetToOriginalState();
            }
        }
        /// <summary>
        /// 重置所有工具到初始状态，用于退出时调用
        /// </summary>
        public void SetAllBaseToolToStartState_ForQuite()
        {
            foreach (var item in allToolsDic.Keys)
            {
                allToolsDic[item].SetToStartState_ForQuite();
            }
        }

        /// <summary>
        /// 根据ID获取热点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseTool GetBaseToolForID(string id)
        {
            return allToolsDic[id];
        }
        /// <summary>
        /// 获取当前选中的热点
        /// </summary>
        /// <returns></returns>
        public BaseTool Get_currentSelectedTool()
        {
            return currentSelectedTool;
        }
        /// <summary>
        /// 设置当前选中的工具
        /// </summary>
        /// <param name="baseTool"></param>
        public void Set_currentSelectedTool(BaseTool baseTool)
        {
            if (currentSelectedTool!=null&& baseTool!=null)
            {
                currentSelectedTool.IsSelected = false;
            }
            currentSelectedTool = baseTool;
           // Debug.Log(currentSelectedTool);
        }
        /// <summary>
        /// 根据工具ID选中工具
        /// </summary>
        /// <param name="ToolName"></param>
        public void IsSelectToolForToolID(string ToolID,bool isSelect)
        {
            if (allToolsDic.ContainsKey(ToolID))
            {
                allToolsDic[ToolID].IsSelected = isSelect;
            }
            else
            {
                Debug.LogError("工具字典中不存在ID为：" + ToolID + "的工具");
            }

        }
        public override void OnDestory()
        {
            base.OnDestory();
        }
    }
}