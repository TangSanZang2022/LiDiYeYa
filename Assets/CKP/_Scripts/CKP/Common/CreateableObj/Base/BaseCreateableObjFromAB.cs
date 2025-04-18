using MouseControlObj;
using System;
using System.Collections;
using System.Collections.Generic;
using Tool;
using UnityEngine;
using Common;
namespace CreateModelForAB
{
    /// <summary>
    /// 创建的物体的状态
    /// </summary>
    public enum BaseCreateableObjFromAB_State
    {
        /// <summary>
        /// 正常状态
        /// </summary>
        Normal,
        /// <summary>
        /// 选中状态
        /// </summary>
        Selected,
        /// <summary>
        /// 拖动状态
        /// </summary>
        Draging
    }
    [RequireComponent(typeof(UnloadAssetObj))]
    /// <summary>
    /// 可通过ab包加载创建的物体
    /// </summary>
    public class BaseCreateableObjFromAB : MonoBehaviour
    {
       
        /// <summary>
        /// 创建时事件
        /// </summary>
        public Action CreatedAction;
        /// <summary>
        /// 放下时事件
        /// </summary>
        public Action LayDownAction;
        /// <summary>
        /// 拿起时事件
        /// </summary>
        public Action PickupAction;

        protected BaseCreateableObjFromAB_State baseCreateableObjFromAB_State;


        // Start is called before the first frame update
        void Start()
        {

        }
        [SerializeField]
        /// <summary>
        /// 跟随鼠标时物体深度
        /// </summary>
        private float pos_Z = 15;
        private BaseMouseControlObj mouseControlObj;
        /// <summary>
        /// 鼠标可控制物体
        /// </summary>
        protected BaseMouseControlObj MouseControlObj
        {
            get
            {
                if (mouseControlObj == null)
                {
                    mouseControlObj = transform.parent.parent.GetComponent<BaseMouseControlObj>();
                }
                return mouseControlObj;
            }
        }
        private FollowMouseObj _followMouseObj;
        /// <summary>
        /// 鼠标可控制物体
        /// </summary>
        protected FollowMouseObj followMouseObj
        {
            get
            {
                if (_followMouseObj == null)
                {
                    _followMouseObj = transform.parent.parent.GetComponent<FollowMouseObj>();
                }
                return _followMouseObj;
            }
        }

      /// <summary>
      /// 初始化
      /// </summary>
        public virtual void Init()
        {
            if (followMouseObj!=null)
            {
                Set_followMouseObj();
            }
        }

        protected virtual void Set_followMouseObj()
        {
            followMouseObj.Set_pos_Z(pos_Z);
            followMouseObj.Pickup();
            followMouseObj.PickupAction += PickupAction;
            LayDownAction += delegate { LayDown_CreateableObjFromAB_Action(); };
            followMouseObj.LayDownAction += LayDownAction;
        }
        /// <summary>
        /// 放下物体执行
        /// </summary>
        public virtual void LayDown_CreateableObjFromAB_Action()
        {
            // followMouseObj.LayDown();
            MouseControlObj.Set_mouseControlUIState(true);
            MouseControlObj.Set_ObjAxisState(true);
        }
        /// <summary>
        /// 点击物体
        /// </summary>
        public virtual void ClickObj()
        {
            // followMouseObj.LayDown();
            MouseControlObj.Set_mouseControlUIState(true);
            MouseControlObj.Set_ObjAxisState(true);

        }
        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="state"></param>
        public virtual void Set_baseCreateableObjFromAB_State(BaseCreateableObjFromAB_State state)
        {
            baseCreateableObjFromAB_State = state;
        }

        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="state"></param>
        public virtual BaseCreateableObjFromAB_State Get_baseCreateableObjFromAB_State()
        {
           return baseCreateableObjFromAB_State;
        }

       
    }
}