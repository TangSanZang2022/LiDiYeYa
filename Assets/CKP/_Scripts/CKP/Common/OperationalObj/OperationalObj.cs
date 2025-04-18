using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using DG.Tweening;
using Tool;

namespace UnityOprationalObj
{
    /// <summary>
    /// 可操作的物体的基类
    /// </summary>
    public class OperationalObj : MonoBehaviour
    {

        MouseClickableObj mouseClickableObj;
        /// <summary>
        /// 皮带ID，在Unity面板中指定
        /// </summary>
        [SerializeField]
        protected string id;
        [SerializeField]
        /// <summary>
        /// 名称
        /// </summary>
        private string objName;
        private Transform bestViewTrans;
        /// <summary>
        /// 最佳视角的位置
        /// </summary>
        protected Transform BestViewTrans
        {
            get
            {
                if (bestViewTrans==null)
                {
                    bestViewTrans = transform.FindChildForName("BestViewTrans");
                }
                return bestViewTrans;
            }
        }
        private Transform bestAniPos;
        /// <summary>
        /// 动画最佳视角
        /// </summary>
        protected Transform BestAniPos
        {
            get
            {
                if (bestAniPos == null)
                {
                    bestAniPos = transform.FindChildForName("BestAniPos");
                }
                return bestAniPos;
            }
        }
        /// <summary>
        /// 鼠标进入的事件
        /// </summary>
        public event Action MouseEnterHandleAction;
        /// <summary>
        /// 鼠标停留的事件
        /// </summary>
        public event Action MouseOverHandleAction;
        /// <summary>
        /// 鼠标移出的事件
        /// </summary>
        public event Action MouseExitHandleAction;

        /// <summary>
        /// 鼠标按下的事件
        /// </summary>
        public event Action MouseDownHandleAction;
        /// <summary>
        /// 是否需要更新物体
        /// </summary>
        protected bool isNeedUpdateObj = false;
        /// <summary>
        /// 接收到的数据
        /// </summary>
        protected object receivedData;
        // Start is called before the first frame update
        protected virtual void Start()
        {
            OnInit();
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if (isNeedUpdateObj)
            {
                Debug.Log("异步更新");
                UpdateObj(receivedData);
                isNeedUpdateObj = false;
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void OnInit()
        {
            mouseClickableObj = GetComponent<MouseClickableObj>();
            if (mouseClickableObj==null)
            {
                mouseClickableObj = gameObject.AddComponent<MouseClickableObj>();
            }
            mouseClickableObj.mouseEnterAction += delegate
            {
               
                MouseEnterHandle();
                if (MouseEnterHandleAction != null)
                {
                    MouseEnterHandleAction();
                }
            };
            mouseClickableObj.mouseOverAction += delegate
            {
                MouseOverHandle();
                if (MouseOverHandleAction != null)
                {
                    MouseOverHandleAction();
                }
            };
            mouseClickableObj.mouseDownAction += delegate
            {
               
                MouseDownHandle();
                if (MouseDownHandleAction != null)
                {
                    MouseDownHandleAction();
                }
            };
            mouseClickableObj.mouseExitAction += delegate
            {
              
                MouseExitHandle();
                if (MouseExitHandleAction != null)
                {
                    MouseExitHandleAction();
                }
            };
        }
        /// <summary>
        /// 获取ID
        /// </summary>
        /// <returns></returns>
        public string GetID()
        {
            if (string.IsNullOrEmpty(id))
            {
                Debug.LogWarning(string.Format("警告，名字为{0}的物体没有给ID赋值", name));
                return GetObjName();
            }
            return id;
        }
        /// <summary>
        /// 设置ID
        /// </summary>
        /// <param name="id"></param>
        public void SetID(string id)
        {
            this.id = id;
        }
        /// <summary>
        /// 获取名称
        /// </summary>
        /// <returns></returns>
        public string GetObjName()
        {
            if (string.IsNullOrEmpty(objName))
            {
                objName = name;
            }
            return objName;
        }
        /// <summary>
        /// 鼠标进入
        /// </summary>
        private void OnMouseEnter()
        {
            //if (EventSystem.current.IsPointerOverGameObject())  //如果鼠标在Ui上，就不执行
            //{
            //    return;
            //}
            //MouseEnterHandle();
            //if (MouseEnterHandleAction != null)
            //{
            //    MouseEnterHandleAction();
            //}
        }
        /// <summary>
        /// 鼠标按下
        /// </summary>
        private void OnMouseDown()
        {
            //if (EventSystem.current.IsPointerOverGameObject())//如果鼠标在Ui上，就不执行
            //{
            //    return;
            //}
            //MouseDownHandle();
            //if (MouseDownHandleAction != null)
            //{
            //    MouseDownHandleAction();
            //}
        }
       
        private void OnMouseOver()
        {
            //if (EventSystem.current.IsPointerOverGameObject())//如果鼠标在Ui上，就不执行
            //{
            //    return;
            //}
            //MouseOverHandle();
            //if (MouseOverHandleAction != null)
            //{
            //    MouseOverHandleAction();
            //}
        }
        /// <summary>
        /// 鼠标移出
        /// </summary>
        private void OnMouseExit()
        {

            //MouseExitHandle();
            //if (MouseExitHandleAction != null)
            //{
            //    MouseExitHandleAction();
            //}
        }

        /// <summary>
        /// 鼠标进入物体后调用
        /// </summary>
        protected virtual void MouseEnterHandle()
        {
            if (EventSystem.current.IsPointerOverGameObject())//如果鼠标在Ui上，就不执行
            {
                return;
            }
            // Debug.Log(string.Format("鼠标进入id为：{0},名称为：{1}的皮带", id, objName));

        }
        /// <summary>
        /// 鼠标进入物体后调用
        /// </summary>
        protected virtual void MouseDownHandle()
        {
            Debug.Log(string.Format("鼠标点击id为：{0},名称为：{1}的物体", id, objName));
        }
        /// <summary>
        /// 鼠标停留
        /// </summary>
        protected virtual void MouseOverHandle()
        {
           
        }
        /// <summary>
        /// 鼠标进入物体后调用
        /// </summary>
        protected virtual void MouseExitHandle()
        {
            // Debug.Log(string.Format("鼠标移出id为：{0},名称为：{1}的皮带", id, objName));
        }
        /// <summary>
        /// 同步在主线程中更新状态方法
        /// </summary>
        /// <param name="data">更新的时候要传入的信息</param>
        public virtual void UpdateObj(object data)
        {

        }
        /// <summary>
        /// 异步更新物体方法
        /// </summary>
        /// <param name="data"></param>
        public void UpdateObjSync(object data)
        {
            receivedData = data;
            isNeedUpdateObj = true;
        }
        /// <summary>
        /// 变为原状态
        /// </summary>
        public virtual void Reduction()
        {

        }
        /// <summary>
        /// 前往最佳观看视角
        /// </summary>
        public virtual void GoToBestViewPos(Transform moveTrans)
        {
            moveTrans.parent = null;
            moveTrans.DOMove(BestViewTrans.position,1f);
            moveTrans.DORotateQuaternion(BestViewTrans.rotation, 1f);
        }
        /// <summary>
        /// 前往动画最佳视角
        /// </summary>
        /// <param name="moveTrans"></param>
        public virtual void GoToBestAniPos(Transform moveTrans)
        {
            if (BestAniPos==null)
            {
                Debug.Log("BestAniPos为空");
                return;
            }
            moveTrans.parent = null;
            moveTrans.DOMove(BestAniPos.position, 1f);
            moveTrans.DORotateQuaternion(BestAniPos.rotation, 1f);
        }
    }
}