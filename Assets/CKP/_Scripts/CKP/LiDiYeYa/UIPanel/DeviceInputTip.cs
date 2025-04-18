using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LiDi.CKP
{
    /// <summary>
    /// 设备输入面板
    /// </summary>
    public class DeviceInputTip : Topic
    {
        public string answer;
        /// <summary>
        /// 对应的设备屏幕
        /// </summary>
        public DeviceScreen deviceScreen;
        public override void Set_topicData(TopicData topicData_New)
        {
            //将自己设置到最前端
            transform.SetSiblingIndex(transform.parent.childCount - 1);
            ErrorTip.SetActive(false);
            ResetAllToggles();
            topicData = topicData_New;
            TitleText.text = topicData.Title;
            List<string> options = new List<string>(topicData.Options);
            int optionsNum = topicData.Options.Count;
            SetToggleMultipleChoice(topicData.MultipleChoice);
            for (int i = optionsNum; i < Toggles.Count; i++)
            {
                int index = i;
                Toggles[index].isOn = false;
                Toggles[index].gameObject.SetActive(false);
            }

            for (int i = 0; i < optionsNum; i++)
            {
                int index = i;
                Toggles[index].gameObject.SetActive(true);
                Toggles[index].transform.FindChildForName<Text>("Label").text = options[index];
            }
        }

        protected override void On_AckButton_Click()
        {

            for (int i = 0; i < Toggles.Count; i++)
            {
                int index = i;
                if (Toggles[index].isOn)
                {
                    answer=Toggles[index].transform.FindChildForName<Text>("Label").text;
                }
            }
            if (string.IsNullOrEmpty(answer))
            {
                return;
            }
            GameFacade.Instance.AddOperatedHotPointTo_operatedHotPointList(new BaseHotPoint() { id = "TopicRight" });
            deviceScreen.targetValue = answer;
            deviceScreen.ChangeToTargetValue();
            gameObject.SetActive(false);
            answer = "";
        }
    }
}