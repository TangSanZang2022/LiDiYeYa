using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LiDi.CKP
{
    /// <summary>
    /// 实训界面基类
    /// </summary>
    public class BaseTaskPanel : UIFramework.BasePanel
    {
        /// <summary>
        /// 步骤提示Item预制体
        /// </summary>
        [SerializeField]
        protected BaseStepToggleItem baseStepToggleItemPrefab;

        private Text _operationNameText;
        /// <summary>
        /// 训练名称
        /// </summary>
        protected Text OperationNameText
        {

            get
            {
                if (_operationNameText == null)
                {
                    _operationNameText = transform.FindChildForName<Text>("OperationNameText");
                }
                return _operationNameText;
            }
        }

        private Transform _stepContent;
        /// <summary>
        /// 存放训练提示步骤父物体
        /// </summary>
        protected Transform StepContent
        {

            get
            {
                if (_stepContent == null)
                {
                    _stepContent = transform.FindChildForName("StepContent");
                }
                return _stepContent;
            }
        }
        private Button _exitButton;
        /// <summary>
        /// 退出按钮
        /// </summary>
        protected Button ExitButton
        {

            get
            {
                if (_exitButton == null)
                {
                    _exitButton = transform.FindChildForName<Button>("ExitButton");
                }
                return _exitButton;
            }
        }
        private GameObject _finishedImage;
        /// <summary>
        /// 完成训练时的图片
        /// </summary>
        protected GameObject FinishedImage
        {

            get
            {
                if (_finishedImage == null)
                {
                    _finishedImage = transform.FindChildForName("FinishedImage").gameObject;
                }
                return _finishedImage;
            }
        }
        private Text _countDownText;
        /// <summary>
        /// 倒计时Text
        /// </summary>
        protected Text CountDownText
        {

            get
            {
                if (_countDownText == null)
                {
                    _countDownText = transform.FindChildForName<Text>("CountDownText");
                }
                return _countDownText;
            }
        }
        protected virtual void Start()
        {
            AddListeners();
            FinishedImage.SetActive(false);
        }
        /// <summary>
        /// 添加事件
        /// </summary>
        protected virtual void AddListeners()
        {
            ExitButton.onClick.AddListener(delegate { On_ExitButton_Click(); });
        }
        /// <summary>
        /// 点击退出按钮
        /// </summary>
        protected virtual void On_ExitButton_Click()
        {
            Debug.Log("退出");
        }

        public override void OnEnter()
        {
            base.OnEnter();
            gameObject.SetActive(true);
            SetPanelToRightState(1);
        }

        public override void OnPause()
        {
            base.OnPause();
            gameObject.SetActive(false);
        }

        public override void OnResume()
        {
            base.OnResume();
            gameObject.SetActive(true);
        }

        public override void OnExit()
        {
            base.OnExit();
            FinishedImage.SetActive(false);
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 初始化面板
        /// </summary>
        /// <param name="trainingProject"></param>
        public virtual void Init(TrainingProject trainingProject,float limitTime=-1)
        {
            CreateStepItem(trainingProject.Steps);
        }
        /// <summary>
        /// 根据步骤创建
        /// </summary>
        /// <param name="steps"></param>
        protected virtual void CreateStepItem(List<Step> steps)
        {
            for (int i = 0; i < StepContent.childCount; i++)
            {
                Destroy(StepContent.GetChild(i).gameObject);
            }

            for (int i = 0; i < steps.Count; i++)
            {
                int index = i;
                BaseStepToggleItem baseStepToggleItem= Instantiate(baseStepToggleItemPrefab, StepContent);
                baseStepToggleItem.Init(steps[index]);
            }
        }
      /// <summary>
      /// 设置步骤状态
      /// </summary>
      /// <param name="stepIndex">步骤index</param>
      /// <param name="state">-1为初始状态，白色，未操作；
      /// 0为操作错误；
      /// 1为操作正确
      /// </param>
        public virtual void SetStepItemState(int stepIndex,int state)
        {
            (StepContent.GetChild(stepIndex).GetComponent<BaseStepToggleItem>()).UpdateState(state);
        }

        /// <summary>
        /// 完成训练时调用
        /// </summary>
        public virtual void FinishedTask()
        {
            SetPanelToRightState(-1);
        }
        /// <summary>
        /// 将面板设置为正确的状态,控制面板中某些物体的显示隐藏状态
        /// </summary>
        /// <param name="state">-1为结束状态，0目前状态，1为开始状态</param>
        protected virtual void SetPanelToRightState(int state)
        {

        }
    }
}