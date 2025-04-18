using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace LiDi.CKP
{
    /// <summary>
    /// 准备界面
    /// </summary>
    public class ReadyPanel : UIFramework.BasePanel
    {
        private Text _operationNameText;
        /// <summary>
        /// 训练或者考核名称
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


        private GameObject _trainGroup;
        /// <summary>
        /// 训练模式物体
        /// </summary>
        protected GameObject TrainGroup
        {

            get
            {
                if (_trainGroup == null)
                {
                    _trainGroup = transform.FindChildForName("TrainGroup").gameObject;
                }
                return _trainGroup;
            }
        }


        private GameObject _appraisalGroup;
        /// <summary>
        /// 考核模式物体
        /// </summary>
        protected GameObject AppraisalGroup
        {

            get
            {
                if (_appraisalGroup == null)
                {
                    _appraisalGroup = transform.FindChildForName("AppraisalGroup").gameObject;
                }
                return _appraisalGroup;
            }
        }
        private Text _explainText;
        /// <summary>
        /// 考核模式当前考核项说明Text
        /// </summary>
        protected Text ExplainText
        {

            get
            {
                if (_explainText == null)
                {
                    _explainText = transform.FindChildForName<Text>("ExplainText");
                }
                return _explainText;
            }
        }
        public override void OnEnter()
        {
            base.OnEnter();
            gameObject.SetActive(true);
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
            gameObject.SetActive(false);
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="trainingProjectIndex">考核项</param>
        /// <param name="gameMode">模式</param>
        public void Init(int trainingProjectIndex, GameMode gameMode)
        {
            switch (gameMode)
            {
                case GameMode.Training:
                    TrainGroup.SetActive(true);
                    AppraisalGroup.SetActive(false);
                    OperationNameText.text = GameFacade.Instance.GetTrainingProjectForIndex(trainingProjectIndex).Comment + "训练";
                    break;
                case GameMode.Appraisal:
                    TrainGroup.SetActive(false);
                    AppraisalGroup.SetActive(true);
                    ExplainText.text = GameFacade.Instance.GetTrainingProjectForIndex(trainingProjectIndex).CommentForAppraisal;
                    OperationNameText.text = GameFacade.Instance.GetTrainingProjectForIndex(trainingProjectIndex).Comment + "考核";

                    break;
                default:
                    break;
            }
            StartCoroutine(IWaitToStartTask(trainingProjectIndex, gameMode));

        }
        /// <summary>
        /// 等待开始任务协程
        /// </summary>
        /// <param name="trainingProjectIndex"></param>
        /// <param name="gameMode"></param>
        /// <returns></returns>
        private IEnumerator IWaitToStartTask(int trainingProjectIndex, GameMode gameMode)
        {
            float waitTime = 3.0f;
            while (waitTime>0)
            {
                CountDownText.text = waitTime.ToString();
                yield return new WaitForSeconds(1);
                waitTime--;
            }
            
            GameFacade.Instance.PopPanel();
            GameFacade.Instance.StartTask(trainingProjectIndex, gameMode);

        }
    }
}