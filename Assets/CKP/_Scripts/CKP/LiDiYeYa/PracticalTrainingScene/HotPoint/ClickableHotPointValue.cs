using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LiDi.CKP
{
    /// <summary>
    /// 可点击热点，阀门
    /// </summary>
    public class ClickableHotPointValue : ClickableHotPoint
    {
        [SerializeField]
        /// <summary>
        /// 开始开关状态
        /// </summary>
        private bool startState;
        [SerializeField]
        /// <summary>
        /// 是否打开
        /// </summary>
        private bool isOpen;
        [SerializeField]
        /// <summary>
        /// 已经点击的次数
        /// </summary>
        private int cilckedNum;
        /// <summary>
        /// 该阀门控制的压力表
        /// </summary>
        [SerializeField]
        private List<BaseMeter> pressureAagesList = new List<BaseMeter>();

        ///// <summary>
        ///// 该阀门控制的压力表
        ///// </summary>
        //[SerializeField]
        //private List<MeterUI> meterUIList = new List<MeterUI>();
        /// <summary>
        /// 总共需要点击的次数
        /// </summary>
        //private int totalCanCilckedNum
        //{
        //    get
        //    {
        //        int num = 0;
        //        List<string> ids = new List<string>(GameFacade.Instance.GetCurrentStep().CorrectHotPointID);
        //        for (int i = 0; i < ids.Count; i++)
        //        {
        //            int index = i;
        //            if (ids[index] == id)
        //            {
        //                num++;
        //            } 
                   
        //        }
        //        return num;
        //    }
        //}

        protected override void Start()
        {
            startState = isOpen;
            base.Start();
        }
        protected override void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("鼠标点击在UI上");
                return;
            }
            else
            {
                Debug.Log("鼠标点击在UI之外");
            }
            isOpen = !isOpen;
            //SetUIMeterState(true);
            CheckPressureAage();
            SetAllChildState(isOpen);

            //cilckedNum++;
            //if (GameFacade.Instance.GetGameMode() == GameMode.Training)
            //{
            //    if (cilckedNum < totalCanCilckedNum && totalCanCilckedNum > 0)
            //    {
            //        SetNeedClick(true);
            //    }
            //    else
            //    {
            //        SetNeedClick(false);
            //    } 
            //}
            base.OnMouseDown();


        }
        /// <summary>
        /// 设置为初始状态
        /// </summary>
        public override void SetToStartState()
        {
           
            base.SetToStartState();
        }
        /// <summary>
        /// 设置热点为初始状态,用于退出时调用
        /// </summary>
        public override void SetToStartState_ForQuite()
        {
            for (int i = 0; i < pressureAagesList.Count; i++)
            {
                int index = i;
                pressureAagesList[index].ResetToStart();
            }
            isOpen = startState;
            SetAllChildState(startState);
            base.SetToStartState_ForQuite();
        }

        public override void ResetBeforeNextStep()
        {
            base.ResetBeforeNextStep();
            Debug.Log("重置：" + id);
            cilckedNum = 0;

        }
        protected override void ClickedHandel()
        {
            //base.ClickedHandel();
            Set_SelectedToolID();
            Set_showObjAfterClick_State(true);
            GameFacade.Instance.AddOperatedHotPointTo_operatedHotPointList(this);
        }

        /// <summary>
        /// 获取开关状态
        /// </summary>
        /// <returns>true为开，close为关</returns>
        public bool GetState()
        {
            return isOpen;
        }

        /// <summary>
        /// 检查压力表
        /// </summary>
        public void CheckPressureAage()
        {
            for (int i = 0; i < pressureAagesList.Count; i++)
            {
                int index = i;
                pressureAagesList[index].Check();
            }
            //for (int i = 0; i < meterUIList.Count; i++)
            //{
            //    int index = i;
            //    meterUIList[index].Check();
            //}

        }

        /// <summary>
        /// 设置阀门到正确值
        /// </summary>
        /// <param name="isOpenValue"></param>
        public void SetToTargetState(bool isOpenValue)
        {
            isOpen = isOpenValue;
            SetAllChildState(isOpen);
        }
        /// <summary>
        /// 设置UI表的显示隐藏状态
        /// </summary>
        /// <param name="isActive"></param>
        public void SetUIMeterState(bool isActive)
        {
            //for (int i = 0; i < meterUIList.Count; i++)
            //{
            //    int index = i;
            //    meterUIList[index].GetComponent<Canvas>().enabled= isActive;
            //}
        }

        public override void ResetToStateBeforeClick()
        {
            base.ResetToStateBeforeClick();
            isOpen = !isOpen;
            //SetUIMeterState(true);
            CheckPressureAage();
            SetAllChildState(isOpen);
            h.Off();
        }
    }
}