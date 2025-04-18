using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using MouseControlObj;
using System;
using Tool;
using CreateModelForAB;
namespace DFDJ
{
    /// <summary>
    /// 模型类型
    /// </summary>
    public enum ModelType
    {
        /// <summary>
        /// 未知
        /// </summary>
        unKnow,
        /// <summary>
        /// 工件
        /// </summary>
        WorkpieceModel,
        /// <summary>
        /// 摆件
        /// </summary>
        PlacedPartsModel,
        /// <summary>
        /// 工装
        /// </summary>
        ToolingModel
    }
    /// <summary>
    /// 从AB包创建的模型
    /// </summary>
    public class CreateableObjFromAB : BaseCreateableObjFromAB
    {
       
        [SerializeField]
        /// <summary>
        /// 保存场景时物体数据
        /// </summary>
        public ObjSaveSceneData objSaveSceneData;
        [SerializeField]
        /// <summary>
        /// 跟随鼠标时物体深度
        /// </summary>
        private float pos_Z = 15;
       
        // Start is called before the first frame update
        void Start()
        {


        }
        public override void Init()
        {

            base.Init();
        }
        private void Update()
        {

        }
        /// <summary>
        /// 设置初始Transform信息
        /// </summary>
        /// <param name="localPos"></param>
        /// <param name="localRot"></param>
        /// <param name="LocalScale"></param>
        public void Set_original_value(Vector3 localPos, Vector3 localRot, Vector3 LocalScale)
        {
            followMouseObj.Set_original_value(localPos, localRot, LocalScale);
        }
        private void OnMouseEnter()
        {

        }

        private void OnMouseDown()
        {
            //ClickObj();
        }
        /// <summary>
        /// 放下物体执行
        /// </summary>
        public override void LayDown_CreateableObjFromAB_Action()
        {
            // followMouseObj.LayDown();
            MouseControlObj.Set_mouseControlUIState(true, new List<string>(), objSaveSceneData.abName);

        }
        /// <summary>
        /// 点击物体
        /// </summary>
        public override void ClickObj()
        {
            // followMouseObj.LayDown();
            MouseControlObj.Set_mouseControlUIState(true,new List<string>() , objSaveSceneData.abName);

        }
        /// <summary>
        /// 获取保存场景的信息
        /// </summary>
        /// <returns></returns>
        public ObjSaveSceneData Get_ObjSaveSceneData()
        {
            objSaveSceneData.position_x = followMouseObj.transform.localPosition.x;
            objSaveSceneData.position_y = followMouseObj.transform.localPosition.y;
            objSaveSceneData.position_z = followMouseObj.transform.localPosition.z;

            objSaveSceneData.rotation_x = followMouseObj.transform.localRotation.x;
            objSaveSceneData.rotation_y = followMouseObj.transform.localRotation.y;
            objSaveSceneData.rotation_z = followMouseObj.transform.localRotation.z;

            objSaveSceneData.scale_x = followMouseObj.transform.localScale.x;
            objSaveSceneData.scale_y = followMouseObj.transform.localScale.y;
            objSaveSceneData.scale_z = followMouseObj.transform.localScale.z;

            return objSaveSceneData;
        }
        /// <summary>
        /// 根据数据来更新物体位置
        /// </summary>
        /// <param name="objSaveSceneData"></param>
        public void UpdatePosForObjSaveSceneData(ObjSaveSceneData objSaveSceneData)
        {
            followMouseObj.transform.localPosition = new Vector3((float)objSaveSceneData.position_x, (float)objSaveSceneData.position_y, (float)objSaveSceneData.position_z);//设置位置
            followMouseObj.transform.localRotation = Quaternion.Euler((float)objSaveSceneData.rotation_x, (float)objSaveSceneData.rotation_x, (float)objSaveSceneData.rotation_x);//设置旋转
            followMouseObj.transform.localScale = new Vector3((float)objSaveSceneData.scale_x, (float)objSaveSceneData.scale_y, (float)objSaveSceneData.scale_z);//设置缩放
        }

    }

}