using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIFramework
{
    /// <summary>
    /// UI面板基类
    /// </summary>
    public class BasePanel : MonoBehaviour
    {
        /// <summary>
        /// 此面板的类型
        /// </summary>
        public UIPanelType panelType;
        /// <summary>
        /// 页面显示出来
        /// </summary>
        public virtual void OnEnter() { }
        /// <summary>
        /// 页面暂停
        /// </summary>
        public virtual void OnPause() { }
        /// <summary>
        /// 页面继续
        /// </summary>
        public virtual void OnResume() { }
        /// <summary>
        /// 页面退出
        /// </summary>
        public virtual void OnExit() { }

        /// <summary>
        /// 设置UI面板类型
        /// </summary>
        /// <param name="uIPanelType"></param>
        public void SetUIPanelType(UIPanelType uIPanelType)
        {
            panelType = uIPanelType;
        }
        /// <summary>
        /// 更新面板数据
        /// </summary>
        /// <param name="panelData"></param>
        public virtual void UpdatePanelData(object panelData)
        {

        }
    }

}