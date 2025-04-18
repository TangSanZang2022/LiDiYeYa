using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace LiDi.CKP
{
    /// <summary>
    /// 训练模式面板
    /// </summary>
    public class AppraisalPanel : BaseTaskPanel
    {
        /// <summary>
        /// 限定时间
        /// </summary>
        private float limitTime;
        /// <summary>
        /// 总分
        /// </summary>
        public float aggregateScore = 0;
        /// <summary>
        /// 成绩单明细Item预制体
        /// </summary>
        [SerializeField]
        protected ReportCardItem reportCardItemPrefab;
        private GameObject _timeImage;
        /// <summary>
        /// 限时Image
        /// </summary>
        protected GameObject TimeImage
        {

            get
            {
                if (_timeImage == null)
                {
                    _timeImage = transform.FindChildForName("TimeImage").gameObject;
                }
                return _timeImage;
            }
        }
        private Text _timeText;
        /// <summary>
        /// 限时Text
        /// </summary>
        protected Text TimeText
        {

            get
            {
                if (_timeText == null)
                {
                    _timeText = transform.FindChildForName<Text>("TimeText");
                }
                return _timeText;
            }
        }
        private GameObject _stepTipImage;
        /// <summary>
        /// 任务提示物体
        /// </summary>
        protected GameObject StepTipImage
        {

            get
            {
                if (_stepTipImage == null)
                {
                    _stepTipImage = transform.FindChildForName("StepTipImage").gameObject;
                }
                return _stepTipImage;
            }
        }
        private GameObject _reportCardImage;
        /// <summary>
        /// 成绩单物体
        /// </summary>
        protected GameObject ReportCardImage
        {

            get
            {
                if (_reportCardImage == null)
                {
                    _reportCardImage = transform.FindChildForName("ReportCardImage").gameObject;
                }
                return _reportCardImage;
            }
        }
        private Transform reportCardContent;
        /// <summary>
        /// 存放成绩单明细父物体
        /// </summary>
        protected Transform ReportCardContent
        {

            get
            {
                if (reportCardContent == null)
                {
                    reportCardContent = transform.FindChildForName("ReportCardContent");
                }
                return reportCardContent;
            }
        }
        private Button _submitButton;
        /// <summary>
        /// 提交成绩按钮
        /// </summary>
        protected Button SubmitButton
        {

            get
            {
                if (_submitButton == null)
                {
                    _submitButton = transform.FindChildForName<Button>("SubmitButton");
                }
                return _submitButton;
            }
        }

        private Text _scoreText;
        /// <summary>
        /// 总分Text
        /// </summary>
        protected Text ScoreText
        {

            get
            {
                if (_scoreText == null)
                {
                    _scoreText = transform.FindChildForName<Text>("ScoreText");
                }
                return _scoreText;
            }
        }
        

        protected override void Start()
        {
            base.Start();
        }

        protected override void AddListeners()
        {
            base.AddListeners();

            SubmitButton.onClick.AddListener(delegate { On_SubmitButton_Click(); });

          
        }
        /// <summary>
        /// 提交考核
        /// </summary>
        private void On_SubmitButton_Click()
        {
           
            FinishedTask();
            GameFacade.Instance.QuitMidway();
        }

        public override void Init(TrainingProject trainingProject, float limitTime)
        {
            base.Init(trainingProject);
            OperationNameText.text = trainingProject.Comment;// + "考核";
            this.limitTime = limitTime;
            if (this.limitTime > 0)
            {
                TimeImage.SetActive(true);
                StartCoroutine(ICountDownOperationTime());
            }
            else
            {
                TimeImage.SetActive(false);
            }
        }
        protected override void On_ExitButton_Click()
        {
          
            base.On_ExitButton_Click();
            GameFacade.Instance.ShowObjForID(ObjIDTool.FPSCam);
            Tools.CKP.TimerTools.Instance.StopAllIe();
            FinishedTask();
            GameFacade.Instance.QuitMidway();

           // FinishedTask();
            //GameFacade.Instance.PopPanel();
        }

        /// <summary>
        /// 时间到
        /// </summary>
        private void TimeOut()
        {
            Tools.CKP.TimerTools.Instance.StopAllIe();
            FinishedTask();
            GameFacade.Instance.QuitMidway();
        }
        protected override void CreateStepItem(List<Step> steps)
        {
            base.CreateStepItem(steps);
        }
        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnPause()
        {
            base.OnPause();
        }

        public override void OnResume()
        {
            base.OnResume();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
        /// <summary>
        /// 任务完成时调用
        /// </summary>
        public override void FinishedTask()
        {
            base.FinishedTask();
           // Tools.CKP.TimerTools.Instance.StopAllIe();
            StopAllCoroutines();
            limitTime = -1;
            CreateReportCard();
            StartCoroutine(ICountDown());
        }
        /// <summary>
        /// 将面板设置为正确的状态,控制面板中某些物体的显示隐藏状态
        /// </summary>
        /// <param name="state">-1为结束状态，0目前状态，1为开始状态</param>
        protected override void SetPanelToRightState(int state)
        {
            base.SetPanelToRightState(state);
            switch (state)
            {

                case -1:
                    ReportCardImage.SetActive(true);
                    TimeImage.SetActive(false);
                    StepTipImage.SetActive(false);
                    ExitButton.gameObject.SetActive(false);
                    SubmitButton.gameObject.SetActive(false);
                    break;
                case 0:

                    break;

                case 1:
                    ReportCardImage.SetActive(false);
                    if (this.limitTime > 0)
                    {
                        TimeImage.SetActive(true); 
                    }
                    StepTipImage.SetActive(true);
                    ExitButton.gameObject.SetActive(true);
                    SubmitButton.gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
           
        }
        /// <summary>
        /// 倒计时协程
        /// </summary>
        /// <returns></returns>
        IEnumerator ICountDown()
        {
            int count = 10;
            while (count > 0)
            {
                CountDownText.text = count.ToString() + "秒后自动退出考核…";
                yield return new WaitForSeconds(1);
                count--;
            }
            //if (GameFacade.Instance.uiExit != null)
            //{
            //    GameFacade.Instance.uiExit();
            //}
            GameFacade.Instance.PopPanel();
          
        }
        /// <summary>
        /// 创建成绩单
        /// </summary>
        private void CreateReportCard()
        {
            for (int i = 0; i < ReportCardContent.childCount; i++)
            {
                int index = i;
                Destroy(ReportCardContent.GetChild(index).gameObject);
            }
           TrainingProject trainingProject= GameFacade.Instance.GetTrainingProjectForIndex();
            for (int i = 0; i < trainingProject.Steps.Count; i++)
            {
                int index = i;
                ReportCardItem reportCardItem = Instantiate(reportCardItemPrefab, ReportCardContent);
                reportCardItem.Init(trainingProject.Steps[index]);
                if (trainingProject.Steps[index].isCorrect)
                {
                    aggregateScore += trainingProject.Steps[index].Score;
                }
            }
            ScoreText.text = aggregateScore.ToString();
            GameFacade.Instance.UploadRes();
            aggregateScore = 0;
        }

        /// <summary>
        /// 倒计时协程
        /// </summary>
        /// <returns></returns>
        IEnumerator ICountDownOperationTime()
        {
            float count = limitTime;
            while (count > 0)
            {
                TimeText.text = count.ToString() + "秒";
                yield return new WaitForSeconds(1);
                count--;
            }
            TimeOut();

        }
        /// <summary>
        /// 获取总分
        /// </summary>
        private void GetAggregateScore()
        {

        }
    }
}