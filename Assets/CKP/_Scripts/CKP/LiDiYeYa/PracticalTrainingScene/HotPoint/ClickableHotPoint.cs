using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LiDi.CKP
{
    /// <summary>
    /// 可点击的热点
    /// </summary>
    public class ClickableHotPoint : BaseHotPoint
    {

        

        protected override void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("鼠标点击在UI上");
                return;
            }
            else
            {
                Debug.Log("鼠标点击在UI之外");
            }
            isClicked = true;
           
        }
       

    }
}