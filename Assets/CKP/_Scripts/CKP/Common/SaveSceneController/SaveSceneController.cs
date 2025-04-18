using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DFDJ;
using LitJson;
namespace Common
{
    /// <summary>
    /// 保存场景控制器
    /// </summary>
    public class SaveSceneController : BaseController
    {
        public SaveSceneController(GameFacade gameFacade) : base(gameFacade)
        { }
        public override void OnInit()
        {
            base.OnInit();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }
        /// <summary>
        /// 将所有场景数据发送到服务器
        /// </summary>
        public void SendAllSaveSceneDataToServer()
        {
            SaveSceneData saveSceneData = Get_AllSaveSceneData();
            string saveSceneDataString = JsonMapper.ToJson(saveSceneData);
            GameFacade.Instance.WriteAllJsonTo_streamingAssetsPath(saveSceneDataString, "/SaveScene/SaveScene.txt");
        }

        /// <summary>
        /// 根据放假你名称获取设备保存场景数据
        /// </summary>
        /// <param name="equipmentRoom"></param>
        /// <returns></returns>
        public SaveSceneData Get_AllSaveSceneData()
        {
            EquipmentRoom equipmentRoom = EquipmentRoom.Unknow;
            SaveSceneData saveSceneData = new SaveSceneData();
            //string[] Names = System.Enum.GetNames(typeof(EquipmentRoom));
            string[] Names = equipmentRoom.GetAllEnumName<EquipmentRoom>();
            for (int i = 1; i < Names.Length; i++)
            {
                int index = i;
                saveSceneData.allRoomSaveSceneData.Add(Get_SaveSceneDataForRoom(Names[index].ToEnum<EquipmentRoom>()));
            }
            return saveSceneData;
        }
        /// <summary>
        /// 根据放假你名称获取设备保存场景数据
        /// </summary>
        /// <param name="equipmentRoom"></param>
        /// <returns></returns>
        public SaveSceneDataRoom Get_SaveSceneDataForRoom(EquipmentRoom equipmentRoom)
        {
            SaveSceneDataRoom saveSceneData = new SaveSceneDataRoom();
            saveSceneData.equipmentRoom = equipmentRoom.ToString();
          
            return saveSceneData;
        }
        public override void OnDestory()
        {
            base.OnDestory();
        }
    }

    [Serializable]
    /// <summary>
    /// 保存场景的所有信息
    /// </summary>
    public class SaveSceneData
    {
        /// <summary>
        /// 所有房间保存场景的信息
        /// </summary>
       public List<SaveSceneDataRoom> allRoomSaveSceneData = new List<SaveSceneDataRoom>();
    }
    [Serializable]
    /// <summary>
    /// 根据建筑类型保存场景的所有信息
    /// </summary>
    public class SaveSceneDataRoom
    {
        /// <summary>
        /// 设备所属房间
        /// </summary>
        public string equipmentRoom;
        /// <summary>
        /// 所有设备的保存场景信息
        /// </summary>
        public List<EquipmentSaveSceneData> equipmentSaveSceneDatas = new List<EquipmentSaveSceneData>();
    }
    /// <summary>
    /// 设备保存场景数据
    /// </summary>
    [Serializable]
    public class EquipmentSaveSceneData
    {
        /// <summary>
        /// 机床设备ID
        /// </summary>
        public string equipmentID;

        /// <summary>
        /// 创建的所有工件数据
        /// </summary>
        public List<ObjSaveSceneData> workpieceObjData = new List<ObjSaveSceneData>();
        /// <summary>
        /// 创建的所有摆件数据
        /// </summary>
        public List<ObjSaveSceneData> placedPartsObjData = new List<ObjSaveSceneData>();
        /// <summary>
        /// 创建的所有工装数据
        /// </summary>
        public List<ObjSaveSceneData> toolingObjData = new List<ObjSaveSceneData>();

    }
    /// <summary>
    /// 每个物体保存场景的数据
    /// </summary>
    [Serializable]
    public class ObjSaveSceneData
    {
        public string unityModelID;
        /// <summary>
        /// 模型类型
        /// </summary>
        public string modelType;
        /// <summary>
        /// ab包的名称
        /// </summary>
        public string abName;
        /// <summary>
        /// ab包的加载路径
        /// </summary>
        public string abPath;
        /// <summary>
        /// 相对坐标X
        /// </summary>
        public double position_x;
        /// <summary>
        /// 相对坐标Y
        /// </summary>
        public double position_y;
        /// <summary>
        /// 相对坐标Z
        /// </summary>
        public double position_z;
        /// <summary>
        /// 旋转X
        /// </summary>
        public double rotation_x;
        /// <summary>
        /// 旋转Y
        /// </summary>
        public double rotation_y;
        /// <summary>
        /// 旋转Z
        /// </summary>
        public double rotation_z;
        /// <summary>
        /// 缩放X
        /// </summary>
        public double scale_x;
        /// <summary>
        /// 缩放Y
        /// </summary>
        public double scale_y;
        /// <summary>
        /// 缩放Z
        /// </summary>
        public double scale_z;
    }
}
