using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
namespace MouseSelectObjs
{
    /// <summary>
    /// 能被选中的物体的状态
    /// </summary>
    internal enum MouseSelectObjsState
    {
        /// <summary>
        /// 待机
        /// </summary>
        Standby,
        /// <summary>
        /// 正在被选中
        /// </summary>
        Selecting,
        /// <summary>
        /// 被选中
        /// </summary>
        Selected,
    }
    /// <summary>
    /// 可以被鼠标框选的物体基类
    /// </summary>
    public class BaseCanSelectObj : MonoBehaviour, IMultipleChoiceHandle
    {
        /// <summary>
        /// 物体状态
        /// </summary>
        private MouseSelectObjsState mouseSelectObjsState;
        // Start is called before the first frame update
        protected virtual void Start()
        {

        }

        // Update is called once per frame
        protected virtual void Update()
        {

        }
        /// <summary>
        /// 判断是否在鼠标绘制出来的矩形内
        /// </summary>
        /// <param name="minX"></param>
        /// <param name="maxX"></param>
        /// <param name="minY"></param>
        /// <param name="maxY"></param>
        /// <returns></returns>
        public bool JudgeIsSelected(float minX, float maxX, float minY, float maxY, Action Selectaction)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            if (screenPos.x <= maxX && screenPos.x >= minX && screenPos.y <= maxY && screenPos.y >= minY)
            {
                Selectaction();
                return true;
            }
            EndHandle();
            return false;
        }
        /// <summary>
        /// 完成选中
        /// </summary>
        public virtual void EndHandle()
        {
            if (mouseSelectObjsState == MouseSelectObjsState.Standby)
            {
                return;
            }
            mouseSelectObjsState = MouseSelectObjsState.Standby;
            Debug.Log(string.Format("剔除选中了{0}", name));
        }
        /// <summary>
        /// 已经选中
        /// </summary>
        public virtual void SelectedHandle()
        {
            if (mouseSelectObjsState == MouseSelectObjsState.Selected)
            {
                return;
            }
            mouseSelectObjsState = MouseSelectObjsState.Selected;
            Debug.Log(string.Format("已经选中了{0}", name));
        }
        /// <summary>
        /// 正在选中
        /// </summary>
        public virtual void SelectingHandle()
        {
            if (mouseSelectObjsState == MouseSelectObjsState.Selecting)
            {
                return;
            }
            mouseSelectObjsState = MouseSelectObjsState.Selecting;
            Debug.Log(string.Format("正在选中了{0}", name));
        }
    }
}
