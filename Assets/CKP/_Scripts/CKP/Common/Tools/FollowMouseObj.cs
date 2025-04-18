using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Tool
{
    /// <summary>
    /// 跟随鼠标的物体
    /// </summary>
    public class FollowMouseObj : MonoBehaviour
    {  /// <summary>
       /// 初始的位置
       /// </summary>
        private Vector3 originalPos;
        /// <summary>
        /// 初始旋转
        /// </summary>
        private Vector3 originalRot;
        /// <summary>
        /// 初始缩放
        /// </summary>
        private Vector3 originalScale;
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
        [Tooltip("射线检测的层，如果为-1，则无需检测层，物体可放置在任意位置")]
        /// <summary>
        /// 射线检测的层，如果为-1，则无需检测层，物体可放置在任意位置
        /// </summary>
        [SerializeField]
        private int rayLayer = 11;
        [SerializeField]
        private Camera myCamera;
        /// <summary>
        /// 跟随鼠标时物体深度
        /// </summary>
        private float pos_Z = 15;
        /// <summary>
        /// 是否是拾起状态
        /// </summary>
        private bool isPickup;
        /// <summary>
        /// 在拾起之前的相对位置
        /// </summary>
        private Vector3 pickUpLocalPos;


        // Start is called before the first frame update
        void Start()
        {
            Set_original_value(Vector3.zero, Vector3.zero, Vector3.one);
            if (myCamera == null)
            {
                myCamera = Camera.main;
            }
            LayDownAction +=delegate{ Set_original_value(Vector3.zero, Vector3.zero, Vector3.one); };
        }


        // Update is called once per frame
        void Update()
        {
            //不是拾起状态就不跟随
            if (!isPickup)
            {
                return;
            }
            Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            //如果鼠标所在位置在可放置区域，则物体在可放置区域表面
            if (rayLayer!=-1)
            {
                if (Physics.Raycast(ray, out hitInfo, 1000, 1 << rayLayer))
                {
                    //缩放中心为鼠标所在位置的物体
                    transform.position = hitInfo.point;
                    if (Input.GetMouseButtonDown(0))//鼠标按下则放下
                    {
                        LayDown();
                    }
                    return;
                }
            }
            else
            {
                if (Physics.Raycast(ray, out hitInfo))//检测到任意物体，就可放置在表面
                {
                    //缩放中心为鼠标所在位置的物体
                    transform.position = hitInfo.point;
                    if (Input.GetMouseButtonDown(0))//鼠标按下则放下
                    {
                        LayDown();
                    }
                    return;
                }
            }
           

            //如果没有检测到任何碰撞，则跟随鼠标
            Vector3 mouseScreenPos = Input.mousePosition;
            Vector3 objWorldPos = myCamera.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, pos_Z));
            transform.position = objWorldPos;
        }
        /// <summary>
        /// 设置深度
        /// </summary>
        /// <param name="z"></param>
        public void Set_pos_Z(float z)
        {
            pos_Z = z;
        }

        /// <summary>
        /// 放下物体
        /// </summary>
        public void LayDown()
        { //不是拾起状态就不执行放下
            if (!isPickup)
            {
                return;
            }
            Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            //如果鼠标所在位置在可放置区域，则物体在可放置区域表面
            if (rayLayer != -1)
            {
                if (Physics.Raycast(ray, out hitInfo, 1000, 1 << rayLayer))
                {
                    //缩放中心为鼠标所在位置的物体
                    transform.position = hitInfo.point;
                    isPickup = false;
                    if (LayDownAction != null)
                    {
                       
                        LayDownAction();
                    }
                    //this.enabled = false;
                }
            }
            else
            {
                if (Physics.Raycast(ray, out hitInfo))//检测到任意物体，就可放置在表面
                {
                    //缩放中心为鼠标所在位置的物体
                    transform.position = hitInfo.point;
                    isPickup = false;
                    if (LayDownAction != null)
                    {
                       
                        LayDownAction();
                    }
                    //this.enabled = false;
                }
            }
           
        }
        /// <summary>
        /// 拾起物体，继续跟随鼠标
        /// </summary>
        public void Pickup()
        {
            if (isPickup)
            {
                return;
            }
            pickUpLocalPos = transform.localPosition;
            isPickup = true;
            if (PickupAction != null)
            {
                PickupAction();
            }
        }
        /// <summary>
        /// 设置初始Transform信息
        /// </summary>
        /// <param name="localPos"></param>
        /// <param name="localRot"></param>
        /// <param name="LocalScale"></param>
        public void Set_original_value(Vector3 localPos,Vector3 localRot,Vector3 LocalScale)
        {
            originalPos = localPos;
            originalRot = localRot;
            originalScale = LocalScale;
        }
        /// <summary>
        /// 复原为初始位置
        /// </summary>
        public void ResetTransform()
        {
            transform.localPosition = originalPos;
            transform.localRotation = Quaternion.Euler(originalRot);
            if (originalScale==Vector3.zero)
            {
                originalScale = Vector3.one;
            }
            transform.localScale = originalScale;
        }
    }


}
