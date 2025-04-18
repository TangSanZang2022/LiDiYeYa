using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
namespace Common
{
    /// <summary>
    /// 加载ab包管理类
    /// </summary>
    public class AssetBundleManager : MonoSingleton<AssetBundleManager>
    {
        //  private event SetABHandle setAssetBundle;
        //AB包缓存---解决AB包无法重复加载的问题 也有利于提高效率。
        private Dictionary<string, AssetBundle> abCache;

        private AssetBundle mainAB = null; //主包

        private AssetBundleManifest mainManifest = null; //主包中配置文件---用以获取依赖包

        /// <summary>
        /// 不通过主包加载的所有ab包缓存,键为加载地址，值为ab包
        /// </summary>
        private Dictionary<string, AssetBundle> allAbWithoutMainAssets = new Dictionary<string, AssetBundle>();

        /// <summary>
        /// 存放所有ab包的名称和地址，用于检测是否有相同的包在加载
        /// </summary>
        private List<AbLoadData> allAbLoadDataList = new List<AbLoadData>();
        //各个平台下的基础路径 --- 利用宏判断当前平台下的streamingAssets路径
        private string basePath
        {
            get
            {
                //使用StreamingAssets路径注意AB包打包时 勾选copy to streamingAssets
#if UNITY_EDITOR || UNITY_STANDALONE
                return Application.dataPath + "/StreamingAssets/";
#elif UNITY_IPHONE
                return Application.dataPath + "/Raw/";
#elif UNITY_ANDROID
                return Application.dataPath + "!/assets/";
#endif

                return Application.dataPath + "/StreamingAssets/";
            }
        }
        //各个平台下的主包名称 --- 用以加载主包获取依赖信息
        private string mainABName
        {
            get
            {

#if UNITY_EDITOR || UNITY_STANDALONE
                return "StandaloneWindows";
#elif UNITY_IPHONE
                return "IOS";
#elif UNITY_ANDROID
                return "Android";
#else
                 return "WebGl";
#endif


            }
        }

        //继承了单例模式提供的初始化函数
        public override void Init()
        {
            base.Init();
            //初始化字典
            abCache = new Dictionary<string, AssetBundle>();
            //  Set_abCache();
        }
        /// <summary>
        /// 设置存放所有AB包的字典
        /// </summary>
        private void Set_abCache()
        {
            // StartCoroutine(ISet_abCache());
        }

