using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LiDi.CKP
{
    /// <summary>
    /// 压力表,旋转
    /// </summary>
    public class PressureMeter : BaseMeter
    {
       
        /// <summary>
        /// 状态改变时的Tween
        /// </summary>
        protected Tween stateChangeTween;
        /// <summary>
        /// 训练模式打开时的旋转角度
        /// </summary>
        [SerializeField]
        protected Vector3 risingRotTrain;

        /// <summary>
        /// 考核模式打开时的旋转角度
        /// </summary>
        [SerializeField]
        protected Vector3 risingRotAppraisal;
        /// <summary>
        /// 训练模式关闭时的旋转角度
        /// </summary>
        [SerializeField]
        protected Vector3 declineRotTrain;
        /// <summary>
        /// 考核模式关闭时的旋转角度
        /// </summary>
        [SerializeField]
        protected Vector3 declineRotAppraisal;
      
        // Start is called before the first frame update
        void Start()
        {
            startValue = transform.localEulerAngles.x;
        }
        /// <summary>
        /// 攀升
        /// </summary>
        public override void Rising()
        {
            if (stateChangeTween != null)
            {
                stateChangeTween.Kill();
            }

            switch (GameFacade.Instance.GetGameMode())
            {
                case GameMode.Training:
                    stateChangeTween = transform.DOLocalRotate(risingRotTrain, stateChangeTimeTrain, RotateMode.FastBeyond360);
                    //stateChangeTween = transform.DOLocalRotateQuaternion(Quaternion.Euler(openedRotTrain), 1);
                    break;
                case GameMode.Appraisal:
                    stateChangeTween = transform.DOLocalRotate(risingRotAppraisal, stateChangeTimeAppraisal, RotateMode.FastBeyond360);
                    //stateChangeTween = transform.DOLocalRotateQuaternion(Quaternion.Euler(openedRotAppraisal), 1);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 下降
        /// </summary>
        public override void Decline()
        {
            if (stateChangeTween != null)
            {
                stateChangeTween.Kill();
            }
            switch (GameFacade.Instance.GetGameMode())
            {
                case GameMode.Training:
                    Debug.Log("关闭1");

                    stateChangeTween = transform.DOLocalRotateQuaternion(Quaternion.Euler(declineRotTrain), 1);
                    break;
                case GameMode.Appraisal:
                    Debug.Log("关闭2");
                    //stateChangeTween = transform.DOLocalRotate(closedRotAppraisal, 1, RotateMode.LocalAxisAdd);
                    stateChangeTween = transform.DOLocalRotateQuaternion(Quaternion.Euler(declineRotAppraisal), 1);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public override void Pause()
        {
            if (stateChangeTween != null)
            {
                stateChangeTween.Kill();
            }
        }
        /// <summary>
        /// 重置到初始状态
        /// </summary>
        public override void ResetToStart()
        {
            if (stateChangeTween != null)
            {
                stateChangeTween.Kill();
            }

             transform.localEulerAngles=new Vector3(startValue, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }

      

      
        /// <summary>
        /// 获取当前数值
        /// </summary>
        /// <returns></returns>
        public override float Get_readingValue()
        {
           
            readingValue = transform.localEulerAngles.x;
            return readingValue;
        }
        /// <summary>
        /// 设置为目标值，用于进行下一步时初始化，以便后续操作
        /// </summary>
        /// <param name="value"></param>
        public override void SetToTargetValue(float value)
        {
            if (stateChangeTween != null)
            {
                stateChangeTween.Kill();
            }
            transform.localEulerAngles = new Vector3(value, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
    }
}