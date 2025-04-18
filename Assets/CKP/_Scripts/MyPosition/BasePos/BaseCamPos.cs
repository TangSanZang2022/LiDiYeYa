using Common;
using DFDJ;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LiDi.CKP
{
    /// <summary>
    /// 相机位置基类
    /// </summary>
    public class BaseCamPos : BasePos
    {
       protected Tweener moveTweener;
        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        {
            base.Init();
        }
      
        /// <summary>
        /// 向点移动
        /// </summary>
        public override void MoveToPoint(Transform trans)
        {
            base.MoveToPoint(trans);
            StartCoroutine(IWaitCameraArrived());
            trans.position = this.transform.position;
            trans.rotation = this.transform.rotation;
        }
       
        /// <summary>
        /// 相机到达
        /// </summary>
        public override void CamArrived()
        {
           // GameFacade.Instance.Remove__currentTweener(moveTweener);

        }
        /// <summary>
        /// 相机离开
        /// </summary>
        public override void Leave()
        {
            Debug.Log(string.Format("相机离开ID为{0}的位置", GetID()));
        }
    }
}
