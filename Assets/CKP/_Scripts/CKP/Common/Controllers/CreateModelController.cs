using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tool;
using System;

namespace DFDJ
{
    public class CreateModelController : BaseController
    {

        /// <summary>
        /// 创建模型控制器
        /// </summary>
        /// <param name="gameFacade"></param>
        public CreateModelController(GameFacade gameFacade) : base(gameFacade)
        {

        }
        /// <summary>
        /// 存放所有已经创建了的物体
        /// </summary>
        private Dictionary<string, CreateableObjFromAB> allCreatedObjFromABDict = new Dictionary<string, CreateableObjFromAB>();
        public override void OnInit()
        {
            base.OnInit();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnDestory()
        {
            base.OnDestory();
        }
        /// <summary>
        /// 在点击模型选项时创建模型
        /// </summary>
        /// <param name="baseModelItem"></param>
        public void CreateModelOnClickModelItem( BaseModelItem baseModelItem)
        {
            BaseModelItemData baseModelItemData = baseModelItem.Get_baseModelItemData();
            if (baseModelItemData.loadPath != null)
            {
                AssetBundleManager.Instance.LoadResourceAsyncForUnityWebRequestWithoutMainAb<GameObject>(baseModelItemData.modelName, baseModelItemData.modelName,typeof(GameObject),  (obj) => CreateModelFromAb(obj, baseModelItem),baseModelItemData.loadPath,true);
                //AssetBundleManager.Instance.LoadAssetBundleFromFile(baseModelItemData.loadPath, baseModelItemData.modelName, baseModelItemData.modelName, (obj) => CreateModelFromAb(obj, baseModelItem));
                //GameObject go= AssetBundleManager.Instance.LoadResource<GameObject>(Application.streamingAssetsPath + baseModelItemData.loadPath, baseModelItemData.modelName, baseModelItemData.modelName);
                //Instantiate(go).AddComponent<FollowMouseObj>();

                // AssetBundleManager.Instance.UnLoad(baseModelItemData.modelName);

            }
        }
        /// <summary>
        /// 创建鼠标可操作的物体
        /// </summary>
        public GameObject CreateMouseControlObj()
        {
            GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("MouseControlObjPrefab/MouseControlObj"));
            return go;
        }
        /// <summary>
        /// 通过ab包创建模型
        /// </summary>
        private GameObject CreateModelFromAb(UnityEngine.Object abObj, BaseModelItem baseModelItem)
        {
            BaseModelItemData baseModelItemData = baseModelItem.Get_baseModelItemData();
            GameObject mouseControlObj = CreateMouseControlObj();
            GameObject createdModel = GameObject.Instantiate((GameObject)abObj, mouseControlObj.transform.FindChildForName("Models"));
            createdModel.transform.localPosition = Vector3.zero;
            createdModel.transform.localRotation = new Quaternion();

            CreateableObjFromAB createableObjFromAB = createdModel.GetComponent<CreateableObjFromAB>();
            if (createableObjFromAB == null)
            {
                createableObjFromAB = createdModel.AddComponent<CreateableObjFromAB>();
            }
            if (createableObjFromAB.objSaveSceneData == null)
            {
                createableObjFromAB.objSaveSceneData = new ObjSaveSceneData();
            }
            //createableObjFromAB.objSaveSceneData.abName = baseModelItemData.modelName;
            //createableObjFromAB.objSaveSceneData.abPath = baseModelItemData.loadPath;
            //createableObjFromAB.objSaveSceneData.modelType = baseModelItem.CreatedModelType.ToString();
            Set_CreateableObjFromAB_Data(createableObjFromAB, baseModelItemData.modelName, baseModelItemData.loadPath, baseModelItem.CreatedModelType.ToString());
            baseModelItem.currentCreatingObj = mouseControlObj;
            createableObjFromAB.LayDownAction = delegate
            {
                baseModelItem.currentCreatingObj = null; baseModelItem.ItemToggle.isOn = false;
            };
          
            createableObjFromAB.Init();
            MouseClickableObj mouseClickableObj = createdModel.AddComponent<MouseClickableObj>();
            mouseClickableObj.mouseDownAction += delegate { createableObjFromAB.ClickObj(); };
            createdModel.AddComponent<MouseHighlighterObj>();
            return mouseControlObj;
        }
        /// <summary>
        /// 根据数据切换模型
        /// </summary>
        /// <param name="objSaveSceneData"></param>
        public void ChangeModelForObjSaveSceneData(ObjSaveSceneData objSaveSceneData)
        {

        }
        /// <summary>
        /// 更换模型
        /// </summary>
        /// <param name="loadPath">路径</param>
        /// <param name="modelName">模型名称</param>
        /// <param name="CreatedModel">之前的模型</param>
        public void ChangeModel(string loadPath, string modelName, Transform CreatedModel)
        {
            GameObject go = AssetBundleManager.Instance.LoadAssetBundleFromFile(loadPath, modelName, modelName);
            go.transform.SetParent(CreatedModel.parent);
            go.transform.localPosition = CreatedModel.localPosition;
            go.transform.localRotation = CreatedModel.localRotation;
            go.transform.localScale = CreatedModel.localScale;
            CreateableObjFromAB createableObjFromAB = go.GetComponent<CreateableObjFromAB>();
            if (createableObjFromAB == null)
            {
                createableObjFromAB = go.AddComponent<CreateableObjFromAB>();
            }
            if (createableObjFromAB.objSaveSceneData == null)
            {
                createableObjFromAB.objSaveSceneData = new ObjSaveSceneData();
            }
         
            Set_CreateableObjFromAB_Data(createableObjFromAB, modelName, loadPath, CreatedModel.GetComponent<CreateableObjFromAB>().objSaveSceneData.modelType);
            GameObject.Destroy(CreatedModel.gameObject);
        }
        /// <summary>
        /// 设置创建出来的物体数据
        /// </summary>
        /// <param name="createableObjFromAB"></param>
        /// <param name="modelName"></param>
        /// <param name="loadPath"></param>
        /// <param name="modelType"></param>
        private void Set_CreateableObjFromAB_Data(CreateableObjFromAB createableObjFromAB, string modelName, string loadPath, string modelType, string unityModelID = "")
        {
            createableObjFromAB.objSaveSceneData.abName = modelName;
            createableObjFromAB.objSaveSceneData.abPath = loadPath;
            createableObjFromAB.objSaveSceneData.modelType = modelType;

            if (string.IsNullOrEmpty(unityModelID))
            {
                unityModelID = TimeTool.DateTime2TimeStamp(DateTime.Now, 8, true).ToString() + KeyTool.CreateRandomCode(6);

            }
            createableObjFromAB.objSaveSceneData.unityModelID = unityModelID;

            MouseClickableObj mouseClickableObj = createableObjFromAB.gameObject.AddComponent<MouseClickableObj>();
            mouseClickableObj.mouseDownAction += delegate { createableObjFromAB.ClickObj(); };

            createableObjFromAB.gameObject.AddComponent<MouseHighlighterObj>();
            if (allCreatedObjFromABDict.ContainsKey(unityModelID))//此情况为根据数据来更新模型的时候
            {
                allCreatedObjFromABDict.Remove(unityModelID);
            }
            allCreatedObjFromABDict.Add(unityModelID, createableObjFromAB);
        }

