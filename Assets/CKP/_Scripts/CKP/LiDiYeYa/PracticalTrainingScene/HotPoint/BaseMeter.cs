using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LiDi.CKP
{
    /// <summary>
    /// 表基类
    /// </summary>
    public class BaseMeter : MonoBehaviour
    {


        /// <summary>
        /// 表ID
        /// </summary>
        public string ID;

        [SerializeField]
        /// <summary>
        /// 当前数值
        /// </summary>
        protected float readingValue;
        /// <summary>
        /// 注释名称
        /// </summary>
        public string CommentName;

        /// <summary>
        /// 初始值
        /// </summary>
        protected float startValue;
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
        [SerializeField]
        /// <summary>
        /// 该表的入口阀
        /// </summary>
        protected List<ClickableHotPointValue> clickableHotPointValuesEnterList = new List<ClickableHotPointValue>();
        [SerializeField]
        /// <summary>
        /// 该表的出口阀
        /// </summary>
        protected List<ClickableHotPointValue> clickableHotPointValuesExitList = new List<ClickableHotPointValue>();
        // Start is called before the first frame update
        void Start()
        {

        }
        /// <summary>
        /// 攀升
        /// </summary>
        public virtual void Rising()
        {
           
        }
        /// <summary>
        /// 下降
        /// </summary>
        public virtual void Decline()
        {
           
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public virtual void Pause()
        {
          
        }
        /// <summary>
        /// 重置到初始状态
        /// </summary>
        public virtual void ResetToStart()
        {
           
        }




        /// <summary>
        /// 获取当前数值
        /// </summary>
        /// <returns></returns>
        public virtual float Get_readingValue()
        {
          
            return readingValue;
        }
        /// <summary>
        /// 设置为目标值，用于进行下一步时初始化，以便后续操作
        /// </summary>
        /// <param name="value"></param>
        public virtual void SetToTargetValue(float value)
        {
            
        }
        /// <summary>
        /// 检测是攀升还是下降
        /// </summary>
        public virtual void Check()
        {
            if (IsEnterOpen())//入口打开
            {
                if (!IsExitOpen())//出口关闭，表需要攀升
                {
                    Rising();
                }
                else//出口阀也打开，则不需要操作
                {
                    Decline();
                }

            }
            else
            {
                if (IsExitOpen())//入口关闭，出口打开，则需要下降
                {
                    Decline();
                }
                else//入口关闭，出口关闭，则需要维持现状
                {
                    Pause();
                }
            }
        }
        /// <summary>
        /// 判断是否入口阀打开
        /// </summary>
        /// <returns></returns>
        protected virtual bool IsEnterOpen()
        {
            bool isEnterOpen = false;
            for (int i = 0; i < clickableHotPointValuesEnterList.Count; i++)
            {
                int index = i;
                if (clickableHotPointValuesEnterList[i].GetState())
                {
                    isEnterOpen = true;
                    break;
                }
            }

            return isEnterOpen;
        }

        /// <summary>
        /// 判断是否出口阀打开
        /// </summary>
        /// <returns></returns>
        protected virtual bool IsExitOpen()
        {
            bool isExitOpen = false;
            for (int i = 0; i < clickableHotPointValuesExitList.Count; i++)
            {
                int index = i;
                if (clickableHotPointValuesExitList[i].GetState())
                {
                    isExitOpen = true;
                    break;
                }
            }

            return isExitOpen;
        }
    }
}