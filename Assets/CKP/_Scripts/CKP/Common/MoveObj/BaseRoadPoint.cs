using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace MoveObjTool
{
    /// <summary>
    /// 移动的路径点
    /// </summary>
    public class BaseRoadPoint : MonoBehaviour
    {


        /// <summary>
        /// 点的Id
        /// </summary>
        [SerializeField]
        private string id;
        /// <summary>
        /// 到达时间
        /// </summary>
        public float arrivedTime;
        /// <summary>
        /// 当前到点的物体
        /// </summary>
        protected BaseMoveObj currentBaseMoveObj;
        /// <summary>
        /// 描述
        /// </summary>
        [SerializeField]
        private string Describe;
       
        /// <summary>
        /// 到达点之后的事件
        /// </summary>
        public UnityAction ArrivedPointAction;
        /// <summary>
        /// 离开点之后的事件
        /// </summary>
        public UnityAction LeavedPointAction;
        /// <summary>
        /// 是否已经到达
        /// </summary>
        protected bool isArrived;
        public string GetID()
        {
            return id;
        }
        /// <summary>
        /// 是否继续移动
        /// </summary>
        /// <returns></returns>
        internal virtual bool IsMoveNext()
        {
            return true;
        }
        /// <summary>
        /// 到了点之后执行
        /// </summary>
        public virtual void ArrivedPoint(BaseMoveObj currentBaseMoveObj)
        {
            this.currentBaseMoveObj = currentBaseMoveObj;
            isArrived = true;
            if (ArrivedPointAction!=null)
            {
                ArrivedPointAction();
            }
        }
        /// <summary>
        /// 离开点时执行
        /// </summary>
        public virtual void LeavedPoint()
        {
            if (!isArrived )
            {
                return;
               
            }
            this.currentBaseMoveObj = null;
            if (LeavedPointAction != null)
            {
                LeavedPointAction();
            }
            isArrived = false;
        }

        public BaseMoveObj Get_currentBaseMoveObj()
        {
            return currentBaseMoveObj;
        }
    }
}
