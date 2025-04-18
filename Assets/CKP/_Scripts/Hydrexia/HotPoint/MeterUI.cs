using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace LiDi.CKP
{
    /// <summary>
    /// UI 类型压力表
    /// </summary>
    public class MeterUI : BaseMeter
    {

        /// <summary>
        /// 训练模式打开时的目标值
        /// </summary>
        [SerializeField]
        protected float risingValueTrain;

        /// <summary>
        /// 考核模式打开时的目标值
        /// </summary>
        [SerializeField]
        protected float risingValueAppraisal;
        /// <summary>
        /// 训练模式关闭时的目标值
        /// </summary>
        [SerializeField]
        protected float declineValueTrain;
        /// <summary>
        /// 考核模式关闭时的目标值
        /// </summary>
        [SerializeField]
        protected float declineValueAppraisal;
        [SerializeField]
        /// <summary>
        /// 每次增加量
        /// </summary>
        private float aug;
        private Text valueText;

        private Text ValueText
        {
            get
            {
                if (valueText==null)
                {
                    valueText = transform.FindChildForName<Text>("ValueText");
                }
                return valueText;
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            startValue =float.Parse( ValueText.text);
        }
       
        public override void Rising()
        {
            base.Rising();
            StopAllCoroutines();
            StartCoroutine(IRising());
        }

        public override void Pause()
        {
            base.Pause();
            StopAllCoroutines();
        }

        public override void Decline()
        {
            base.Decline();
            StopAllCoroutines();
            StartCoroutine(IDecline());
        }

        public override float Get_readingValue()
        {
            //return base.Get_readingValue();
            return float.Parse(ValueText.text);
        }

        public override void ResetToStart()
        {
            StopAllCoroutines();
            base.ResetToStart();
              ValueText.text= startValue.ToString();
        }

        public override void SetToTargetValue(float value)
        {
            StopAllCoroutines();
            //base.SetToTargetValue(value);
            ValueText.text = value.ToString();
        }
        /// <summary>
        /// 攀升协程
        /// </summary>
        /// <returns></returns>
        IEnumerator IRising()
        {
            float currentValue = float.Parse(ValueText.text);
            float maxValue = GameFacade.Instance.GetGameMode() == GameMode.Training ? risingValueTrain : risingValueAppraisal;
            while (currentValue<= maxValue)
            {
                yield return new WaitForSeconds(stateChangeTimeTrain);
                currentValue+= aug;
                ValueText.text = currentValue.ToString();

            }
            ValueText.text = maxValue.ToString();
        }

        /// <summary>
        /// 攀升协程
        /// </summary>
        /// <returns></returns>
        IEnumerator IDecline()
        {
            float currentValue = float.Parse(ValueText.text);
            float minValue = GameFacade.Instance.GetGameMode() == GameMode.Training ? declineValueTrain : declineValueTrain;
            while (currentValue > minValue)
            {
                yield return new WaitForSeconds(stateChangeTimeTrain);
                currentValue -= aug;
                ValueText.text = currentValue.ToString();

            }
            ValueText.text = minValue.ToString();
        }
    }
}