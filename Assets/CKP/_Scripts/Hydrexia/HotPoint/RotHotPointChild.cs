using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace LiDi.CKP
{
    /// <summary>
    /// 旋转热点子物体，点击热点之后会旋转
    /// </summary>
    public class RotHotPointChild : BaseHotPointChild
    {
        /// <summary>
        /// 打开时的旋转角度
        /// </summary>
        [SerializeField]
        protected Vector3 openedRot;
        /// <summary>
        /// 关闭时的旋转角度
        /// </summary>
        [SerializeField]
        protected Vector3 closedRot;
        // Start is called before the first frame update
        void Start()
        {

        }

        public override void Open()
        {
            base.Open();
            stateChangeTween = transform.DOLocalRotate(openedRot, stateChangeTime).OnComplete(delegate
            {
                OpenedHandle();
            });
        }

        public override void Close()
        {
            base.Close();
            stateChangeTween = transform.DOLocalRotate(closedRot, stateChangeTime).OnComplete(delegate
            {
                ClosedHandle();
            });
        }


    }
}