        private IEnumerator ISet_abCache(string abPath = null, string main_abName = null)
        {
            if (string.IsNullOrEmpty(abPath))
            {
                abPath = basePath;
            }
            if (string.IsNullOrEmpty(main_abName))
            {
                main_abName = mainABName;
            }
            if (mainAB == null)
            {
                //根据各个平台下的基础路径和主包名加载主包

                UnityWebRequest mainABReauest = UnityWebRequest.Get(abPath + main_abName);
                Debug.Log(abPath + main_abName);


                yield return mainABReauest.SendWebRequest();
                if (mainABReauest.isHttpError)
                {
                    Debug.LogWarning(string.Format("请求URL：{0}出错，错误内容{1}", abPath + main_abName, mainABReauest.error));
                }
                else
                {
                    byte[] bytes = mainABReauest.downloadHandler.data;
                    Debug.Log(bytes.Length);
                    mainAB = AssetBundle.LoadFromMemory(bytes);
                    //获取主包下的AssetBundleManifest资源文件（存有依赖信息）
                    mainManifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                    Debug.Log(mainManifest.name);
                }
                mainABReauest.Dispose();
            }
        }
        /// <summary>
        /// 通过UnityWebRequest加载AB包
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        private IEnumerator LoadABPackageForUnityWebRequest(string abName, System.Action<AssetBundle> finishLoadObjectHandler, string abPath = null, string main_abName = null)
        {
            yield return StartCoroutine(ISet_abCache(abPath, main_abName));
            AssetBundle ab;
            //加载ab包，需一并加载其依赖包。
            //根据manifest获取所有依赖包的名称 固定API
            string[] dependencies = mainManifest.GetAllDependencies(abName);
            //循环加载所有依赖包
            for (int i = 0; i < dependencies.Length; i++)
            {
                //如果不在缓存则加入
                if (!abCache.ContainsKey(dependencies[i]))
                {
                    //根据依赖包名称进行加载
                    ab = AssetBundle.LoadFromFile(abPath + dependencies[i]);
                    //注意添加进缓存 防止重复加载AB包
                    abCache.Add(dependencies[i], ab);
                }
            }
            //加载目标包 -- 同理注意缓存问题
            if (abCache.ContainsKey(abName))
            {
                ab = abCache[abName];
            }
            else
            {
                UnityWebRequest reauest = UnityWebRequest.Get(abPath + abName);
                yield return reauest.SendWebRequest();
                if (!reauest.isHttpError)
                {
                    byte[] bytes = reauest.downloadHandler.data;
                    ab = AssetBundle.LoadFromMemory(bytes);
                    finishLoadObjectHandler(ab);
                }
                else
                {
                    Debug.LogWarning(string.Format("请求URL：{0}出错，错误内容{1}", abPath + abName, reauest.error));
                }
                reauest.Dispose();


            }

        }
        /// <summary>
        /// 通过UnityWebRequest加载AB包
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        private IEnumerator LoadABPackageForUnityWebRequestWithoutMainAb(string abName, System.Action<AssetBundle> finishLoadObjectHandler, string abPath)
        {

            AssetBundle ab;
            UnityWebRequest reauest = UnityWebRequest.Get(abPath);
            reauest.SendWebRequest();
            yield return IDownLoadProgress(reauest);

            Debug.Log(abPath + "reauest.downloadProgress" + reauest.downloadProgress);
            if (!reauest.isHttpError && reauest.downloadProgress==1)
            {
                
                if (!allAbWithoutMainAssets.ContainsKey(abName))
                {
                    byte[] bytes = reauest.downloadHandler.data;
                    Debug.Log("abPath::" + abPath);
                    ab = AssetBundle.LoadFromMemory(bytes);
                    allAbWithoutMainAssets.Add(abName, ab);//添加到字典
                }
                else
                {
                    Debug.Log(abPath + "的ab包已经存在在字典中");
                    ab = allAbWithoutMainAssets[abName];
                }

                finishLoadObjectHandler(ab);
            }
            else
            {
                finishLoadObjectHandler(null);
                Debug.LogWarning(string.Format("请求URL：{0}出错，错误内容{1}", abPath + abName, reauest.error));
            }
            reauest.Dispose();
        }
        //加载AB包
        private AssetBundle LoadABPackage(string path, string abName)
        {
            AssetBundle ab;
            //加载ab包，需一并加载其依赖包。
            if (mainAB == null)
            {

                //根据各个平台下的基础路径和主包名加载主包
                mainAB = AssetBundle.LoadFromFile(path + mainABName);
                //获取主包下的AssetBundleManifest资源文件（存有依赖信息）
                mainManifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            }
            //根据manifest获取所有依赖包的名称 固定API
            string[] dependencies = mainManifest.GetAllDependencies(abName);
            //循环加载所有依赖包
            for (int i = 0; i < dependencies.Length; i++)
            {
                //如果不在缓存则加入
                if (!abCache.ContainsKey(dependencies[i]))
                {
                    //根据依赖包名称进行加载
                    ab = AssetBundle.LoadFromFile(path + dependencies[i]);
                    //注意添加进缓存 防止重复加载AB包
                    abCache.Add(dependencies[i], ab);
                }
            }
            //加载目标包 -- 同理注意缓存问题
            if (abCache.ContainsKey(abName)) return abCache[abName];
            else
            {
                ab = AssetBundle.LoadFromFile(path + abName);
                abCache.Add(abName, ab);
                return ab;
            }
        }


        //==================三种资源同步加载方式==================
        //提供多种调用方式 便于其它语言的调用（Lua对泛型支持不好）
        #region 同步加载的三个重载
        /// <summary>
        /// 同步加载资源---泛型加载 简单直观 无需显示转换
        /// </summary>
        /// <param name="abName">ab包的名称</param>
        /// <param name="resName">资源名称</param>
        public T LoadResource<T>(string path, string abName, string resName) where T : Object
        {
            //加载目标包
            AssetBundle ab = LoadABPackage(path, abName);
            //返回资源
            return ab.LoadAsset<T>(resName);
        }
        //不指定类型 有重名情况下不建议使用 使用时需显示转换类型
        public Object LoadResource(string path, string abName, string resName)
        {
            //加载目标包
            AssetBundle ab = LoadABPackage(path, abName);

            //返回资源
            return ab.LoadAsset(resName);
        }


