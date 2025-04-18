using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Common;

namespace LiDi.CKP
{
    /// <summary>
    /// 观察动画相机的最佳视角
    /// </summary>
    public class AniCamPos : BasePos
    {
        public override void MoveToPoint(Transform trans)
        {
            base.MoveToPoint(trans);
            trans.DOMove(transform.position, moveTime);
            trans.DORotateQuaternion(transform.rotation, moveTime).OnComplete(() => CamArrived());
        }

        public override void CamArrived()
        {
            base.CamArrived();
        }

        public override void Leave()
        {
            base.Leave();
        }
    }
}