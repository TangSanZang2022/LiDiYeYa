using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common
{
    /// <summary>
    /// 从缓存加载场景
    /// </summary>
    public  class MySceneManegerForCache : MonoSingleton<MySceneManegerForCache>
    {
        /// <summary>
        /// 正在加载的场景
        /// </summary>
        public  Dictionary<string, AsyncOperation> loadingAODict = new Dictionary<string, AsyncOperation>();
        // Start is called before the first frame update
        void Start()
        {

        }
       
        /// <summary>
        /// 将场景添加到加载完毕的场景中
        /// </summary>
        /// <param name="sceneNameList"></param>
        public void LoadingSceneTODict(List<string> sceneNameList)
        {
            StartCoroutine(ILoadingSceneTODict(sceneNameList));
        }

        private IEnumerator ILoadingSceneTODict(List<string> sceneNameList)
        {
            for (int i = 0; i < sceneNameList.Count; i++)
            {
                int index = i;
                Debug.Log(sceneNameList[index] + "__" + SceneManager.GetActiveScene().name);
                if (!loadingAODict.ContainsKey(sceneNameList[index]) && SceneManager.GetActiveScene().name != sceneNameList[index])
                {

                    AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneNameList[index]);
                    asyncOperation.allowSceneActivation = false;
                    loadingAODict.Add(sceneNameList[index], asyncOperation);
                    yield return new WaitUntil(() => asyncOperation.progress >= 0.9f);
                    Debug.Log(sceneNameList[index] + "加载进度" + asyncOperation.progress);
                    Debug.Log(11111);
                }

            }
        }
        /// <summary>
        /// 将预加载场景添加到字典中
        /// </summary>
        /// <param name="sceneName"></param>
        private void AddSceneToDict(string sceneName)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            asyncOperation.allowSceneActivation = false;
            loadingAODict.Add(sceneName, asyncOperation);
        }
        /// <summary>
        /// 从列表中加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        public void LoadSceneFromList(string sceneName)
        {
            if (SceneManager.GetActiveScene().name == sceneName)
            {
                return;
            }
            StartCoroutine(ILoadSceneFromList(sceneName));
        }

        private IEnumerator ILoadSceneFromList(string sceneName)
        {

            if (loadingAODict.ContainsKey(sceneName))
            {
                AsyncOperation asyncOperation = loadingAODict[sceneName];
                Debug.Log(sceneName + "加载进度为：" + asyncOperation.progress);
                if (asyncOperation.progress < 0.9f)
                {
                    Debug.Log("等待WaitUntil……");
                    yield return new WaitUntil(() => asyncOperation.progress >= 0.9f);
                }
                Debug.Log("开始加载场景" + sceneName + asyncOperation.progress);

                asyncOperation.allowSceneActivation = true;
                loadingAODict.Remove(sceneName);
            }
            else//字典中不存在，则需要加载
            {
                AddSceneToDict(sceneName);
                LoadSceneFromList(sceneName);
            }

        }
    }
}