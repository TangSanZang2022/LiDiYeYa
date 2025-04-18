using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LiDi.CKP
{
    /// <summary>
    /// 开关热点之后需要动起来的物体,旋转类型
    /// </summary>
    public class RotOpenCloseHandleObj : BaseOpenCloseHandleObj
    {
        /// <summary>
        /// 训练模式打开时的旋转角度
        /// </summary>
        [SerializeField]
        protected Vector3 openedRotTrain;

        /// <summary>
        /// 考核模式打开时的旋转角度
        /// </summary>
        [SerializeField]
        protected Vector3 openedRotAppraisal;
        /// <summary>
        /// 训练模式关闭时的旋转角度
        /// </summary>
        [SerializeField]
        protected Vector3 closedRotTrain; 
        /// <summary>
        /// 考核模式关闭时的旋转角度
        /// </summary>
        [SerializeField]
        protected Vector3 closedRotAppraisal;
        // Start is called before the first frame update
        void Start()
        {

        }

        
        public override bool isCurrect()
        {
            return base.isCurrect();
        }
        /// <summary>
        /// 打开时操作
        /// </summary>
        public override void OpenedOperation()
        {
            base.OpenedOperation();
            switch (GameFacade.Instance.GetGameMode())
            {
                case GameMode.Training:
                    stateChangeTween = transform.DOLocalRotate(openedRotTrain, stateChangeTimeTrain,RotateMode.FastBeyond360);
                    //stateChangeTween = transform.DOLocalRotateQuaternion(Quaternion.Euler(openedRotTrain), 1);
                    break;
                case GameMode.Appraisal:
                    stateChangeTween = transform.DOLocalRotate(openedRotAppraisal, stateChangeTimeAppraisal, RotateMode.FastBeyond360);
                    //stateChangeTween = transform.DOLocalRotateQuaternion(Quaternion.Euler(openedRotAppraisal), 1);
                    break;
                default:
                    break;
            }
            
        }
        /// <summary>
        /// 关闭时操作
        /// </summary>
        public override void ClosedOperation()
        {
            base.ClosedOperation();
           
            switch (GameFacade.Instance.GetGameMode())
            {
                case GameMode.Training:
                    Debug.Log("关闭1");
                   
                    stateChangeTween = transform.DOLocalRotateQuaternion(Quaternion.Euler(closedRotTrain), 1);
                    break;
                case GameMode.Appraisal:
                    Debug.Log("关闭2");
                    //stateChangeTween = transform.DOLocalRotate(closedRotAppraisal, 1, RotateMode.LocalAxisAdd);
                    stateChangeTween = transform.DOLocalRotateQuaternion(Quaternion.Euler(closedRotAppraisal), 1);
                    break;
                default:
                    break;
            }
           
        }

    }
}