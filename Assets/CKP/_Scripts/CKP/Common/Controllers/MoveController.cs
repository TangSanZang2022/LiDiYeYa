using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

namespace Common
{
    public class MoveController : MonoBehaviour
    {

        public static MoveController instance;

        [Header("模型以及相机初始位置，自动初始化")]
        private Vector3 cameraInitialPos;
        private Vector3 cameraInitialRot;
        private Vector3 modelInitialRot;
        private Vector3 modelInitialPos;
        private Vector3 zoomTargetPos;

        [Header("要控制的物体")]
        [Tooltip("要控制的相机")]
        public Camera myCamera;
        [Tooltip("相机缩放的焦点")]
        public Transform zoomTarget;
        [Tooltip("要观察的模型")]
        public Transform model;
        [Tooltip("判断模型是否出界的物体")]
        public Transform modelScreenPixelMaxTrans;
        [Tooltip("上下旋转默认中心")]
        public Transform rotTarget;

        public Transform moveTarget;
        /// <summary>
        /// 围绕中心点转的物体
        /// </summary>
        public Transform rotAroundTrans;
        /// <summary>
        /// 上下旋转中心坐标
        /// </summary>
        Vector3 rotTargetPos;

        [Tooltip("中心点，计算控制相机距离中心点的距离")]
        public Transform centerTarget;

        /// <summary>
        /// 是否锁定判断模型出界的物体
        /// </summary>
        public bool lock_modelScreenPixelMaxTrans;
        [Header("设置的参数")]
        [Tooltip("缩放速率")]
        public int zoomRate = 10;
        [Tooltip("最远观察距离")]
        public float maxObservationDis = 50;
        [Tooltip("最近观察距离")]
        public float minObservationDis = 0.1f;
        //[Tooltip("水平方向最大移动距离")]
        //public float maxDisH = 2f;
        //[Tooltip("竖直方向最大移动距离")]
        //public float maxDisV = 5f;
        [Tooltip("水平竖直平面移动速度")]
        public float deltaMoveSpeed = 1f;
        [Tooltip("绕自身水平转动时的速度")]
        public float rotSpeedH = 3.5f;
        [Tooltip("绕自身竖直转动时的速度")]
        public float rotSpeedV = 2.5f;
        [Tooltip("相机水平移动时的速度")]
        public float moveSpeed = 0.05f;
        [Tooltip("围绕中心点竖直方向向上最大旋转角度")]
        public float maxRotVAroundTrans = 80f;
        [Tooltip("围绕中心点竖直方向向下旋转角度")]
        public float minRotVAroundTrans = 5f;
        [Tooltip("围绕中心点旋转速度")]
        /// <summary>
        /// 围绕中心点旋转速度
        /// </summary>
        public float rotAroundTransSpeed = 2;
        //[Tooltip("相机最低高度")]
        /// <summary>
        /// 相机最低高度
        /// </summary>
        //public float minHeight;
        //[Tooltip("相机最高高度")]
        /// <summary>
        /// 相机最高高度
        /// </summary>
        //public float maxHeight;
        [Tooltip("围绕自身竖直方向向上最大旋转角度")]
        public float maxRotVAroundSelf = 80f;
        [Tooltip("围绕自身竖直方向向下旋转角度")]
        public float minRotVAroundSelf = -80f;

        [Tooltip("相机距离中心点的距离")]
        public float maxDisBetweenCenter_Cam = 50.0f;
        [Tooltip("层为Ground的index为多少")]
        public int groundLayerIndex = 8;
        [SerializeField]
        /// <summary>
        /// 可以上下左右拖动移动
        /// </summary>
        private bool canMoveHV = true;
        /// <summary>
        /// 可以缩放
        /// </summary>
        private bool canZoom = true;
        [SerializeField]
        /// <summary>
        /// 是否锁住缩放中心
        /// </summary>
        private bool lock_ZoomTarget = false;
        [SerializeField]
        /// <summary>
        /// 像屏幕中心缩放
        /// </summary>
        private bool zoomToScreenCenter = false;
        /// <summary>
        /// 可以旋转
        /// </summary>
        private bool canRot = true;
        /// <summary>
        /// 是否围绕目标点旋转
        /// </summary>
        private bool isRotAroundTarget = true;
        [SerializeField]
        /// <summary>
        /// 是否锁定相机围绕旋转的中心点，如果锁定，则相机围绕固定点旋转
        /// </summary>
        private bool lock_rotTarget;

        [SerializeField]
        /// <summary>
        /// 用移动相机来实现缩放，如果为False,则为用设置相机Field of View来实现缩放
        /// </summary>
        private bool moveForZoom;
        [SerializeField]
        /// <summary>
        /// 相机最大FielOfView
        /// </summary>
        private float maxFielOfView = 100;
        [SerializeField]
        /// <summary>
        /// 相机最小FielOfView
        /// </summary>
        private float minFielOfView = 5;
        /// <summary>
        /// 相机开始FielOfView
        /// </summary>
        private float normalFielOfView;
        [Tooltip("是否wsad控制相机上下左右移动")]
        [SerializeField]
        /// <summary>
        /// 是否wsad控制相机上下左右移动
        /// </summary>
        private bool isKeyboardControlCamMove = false;
        [Tooltip("是否键盘控制相机缩放")]
        [SerializeField]
        /// <summary>
        /// 是否键盘控制相机缩放
        /// </summary>
        private bool isKeyboardControlCamZoom = false;
        [Tooltip("模型屏幕坐标在屏幕宽度向内缩放多少个像素内移动")]
        [SerializeField]
        /// <summary>
        /// 模型屏幕坐标在屏幕宽度向内缩放多少个像素内移动
        /// </summary>
        private float modelScreenPixelMax_X = 100;
        [Tooltip("模型屏幕坐标在屏幕宽度向内缩放多少个像素内移动")]
        [SerializeField]
        /// <summary>
        /// 模型屏幕坐标在屏幕高度向内缩放多少个像素内移动
        /// </summary>
        private float modelScreenPixelMax_Y = 100;

        [SerializeField]
        /// <summary>
        ///是否是2D
        /// </summary>
        private bool is2D = false;
        [SerializeField]
        /// <summary>
        /// 2D模式的时候的位置
        /// </summary>
        private Vector3 pos_2D;
        [SerializeField]
        /// <summary>
        /// 2D模式的时候的旋转
        /// </summary>
        private Vector3 rot_2D;

