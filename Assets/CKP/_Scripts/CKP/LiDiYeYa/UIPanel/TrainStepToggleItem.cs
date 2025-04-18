using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace LiDi.CKP
{
    /// <summary>
    /// 步骤提示Item
    /// </summary>
    public class TrainStepToggleItem : BaseStepToggleItem
    {
       
        // Start is called before the first frame update
        void Start()
        {

        }
        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init(Step step)
        {
            base.Init(step);
            SelfToggle.onValueChanged.AddListener(delegate
            {
                if (SelfToggle.isOn)
                {
                    NumText.color = textColors[1];
                    TipsLabel.color = textColors[1];
                    Background.sprite = numSprites[1];
                }
                else
                {
                    NumText.color = textColors[0];
                    TipsLabel.color = textColors[0];
                    Background.sprite = numSprites[0];
                }
            });

            //序号
            NumText.text = step.StepIndex.ToString();
            //步骤名称
            TipsLabel.text = step.StepName;
            //Debug.Log(TipsLabel.GetComponent<RectTransform>().sizeDelta.y);
            GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, TipsLabel.preferredHeight + 25);

        }
       
    }
}