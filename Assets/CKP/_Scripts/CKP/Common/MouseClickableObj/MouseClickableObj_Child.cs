using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tool
{
    /// <summary>
    /// 鼠标可点击物体的子物体，用于添加meshCollider和检测鼠标点击
    /// </summary>
    public class MouseClickableObj_Child : MonoBehaviour
    {

        /// <summary>
        /// 点击的间隔时间
        /// </summary>
        private float click_intervalTime;
        /// <summary>
        ///是否点击了一次
        /// </summary>
        private bool isClickOne;

        /// <summary>
        ///是否鼠标左键按下
        /// </summary>
        private bool isMouseLeftDown;
        /// <summary>
        ///是否鼠标右键按下
        /// </summary>
        private bool isMouseRightDown;
        /// <summary>
        /// 可点击的物体
        /// </summary>
        public MouseClickableObj mouseClickableObj
        {
            get;
            set;
        }
        public void SetColliderState(bool state)
        {
            GetComponent<MeshCollider>().enabled = state;
        }
        private void OnMouseEnter()
        {
            if (mouseClickableObj.IsClickable && mouseClickableObj.mouseEnterAction != null)
            {
                if (mouseClickableObj.isIgnoreUI)//忽略UI
                {
                    if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())//如果鼠标在Ui上，就不执行
                    {
                        return;
                    }
                }
                mouseClickableObj.mouseEnterAction();
            }
        }

        private void OnMouseOver()
        {
            if (mouseClickableObj.IsClickable)
            {
                if (mouseClickableObj.isIgnoreUI)//忽略UI
                {
                    if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())//如果鼠标在Ui上，就不执行
                    {
                        return;
                    }
                }
                if (mouseClickableObj.mouseOverAction != null)
                {
                    mouseClickableObj.mouseOverAction();
                }
                //Debug.Log(isMouseLeftDown);
                if (Input.GetMouseButtonDown(0) && !isMouseLeftDown)
                //if (Input.GetMouseButtonDown(0))
                {
                    StartCheckDoubleClick();
                    isMouseLeftDown = true;
                    //StartCheckClick(0);
                }
                if (Input.GetMouseButtonDown(1))
                {
                    isMouseRightDown = true;
                    //StartCheckClick(1);
                }
            }

        }
        private void OnMouseDown()
        {
            if (mouseClickableObj.IsClickable && mouseClickableObj.mouseDownAction != null)
            {
                if (mouseClickableObj.isIgnoreUI)//忽略UI
                {
                    if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())//如果鼠标在Ui上，就不执行
                    {
                        return;
                    }
                }
                mouseClickableObj.mouseDownAction();
            }
        }

        private void OnMouseExit()
        {
            if (mouseClickableObj.IsClickable && mouseClickableObj.mouseExitAction != null)
            {
                if (mouseClickableObj.isIgnoreUI)//忽略UI
                {
                    if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())//如果鼠标在Ui上，就不执行
                    {
                        return;
                    }
                }
                mouseClickableObj.mouseExitAction();
            }

            StopCheckClick();
        }

        private void StopCheckClick()
        {
            isMouseLeftDown = false;
            isMouseRightDown = false;

        }
        /// <summary>
        /// 开始检测是否鼠标左键点击还是鼠标右键点击
        /// </summary>
        /// <param name="mouseIndex"></param>
        private void StartCheckClick(int mouseIndex)
        {
            if (gameObject.activeSelf)
            {
                if (CheckClick == null)
                {
                    CheckClick = ICheckClick(mouseIndex);
                    StartCoroutine(CheckClick);
                }
            }

        }
        private IEnumerator CheckClick;
        IEnumerator ICheckClick(int mouseIndex)
        {
            while (true)
            {
                yield return 0;

                if (mouseIndex == 0)//鼠标左键
                {
                    // Debug.Log("鼠标按下0");
                    if (Input.GetMouseButtonUp(0) && isMouseLeftDown)
                    {
                        //if (mouseClickableObj.mouseLeftClick != null)
                        //{
                        //mouseClickableObj.mouseLeftClick();
                        isMouseLeftDown = false;
                        //}
                    }
                    else if (!isMouseLeftDown)
                    { break; }
                }
                else//鼠标右键
                {
                    // Debug.Log("鼠标按下1");
                    if (Input.GetMouseButtonUp(1) && isMouseRightDown)
                    {
                        if (mouseClickableObj.mouseRightClick != null)
                        {
                            mouseClickableObj.mouseRightClick();
                            isMouseRightDown = false;
                        }
                    }
                    else if (!isMouseRightDown)
                    {
                        break;
                    }
                }

            }
            CheckClick = null;
        }

        /// <summary>
        /// 开始检测是否双击
        /// </summary>
        private void StartCheckDoubleClick()
        {
            if (isClickOne)
            {
                isClickOne = false;
                if (mouseClickableObj.mouseDoubleClick != null)
                {
                    mouseClickableObj.mouseDoubleClick();
                    StopCoroutine(ie);
                    click_intervalTime = 0;
                    ie = null;
                    isClickOne = false;
                    StopCheckClick();
                }
            }
            else
            {
                if (ie == null)
                {
                    //Debug.Log("开始检测双击");
                    ie = ITimer();
                    isClickOne = true;
                    StartCoroutine(ie);
                }
            }
        }

        IEnumerator ie;

        /// <summary>
        /// 计时器
        /// </summary>
        /// <returns></returns>
        IEnumerator ITimer()
        {

            click_intervalTime = 0;
            while (true)
            {
                //Debug.Log(111);
                click_intervalTime += Time.deltaTime;
                yield return 0;
                if (Input.GetMouseButtonUp(0) && isMouseLeftDown)
                {
                    isMouseLeftDown = false;
                }
                if (click_intervalTime > 0.5f)
                {
                    click_intervalTime = 0;
                    ie = null;
                    isClickOne = false;
                    StopCheckClick();
                    //Debug.Log(222);
                    if (mouseClickableObj.mouseLeftClick != null)
                    {
                        mouseClickableObj.mouseLeftClick();
                        
                       
                    }
                    break;
                }
            }
        }
    }
}