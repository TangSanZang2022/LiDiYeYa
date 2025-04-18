using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityOprationalObj;
using System;
using UnityEngine.EventSystems;
using Common;

namespace DFDJ
{
    /// <summary>
    /// 设备所属房间
    /// </summary>
    public enum EquipmentRoom
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknow,
    }
    /// <summary>
    /// 设备基类
    /// </summary>
    public class BaseEquipment : OperationalObj
    {
        [SerializeField]
        /// <summary>
        /// 对应服务器的ID
        /// </summary>
        protected string IDForServer;
        /// <summary>
        /// 当前设备所属房间
        /// </summary>
        public EquipmentRoom equipmentRoom;
        /// <summary>
        /// 要更新状态的所有子物体列表
        /// </summary>
        protected List<IUpdateHandle> updateHandleChild = new List<IUpdateHandle>();
        /// <summary>
        /// 不同工件对应不同动画的模型动画
        /// </summary>
        protected List<MyAniModel> workPieceModelAnis = new List<MyAniModel>();
      
        // protected Animator animator;
        /// <summary>
        /// 是否在最佳视角的位置
        /// </summary>
        public bool isAtBestViewPos = false;
     
      
        protected override void Start()
        {
            base.Start();
          
           
        }
        protected override void Update()
        {
            base.Update();
        }
        /// <summary>
        ///初始化
        /// </summary>
        protected override void OnInit()
        {
            base.OnInit();
            updateHandleChild.AddRange(GetComponentsInChildren<IUpdateHandle>());
          
        }

        private void OnDestroy()
        {
         
        }
       
     
        /// <summary>
        /// 播放动画
        /// </summary>
        public virtual void PlayOrStopAnisModelAni(bool play)
        {
           
        }
      
        /// <summary>
        /// 获取服务器对应的ID，用于数据接口
        /// </summary>
        /// <returns></returns>
        public string GetIDForServer()
        {
            return IDForServer;
        }
      
      


     
        protected override void MouseEnterHandle()
        {
            if (EventSystem.current.IsPointerOverGameObject())//如果鼠标在Ui上，就不执行
            {
                return;
            }

            base.MouseEnterHandle();
        }

        protected override void MouseDownHandle()
        {
            if (EventSystem.current.IsPointerOverGameObject())//如果鼠标在Ui上，就不执行
            {
                return;
            }
            base.MouseDownHandle();

          

        }
      
        /// <summary>
        /// 更新所有子物体
        /// </summary>
        /// <param name="data"></param>
        public void UpdateAllChild(object data)
        {
            foreach (IUpdateHandle item in updateHandleChild)
            {
                item.UpdateHandle(data);
            }
        }

        protected override void MouseExitHandle()
        {
            base.MouseExitHandle();
         
        }
        /// <summary>
        /// 更新物体数据，状态等
        /// </summary>
        /// <param name="data"></param>
        public override void UpdateObj(object data)
        {
            base.UpdateObj(data);


            //Debug.Log(string.Format("收到{0}，更新设备ID为{1}的设备状态", data, GetID()));
        }
        /// <summary>
        /// 设置动画状态
        /// </summary>
        /// <param name="data"></param>
        public virtual void SetAniState(object data)
        {
            string state = data.ToString();

            if (state.Contains("Play"))
            {
                //GoToBestAniPos(Camera.main.transform);
                //if (animator != null)
                //{
                //    animator.ResetTrigger("Stop");
                //    animator.SetTrigger("Play");

                //}
                PlayOrStopAnisModelAni(true);
                Debug.Log("前往动画最佳视角");
            }
            else
            {
                //if (animator != null)
                //{
                //    animator.ResetTrigger("Play");
                //    animator.SetTrigger("Stop");

                //}
                PlayOrStopAnisModelAni(false);
                // GoToBestViewPos(Camera.main.transform);

            }
            //UpdateAllChild(data);
        }
        /// <summary>
        /// 恢复为初始状态
        /// </summary>
        public override void Reduction()
        {
            base.Reduction();
            //if (animator != null)
            //{
            //    animator.SetTrigger("Stop");
            //}
            PlayOrStopAnisModelAni(false);
        }
        /// <summary>
        /// 前往最佳视角
        /// </summary>
        /// <param name="moveTrans"></param>
        public override void GoToBestViewPos(Transform moveTrans)
        {
            isAtBestViewPos = true;
            GameFacade.Instance.Set_myCameraFieldOfViewToNormal();
            base.GoToBestViewPos(moveTrans);
        }

        public override void GoToBestAniPos(Transform moveTrans)
        {
            GameFacade.Instance.Set_myCameraFieldOfViewToNormal();
            base.GoToBestAniPos(moveTrans);
        }


        /// <summary>
        /// 完成获取实时位置数据后回调
        /// </summary>
        /// <param name="data"></param>
        public virtual void RealTimePosHTTPRequestFinishedAction(object data)
        {

        }
       

       

        /// <summary>
        /// 获取保存场景的数据
        /// </summary>
        /// <returns></returns>
        public virtual EquipmentSaveSceneData Get_EquipmentSaveSceneData()
        {
            EquipmentSaveSceneData saveSceneData = new EquipmentSaveSceneData();
            saveSceneData.equipmentID = GetID();
          
            return saveSceneData;
        }
     

        /// <summary>
        /// 根据此设备的保存场景数据来初始化场景
        /// </summary>
        /// <param name="equipmentSaveSceneData"></param>
        public void InitCreateedObjFor_EquipmentSaveSceneData(EquipmentSaveSceneData equipmentSaveSceneData)
        {
          
        }
      

        
    }



   
}