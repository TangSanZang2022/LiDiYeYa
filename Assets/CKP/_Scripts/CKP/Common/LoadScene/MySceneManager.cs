using Common;
using QingFeng;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Common
{
    /// <summary>
    /// 加载场景结束之后委托
    /// </summary>
    public delegate void MyLoadSceneCompleted(InitSceneBaseData initSceneBaseData);
    /// <summary>
    /// 场景控制器
    /// </summary>
    public static class MySceneManager
    {
      
        /// <summary>
        /// 每次运行必须从Start场景开始加载
        /// </summary>
        [RuntimeInitializeOnLoadMethod]
        static void LoadToFirstScene()
        {
            //if (SceneManager.GetActiveScene().name != "QingFengDaPing")
            //{
            //    SceneManager.LoadScene("QingFengDaPing");
            //}
        }
        private static AsyncOperation ao;
        public static AsyncOperation Ao
        {
            get
            {
                return ao;
            }
            set
            {
                ao = value;
            }

        }

        /// <summary>
        /// 是否正在加载
        /// </summary>
        private static bool isLoading = false;
        /// <summary>
        /// 是否正在加载
        /// </summary>
        public static bool IsLoading
        {
            get
            {
                return isLoading;
            }
            set
            {
                if (!value)
                {
                    Ao = null;
                }
                isLoading = value;
            }
        }


        /// <summary>
        /// 通过场景名称来加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        public static void LoadSceneSync(string sceneName,Action action=null, bool isToTransitionScene = true)
        {
            if (isLoading || SceneManager.GetActiveScene().name == sceneName)
            {
                return;
            }
            if (isToTransitionScene)
            {
                SceneManager.LoadSceneAsync(sceneName).completed += delegate
                {
                    IsLoading = true;
                    Ao = SceneManager.LoadSceneAsync(sceneName);
                    Ao.completed += delegate
                    {
                        if (action != null)
                        {
                            action.Invoke();
                           
                        }
                    };
                    CreateLoadingCanvas();
                };
            }
            else
            {
                IsLoading = true;
                Ao = SceneManager.LoadSceneAsync(sceneName);
                CreateLoadingCanvas();
            }

        }

        /// <summary>
        /// 通过场景ID来加载场景
        /// </summary>
        /// <param name="id"></param>
        public static void LoadSceneSync(int id, bool isToTransitionScene = true)
        {
            if (isLoading || SceneManager.GetActiveScene().buildIndex == id)
            {
                return;
            }

            if (isToTransitionScene)
            {
                SceneManager.LoadSceneAsync("LoadingScene").completed += delegate
                {
                    IsLoading = true;
                    Ao = SceneManager.LoadSceneAsync(id);
                    CreateLoadingCanvas();
                };
            }
            else
            {
                IsLoading = true;
                Ao = SceneManager.LoadSceneAsync(id);
                CreateLoadingCanvas();
            }

        }
        /// <summary>
        /// 创建加载界面
        /// </summary>
        private static void CreateLoadingCanvas()
        {
            //GameFacade.Instance.PushPanel(UIPanelType.LoadingCanvas);//通过UI框架弹出加载界面UI
            LoadingCanvas loadingCanvas = GameObject.Instantiate(Resources.Load<LoadingCanvas>("Prefabs/LoadingCanvas"));
        }
    }

}


