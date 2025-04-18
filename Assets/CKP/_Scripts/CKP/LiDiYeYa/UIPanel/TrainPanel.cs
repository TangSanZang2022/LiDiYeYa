using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIFramework;
using Common;
namespace LiDi.CKP
{
    /// <summary>
    /// 实训面板
    /// </summary>
    public class TrainPanel : BaseTaskPanel
    {

      
        private Text _finishedText;
        /// <summary>
        /// 完成时Text
        /// </summary>
        private Text FinishedText
        {

            get
            {
                if (_finishedText == null)
                {
                    _finishedText = transform.FindChildForName<Text>("FinishedText");
                }
                return _finishedText;
            }
        }
        

        protected override void Start()
        {
            base.Start();
            FinishedText.gameObject.SetActive(false);
            CountDownText.gameObject.SetActive(false);
        }

        public override void OnExit()
        {
            base.OnExit();
            FinishedText.gameObject.SetActive(false);
            CountDownText.gameObject.SetActive(false);
        }
        public override void Init(TrainingProject trainingProject,float limitTime)
        {
            base.Init(trainingProject, limitTime);
            OperationNameText.text = trainingProject.Comment; //+ "训练";
            
        }

        protected override void CreateStepItem(List<Step> steps)
        {
            base.CreateStepItem(steps);

        }
        /// <summary>
        /// 设置步骤状态
        /// </summary>
        /// <param name="stepIndex">步骤index</param>
        /// <param name="state">-1为初始状态，白色，未操作；
        /// 0为操作错误；
        /// 1为操作正确
        /// </param>
        public override void SetStepItemState(int stepIndex, int state)
        {
            base.SetStepItemState(stepIndex, state);
            
        }
        /// <summary>
        /// 点击退出按钮
        /// </summary>
        protected override void On_ExitButton_Click()
        {
           // GameFacade.Instance.StopAudio();
            Tools.CKP.TimerTools.Instance.StopAllIe();
            base.On_ExitButton_Click();
            GameFacade.Instance. QuitMidway();
            //if (GameFacade.Instance.uiExit != null)
            //{
            //    GameFacade.Instance.uiExit();
            //}
            GameFacade.Instance.PopPanel();

           
        }
        /// <summary>
        /// 任务完成
        /// </summary>
        public override void FinishedTask()
        {
            base.FinishedTask();
            
            StartCoroutine(ICountDown());
            //GameFacade.Instance.PostUpdateProgress();
        }

     

        /// <summary>
        /// 倒计时协程
        /// </summary>
        /// <returns></returns>
        IEnumerator ICountDown()
        {
            int count = 3;
            while (count>0)
            {
                CountDownText.text = count.ToString() + "秒后自动退出训练";
                yield return new WaitForSeconds(1);
                count--;
            }
            //if (GameFacade.Instance.uiExit != null)
            //{
            //    GameFacade.Instance.uiExit();
            //}
            GameFacade.Instance.PopPanel();
        }

        protected override void SetPanelToRightState(int state)
        {
            base.SetPanelToRightState(state);
            switch (state)
            {

                case -1:
                    FinishedImage.SetActive(true);
                    FinishedText.text = "已完成" + OperationNameText.text;
                    FinishedText.gameObject.SetActive(true);
                    CountDownText.gameObject.SetActive(true);
                    ExitButton.gameObject.SetActive(false);
                   


                    break;
                case 0:

                    break;

                case 1:
                    FinishedImage.SetActive(false);
                    FinishedText.gameObject.SetActive(false);
                    CountDownText.gameObject.SetActive(false);
                    ExitButton.gameObject.SetActive(true);
                   
                    break;
                default:
                    break;
            }
        }
    }
}