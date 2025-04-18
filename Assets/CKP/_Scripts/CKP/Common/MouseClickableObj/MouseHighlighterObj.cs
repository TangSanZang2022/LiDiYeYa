using HighlightingSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Tool
{
    /// <summary>
    /// 鼠标控制高亮物体
    /// </summary>
    public class MouseHighlighterObj : HighlighterObj
    {
        MouseClickableObj mouseClickableObj;
        protected override void Start()
        {
            base.Start();
            mouseClickableObj = GetComponent<MouseClickableObj>();

            if (mouseClickableObj == null)
            {
                Debug.Log(string.Format("{0}物体的HighlighterObj无法获取MouseClickableObj，已自动添加", name));
                mouseClickableObj = gameObject.AddComponent<MouseClickableObj>();
            }
            mouseClickableObj.mouseEnterAction += () => {
                
               };
            mouseClickableObj.mouseExitAction += () =>
            {
              };
            mouseClickableObj.mouseOverAction += () =>
            {
               
            };

        }
    }
}