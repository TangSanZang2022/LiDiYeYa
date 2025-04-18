using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Tool
{
    /// <summary>
    /// 鼠标可点击物体
    /// </summary>
    public class MouseClickableObj : MonoBehaviour
    {
        /// <summary>
        /// 双击间隔时间
        /// </summary>
        public float doubleClickWaitTime = 0.2f;
        /// <summary>
        /// 此物体添加MeshCollidee的父物体的名称合集
        /// </summary>
        public List<string> add_MouseClickableObj_Child_partntNameList = new List<string>();

        private List<MouseClickableObj_Child> allMouseClickableObj_ChildList = new List<MouseClickableObj_Child>();
        private bool isClickable;
        /// <summary>
        /// 是否可点击
        /// </summary>
        public bool IsClickable
        {
            get
            {
                return isClickable;
            }

            set
            {
                isClickable = value;
                SetAllMeshCollidersState(isClickable);
            }
        }

        /// <summary>
        /// 是否忽略UI
        /// </summary>
        public bool isIgnoreUI;
        /// <summary>
        /// 鼠标进入事件
        /// </summary>
        public Action mouseEnterAction;
        /// <summary>
        /// 鼠标停留事件
        /// </summary>
        public Action mouseOverAction;
        /// <summary>
        /// 鼠标按下事件
        /// </summary>
        public Action mouseDownAction;
        /// <summary>
        /// 鼠标移出事件
        /// </summary>
        public Action mouseExitAction;
        /// <summary>
        /// 鼠标左键点击
        /// </summary>
        public Action mouseLeftClick;

        /// <summary>
        /// 鼠标双击
        /// </summary>
        public Action mouseDoubleClick;
        /// <summary>
        /// 鼠标右键点击
        /// </summary>
        public Action mouseRightClick;
        // Start is called before the first frame update
        void Start()
        {
            ModelAddMeshCollider();
            //mouseEnterAction += delegate { Debug.Log("mouseEnter：" + name); };
            //mouseDownAction += delegate { Debug.Log("mouseDown：" + name); };
            //mouseExitAction += delegate { Debug.Log("mouseExit：" + name); };

            //mouseLeftClick += delegate { Debug.Log("mouseLeftClick：" + name); };
            //mouseDoubleClick += delegate { Debug.Log("mouseDoubleClick：" + mouseDoubleClick); };
            //mouseRightClick += delegate { Debug.Log("mouseRightClick：" + mouseRightClick); };
            IsClickable = true;
        }

        void ModelAddMeshCollider()
        {
            if (add_MouseClickableObj_Child_partntNameList.Count != 0)
            {
                foreach (var item in add_MouseClickableObj_Child_partntNameList)
                {
                    if (transform.FindChildForName(item)!=null)
                    {
                        foreach (var render in transform.FindChildForName(item).GetComponentsInChildren<MeshRenderer>())
                        {
                            if (render.gameObject.GetComponent<MouseClickableObj_Child>() == null)
                            {
                                MouseClickableObj_Child mouseClickableObj_Child = render.gameObject.AddComponent<MouseClickableObj_Child>();
                                mouseClickableObj_Child.mouseClickableObj = this;
                                allMouseClickableObj_ChildList.Add(mouseClickableObj_Child);
                                render.gameObject.AddComponent<MeshCollider>();
                            }
                        } 
                    }
                }

            }
            else
            {
                foreach (var render in transform.GetComponentsInChildren<MeshRenderer>())
                {

                    if (render.gameObject.GetComponent<MouseClickableObj_Child>() == null)
                    {
                        MouseClickableObj_Child mouseClickableObj_Child = render.gameObject.AddComponent<MouseClickableObj_Child>();
                        mouseClickableObj_Child.mouseClickableObj = this;
                        allMouseClickableObj_ChildList.Add(mouseClickableObj_Child);
                        render.gameObject.AddComponent<MeshCollider>();
                    }
                }
            }

        }

        private void SetAllMeshCollidersState(bool state)
        {
            for (int i = 0; i < allMouseClickableObj_ChildList.Count; i++)
            {
                int index = i;
                allMouseClickableObj_ChildList[index].SetColliderState(state);
            }
        }

       
    }
}
