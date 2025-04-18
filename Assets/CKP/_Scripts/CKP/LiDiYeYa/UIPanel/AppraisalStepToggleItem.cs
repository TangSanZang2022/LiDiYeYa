using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LiDi.CKP
{
    /// <summary>
    /// 考核模式任务提示Item
    /// </summary>
    public class AppraisalStepToggleItem : BaseStepToggleItem
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
                    //NumText.color = textColors[1];
                    TipsLabel.color = textColors[1];
                    Background.sprite = numSprites[1];
                }
                else
                {
                   // NumText.color = textColors[0];
                    //TipsLabel.color = textColors[0];
                    //Background.sprite = numSprites[0];
                }
            });

            //序号
           // NumText.text = step.StepIndex.ToString();
            //步骤名称
            TipsLabel.text ="任务"+ step.StepIndex.ToString();
            //Debug.Log(TipsLabel.GetComponent<RectTransform>().sizeDelta.y);
            GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, TipsLabel.preferredHeight + 25);

        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="state">-1为初始状态，白色，未操作；
        /// 0为操作错误；
        /// 1为操作正确</param>
        public override void UpdateState(int state)
        {
            if (state == -1)
            {
                SelfToggle.isOn = false;
                TipsLabel.color = textColors[0];
                Background.sprite = numSprites[0];
            }
            else if (state == 0)
            {
                SelfToggle.isOn = false;
                //NumText.color = textColors[2];
                TipsLabel.color = textColors[2];
                Background.sprite = numSprites[2];
            }
            else if (state == 1)
            {
                SelfToggle.isOn = true;
            }
        }

    }
}