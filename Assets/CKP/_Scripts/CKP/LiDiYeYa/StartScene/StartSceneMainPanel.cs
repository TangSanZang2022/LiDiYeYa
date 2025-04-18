using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFramework;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using Common;

namespace LiDi.CKP
{
    /// <summary>
    /// 开始场景主界面
    /// </summary>
    public class StartSceneMainPanel : BasePanel
    {
        private Button studyButton;
        /// <summary>
        /// 知识学习按钮
        /// </summary>
        public Button StudyButton
        {
            get
            {
                if (studyButton == null)
                {
                    studyButton = transform.FindChildForName<Button>("StudyButton");
                }
                return studyButton;
            }
        }
        private Button practicalTrainingButton;
        /// <summary>
        /// 实训按钮
        /// </summary>
        public Button PracticalTrainingButton
        {
            get
            {
                if (practicalTrainingButton == null)
                {
                    practicalTrainingButton = transform.FindChildForName<Button>("PracticalTrainingButton");
                }
                return practicalTrainingButton;
            }
        }
        private Button examinationButton;
        /// <summary>
        /// 考核按钮
        /// </summary>
        public Button ExaminationButton
        {
            get
            {
                if (examinationButton == null)
                {
                    examinationButton = transform.FindChildForName<Button>("ExaminationButton");
                }
                return examinationButton;
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            AddAllListener();
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
        /// 添加所有按钮事件
        /// </summary>
        private void AddAllListener()
        {
            UIEventListener.GetUIEventListener(StudyButton.transform).pointClickHandler += On_StudyButton_Click;
            UIEventListener.GetUIEventListener(PracticalTrainingButton.transform).pointClickHandler += On_PracticalTrainingButton_Click;
            UIEventListener.GetUIEventListener(ExaminationButton.transform).pointClickHandler += On_ExaminationButton_Click;
        }
        /// <summary>
        /// 知识学习按钮点击
        /// </summary>
        /// <param name="eventData"></param>
        private void On_StudyButton_Click(PointerEventData eventData)
        {
            MySceneManager.LoadSceneSync("ZhiShiXueXi");
        }

        private void On_PracticalTrainingButton_Click(PointerEventData eventData)
        {
            //throw new NotImplementedException();
        }

        private void On_ExaminationButton_Click(PointerEventData eventData)
        {
            //throw new NotImplementedException();
        }
    }
}