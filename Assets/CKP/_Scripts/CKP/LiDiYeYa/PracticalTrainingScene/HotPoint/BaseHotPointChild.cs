using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LiDi.CKP
{
    /// <summary>
    /// 热点子物体
    /// </summary>
    public class BaseHotPointChild : MonoBehaviour
    {
        [SerializeField]
        /// <summary>
        /// 状态改变时间
        /// </summary>
        protected float stateChangeTime = 1f;
        /// <summary>
        /// 状态改变时的Tween
        /// </summary>
        protected Tween stateChangeTween;
        /// <summary>
        /// 打开或者关闭之后物体操作的物体
        /// </summary>
       [SerializeField]
        protected List<BaseOpenCloseHandleObj> baseOpenCloseHandleObjsList = new List<BaseOpenCloseHandleObj>();

        /// <summary>
        /// 开
        /// </summary>
        public virtual void Open()
        {
            if (stateChangeTween != null)
            {
                stateChangeTween.Kill();
            }
        }
        /// <summary>
        /// 关
        /// </summary>
        public virtual void Close()
        {
            if (stateChangeTween != null)
            {
                stateChangeTween.Kill();
            }

        }
        /// <summary>
        /// 打开之后的回调
        /// </summary>
        protected virtual void OpenedHandle()
        {
            for (int i = 0; i < baseOpenCloseHandleObjsList.Count; i++)
            {
                int index = i;
                baseOpenCloseHandleObjsList[index].OpenedOperation();
            }
        }

        /// <summary>
        /// 关闭之后的回调
        /// </summary>
        protected virtual void ClosedHandle()
        {
            for (int i = 0; i < baseOpenCloseHandleObjsList.Count; i++)
            {
                int index = i;
                baseOpenCloseHandleObjsList[index].ClosedOperation();
            }
        }
        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="isOpen"></param>
        internal void SetState(bool isOpen)
        {
            if (isOpen)
            {
                Open();
            }
            else
            {
                Close();
            }
        }
    }
}