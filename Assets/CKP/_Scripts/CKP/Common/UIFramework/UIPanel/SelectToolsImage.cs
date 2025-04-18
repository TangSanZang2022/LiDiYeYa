using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIFramework;
using System;
using UnityEngine.EventSystems;

namespace LiDi.CKP
{
    /// <summary>
    /// 选择工具界面
    /// </summary>
    public class SelectToolsImage : MonoBehaviour
    {
        private Toggle toolToggle;
        /// <summary>
        /// 选项Toggle
        /// </summary>
        private Toggle ToolToggle
        {
            get
            {
                if (toolToggle == null)
                {
                    toolToggle = transform.FindChildForName<Toggle>("ToolToggle");
                }
                return toolToggle;
            }
        }

        private Transform content;
        /// <summary>
        /// 存放选项Toggle的父物体
        /// </summary>
        private Transform Content
        {
            get
            {
                if (content == null)
                {
                    content = transform.FindChildForName("Content");
                }
                return content;
            }
        }

        /// <summary>
        /// 存放所有已经创建的选项
        /// </summary>
        List<Toggle> createdToolToggleList = new List<Toggle>();
        // Start is called before the first frame update
        void Start()
        {
            Test();
        }

        private void Test()
        {
            InitToolTogggeGroup(new Dictionary<string, string>(){
                { "Tool01", "扳手" },
                { "Tool02", "梅花起子" },
                { "Tool03", "平口起子"  }
            }
               );
        }
        /// <summary>
        /// 根据工具名称列表初始化工具选择UI
        /// </summary>
        /// <param name="toolNameDict"></param>
        public void InitToolTogggeGroup(Dictionary<string, string> toolNameDict)
        {
            foreach (var item in toolNameDict.Keys)
            {
                Toggle toggle = Instantiate(ToolToggle, Content);
                createdToolToggleList.Add(toggle);
                toggle.transform.FindChildForName<Text>("Label").text = toolNameDict[item];
                toggle.name = item;
                UIEventListener.GetUIEventListener(toggle.transform).pointClickHandler += On_toggleClick;
                GameFacade.Instance.GetBaseToolForID(item).PickUpAction += delegate
                {
                    toggle.isOn = true;
                };
                GameFacade.Instance.GetBaseToolForID(item).PutDownAction += delegate
                {
                    toggle.isOn = false;
                    GameFacade.Instance.Set_currentSelectedTool(null);
                };
                toggle.gameObject.SetActive(true);
            }

            //for (int i = 0; i < toolNameDict.Count; i++)
            //{
            //    int index = i;
            //    Toggle toggle = Instantiate(ToolToggle, Content);
            //    createdToolToggleList.Add(toggle);
            //    toggle.transform.FindChildForName<Text>("Label").text = toolNameDict[index];
            //    UIEventListener.GetUIEventListener(toggle.transform).pointClickHandler += On_toggleClick;
            //    toggle.gameObject.SetActive(true);
            //}
        }
        /// <summary>
        /// 点击了工具选项
        /// </summary>
        /// <param name="eventData"></param>
        private void On_toggleClick(PointerEventData eventData)
        {
            if (eventData.pointerPress.transform.GetComponent<Toggle>().isOn)
            {
                GameFacade.Instance.SelectToolForToolID(eventData.pointerPress.transform.name,true);
                Debug.Log("选择了工具：" + eventData.pointerPress.transform.FindChildForName<Text>("Label").text);
            }
            else
            {
                GameFacade.Instance.SelectToolForToolID(eventData.pointerPress.transform.name, false);
                Debug.Log("放下了工具：" + eventData.pointerPress.transform.FindChildForName<Text>("Label").text);
            }
        }


    }
}