using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UIFramework
{
   
    /// <summary>
    /// 可双击的UI
    /// </summary>
    public class DoubleClickableUI : MonoBehaviour
    {
        
        /// <summary>
        /// 鼠标双击事件
        /// </summary>
        public event PointEventHandler pointDoubleClickHandler;
        /// <summary>
        /// 点击图片的间隔时间
        /// </summary>
        private float maskImage_Click_intervalTime;
        /// <summary>
        /// 第一次点击的坐标
        /// </summary>
        private string firstClickPoint;

        IEnumerator ie;
        // Start is called before the first frame update
        void Start()
        {
            UIEventListener.GetUIEventListener(transform).pointClickHandler += OnClick;
        }

      

        private void OnClick(PointerEventData eventData)
        {
            Debug.Log(eventData.position.ToString());
            if ( string.IsNullOrEmpty(firstClickPoint))
            {
                firstClickPoint = eventData.position.ToString();
            }
            else
            {
                if (firstClickPoint == eventData.position.ToString())
                {
                    firstClickPoint = null;
                    if (pointDoubleClickHandler!=null)
                    {
                        pointDoubleClickHandler(eventData);
                    }
                }
            }
            if (ie == null)
            {
                ie = ITimer();
                StartCoroutine(ie);
            }

        }
        /// <summary>
        /// 计时器
        /// </summary>
        /// <returns></returns>
        IEnumerator ITimer()
        {
            maskImage_Click_intervalTime = 0;
            while (true)
            {
                maskImage_Click_intervalTime += Time.deltaTime;
                yield return 0;
                if (maskImage_Click_intervalTime >0.5f)
                {
                    maskImage_Click_intervalTime = 0;
                    ie = null;
                    firstClickPoint = "";
                    break;
                }
            }
        }
        private void OnDisable()
        {
            firstClickPoint = "";
        }
    }
}
