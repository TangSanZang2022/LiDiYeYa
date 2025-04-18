using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Reflection;
using Common;
namespace QingFeng
{
    /// <summary>
    /// 加载场景管理器，用于到达场景之后记录数据以便返回场景之后用于初始化
    /// </summary>
    public class MyLoadSceneManager : MonoBehaviour
    {
        /// <summary>
        /// 存放所有场景初始化的数据
        /// </summary>
        private Dictionary<string, InitSceneBaseData> allSceneDataDict = new Dictionary<string, InitSceneBaseData>();

        /// <summary>
        /// 存放所有场景初始化的数据
        /// </summary>
       // private Dictionary<string, InitSceneBaseData> allSceneDataDict = new Dictionary<string, InitSceneBaseData>();
        // Start is called before the first frame update
        void Start()
        {
            AddAllSceneData();
        }
        /// <summary>
        /// 添加当前场景的数据到列表
        /// </summary>
        public void AddAllSceneData()
        {
            Debug.Log(SceneManager.GetActiveScene().name);
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                int sceneIndex = i;
                string scenePath = SceneUtility.GetScenePathByBuildIndex(sceneIndex);
                string[] scenePathArray = scenePath.Split('/');
                string sceneName = scenePathArray[scenePathArray.Length - 1].Replace(".unity", "");
                string className = "QingFeng." + sceneName + "Data";
                Type type = Type.GetType(className);
                Debug.Log("className:" + className);
                InitSceneBaseData initSceneBaseData = type.Assembly.CreateInstance(className) as InitSceneBaseData;
                initSceneBaseData.sceneName = sceneName;
                allSceneDataDict.Add(initSceneBaseData.sceneName, initSceneBaseData);
            }
        }
        /// <summary>
        /// 获取场景初始化数据
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        public InitSceneBaseData GetInitSceneBaseData(string sceneName)
        {
            InitSceneBaseData initSceneBaseData;
            if (allSceneDataDict.TryGetValue(sceneName, out initSceneBaseData))
            {
                return initSceneBaseData;
            }
            return null;

        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="newData"></param>
        public void SetInitSceneBaseData(string sceneName, InitSceneBaseData newData)
        {
            if (allSceneDataDict.ContainsKey(sceneName))
            {
                allSceneDataDict[sceneName]= newData;
            }
        }

    }
    [Serializable]
    /// <summary>
    /// 初始化场景的数据
    /// </summary>
    public class InitSceneBaseData
    {
        /// <summary>
        /// 场景名称
        /// </summary>
        public string sceneName;
    }
    /// <summary>
    /// 加载页面初始页面数据
    /// </summary>
    public class LoadingSceneData : InitSceneBaseData
    {
        /// <summary>
        /// 被点击的Toggle名称
        /// </summary>
        public string mainUIToggleName;
    }
    /// <summary>
    /// 氢枫大屏初始页面数据
    /// </summary>
    public class QingFengDaPingData : InitSceneBaseData
    {
        /// <summary>
        /// 被点击的Toggle名称
        /// </summary>
        public string mainUIToggleName;
    }

    /// <summary>
    /// 氢枫迁安站初始页面数据
    /// </summary>
    public class QingFengQianAnData : InitSceneBaseData
    {
        /// <summary>
        /// 被点击的Toggle名称
        /// </summary>
        public string mainUIToggleName;
    }
    /// <summary>
    /// 氢枫迁安站初始页面数据
    /// </summary>
    public class QingFengDaPingStartSceneData : InitSceneBaseData
    {
        /// <summary>
        /// 被点击的Toggle名称
        /// </summary>
        public string mainUIToggleName;
    }
}