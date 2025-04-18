using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DFDJ
{
    /// <summary>
    /// 相机移动控制器
    /// </summary>
    public class CameraMoveController : BaseController
    {
        public CameraMoveController(GameFacade facade) : base(facade)
        { }

        /// <summary>
        /// 可以上下左右拖动移动
        /// </summary>
        private bool canMoveHV = true;
        /// <summary>
        /// 可以缩放
        /// </summary>
        private bool canZoom = true;
        /// <summary>
        /// 可以旋转
        /// </summary>
        private bool canRot = true;
        private MoveController moveController;

        public override void OnInit()
        {
            if (moveController==null)
            {
                moveController = GameObject.FindObjectOfType<MoveController>();
            }
        }
        public override void OnUpdate()
        {
            
        }
        public override void OnDestory()
        {
            
        }

        /// <summary>
        /// 设置缩放速率
        /// </summary>
        /// <param name="zoomRate"></param>
        public void SetZoomRate(int zoomRate)
        {
            moveController.zoomRate = zoomRate;
        }

        /// <summary>
        /// 设置最远观察距离
        /// </summary>
        /// <param name="zoomRate"></param>
        public void SetMaxObservationDis(float maxObservationDis)
        {
            moveController.maxObservationDis = maxObservationDis;
        }
        /// <summary>
        /// 设置最近观察距离
        /// </summary>
        /// <param name="zoomRate"></param>
        public void SetMinObservationDis(float minObservationDis)
        {
            moveController.minObservationDis = minObservationDis;
        }

        /// <summary>
        /// 设置水平方向最大移动距离
        /// </summary>
        /// <param name="zoomRate"></param>
        public void SetMaxDisH(float maxDisH)
        {
            //moveController.maxDisH = maxDisH;
        }
        /// <summary>
        /// 设置竖直方向最大移动距离
        /// </summary>
        /// <param name="zoomRate"></param>
        public void SetMaxDisV(float maxDisV)
        {
            //moveController.maxDisV = maxDisV;
        }
        /// <summary>
        /// 设置水平竖直平面移动速度
        /// </summary>
        /// <param name="zoomRate"></param>
        public void SetDeltaMoveSpeed(float deltaMoveSpeed)
        {
            moveController.deltaMoveSpeed = deltaMoveSpeed;
        }

        /// <summary>
        /// 设置水平转动时的速度
        /// </summary>
        /// <param name="zoomRate"></param>
        public void SetRotSpeedH(float rotSpeedH)
        {
            moveController.rotSpeedH = rotSpeedH;
        }
        /// <summary>
        /// 设置竖直转动时的速度
        /// </summary>
        /// <param name="zoomRate"></param>
        public void SetRotSpeedV(float rotSpeedV)
        {
            moveController.rotSpeedV = rotSpeedV;
        }
        /// <summary>
        /// 设置相机水平移动时的速度
        /// </summary>
        /// <param name="zoomRate"></param>
        public void SetMoveSpeed(float moveSpeed)
        {
            moveController.moveSpeed = moveSpeed;
        }

        /// <summary>
        /// 设置竖直方向最大旋转角度
        /// </summary>
        /// <param name="zoomRate"></param>
        public void SetMaxRotV(float maxRotV)
        {
            moveController.maxRotVAroundTrans = maxRotV;
        }
        /// <summary>
        /// 设置是否可以水平移动
        /// </summary>
        /// <param name="canMoveHV"></param>
        public void Set_canMoveHV(bool canMoveHV)
        {
            moveController.Set_canMoveHV(canMoveHV);
        }
        /// <summary>
        /// 设置是否可以缩放
        /// </summary>
        /// <param name="canMoveHV"></param>
        public void Set_canZoom(bool canZoom)
        {
            moveController.Set_canZoom(canZoom);
        }
        /// <summary>
        /// 设置是否可以旋转
        /// </summary>
        /// <param name="canMoveHV"></param>
        public void Set_canRot(bool canRot)
        {
            moveController.Set_canRot(canRot);
        }
        /// <summary>
        /// 设置相机是否围绕目标点旋转
        /// </summary>
        /// <param name="isRotAroundTarget"></param>
        public void Set_isRotAroundTarget(bool isRotAroundTarget)
        {
            moveController.Set_isRotAroundTarget(isRotAroundTarget);
        }

        /// <summary>
        /// 设置相机旋转中心
        /// </summary>
        /// <param name="pos"></param>
        public void Set_rotTarget(Vector3 pos)
        {
            moveController.Set_rotTarget(pos);
        }
        /// <summary>
        /// 设置旋转中心点
        /// </summary>
        /// <param name="pos"></param>
        public void Set_zoomTarget(Vector3 pos)
        {
            moveController.Set_zoomTarget(pos);
        }
        /// <summary>
        /// 设置lock_rotTarget
        /// </summary>
        /// <param name="isLock"></param>
        public void Set_lock_rotTarget(bool isLock)
        {
            moveController.Set_lock_rotTarget(isLock);
        }
        /// <summary>
        /// 设置实现缩放的方式，如果为False,则为用设置相机Field of View来实现缩放
        /// </summary>
        /// <param name="isMoveForZoom"></param>
        public void Set_moveForZoom(bool isMoveForZoom)
        {
            moveController.Set_moveForZoom(isMoveForZoom);
        }
        /// <summary>
        /// 设置相机FieldOfView到初始状态
        /// </summary>
        public void Set_myCameraFieldOfViewToNormal()
        {
            moveController.Set_myCameraFieldOfViewToNormal();
        }

        /// <summary>
        /// 设置定位物体模型的位置
        /// </summary>
        /// <param name="modelNewPos"></param>
        public void Set_Model(Vector3 modelNewPos)
        {
            moveController.Set_Model(modelNewPos);
        }

        /// <summary>
        /// 复位方法
        /// </summary>
        public void ResetCam()
        {
            moveController.ResetCam();
        }
    }
}
