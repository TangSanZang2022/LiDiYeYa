using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Common
{
    /// <summary>
    /// 下载并保存AB包
    /// </summary>
    public class DownLoadAssetBundle : MonoSingleton<DownLoadAssetBundle>
    {
        //private string mainAssetBundleURL = @"http://120.24.90.173/Luademo/AssetBundles/AssetBundles";
        //private string aLLAssetBundleURL = @"http://120.24.90.173/Luademo/AssetBundles/";


        //AB资源文件保存在服务器中的位置(我的服务器寄了，加载不到了)
        private string mainAssetBundleURL = "http://192.168.101.60:1111/DFDJ/Main/StandaloneWindows";
        private string aLLAssetBundleURL = "http://192.168.101.60:1111/DFDJ/Other/";

        /// <summary>
        /// 在streamingAssetsPath下储存
        /// </summary>
        private string saveAssetBundlePath = "//ModelAssetBundles/";
        void Start()
        {

        }

        void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    LoadAssetBundle();
            //}
        }
        public void StartDownLoadAB()
        {
            StartCoroutine("DownLoadMainAssetBundle");
        }
        /// <summary>
        /// 下载主[目录]AssetBundle文件
        /// </summary>
        IEnumerator DownLoadMainAssetBundle()
        {
            //创建一个获取 AssetBundle 文件的 web 请求.
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(new Uri(mainAssetBundleURL));
            //UnityWebRequest request = UnityWebRequest.Get(mainAssetBundleURL);

            if (request.isHttpError)
            {
                Debug.LogError(request.error);
            }
            //发送这个 web 请求.
            yield return request.SendWebRequest();

            //从 web 请求中获取内容，会返回一个 AssetBundle 类型的数据.
            AssetBundle ab = DownloadHandlerAssetBundle.GetContent(request);
            //byte[] bytes = request.downloadHandler.data;
            //AssetBundle ab;
            //ab = AssetBundle.LoadFromMemory(bytes);
            if (ab == null)
            {
                Debug.Log("not ab");
            }

            //从这个“目录文件 AssetBundle”中获取 manifest 数据.
            AssetBundleManifest manifest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            //保存主包
          
            //获取这个 manifest 文件中所有的 AssetBundle 的名称信息.
            string[] names = manifest.GetAllAssetBundles();
            StartCoroutine(DownLoadAssetBundleAndSave(mainAssetBundleURL));
            for (int i = 0; i < names.Length; i++)
            {
                //组拼出下载的路径链接.
                Debug.Log(aLLAssetBundleURL + names[i]);

                //下载单个AssetBundle文件加载到场景中.
                //StartCoroutine(DownLoadSingleAssetBundle(aLLAssetBundleURL + names[i]));

                //下载AssetBundle并保存到本地.
                StartCoroutine(DownLoadAssetBundleAndSave(aLLAssetBundleURL + names[i]));
            }

            ab.Unload(false);
        }

        /// <summary>
        /// 下载单个AssetBundle文件加载到场景中
        /// </summary>
        IEnumerator DownLoadSingleAssetBundle(string url)
        {
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url);
            yield return request.SendWebRequest();
            AssetBundle ab = DownloadHandlerAssetBundle.GetContent(request);

            //通过获取到的 AssetBundle 对象获取内部所有的资源的名称(路径)，返回一个数组.
            string[] names = ab.GetAllAssetNames();
            for (int i = 0; i < names.Length; i++)
            {
                //Debug.Log(names[i]);

                //截取路径地址中的文件名，且无后缀名. 需要引入 System.IO 命名空间.
                string tempName = Path.GetFileNameWithoutExtension(names[i]);
                Debug.Log(tempName);

                //实例化.
                GameObject obj = ab.LoadAsset<GameObject>(tempName);
                GameObject.Instantiate<GameObject>(obj);
            }
        }

        /// <summary>
        /// 下载AssetBundle并保存到本地
        /// </summary>
        IEnumerator DownLoadAssetBundleAndSave(string url)
        {
            //UnityWebRequestAssetBundle.GetAssetBundle(string uri)使用这个API下载回来的资源它是不支持原始数据访问的.
            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();

            //表示下载状态是否完毕.
            if (request.isDone)
            {
                //使用 IO 技术把这个 request 对象存储到本地.(需要后缀)
                //SaveAssetBundle(Path.GetFileName(url), request.downloadHandler.data, request.downloadHandler.data.Length);
                SaveAssetBundle2(Path.GetFileName(url), request);
            }
        }

        /// <summary>
        /// 方法1
        /// 存储AssetBundle为本地文件
        /// </summary>
        private void SaveAssetBundle(string fileName, byte[] bytes, int count)
        {
            //创建一个文件信息对象.
            FileInfo fileInfo = new FileInfo(Application.streamingAssetsPath + saveAssetBundlePath + fileName);

            //通过文件信息对象的“创建”方法，得到一个文件流对象.
            FileStream fs = fileInfo.Create();

            //通过文件流对象，往这个文件内写入信息.
            //fs.Write(字节数组, 开始位置, 数据长度);
            fs.Write(bytes, 0, count);

            //文件写入存储到硬盘，关闭文件流对象，销毁文件对象.
            fs.Flush();
            fs.Close();
            fs.Dispose();

            Debug.Log(fileName + "下载完毕");
        }

        /// <summary>
        /// 方法2
        /// 存储AssetBundle为本地文件
        /// </summary>
        private void SaveAssetBundle2(string fileName, UnityWebRequest request)
        {
            if (File.Exists(Application.streamingAssetsPath + saveAssetBundlePath + fileName))
            {
                return;
            }
            //构造文件流.
            FileStream fs = File.Create(Application.streamingAssetsPath + saveAssetBundlePath + fileName);

            //将字节流写入文件里,request.downloadHandler.data可以获取到下载资源的字节流.
            fs.Write(request.downloadHandler.data, 0, request.downloadHandler.data.Length);

            //文件写入存储到硬盘，关闭文件流对象，销毁文件对象.
            fs.Flush();
            fs.Close();
            fs.Dispose();

            Debug.Log(fileName + "下载完毕");
        }

        /// <summary>
        /// 从本地加载 AB 资源并实例化
        /// </summary>
        private void LoadAssetBundle()
        {
            //从本地加载 AB 资源到内存.
            AssetBundle assetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/player1.ab");

            //从 AB 资源中获取资源.
            GameObject player = assetBundle.LoadAsset<GameObject>("Necromancer");

            GameObject.Instantiate<GameObject>(player, Vector3.zero, Quaternion.identity);


        }


    }
}