using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

namespace MyTest
{
    public class MyTest : MonoBehaviour
    {
        public string abName;
        public string resName;
        public string path;

        public string abName_Sky;
        public string resName_Sky;
        public string path_Sky;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void CreateObj(Object o)
        {
            GameObject.Instantiate(o as GameObject);
        }
        private void ChangeSkyBox(Object o)
        {
            RenderSettings.skybox = null;
            RenderSettings.skybox = o as Material;
            //GameObject.Instantiate(o as GameObject);
        }
        //private void OnGUI()
        //{
        //    //if (GUI.Button(new Rect(100,200,100,20),"CreateObj"))
        //    //{

        //    //    AssetBundleManager.Instance.LoadResourceAsyncForUnityWebRequestWithoutMainAb<GameObject>(abName, resName, typeof(GameObject), (o) => CreateObj(o), path);
        //    //}
        //    //if (GUI.Button(new Rect(100, 300, 100, 20), "CreateSkyBox"))
        //    //{
        //    //    AssetBundleManager.Instance.LoadResourceAsyncForUnityWebRequestWithoutMainAb<Object>(abName_Sky, resName_Sky, typeof(Object), (o) => ChangeSkyBox(o), path_Sky);
        //    //}
        //    if (GUI.Button(new Rect(100, 200, 100, 20), "loadScene1"))
        //    {
        //        Debug.Log(Resources.Load<TextAsset>("TextConfig/SceneDataText1").text);
        //        CreateSceneData createSceneData = XmlController.ReadJsonForLitJson<CreateSceneData>(Resources.Load<TextAsset>("TextConfig/SceneDataText1").text);
        //        GameFacade.Instance.InitSceneForCreateSceneData(createSceneData);
        //    }
        //    if (GUI.Button(new Rect(100, 300, 100, 20), "loadScene2"))
        //    {
        //        CreateSceneData createSceneData = XmlController.ReadJsonForLitJson<CreateSceneData>(Resources.Load<TextAsset>("TextConfig/SceneDataText2").text);
        //        GameFacade.Instance.InitSceneForCreateSceneData(createSceneData);
        //    }
        //    if (GUI.Button(new Rect(100, 400, 100, 20), "updateModel1"))
        //    {
        //        CreateModelData createModelData = XmlController.ReadJsonForLitJson<CreateModelData>(Resources.Load<TextAsset>("TextConfig/ModelDataText1").text);
        //        GameFacade.Instance.UpdateModelForCreateModelData(createModelData);
        //    }
        //    if (GUI.Button(new Rect(100, 500, 100, 20), "updateModel2"))
        //    {
        //        CreateModelData createModelData = XmlController.ReadJsonForLitJson<CreateModelData>(Resources.Load<TextAsset>("TextConfig/ModelDataText2").text);
        //        GameFacade.Instance.UpdateModelForCreateModelData(createModelData);
        //    }

        //}
    }
}