        [Header("自动获取的变量，不需赋值")]
        private float currentObservationDis;
        private float mouseX;
        private float mouseY;
        private Vector3 zoomDir;
        /// <summary>
        /// 旋转中心默认位置
        /// </summary>
        Vector3 rotTargetNormalPos;
        /// <summary>
        /// 移动系数，Y在minHeight和maxHeight以外的时候，为0，其他时候为1
        /// </summary>
        private float moveCoefficient;
        Vector3 mouseScreenPos;


        /// <summary>
        /// 手指控制的时候各个变量
        /// </summary>
        Touch firstFinger;
        Touch secondFinger;
        Vector2 deltaPos;
        bool isStartIZoomByTwoFinger;
        float twoFingerOldDis;
        float twoFingerNewDis;
        bool isGetdistance;
        Vector3 ScreenPoint;
        Vector3 distance;
        IEnumerator iCamMove;
        void Awake()
        {
            Init();
        }
        // Update is called once per frame
        void Update()
        {

            //if (EventSystem.current.IsPointerOverGameObject()&& EventSystem.current!=null && !EventSystem.current.gameObject.CompareTag("Ignore")) //鼠标在UI上
            //{
            //    Debug.Log(EventSystem.current.name);
            //    return;
            //}

            //if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            //{
            //    return;
            //}
            //if (Input.GetKeyDown(KeyCode.R))
            //{
            //    Reset();
            //}
            ////安卓端调用此方法功能：一个手指活动屏幕模型旋转，一个手指长按屏幕后再滑动模型平面移动，两个手指实现模型朝着两手指中间放大缩小
            //if (Application.platform == RuntimePlatform.Android)
            //{
            //    MoveByFinger();
            //}
            ////PC端调用Move()方法，功能：鼠标左键控制相机平面移动，按住鼠标右键拖动鼠标相机围绕物体旋转，滚轮相机朝着鼠标位子缩放
            //else
            //{
            //    MoveByMouse();
            //}
            if (Input.GetMouseButtonDown(1))
            {
                //获取上下旋转中心的屏幕坐标
                //Vector3 rotTargetScreenPos = myCamera.WorldToScreenPoint(rotTarget.position);
                ////得到和鼠标Y值相同，和上下旋转中心X，Z值相同的屏幕坐标newScreenPos
                //Vector3 newScreenPos = new Vector3(rotTargetScreenPos.x, Input.mousePosition.y, rotTargetScreenPos.z);
                ////将newScreenPos转换为世界坐标
                //Vector3 newWorldPos = myCamera.ScreenToWorldPoint(newScreenPos);
                ////将和鼠标等高，和默认旋转中心X，Z相同的坐标为新旋转中心
                //rotTargetPos = newWorldPos;
                ////检测鼠标等高所在地方是否有物体
                //Ray ray = myCamera.ScreenPointToRay(new Vector3(rotTargetScreenPos.x, Input.mousePosition.y));
                //RaycastHit hitInfo;
                //if (Physics.Raycast(ray, out hitInfo))
                //{
                //    //如果有物体，就以物体和鼠标等高的地方为上下旋转中心
                //    rotTargetPos = hitInfo.point;
                //}
                // Ray ray1 = myCamera.ViewportPointToRay(myCamera.transform.forward);
                if (!lock_rotTarget)
                {
                    RaycastHit hit;
                    if (Physics.Linecast(myCamera.transform.position, myCamera.transform.position + myCamera.transform.forward * 10000, out hit, 1 << groundLayerIndex))
                    {
                        rotTarget.transform.position = hit.point;
                    }
                    else
                    {
                        rotTarget.position = rotTargetNormalPos;
                    }
                    //if (rotAroundScreenCenter)//围绕屏幕中心旋转
                    //{
                    //    Vector3 camScreenPos = myCamera.WorldToScreenPoint(myCamera.transform.position);
                    //    Vector3 rotTargetNowPos = myCamera.ScreenToWorldPoint(new Vector3(camScreenPos.x, camScreenPos.y, rotTarget.transform.position.z));
                    //    rotTarget.transform.position = rotTargetNowPos;
                    //}
                }
                rotAroundTrans.position = myCamera.transform.position;
                rotAroundTrans.rotation = myCamera.transform.localRotation;



            }
            if (!lock_modelScreenPixelMaxTrans)
            {
                RaycastHit hit;
                if (Physics.Linecast(myCamera.transform.position, myCamera.transform.position + myCamera.transform.forward * 10000, out hit, 1 << groundLayerIndex))
                {
                    modelScreenPixelMaxTrans.position = hit.point;
                }
            }
            else
            {
                modelScreenPixelMaxTrans.position = model.position;
            }
        }

        private void LateUpdate()
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject() && !EventSystem.current.gameObject.CompareTag("Ignore")) //鼠标在UI上
            {
                //  Debug.Log(EventSystem.current.name);
                return;
            }

