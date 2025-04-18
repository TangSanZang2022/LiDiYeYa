using System.Collections;
using System.Collections.Generic;
using UIFramework;
using UnityEngine;
using UnityEngine.UI;
namespace IntelligentManufacturingPracticePlatform
{
    /// <summary>
    /// 东方电机退出界面
    /// </summary>
    public class ExitPanel : BasePanel
    {
       
        private Button confirmButton;
        /// <summary>
        /// 是按钮
        /// </summary>
        private Button ConfirmButton
        {
            get
            {
                if (confirmButton==null)
                {
                    confirmButton = transform.FindChildForName("ConfirmButton").GetComponent<Button>();
                }
                return confirmButton;
            }
        }
        private Button cancelButton;
        /// <summary>
        /// 否按钮
        /// </summary>
        private Button CancelButton
        {
            get
            {
                if (cancelButton == null)
                {
                    cancelButton = transform.FindChildForName("CancelButton").GetComponent<Button>();
                }
                return cancelButton;
            }
        }
        private Button closeButton;
        /// <summary>
        /// 关闭按钮
        /// </summary>
        private Button CloseButton
        {
            get
            {
                if (closeButton == null)
                {
                    closeButton = transform.FindChildForName("CloseButton").GetComponent<Button>();
                }
                return closeButton;
            }
        }
        void Awake()
        {
            Init();
            AddAllListener();
        }

        public override void OnEnter()
        {
            gameObject.SetActive(true);

        }

        public override void OnPause()
        {
            gameObject.SetActive(false);
        }

        public override void OnResume()
        {
            gameObject.SetActive(true);
        }

        public override void OnExit()
        {
            gameObject.SetActive(false);
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
           
        }
        /// <summary>
        /// 添加点击事件
        /// </summary>
        private void AddAllListener()
        {
            ConfirmButton.onClick.AddListener(() => OnExitButtonClick());
            CancelButton.onClick.AddListener(() => GameFacade.Instance.PopPanel());
            CloseButton.onClick.AddListener(() => GameFacade.Instance.PopPanel());
        }
        /// <summary>
        /// 点击退出按钮
        /// </summary>
        private void OnExitButtonClick()
        {
            ExitSystem();
        }
        /// <summary>
        /// 退出系统
        /// </summary>
        private void ExitSystem()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
}