        //利用参数传递类型，适合对泛型不支持的语言调用，使用时需强转类型
        public Object LoadResource(string path, string abName, string resName, System.Type type)
        {
            //加载目标包
            AssetBundle ab = LoadABPackage(path, abName);

            //返回资源
            return ab.LoadAsset(resName, type);
        }
        /// <summary>
        /// 从本地加载 AB 资源并实例化
        /// </summary>
        public Object LoadAssetBundleFromFile(string path, string abName, string resName, System.Action<Object> finishLoadObjectHandler, bool unloadAB = true)
        {
            //从本地加载 AB 资源到内存.
            AssetBundle assetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + path + abName);

            //从 AB 资源中获取资源.
            Object o = assetBundle.LoadAsset(resName);
            if (finishLoadObjectHandler != null)
            {
                finishLoadObjectHandler(o);
            }
            if (unloadAB)
            {

                assetBundle.Unload(false);
            }
            return o;
        }

        /// <summary>
        /// 从本地加载 AB 资源并实例化
        /// </summary>
        public GameObject LoadAssetBundleFromFile(string path, string abName, string resName, bool unloadAB = true)
        {
            //从本地加载 AB 资源到内存.
            AssetBundle assetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + path + abName);

            //从 AB 资源中获取资源.
            Object o = assetBundle.LoadAsset(resName);
            GameObject go = Instantiate((GameObject)o);
            if (unloadAB)
            {
                assetBundle.Unload(false);
            }
            return go;
        }
        #endregion


        //================三种资源异步加载方式======================
        /// <summary>
        /// 提供异步加载----注意 这里加载AB包是同步加载，只是加载资源是异步
        /// </summary>
        /// <param name="abName">ab包名称</param>
        /// <param name="resName">资源名称</param>
        public void LoadResourceAsync(string path, string abName, string resName, System.Action<Object> finishLoadObjectHandler)
        {
            AssetBundle ab = LoadABPackage(path, abName);
            //开启协程 提供资源加载成功后的委托
            StartCoroutine(LoadRes(ab, resName, finishLoadObjectHandler));
        }


        private IEnumerator LoadRes(AssetBundle ab, string resName, System.Action<Object> finishLoadObjectHandler)
        {
            if (ab == null) yield break;
            //异步加载资源API
            AssetBundleRequest abr = ab.LoadAssetAsync(resName);
            yield return abr;
            //委托调用处理逻辑
            finishLoadObjectHandler(abr.asset);
        }


        //根据Type异步加载资源
        public void LoadResourceAsync(string path, string abName, string resName, System.Type type, System.Action<Object> finishLoadObjectHandler)
        {
            AssetBundle ab = LoadABPackage(path, abName);
            StartCoroutine(LoadRes(ab, resName, type, finishLoadObjectHandler));
        }


        private IEnumerator LoadRes(AssetBundle ab, string resName, System.Type type, System.Action<Object> finishLoadObjectHandler)
        {
            if (ab == null) yield break;
            AssetBundleRequest abr = ab.LoadAssetAsync(resName, type);
            yield return abr;
            //委托调用处理逻辑
            finishLoadObjectHandler(abr.asset);
        }


        //泛型加载
        public void LoadResourceAsync<T>(string path, string abName, string resName, System.Action<Object> finishLoadObjectHandler) where T : Object
        {
            AssetBundle ab = LoadABPackage(path, abName);
            StartCoroutine(LoadRes<T>(ab, resName, finishLoadObjectHandler));
        }

        private IEnumerator LoadRes<T>(AssetBundle ab, string resName, System.Action<Object> finishLoadObjectHandler) where T : Object
        {
            if (ab == null) yield break;
            AssetBundleRequest abr = ab.LoadAssetAsync<T>(resName);
            yield return abr;
            //委托调用处理逻辑
            finishLoadObjectHandler(abr.asset as T);
        }


        //====================AB包的两种卸载方式=================
        //单个包卸载
        public void UnLoad(string abName)
        {
            if (abCache.ContainsKey(abName))
            {
                abCache[abName].Unload(false);
                //注意缓存需一并移除
                abCache.Remove(abName);
            }
        }

        //所有包卸载
        public void UnLoadAll()
        {
            AssetBundle.UnloadAllAssetBundles(false);
            //注意清空缓存
            abCache.Clear();
            mainAB = null;
            mainManifest = null;
        }


        //=======================UnityWebRequest加载AB包方式===================
        /// <summary>
        ///  根据AB包名称加载
        ///  提供异步加载----注意 这里加载AB包也是异步加载，只是加载资源是异步
        /// </summary>
        /// <param name="abName">ab包的名称</param>
        /// <param name="resName">资源名称</param>
        public void LoadResourceAsyncForUnityWebRequest(string abName, string resName, System.Action<Object> finishLoadObjectHandler, string abPath = null, string main_abName = null)
        {
            StartCoroutine(ILoadResForUnityWebRequest(abName, resName, finishLoadObjectHandler, abPath, main_abName));
        }
        /// <summary>
        /// 根据AB包名称加载协程，要先等待mainAB加载完，所以要在协程中进行
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="resName"></param>
        /// <param name="finishLoadObjectHandler"></param>
        /// <returns></returns>
        private IEnumerator ILoadResForUnityWebRequest(string abName, string resName, System.Action<Object> finishLoadObjectHandler, string abPath = null, string main_abName = null)
        {
            //加载目标包
            AssetBundle ab = null;
            yield return StartCoroutine(LoadABPackageForUnityWebRequest(abName, (getAB) => { ab = getAB; }, abPath, main_abName));
            StartCoroutine(LoadRes(ab, resName, finishLoadObjectHandler));
        }
        /// <summary>
        ///  根据AB包类型加载
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="resName"></param>
        /// <param name="type"></param>
        /// <param name="finishLoadObjectHandler"></param>
        public void LoadResourceAsyncForUnityWebRequest(string abName, string resName, System.Type type, System.Action<Object> finishLoadObjectHandler, string abPath = null, string main_abName = null)
        {
            StartCoroutine(ILoadResForUnityWebRequest(abName, resName, type, finishLoadObjectHandler, abPath, main_abName));
        }
        /// <summary>
        /// 根据AB包type加载协程，要先等待mainAB加载完，所以要在协程中进行
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="resName"></param>
        /// <param name="finishLoadObjectHandler"></param>
        /// <returns></returns>
        private IEnumerator ILoadResForUnityWebRequest(string abName, string resName, System.Type type, System.Action<Object> finishLoadObjectHandler, string abPath = null, string main_abName = null)
        {
            //加载目标包
            AssetBundle ab = null;
            yield return StartCoroutine(LoadABPackageForUnityWebRequest(abName, (getAB) => { ab = getAB; }, abPath, main_abName));
            StartCoroutine(LoadRes(ab, resName, type, finishLoadObjectHandler));
        }

        /// <summary>
        ///  根据AB包类型加载
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="resName"></param>
        /// <param name="type"></param>
        /// <param name="finishLoadObjectHandler"></param>
        public void LoadResourceAsyncForUnityWebRequest<T>(string abName, string resName, System.Type type, System.Action<Object> finishLoadObjectHandler, string abPath = null, string main_abName = null) where T : Object
        {
            StartCoroutine(ILoadResForUnityWebRequest<T>(abName, resName, type, finishLoadObjectHandler, abPath, main_abName));
        }
        /// <summary>
        /// 根据AB包type加载协程，要先等待mainAB加载完，所以要在协程中进行
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="resName"></param>
        /// <param name="finishLoadObjectHandler"></param>
        /// <returns></returns>
        private IEnumerator ILoadResForUnityWebRequest<T>(string abName, string resName, System.Type type, System.Action<Object> finishLoadObjectHandler, string abPath = null, string main_abName = null) where T : Object
        {
            //加载目标包
            AssetBundle ab = null;
            yield return StartCoroutine(LoadABPackageForUnityWebRequest(abName, (getAB) => { ab = getAB; }, abPath, main_abName));
            StartCoroutine(LoadRes<T>(ab, resName, finishLoadObjectHandler));
        }
        /// <summary>
        /// 根据AbLoadData加载ab包
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="abName"></param>
        /// <param name="resName"></param>
        /// <param name="type"></param>
        /// <param name="finishLoadObjectHandler"></param>
        /// <param name="abPath"></param>
        /// <param name="unloadAB"></param>
        private void LoadForAbLoadData<T>(AbLoadData abLoadData) where T : Object
        {
            if (allAbWithoutMainAssets.ContainsKey(abLoadData.abName) )//if (allAbWithoutMainAssets.ContainsKey(abLoadData.abName) && !abLoadData.unloadAB)//字典中存在且缓存资源
            {
                AssetBundle ab = allAbWithoutMainAssets[abLoadData.abName];
                Debug.Log("名为：" + abLoadData.abPath + abLoadData.abName + "的ab包在字典allAbWithoutMainAssets中存在AB包名为：" + ab.name);
                if (ab == null)
                {
                    Debug.LogWarning("路径" + abLoadData.abPath + "中加载到的ab包为空");
                }
                StartCoroutine(LoadResWithoutMainAb<T>(abLoadData, ab, abLoadData.resName, abLoadData.finishLoadObjectHandler, abLoadData.unloadAB));
            }
            else
            {
                StartCoroutine(ILoadResForUnityWebRequestWithoutMainAb<T>(abLoadData, abLoadData.abName, abLoadData.resName, abLoadData.type, abLoadData.finishLoadObjectHandler, abLoadData.abPath, abLoadData.unloadAB));
            }
        }
        /// <summary>
        /// 根据AB包类型直接加载ab包，不加载主包，
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="abName"></param>
        /// <param name="resName"></param>
        /// <param name="type"></param>
        /// <param name="finishLoadObjectHandler"></param>
        /// <param name="abPath">路径，必须为可以直接加载路径，abName不会进行拼接</param>
        /// <param name="unloadAB"></param>
        public void LoadResourceAsyncForUnityWebRequestWithoutMainAb<T>(string abName, string resName, System.Type type, System.Action<Object> finishLoadObjectHandler, string abPath, bool unloadAB = true) where T : Object
        {
            if (string.IsNullOrEmpty(abPath))
            {
                return;
            }
            if (string.IsNullOrEmpty(resName))
            {
                string[] s = abPath.Split("/".ToCharArray());
                resName = s[s.Length - 1];
            }
            if (string.IsNullOrEmpty(abName))
            {
                abName = resName;
            }
            Debug.Log(abPath + "///" + resName);
            AbLoadData abLoadData = new AbLoadData() { abName = abName, resName = resName, type = type, finishLoadObjectHandler = finishLoadObjectHandler, abPath = abPath, unloadAB = unloadAB };

            if (allAbLoadDataList.ToArray().Find((a) => a.abPath == abLoadData.abPath) == null)//如果字典中不存在这个正在加载的ab包，则加载，否则等待正在加载的包完成之后再加载
            {

                if (allAbWithoutMainAssets.ContainsKey(abName))//if (allAbWithoutMainAssets.ContainsKey(abName) && !unloadAB)//字典中存在且缓存资源
                {
                    AssetBundle ab = allAbWithoutMainAssets[abName];
                    Debug.Log("名为：" + abPath + abName + "的ab包在字典allAbWithoutMainAssets中存在AB包名为：" + ab.name);
                    if (ab == null)
                    {
                        Debug.LogWarning("路径" + abPath + "中加载到的ab包为空");
                    }
                    StartCoroutine(LoadResWithoutMainAb<T>(abLoadData, ab, resName, finishLoadObjectHandler, unloadAB));
                }
                else 
                {
                    StartCoroutine(ILoadResForUnityWebRequestWithoutMainAb<T>(abLoadData, abName, resName, type, finishLoadObjectHandler, abPath, unloadAB));
                }
            }
            allAbLoadDataList.Add(abLoadData);
        }
        /// <summary>
        /// 根据AB包type加载协程，要先等待mainAB加载完，所以要在协程中进行
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="resName"></param>
        /// <param name="finishLoadObjectHandler"></param>
        /// <returns></returns>
        private IEnumerator ILoadResForUnityWebRequestWithoutMainAb<T>(AbLoadData abLoadData, string abName, string resName, System.Type type, System.Action<Object> finishLoadObjectHandler, string abPath, bool unLoadAB) where T : Object
        {
            //加载目标包
            AssetBundle ab = null;
            yield return StartCoroutine(LoadABPackageForUnityWebRequestWithoutMainAb(abName, (getAB) => { ab = getAB; }, abPath));
            if (ab==null)
            {
                Debug.LogWarning("路径"+abPath + "中加载到的ab包为空");
            }
            StartCoroutine(LoadResWithoutMainAb<T>(abLoadData, ab, resName, finishLoadObjectHandler, unLoadAB));
        }
        private IEnumerator LoadResWithoutMainAb<T>(AbLoadData abLoadData, AssetBundle ab, string resName, System.Action<Object> finishLoadObjectHandler, bool unLoadAB) where T : Object
        {
            if (ab == null)
            {
                allAbLoadDataList.Remove(abLoadData);
                //allAbWithoutMainAssets.RemoveDictValue<string, AssetBundle>(ab);
                allAbWithoutMainAssets.Remove(abLoadData.abName);
                yield break;
            } 


            AssetBundleRequest abr = ab.LoadAssetAsync<T>(resName);

            yield return abr;
            //委托调用处理逻辑
            finishLoadObjectHandler(abr.asset as T);
          

            //if (unLoadAB)
            //{
            //    allAbWithoutMainAssets.RemoveDictValue<string, AssetBundle>(ab);//如果要卸载，则从字典中删除
            //    ab.Unload(false);
            //    Resources.UnloadUnusedAssets();
            //    System.GC.Collect();
            //}
            if (allAbLoadDataList.Contains(abLoadData))
            {
                string abPath = abLoadData.abPath;
                allAbLoadDataList.Remove(abLoadData);
                AbLoadData abLoadData_SameName = allAbLoadDataList.ToArray().Find((a) => a.abPath == abPath);
                if (abLoadData_SameName != null)//如果字典中不存在这个正在加载的ab包，则加载，否则等待正在加载的包完成之后再加载
                {
                    LoadForAbLoadData<T>(abLoadData_SameName);
                }
                else
                {
                   // allAbWithoutMainAssets.RemoveDictValue<string, AssetBundle>(ab);//如果要卸载，则从字典中删除
                                                                                   
                    allAbWithoutMainAssets.Remove(abLoadData.abName);
                    if (ab!=null)
                    {
                        ab.Unload(false);
                    } 
                    Resources.UnloadUnusedAssets();
                    System.GC.Collect();

                }
            }
            Debug.Log("allAbLoadDataList长度+"+ allAbLoadDataList.Count);
            if (allAbLoadDataList.Count==0)//全部加载完成
            {
                Debug.Log(allAbWithoutMainAssets.Count);
                allAbLoadDataList.Clear();
                allAbWithoutMainAssets.Clear();
              
                Debug.Log("allAbLoadDataList所有模型加载完成");
              //  UnityCallJs.Instance.Send_ModelLoaded_Unity();
            }
        }

        /// <summary>
        /// 下载进度
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private IEnumerator IDownLoadProgress(UnityWebRequest request)
        {
            while (request.downloadProgress!=1)
            {
                Debug.Log(string.Format("{0}下载进度为：{1}", request.url, request.downloadProgress));
                yield return 0;
            }
            Debug.Log(request.url + "下载完成");
        }
        /// <summary>
        /// 加载进度
        /// </summary>
        /// <param name="abr"></param>
        /// <returns></returns>
        private IEnumerator ILoadingProgress(AssetBundleRequest abr)
        {
            while (!abr.isDone)
            {
                Debug.Log(string.Format("{0}的加载进度为：{1}", abr.asset, abr.progress));
                yield return 0;
            }
        }
        private void OnDestroy()
        {
            UnLoadAll();
        }
    }
    /// <summary>
    /// ab包名称和加载路径，用于判断是否有同名ab包同时加载并等待加载
    /// </summary>
    public class AbLoadData
    {
        /// <summary>
        /// ab包名称
        /// </summary>
        public string abName;
        /// <summary>
        /// ab包加载的资源名称
        /// </summary>
        public string resName;
        /// <summary>
        /// 类型
        /// </summary>
        public System.Type type;
        /// <summary>
        /// 回调
        /// </summary>
        public System.Action<Object> finishLoadObjectHandler;
        /// <summary>
        /// 加载路径
        /// </summary>
        public string abPath;
        /// <summary>
        /// 加载完之后是否卸载ab包
        /// </summary>
        public bool unloadAB;
    }
}