            if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                return;
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                //Reset();
            }
            //安卓端调用此方法功能：一个手指活动屏幕模型旋转，一个手指长按屏幕后再滑动模型平面移动，两个手指实现模型朝着两手指中间放大缩小
            if (Application.platform == RuntimePlatform.Android)
            {
                MoveByFinger();
            }
            //PC端调用Move()方法，功能：鼠标左键控制相机平面移动，按住鼠标右键拖动鼠标相机围绕物体旋转，滚轮相机朝着鼠标位子缩放
            else
            {
                MoveByMouse();
            }
        }

        internal void Set_canZoom(bool canZoom)
        {
            this.canZoom = canZoom;
        }
        /// <summary>
        /// 设置是否锁住旋转中心
        /// </summary>
        /// <param name="lock_ZoomTarget"></param>
        internal void Set_lock_ZoomTarget(bool lock_ZoomTarget)
        {
            this.lock_ZoomTarget = lock_ZoomTarget;
        }
        internal void Set_canRot(bool canRot)
        {
            this.canRot = canRot;
        }

        internal void Set_canMoveHV(bool canMoveHV)
        {
            this.canMoveHV = canMoveHV;
        }

        internal void Set_isRotAroundTarget(bool isRotAroundTarget)
        {
            this.isRotAroundTarget = isRotAroundTarget;
        }
        /// <summary>
        /// 设置相机旋转中心
        /// </summary>
        /// <param name="pos"></param>
        public void Set_rotTarget(Vector3 pos)
        {
            rotTarget.position = pos;
            rotTargetNormalPos = pos;
        }
        /// <summary>
        /// 设置旋转中心点
        /// </summary>
        /// <param name="pos"></param>
        public void Set_zoomTarget(Vector3 pos)
        {
            zoomTarget.position = pos;
        }
        /// <summary>
        /// 设置lock_rotTarget
        /// </summary>
        /// <param name="isLock"></param>
        public void Set_lock_rotTarget(bool isLock)
        {
            lock_rotTarget = isLock;
        }
        /// <summary>
        /// 设置实现缩放的方式，如果为False,则为用设置相机Field of View来实现缩放
        /// </summary>
        /// <param name="isMoveForZoom"></param>
        public void Set_moveForZoom(bool isMoveForZoom)
        {
            moveForZoom = isMoveForZoom;
            if (isMoveForZoom)
            {
                myCamera.DOFieldOfView(normalFielOfView, 1f);
            }
        }
        /// <summary>
        /// 设置相机FieldOfView到初始状态
        /// </summary>
        public void Set_myCameraFieldOfViewToNormal()
        {
            Debug.Log("还原相机normalFielOfView");
            if (DOFieldOfViewTweener != null)
            {
                DOFieldOfViewTweener.Kill();
            }
            myCamera.DOFieldOfView(normalFielOfView, 1f).OnComplete(() => DOFieldOfViewTweener = null);
        }
        /// <summary>
        /// 设置定位物体模型的位置
        /// </summary>
        /// <param name="modelNewPos"></param>
        public void Set_Model(Vector3 modelNewPos)
        {
            model.position = modelNewPos;
        }

        /// <summary>
        /// 设置是否为2D
        /// </summary>
        /// <param name="is2D"></param>
        public void Set_2DMode(bool is2D)
        {
            this.is2D = is2D;
            if (is2D)
            {
                myCamera.transform.localPosition = pos_2D;
                myCamera.transform.localRotation = Quaternion.Euler(rot_2D);
            }

        }
        /// <summary>
        /// 获取2D模式还是3D模式
        /// </summary>
        /// <returns></returns>
        public bool Get_2DMode()
        {
            return is2D;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            instance = this;
            SetAllTargets();
            cameraInitialPos = myCamera.transform.position;
            cameraInitialRot = myCamera.transform.localEulerAngles;
            modelInitialRot = model.localEulerAngles;
            modelInitialPos = model.position;
            zoomTargetPos = zoomTarget.position;
            normalFielOfView = myCamera.fieldOfView;
            rotTargetNormalPos = rotAroundTrans.position;

        }

        private void SetAllTargets()
        {
            if (myCamera == null)
            {
                myCamera = Camera.main;
            }
            if (zoomTarget == null)
            {
                zoomTarget = new GameObject("zoomTarget").transform;
            }
            if (model == null)
            {
                model = new GameObject("model").transform;
            }
            if (rotTarget == null)
            {
                rotTarget = new GameObject("rotTarget").transform;
            }
            if (moveTarget == null)
            {
                moveTarget = new GameObject("moveTarget").transform;
            }
            if (rotAroundTrans == null)
            {
                rotAroundTrans = new GameObject("rotAroundTrans").transform;
            }
            if (centerTarget == null)
            {
                centerTarget = new GameObject("centerTarget").transform;
            }
            if (modelScreenPixelMaxTrans == null)
            {
                modelScreenPixelMaxTrans = new GameObject("modelScreenPixelMaxTrans").transform;
            }

        }
        /// <summary>
        /// PC端鼠标控制移动方法
        /// </summary>
        private void MoveByMouse()
        {
            //MoveByMouse0();
            MoveHAndVForPC();
            ZoomByScrollWheel();
            //ZoomByKeyboard();
            RotByMouse1();
        }
        /// <summary>
        /// 安卓端通过手指控制
        /// </summary>
        private void MoveByFinger()
        {
            if (Input.touchCount == 0)
            {
                StopAllCoroutines();
                isStartIZoomByTwoFinger = false;
                isGetdistance = false;
            }
            else if (Input.touchCount == 1)
            {
                StopAllCoroutines();
                isGetdistance = false;
                isStartIZoomByTwoFinger = false;
                RotByOneFinger();
            }
            else if (Input.touchCount == 2)
            {
                if (!isStartIZoomByTwoFinger)
                {
                    StopAllCoroutines();
                    isStartIZoomByTwoFinger = true;
                    StartCoroutine("IZoomByTwoFinger");
                }
            }
            else
            {
                StopAllCoroutines();
                isStartIZoomByTwoFinger = false;
            }
        }

        /// <summary>
        /// PC端模型水平垂直移动
        /// </summary>
        public void MoveHAndVForPC()
        {
            //if (Input.GetMouseButtonDown(0))
            //{
            //    //将物体由世界坐标系转化为屏幕坐标系，由vector3 结构体变量ScreenSpace存储，以用来明确屏幕坐标系Z轴的位置  
            //    ScreenPoint = Camera.main.WorldToScreenPoint(model.position);

            //    //由于鼠标的坐标系是二维的，需要转化成三维的世界坐标系；  
            //    Vector3 WorldPostion = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, ScreenPoint.z));

            //    //三维的情况下才能来计算鼠标位置与物体的距离  
            //    distance = model.position - WorldPostion;
            //}
            ////当鼠标左键按下时  
            //if (Input.GetMouseButton(0))
            //{
            //    //得到现在鼠标的二维坐标系位置  
            //    Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, ScreenPoint.z);
            //    //将当前鼠标的2维位置转化成三维的位置，再加上鼠标的移动距离  
            //    Vector3 CurPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + distance;
            //    //CurPosition就是物体应该的移动向量赋给transform的position属性        
            //    model.position = CurPosition;
            //    //鼠标释放前都起作用   
            //}
            #region//2021.05.26修改为移动模型改为移动相机
            //if (Input.GetMouseButtonDown(0))
            //{
            //    //获得鼠标按下的屏幕位置  
            //    ScreenPoint = Input.mousePosition;


            //}
            //当鼠标左键按下时  
            if (!canMoveHV)
            {
                return;
            }
            if (Input.GetMouseButtonDown(2) && iCamMove == null)
            {

                iCamMove = ICamMove();
                moveTarget.rotation = myCamera.transform.rotation;
                moveTarget.position = myCamera.transform.position;
                StartCoroutine(iCamMove);
            }
            else if (isKeyboardControlCamMove && ((Input.GetKeyDown(KeyCode.W)) || (Input.GetKeyDown(KeyCode.S)) || (Input.GetKeyDown(KeyCode.A)) || (Input.GetKeyDown(KeyCode.D))) && iCamMove == null)
            {
                iCamMove = ICamMove_Keyboard();
                moveTarget.rotation = myCamera.transform.rotation;
                moveTarget.position = myCamera.transform.position;
                StartCoroutine(iCamMove);
            }

            #endregion

        }
        Tweener moveTweener = null;
        /// <summary>
        /// 向旋转目标运动的Tweener
        /// </summary>
        Tweener moveTweener_Rot = null;
        /// <summary>
        /// 向旋转目标旋转的Tweener
        /// </summary>
        Tweener rotTweener_Rot = null;

        /// <summary>
        /// 相机移动协程
        /// </summary>
        /// <returns></returns>
        IEnumerator ICamMove()
        {

            while (Input.GetMouseButton(2)) //(Input.GetMouseButton(2) && (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0))
            {
                yield return new WaitForSeconds(0.02f);

                //获得鼠标按下的屏幕位置  
                if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
                {
                    // Debug.Log(myCamera.WorldToScreenPoint(myCamera.transform.position));
                    Vector3 modelScreenPosNow = myCamera.WorldToScreenPoint(modelScreenPixelMaxTrans.position);
                    if (modelScreenPosNow.x > Screen.width - modelScreenPixelMax_X && Input.GetAxis("Mouse X") > 0)//相机应该向右移动
                    {
                        continue;
                    }
                    else if (modelScreenPosNow.x < modelScreenPixelMax_X && Input.GetAxis("Mouse X") < 0)
                    {
                        continue;
                    }
                    else if (modelScreenPosNow.y > Screen.height - modelScreenPixelMax_Y && Input.GetAxis("Mouse Y") > 0)
                    {
                        continue;
                    }
                    else if (modelScreenPosNow.y < modelScreenPixelMax_Y && Input.GetAxis("Mouse Y") < 0)
                    {
                        continue;
                    }

                    float disBetweenZoomTargrt = Vector3.Distance(myCamera.transform.position, zoomTarget.position);
                    ScreenPoint = Input.mousePosition;
                    yield return new WaitForSeconds(0.02f);
                    Vector3 curScreenSpace = Input.mousePosition;
                    distance = curScreenSpace - ScreenPoint;
                    //Debug.Log(disBetweenZoomTargrt);
                    float dis = (distance.magnitude / 10) * (disBetweenZoomTargrt);//得到鼠标移动的模长,根据鼠标移动的距离长短来确定相机移动的速度
                    Vector3 dir = distance.normalized;//得到方向


                    //CurPosition就是物体应该的移动向量赋给transform的position属性        
                    //myCamera.transform.Translate(-dir * moveSpeed * dis, Space.Self);

                    Vector3 endPos = (-dir * moveSpeed * dis);

                    //if (endPos.y < minHeight)
                    //{
                    //    float moveDis = (moveTarget.position.y - minHeight);
                    //    endPos = (-dir * moveSpeed * moveDis);
                    //    //dis = 0;
                    //}
                    //else if (moveTarget.position.y >= maxHeight)
                    //{
                    //    float moveDis = (maxHeight - moveTarget.position.y);
                    //    endPos = (-dir * moveSpeed * moveDis);
                    //    // dis = 0;
                    //}
                    moveTarget.Translate(endPos, Space.Self);
                    float dis_center = Vector3.Distance(moveTarget.position, centerTarget.position);//中心点和相机之间的距离
                    if (dis_center > maxDisBetweenCenter_Cam)
                    {
                        Vector3 dir_center = (moveTarget.position - centerTarget.position).normalized;//中心点距离和相机的方向
                        moveTarget.position = dir_center * maxDisBetweenCenter_Cam;
                    }

                    if (moveTweener_Rot != null)
                    {
                        moveTweener_Rot.Kill();
                    }
                    if (rotTweener_Rot != null)
                    {
                        rotTweener_Rot.Kill();
                    }
                    if (moveTweener != null)
                    {
                        moveTweener.Kill();
                    }
                    Vector3 camPos = myCamera.transform.position;
                    moveTweener = myCamera.transform.DOMove(moveTarget.position, 0f);

                }
                else
                {
                    //if (moveTweener != null)
                    //{
                    //    moveTweener.Kill();
                    //}
                }

            }
            if (moveTweener != null)
            {
                moveTweener.Kill();
            }
            //moveTarget.position = myCamera.transform.position;
            iCamMove = null;
        }

        /// <summary>
        /// 相机移动协程,键盘控制
        /// </summary>
        /// <returns></returns>
        IEnumerator ICamMove_Keyboard()
        {

            while ((Input.GetKey(KeyCode.W) && !Input.GetMouseButton(0)) || (Input.GetKey(KeyCode.S) && !Input.GetMouseButton(0)) || (Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.D)))
            //  while ( (Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.D)))
            {
                yield return new WaitForSeconds(0.02f);
                //获得鼠标按下的屏幕位置  
                if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                {
                    Vector3 modelScreenPosNow = myCamera.WorldToScreenPoint(modelScreenPixelMaxTrans.position);
                    if (modelScreenPosNow.x > Screen.width - modelScreenPixelMax_X && Input.GetAxis("Horizontal") > 0)//相机应该向右移动
                    {
                        continue;
                    }
                    else if (modelScreenPosNow.x < modelScreenPixelMax_X && Input.GetAxis("Horizontal") < 0)
                    {
                        continue;
                    }
                    else if (modelScreenPosNow.y > Screen.height - modelScreenPixelMax_Y && Input.GetAxis("Vertical") > 0)
                    {
                        continue;
                    }
                    else if (modelScreenPosNow.y < modelScreenPixelMax_Y && Input.GetAxis("Vertical") < 0)
                    {
                        continue;
                    }
                    float disBetweenZoomTargrt = Vector3.Distance(myCamera.transform.position, zoomTarget.position);
                    yield return new WaitForSeconds(0.02f);
                    distance = new Vector3(-Input.GetAxis("Horizontal"), -Input.GetAxis("Vertical"));
                    float dis = (distance.magnitude / 10) * (disBetweenZoomTargrt);//得到鼠标移动的模长,根据鼠标移动的距离长短来确定相机移动的速度
                    Vector3 dir = distance.normalized;//得到方向


                    //CurPosition就是物体应该的移动向量赋给transform的position属性        
                    myCamera.transform.Translate(-dir * moveSpeed * dis, Space.Self);
                    //myCamera.transform.DOLocalMove(myCamera.transform.localPosition+(- dir * moveSpeed * dis),0.01f);
                    Vector3 endPos = (-dir * moveSpeed * dis);


                    //moveTarget.Translate(endPos, Space.Self);
                    //float dis_center = Vector3.Distance(moveTarget.position, centerTarget.position);//中心点和相机之间的距离
                    //if (dis_center > maxDisBetweenCenter_Cam)
                    //{
                    //    Vector3 dir_center = (moveTarget.position - centerTarget.position).normalized;//中心点距离和相机的方向
                    //    moveTarget.position = dir_center * maxDisBetweenCenter_Cam;
                    //}

                    //if (moveTweener_Rot != null)
                    //{
                    //    moveTweener_Rot.Kill();
                    //}
                    //if (rotTweener_Rot != null)
                    //{
                    //    rotTweener_Rot.Kill();
                    //}
                    //if (moveTweener != null)
                    //{
                    //    moveTweener.Kill();
                    //}
                    //moveTweener = myCamera.transform.DOMove(moveTarget.position, 0f);
                }
                else
                {
                    //if (moveTweener != null)
                    //{
                    //    moveTweener.Kill();
                    //}
                }

            }
            if (moveTweener != null)
            {
                moveTweener.Kill();
            }
            //moveTarget.position = myCamera.transform.position;
            iCamMove = null;
        }
        /// <summary>
        /// 围绕某点旋转指定角度
        /// </summary>
        /// <param name="position">自身坐标</param>
        /// <param name="center">旋转中心</param>
        /// <param name="axis">围绕旋转轴</param>
        /// <param name="angle">旋转角度</param>
        /// <returns></returns>
        public Vector3 RotateRound(Vector3 position, Vector3 center, Vector3 axis, float angle)
        {
            return Quaternion.AngleAxis(angle, axis) * (position - center) + center;
        }

        public void MoveHAndVForAndroid()
        {
            if (!isGetdistance)
            {
                //将物体由世界坐标系转化为屏幕坐标系，由vector3 结构体变量ScreenSpace存储，以用来明确屏幕坐标系Z轴的位置  
                ScreenPoint = Camera.main.WorldToScreenPoint(model.position);

                //由于鼠标的坐标系是二维的，需要转化成三维的世界坐标系；  
                Vector3 WorldPostion = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, ScreenPoint.z));

                //三维的情况下才能来计算鼠标位置与物体的距离  
                distance = model.position - WorldPostion;
                isGetdistance = true;
            }
            //当鼠标左键按下时  
            else
            {
                //得到现在鼠标的二维坐标系位置  
                Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, ScreenPoint.z);
                //将当前鼠标的2维位置转化成三维的位置，再加上鼠标的移动距离  
                Vector3 CurPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + distance;
                //CurPosition就是物体应该的移动向量赋给transform的position属性        
                model.position = CurPosition;
                //鼠标释放前都起作用   
            }
        }
        Tweener DOFieldOfViewTweener = null;
        /// <summary>
        /// 滑轮键缩放
        /// </summary>
        public void ZoomByScrollWheel()
        {
            if (!canZoom)
            {
                return;
            }

            float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
            if (scrollWheel != 0)
            {
                Vector3 modelScreenPosNow = myCamera.WorldToScreenPoint(modelScreenPixelMaxTrans.position);
                if (modelScreenPosNow.x > Screen.width - modelScreenPixelMax_X && scrollWheel > 0)//相机应该向右移动
                {
                    return;
                }
                else if (modelScreenPosNow.x < modelScreenPixelMax_X && scrollWheel > 0)
                {
                    return;
                }
                else if (modelScreenPosNow.y > Screen.height - modelScreenPixelMax_Y && scrollWheel > 0)
                {
                    return;
                }
                else if (modelScreenPosNow.y < modelScreenPixelMax_Y && scrollWheel > 0)
                {
                    return;
                }
                if (moveForZoom)//相机向目标点移动实现缩放
                {
                    if (!lock_ZoomTarget)//没有将缩放中心锁住
                    {
                        if (!zoomToScreenCenter)//不向屏幕中心缩放
                        {
                            //获取缩放中心的屏幕坐标
                            //Vector3 zoomTargetScreenPos = myCamera.WorldToScreenPoint(zoomTarget.position);
                            ////鼠标所在位置的坐标同步缩放中心的Z值
                            //mouseScreenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, zoomTargetScreenPos.z);
                            ////将缩放中心坐标重置为鼠标所在位置
                            //zoomTarget.position = myCamera.ScreenToWorldPoint(mouseScreenPos);
                            Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);
                            RaycastHit hitInfo;
                            //如果鼠标所在位置有物体
                            if (Physics.Raycast(ray, out hitInfo,1000,1<<groundLayerIndex))
                            //if (Physics.Raycast(ray, out hitInfo))
                            {
                                //缩放中心为鼠标所在位置的物体
                                zoomTarget.position = hitInfo.point;
                            }
                        }
                        else//向屏幕中心缩放
                        {
                            Ray ray = myCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
                            RaycastHit hitInfo;
                            //如果鼠标所在位置有物体
                            if (Physics.Raycast(ray, out hitInfo, 1000,  1 << groundLayerIndex))
                            //if (Physics.Raycast(ray, out hitInfo))
                            {
                                //缩放中心为鼠标所在位置的物体
                                zoomTarget.position = hitInfo.point;
                            }
                            //获取缩放中心的屏幕坐标
                            //Vector3 zoomTargetScreenPos = myCamera.WorldToScreenPoint(zoomTarget.position);
                            ////相机所在位置的坐标同步缩放中心的Z值
                            //mouseScreenPos = new Vector3(Screen.width / 2, Screen.height / 2, zoomTargetScreenPos.z);
                            //zoomTarget.position = myCamera.ScreenToWorldPoint(mouseScreenPos);
                        }
                    }
                    //获取相机到缩放距离的方向
                    zoomDir = (zoomTarget.position - myCamera.transform.position).normalized;
                    float dis = Vector3.Distance(zoomTarget.position, myCamera.transform.position);
                    if (scrollWheel > 0)
                    {
                        Zoom(zoomDir, Time.deltaTime * zoomRate * dis/2);
                    }
                    else
                    {
                        Zoom(zoomDir, -Time.deltaTime * zoomRate * dis/2);
                    }
                }
                else//设置相机的Field of View来实现缩放
                {
                    // DOFieldOfViewTweener = myCamera.DOFieldOfView(myCamera.fieldOfView -= scrollWheel*50, 0.5f);
                    float value = myCamera.fieldOfView - scrollWheel * 30f;
                    if (DOFieldOfViewTweener != null)
                    {
                        DOFieldOfViewTweener.Kill();
                    }
                    DOFieldOfViewTweener = myCamera.DOFieldOfView(value, 0f).OnComplete(() =>
                    {
                        DOFieldOfViewTweener = null;
                        if (myCamera.fieldOfView > maxFielOfView) { DOFieldOfViewTweener = myCamera.DOFieldOfView(maxFielOfView, 0f).OnComplete(() => DOFieldOfViewTweener = null); }
                        if (myCamera.fieldOfView < minFielOfView) { DOFieldOfViewTweener = myCamera.DOFieldOfView(minFielOfView, 0f).OnComplete(() => DOFieldOfViewTweener = null); };
                    });

                }
            }

        }
        /// <summary>
        /// 键盘控制缩放
        /// </summary>
        public void ZoomByKeyboard()
        {
            if (!canZoom || !isKeyboardControlCamZoom)
            {
                return;
            }

            float scrollWheel = Input.GetAxis("Vertical");
            //Debug.Log(Input.GetAxis("Vertical"));
            if (scrollWheel != 0)
            {
                Vector3 modelScreenPosNow = myCamera.WorldToScreenPoint(modelScreenPixelMaxTrans.position);
                if (modelScreenPosNow.x > Screen.width - modelScreenPixelMax_X && scrollWheel > 0)//相机应该向右移动
                {
                    return;
                }
                else if (modelScreenPosNow.x < modelScreenPixelMax_X && scrollWheel > 0)
                {
                    return;
                }
                else if (modelScreenPosNow.y > Screen.height - modelScreenPixelMax_Y && scrollWheel > 0)
                {
                    return;
                }
                else if (modelScreenPosNow.y < modelScreenPixelMax_Y && scrollWheel > 0)
                {
                    return;
                }
                Debug.Log(scrollWheel);
                if (moveForZoom)//相机向目标点移动实现缩放
                {
                    if (!lock_ZoomTarget)//没有将缩放中心锁住
                    {
                        if (!zoomToScreenCenter)
                        {
                            //获取缩放中心的屏幕坐标
                            //Vector3 zoomTargetScreenPos = myCamera.WorldToScreenPoint(zoomTarget.position);
                            ////鼠标所在位置的坐标同步缩放中心的Z值
                            //mouseScreenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, zoomTargetScreenPos.z);
                            ////将缩放中心坐标重置为鼠标所在位置
                            //zoomTarget.position = myCamera.ScreenToWorldPoint(mouseScreenPos);
                            Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);
                            RaycastHit hitInfo;
                            //如果鼠标所在位置有物体
                            if (Physics.Raycast(ray, out hitInfo, 1000, 1 << groundLayerIndex))
                            //if (Physics.Raycast(ray, out hitInfo))
                            {
                                //缩放中心为鼠标所在位置的物体
                                zoomTarget.position = hitInfo.point;
                            }
                        }
                        else//向屏幕中心缩放
                        {
                            Ray ray = myCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
                            RaycastHit hitInfo;
                            //如果鼠标所在位置有物体
                            if (Physics.Raycast(ray, out hitInfo, 1000, 1 << groundLayerIndex))
                            //if (Physics.Raycast(ray, out hitInfo))
                            {
                                //缩放中心为鼠标所在位置的物体
                                zoomTarget.position = hitInfo.point;
                            }
                            //获取缩放中心的屏幕坐标
                            //Vector3 zoomTargetScreenPos = myCamera.WorldToScreenPoint(zoomTarget.position);
                            ////相机所在位置的坐标同步缩放中心的Z值
                            //mouseScreenPos = new Vector3(Screen.width / 2, Screen.height / 2, zoomTargetScreenPos.z);
                            //zoomTarget.position = myCamera.ScreenToWorldPoint(mouseScreenPos);
                        }

                    }

                    //获取相机到缩放距离的方向
                    zoomDir = (zoomTarget.position - myCamera.transform.position).normalized;
                    float dis = Vector3.Distance(zoomTarget.position, myCamera.transform.position);
                    if (scrollWheel > 0)
                    {
                        Zoom(zoomDir, Time.deltaTime * zoomRate * dis);
                    }
                    else
                    {
                        Zoom(zoomDir, -Time.deltaTime * zoomRate * dis);
                    }
                }
                else//设置相机的Field of View来实现缩放
                {
                    // DOFieldOfViewTweener = myCamera.DOFieldOfView(myCamera.fieldOfView -= scrollWheel*50, 0.5f);
                    float value = myCamera.fieldOfView - scrollWheel * 30f;
                    if (DOFieldOfViewTweener != null)
                    {
                        DOFieldOfViewTweener.Kill();
                    }
                    DOFieldOfViewTweener = myCamera.DOFieldOfView(value, 0f).OnComplete(() =>
                    {
                        DOFieldOfViewTweener = null;
                        if (myCamera.fieldOfView > maxFielOfView) { DOFieldOfViewTweener = myCamera.DOFieldOfView(maxFielOfView, 0f).OnComplete(() => DOFieldOfViewTweener = null); }
                        if (myCamera.fieldOfView < minFielOfView) { DOFieldOfViewTweener = myCamera.DOFieldOfView(minFielOfView, 0f).OnComplete(() => DOFieldOfViewTweener = null); };
                    });

                }
            }

        }
        /// <summary>
        /// 相机向物体移动
        /// </summary>
        /// <param name="zoomDir">移动方向</param>
        /// <param name="zoomDis">移动距离</param>
        public void Zoom(Vector3 zoomDir, float zoomDis)
        {
            transform.DOKill();
            //获取当前相机和缩放中心的距离
            //zoomDis = zoomDis < 5 ? 5 : zoomDis;
            currentObservationDis = (zoomTarget.position - myCamera.transform.position).magnitude;
            Vector3 targetPos = myCamera.transform.position + zoomDir * zoomDis;
            float endDis = Vector3.Distance(targetPos, zoomTarget.position);
            if (endDis < maxObservationDis && endDis > minObservationDis)
            {
                // myCamera.transform.Translate(zoomDir * zoomDis, Space.World);
                if (moveTweener_Rot != null)
                {
                    moveTweener_Rot.Kill();
                }
                if (rotTweener_Rot != null)
                {
                    rotTweener_Rot.Kill();
                }
                if (moveTweener != null)
                {
                    moveTweener.Kill();
                }
                transform.DOKill();
                moveTweener = myCamera.transform.DOMove(transform.position + zoomDir * zoomDis, 0.5f);
            }

            //if (zoomDis < 0 && currentObservationDis > maxObservationDis)
            //{
            //    return;
            //}
            //else if (zoomDis > 0 && currentObservationDis < minObservationDis)
            //{
            //    return;
            //}
            //lastCamPos = myCamera.transform.position;
            //myCamera.transform.Translate(zoomDir * zoomDis, Space.World);
            //if (currentObservationDis > maxObservationDis|| currentObservationDis < minObservationDis)
            //{
            //    myCamera.transform.position = lastCamPos;
            //}

        }
        /// <summary>
        /// 鼠标右键按住转动摄像机
        /// </summary>
        public void RotByMouse1()
        {
            if (!canRot)
            {
                return;
            }
            #region//2022.04.16将确认相机旋转点放到Update中执行，避免在LateUpdate中旋转相机时卡顿
            //if (Input.GetMouseButtonDown(1))
            //{
            //    //获取上下旋转中心的屏幕坐标
            //    Vector3 rotTargetScreenPos = myCamera.WorldToScreenPoint(rotTarget.position);
            //    //得到和鼠标Y值相同，和上下旋转中心X，Z值相同的屏幕坐标newScreenPos
            //    Vector3 newScreenPos = new Vector3(rotTargetScreenPos.x, Input.mousePosition.y, rotTargetScreenPos.z);
            //    //将newScreenPos转换为世界坐标
            //    Vector3 newWorldPos = myCamera.ScreenToWorldPoint(newScreenPos);
            //    //将和鼠标等高，和默认旋转中心X，Z相同的坐标为新旋转中心
            //    rotTargetPos = newWorldPos;
            //    //检测鼠标等高所在地方是否有物体
            //    Ray ray = myCamera.ScreenPointToRay(new Vector3(rotTargetScreenPos.x, Input.mousePosition.y));
            //    RaycastHit hitInfo;
            //    if (Physics.Raycast(ray, out hitInfo))
            //    {
            //        //如果有物体，就以物体和鼠标等高的地方为上下旋转中心
            //        rotTargetPos = hitInfo.point;
            //    }
            //    Ray ray1 = myCamera.ViewportPointToRay(myCamera.transform.forward);
            //    RaycastHit hit;
            //    if (Physics.Linecast(myCamera.transform.position, myCamera.transform.position + myCamera.transform.forward * 500, out hit, 1 << 8))
            //    {
            //        rotTarget.transform.position = hit.point;
            //    }
            //}
            #endregion
            if (Input.GetMouseButton(1))
            {
                mouseX = Input.GetAxis("Mouse X");
                mouseY = Input.GetAxis("Mouse Y");
                if (is2D)
                {
                    mouseY = 0;
                }
                Rot();
            }

            //mouseX = Input.GetAxis("Mouse X");
            //mouseY = Input.GetAxis("Mouse Y");

        }
        /// <summary>
        /// 一根手指转动
        /// </summary>
        public void RotByOneFinger()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 rotTargetScreenPos = myCamera.WorldToScreenPoint(rotTarget.position);
                Vector3 newScreenPos = new Vector3(rotTargetScreenPos.x, Input.mousePosition.y, rotTargetScreenPos.z);
                Vector3 newWorldPos = myCamera.ScreenToWorldPoint(newScreenPos);
                rotTargetPos = newWorldPos;
                Ray ray = myCamera.ScreenPointToRay(new Vector3(rotTargetScreenPos.x, Input.mousePosition.y));
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))
                {
                    rotTargetPos = hitInfo.point;
                }
            }
            firstFinger = Input.GetTouch(0);
            if (firstFinger.phase == TouchPhase.Moved)
            {
                deltaPos = firstFinger.deltaPosition;

                mouseX = deltaPos.x;
                mouseY = deltaPos.y;
                Rot();
            }
        }
        /// <summary>
        /// 旋转
        /// </summary>
        /// <param name="mouseX">鼠标X轴坐标变化值</param>
        /// <param name="MouseY">鼠标Y轴坐标变化值</param>
        public void Rot()
        {
            //if (Mathf.Abs(mouseX) > Mathf.Abs(mouseY))
            //{
            //    if (mouseX < 0)
            //    {
            //        model.Rotate(Vector3.up, rotSpeedH);
            //    }
            //    else if (mouseX > 0)
            //    {
            //        model.Rotate(Vector3.up, -rotSpeedH);

            //    }
            //}
            //else if (Mathf.Abs(mouseX) * 2 < Mathf.Abs(mouseY))
            //{
            //    if (mouseY < 0)
            //    {
            //        if (rotV < maxRotV)
            //        {
            //            //myCamera.transform.RotateAround(model.transform.position, Vector3.right, rotSpeed);
            //            myCamera.transform.RotateAround(rotTargetPos, Vector3.right, rotSpeedV);
            //            rotV += rotSpeedV;
            //        }
            //    }
            //    else if (mouseY > 0)
            //    {
            //        if (rotV > -maxRotV)
            //        {
            //            //myCamera.transform.RotateAround(model.transform.position, Vector3.right, -rotSpeed);
            //            myCamera.transform.RotateAround(rotTargetPos, Vector3.right, -rotSpeedV);
            //            rotV -= rotSpeedV;
            //        }
            //    }
            //}
            //Quaternion targetRot = Quaternion.LookRotation(new Vector3(mouseX,0, mouseY) * rotSpeedH);
            //myCamera.transform.rotation = Quaternion.Lerp(myCamera.transform.rotation, targetRot, Time.deltaTime);
            //mouseX = 0;
            //mouseY = 0;
            rotTarget.LookAt(rotAroundTrans);
            rotTarget.localRotation = Quaternion.Euler(0, rotTarget.localEulerAngles.y, 0);
            float angle = Mathf.Acos(Vector3.Dot(rotTarget.forward.normalized, (rotAroundTrans.position - rotTarget.position).normalized)) * Mathf.Rad2Deg;

            if (angle >= maxRotVAroundTrans)
            {
            }
            if ((angle < minRotVAroundTrans && mouseY > 0) || (angle > maxRotVAroundTrans && mouseY < 0))
            {
                mouseY = 0;
            }
            #region //2021.05.26修改为旋转相机
            if (!isRotAroundTarget)
            {
                if (Mathf.Abs(mouseX) > Mathf.Abs(mouseY))
                {
                    myCamera.transform.DORotate(myCamera.transform.localEulerAngles + Vector3.up * rotSpeedH * (mouseX == 0 ? 0 : (mouseX < 0 ? 1 : -1)), 1f);
                }
                else if (Mathf.Abs(mouseX) < Mathf.Abs(mouseY))
                {
                    Quaternion selfQ = myCamera.transform.localRotation;
                    Quaternion addQ = Quaternion.Euler(rotSpeedV * (mouseY == 0 ? 0 : (mouseY < 0 ? -1 : 1)), 0, 0);
                    if (moveTweener != null)
                    {
                        moveTweener.Kill();
                    }
                    moveTweener = myCamera.transform.DORotateQuaternion(selfQ * addQ, 1f);
                }
                #endregion

            }
            #region //2022.04.13修改为相机围绕目标点转
            else if (!lock_rotTarget)//没有锁定相机旋转中心
            {

                rotAroundTrans.RotateAround(rotTarget.position, Vector3.up, mouseX * rotAroundTransSpeed);
                rotAroundTrans.RotateAround(rotTarget.position, rotAroundTrans.right, -mouseY * rotAroundTransSpeed);

                if (moveTweener_Rot != null)
                {
                    moveTweener_Rot.Kill();
                }
                if (rotTweener_Rot != null)
                {
                    rotTweener_Rot.Kill();
                }
                if (moveTweener != null)
                {
                    moveTweener.Kill();
                }
                moveTweener_Rot = myCamera.transform.DOMove(rotAroundTrans.position, 0.01f);
                rotTweener_Rot = myCamera.transform.DORotate(rotAroundTrans.localEulerAngles, 0.01f);
            }
            else//锁定了旋转中心
            {
                rotAroundTrans.RotateAround(rotTarget.position, Vector3.up, mouseX * rotAroundTransSpeed);
                rotAroundTrans.RotateAround(rotTarget.position, rotAroundTrans.right, -mouseY * rotAroundTransSpeed);
                if (moveTweener_Rot != null)
                {
                    moveTweener_Rot.Kill();
                }
                if (rotTweener_Rot != null)
                {
                    rotTweener_Rot.Kill();
                }
                if (moveTweener != null)
                {
                    moveTweener.Kill();
                }
                moveTweener_Rot = myCamera.transform.DOMove(rotAroundTrans.localPosition, 0.01f);
                rotTweener_Rot = myCamera.transform.DORotate(rotAroundTrans.localEulerAngles, 0.01f);
            }
            #endregion
        }
        /// <summary>
        /// 两根手指缩放协程
        /// </summary>
        public IEnumerator IZoomByTwoFinger()
        {
            float includedAngle = 0;
            Vector2 centerPos;
            Vector2 firstLine;
            Vector2 secondLine;
            float firstLineDis;
            float secondLineDis;
            while (true)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    continue;
                }
                firstFinger = Input.GetTouch(0);
                secondFinger = Input.GetTouch(1);
                centerPos = (firstFinger.position + secondFinger.position) / 2;
                //获取前一帧手指之间的距离
                twoFingerOldDis = Vector2.Distance(firstFinger.position, secondFinger.position);
                yield return 0;
                //如果手指没有移动，就重新检测
                if (Input.GetTouch(0).phase != TouchPhase.Moved && Input.GetTouch(1).phase != TouchPhase.Moved)
                {
                    continue;
                }
                firstLineDis = Vector3.Distance(Input.GetTouch(0).position, firstFinger.position);
                secondLineDis = Vector3.Distance(Input.GetTouch(1).position, secondFinger.position);
                //获取第一根手指移动的向量
                firstLine = Input.GetTouch(0).position - firstFinger.position;
                //获取第二根手指移动的向量
                secondLine = Input.GetTouch(1).position - secondFinger.position;

                firstFinger = Input.GetTouch(0);
                secondFinger = Input.GetTouch(1);
                //获取此时手指之间的距离
                twoFingerNewDis = Vector2.Distance(firstFinger.position, secondFinger.position);
                //得到前后两帧手指之间的距离差
                float scrollWheel = twoFingerNewDis - twoFingerOldDis;
                //得到两根手指移动向量的点乘
                includedAngle = Vector3.Dot(firstLine, secondLine);
                //如果两个手指相向运动且手指之间按的距离在变化，则缩放
                if (includedAngle <= 0 && scrollWheel != 0)
                {
                    Vector3 zoomTargetScreenPos = myCamera.WorldToScreenPoint(zoomTarget.position);
                    mouseScreenPos = new Vector3(centerPos.x, centerPos.y, zoomTargetScreenPos.z);
                    zoomTarget.position = myCamera.ScreenToWorldPoint(mouseScreenPos);
                    Ray ray = myCamera.ScreenPointToRay(centerPos);
                    RaycastHit hitInfo;
                    if (Physics.Raycast(ray, out hitInfo))
                    {
                        zoomTarget.position = hitInfo.point;
                    }
                    currentObservationDis = (zoomTarget.position - myCamera.transform.position).magnitude;
                    zoomDir = (zoomTarget.position - myCamera.transform.position).normalized;
                    if (scrollWheel > 0)
                    {
                        Zoom(zoomDir, Time.deltaTime * zoomRate * 0.25f);
                    }
                    else if (scrollWheel < 0)
                    {
                        Zoom(zoomDir, -Time.deltaTime * zoomRate * 0.25f);
                    }
                }
                //平移
                if (includedAngle > 0 && firstLineDis > 2 && secondLineDis > 2)
                {
                    MoveHAndVForAndroid();
                    //MoveByOneFinger();
                }
                if (Input.touchCount != 2)
                {
                    isStartIZoomByTwoFinger = false;
                    break;
                }
            }


        }
        /// <summary>
        /// 复位方法
        /// </summary>
        public void ResetCam()
        {
            myCamera.transform.position = cameraInitialPos;
            myCamera.transform.localEulerAngles = cameraInitialRot;
            model.localEulerAngles = modelInitialRot;
            model.position = modelInitialPos;
            zoomTarget.position = zoomTargetPos;
            Debug.Log("相机复位");
        }
       
    }

}

