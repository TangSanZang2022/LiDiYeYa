using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LiDi.CKP
{
    /// <summary>
    /// 开关热点之后需要动起来的物体
    /// </summary>
    public class BaseOpenCloseHandleObj : MonoBehaviour
    {


        /// <summary>
        /// 注释名称
        /// </summary>
        public string CommentName;
        [SerializeField]
        /// <summary>
        /// 状态改变时间训练模式
        /// </summary>
        protected float stateChangeTimeTrain = 1f;
        [SerializeField]
        /// <summary>
        /// 状态改变时间考核模式
        /// </summary>
        protected float stateChangeTimeAppraisal = 1f;
        /// <summary>
        /// 状态改变时的Tween
        /// </summary>
        protected Tween stateChangeTween;
        // Start is called before the first frame update
        void Start()
        {

        }
        /// <summary>
        /// 是否正确
        /// </summary>
        /// <returns></returns>
        public virtual bool isCurrect()
        {
            return true;
        }
        /// <summary>
        /// 打开之后操作
        /// </summary>
        public virtual void OpenedOperation()
        {
            if (stateChangeTween != null)
            {
                stateChangeTween.Kill();
            }

        }
        /// <summary>
        /// 关闭之后操作
        /// </summary>
        public virtual void ClosedOperation()
        {
            if (stateChangeTween != null)
            {
                stateChangeTween.Kill();
            }
        }
        /// <summary>
        /// 停止运动
        /// </summary>
        public virtual void Pause_stateChangeTween()
        {
            if (stateChangeTween!=null)
            {
                stateChangeTween.Kill();
            }
        }
    }
}