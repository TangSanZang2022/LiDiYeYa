using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Common;
namespace MonoSingleton
{
    public class Test : MonoBehaviour
    {
       static bool isInit;
       

       
        // Start is called before the first frame update
        void Start()
        {
            if (!isInit)
            {

                //Debug.Log("添加场景");
                //MySceneManegerForCache.Instance.LoadingSceneTODict(new List<string> { "QingFengQianAn", "QingFengQianAnTest1" });
                //SceneManager.sceneLoaded += delegate { MySceneManegerForCache.Instance.LoadingSceneTODict(new List<string> { "QingFengQianAn", "QingFengQianAnTest1" }); };
                //isInit = true;
            }
            // TestManager.Instance.Func();

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
               // MySceneManegerForCache.Instance.LoadSceneFromList("QingFengQianAnTest1");
                MySceneManager.LoadSceneSync("QingFengQianAnTest1");
                Debug.Log("A");
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                //MySceneManegerForCache.Instance.LoadSceneFromList("QingFengQianAn");
                MySceneManager.LoadSceneSync("QingFengQianAn");
                Debug.Log("B");
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                //MySceneManegerForCache.Instance.LoadSceneFromList("QingFengQianAnTest2");
                MySceneManager.LoadSceneSync("QingFengQianAnTest2");
            }


        }
        
    }
}