        /// <summary>
        /// 根据保存场景的数据来初始化场景
        /// </summary>
        /// <param name="saveSceneData">保存场景的全部数据</param>
        public void InitSceneFor_SaveSceneData(SaveSceneData saveSceneData)
        {

           
        }
        /// <summary>
        /// 通过ObjSaveSceneData,加载ab包创建模型
        /// </summary>
        private GameObject CreateModelFromAbForSaveSceneData(ObjSaveSceneData objSaveSceneData)
        {
            GameObject mouseControlObj = CreateMouseControlObj();
            GameObject createdModel = AssetBundleManager.Instance.LoadAssetBundleFromFile(objSaveSceneData.abPath, objSaveSceneData.abName, objSaveSceneData.abName);

            createdModel.transform.parent = mouseControlObj.transform.FindChildForName("Models");
            //GameObject createdModel = GameObject.Instantiate((GameObject)abObj, mouseControlObj.transform.FindChildForName("Models"));//通过ab包创建的模型
            createdModel.transform.localPosition = Vector3.zero;
            createdModel.transform.localRotation = new Quaternion();
            CreateableObjFromAB createableObjFromAB = createdModel.GetComponent<CreateableObjFromAB>();
            if (createableObjFromAB == null)
            {
                createableObjFromAB = createdModel.AddComponent<CreateableObjFromAB>();
            }
            if (createableObjFromAB.objSaveSceneData == null)
            {
                createableObjFromAB.objSaveSceneData = new ObjSaveSceneData();
            }
            //createableObjFromAB.objSaveSceneData.abName = baseModelItemData.modelName;
            //createableObjFromAB.objSaveSceneData.abPath = baseModelItemData.loadPath;
            //createableObjFromAB.objSaveSceneData.modelType = baseModelItem.CreatedModelType.ToString();
            Set_CreateableObjFromAB_Data(createableObjFromAB, objSaveSceneData.abName, objSaveSceneData.abPath, objSaveSceneData.modelType, objSaveSceneData.unityModelID);
           
            mouseControlObj.transform.localPosition = new Vector3((float)objSaveSceneData.position_x, (float)objSaveSceneData.position_y, (float)objSaveSceneData.position_z);
            mouseControlObj.transform.localRotation = Quaternion.Euler((float)objSaveSceneData.rotation_x, (float)objSaveSceneData.rotation_y, (float)objSaveSceneData.rotation_z);
            mouseControlObj.transform.localScale = new Vector3((float)objSaveSceneData.scale_x, (float)objSaveSceneData.scale_y, (float)objSaveSceneData.scale_z);
            return createdModel;
        }
        /// <summary>
        /// 根据每一条数据来更新已经创建的模型
        /// </summary>
        /// <param name="objSaveSceneData"></param>
        public void UpdateModelForObjSaveSceneData(ObjSaveSceneData objSaveSceneData)
        {
            CreateableObjFromAB createableObjFromAB_Old;
            if (allCreatedObjFromABDict.TryGetValue(objSaveSceneData.unityModelID, out createableObjFromAB_Old))
            {
                createableObjFromAB_Old.UpdatePosForObjSaveSceneData(objSaveSceneData);//先更新位置
                if ((objSaveSceneData.abPath+ objSaveSceneData.abName)!= (createableObjFromAB_Old.objSaveSceneData.abPath + createableObjFromAB_Old.objSaveSceneData.abName))//模型ab包路径和名称不一样的时候要删掉原来的CreateableObjFromAB创建新的CreateableObjFromAB
                {
                    GameObject go = AssetBundleManager.Instance.LoadAssetBundleFromFile(objSaveSceneData.abPath, objSaveSceneData.abName, objSaveSceneData.abName);
                    go.transform.SetParent(createableObjFromAB_Old.transform.parent);
                    go.transform.localPosition = createableObjFromAB_Old.transform.localPosition;
                    go.transform.localRotation = createableObjFromAB_Old.transform.localRotation;
                    go.transform.localScale = createableObjFromAB_Old.transform.localScale;
                    CreateableObjFromAB createableObjFromAB_New = go.GetComponent<CreateableObjFromAB>();//新创建的模型
                    if (createableObjFromAB_New == null)
                    {
                        createableObjFromAB_New = go.AddComponent<CreateableObjFromAB>();
                    }
                    if (createableObjFromAB_New.objSaveSceneData == null)
                    {
                        createableObjFromAB_New.objSaveSceneData = new ObjSaveSceneData();
                    }
                    GameObject.Destroy(createableObjFromAB_Old.gameObject);
                    Set_CreateableObjFromAB_Data(createableObjFromAB_New, objSaveSceneData.abName, objSaveSceneData.abPath, objSaveSceneData.modelType, objSaveSceneData.unityModelID);
                }
            } 
        }



    }
}
