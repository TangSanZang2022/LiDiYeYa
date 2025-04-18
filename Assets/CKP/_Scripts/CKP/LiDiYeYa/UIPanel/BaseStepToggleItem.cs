using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace LiDi.CKP
{
    /// <summary>
    /// 提示步骤Item基类
    /// </summary>
    public class BaseStepToggleItem : MonoBehaviour
    {
        /// <summary>
        /// 完成和未完成文字颜色
        /// </summary>
        [SerializeField]
        protected Color[] textColors;

        /// <summary>
        /// 完成和未完成序号外框
        /// </summary>
        [SerializeField]
        protected Sprite[] numSprites;

        /// <summary>
        ///该Item对应的步骤
        /// </summary>
        protected Step step;

        private Toggle _selfToggle;
        /// <summary>
        /// Toggle
        /// </summary>
        protected Toggle SelfToggle
        {

            get
            {
                if (_selfToggle == null)
                {
                    _selfToggle = transform.GetComponent<Toggle>();
                }

                return _selfToggle;
            }
        }

        private Text _numText;
        /// <summary>
        /// 序号Text
        /// </summary>
        protected Text NumText
        {

            get
            {
                if (_numText == null)
                {
                    _numText = transform.FindChildForName<Text>("NumText");
                }
                return _numText;
            }
        }

        private Image _background;
        /// <summary>
        /// 序号背景图片
        /// </summary>
        protected Image Background
        {

            get
            {
                if (_background == null)
                {
                    _background = transform.FindChildForName<Image>("Background");
                }
                return _background;
            }
        }

        private Text _tipsLabel;
        /// <summary>
        /// 提示Text
        /// </summary>
        protected Text TipsLabel
        {

            get
            {
                if (_tipsLabel == null)
                {
                    _tipsLabel = transform.FindChildForName<Text>("TipsLabel");
                }
                return _tipsLabel;
            }
        }
        /// <summary>
        /// 初始化步骤
        /// </summary>
        /// <param name="step"></param>
        public virtual void Init(Step step)
        {
            this.step = step;
        }


        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="state">-1为初始状态，白色，未操作；
        /// 0为操作错误；
        /// 1为操作正确</param>
        public virtual void UpdateState(int state)
        {
            if (state==-1)
            {
                Init(step);
            }
            else if (state == 0)
            {
                SelfToggle.isOn = false;
            }
            else if (state==1)
            {
                SelfToggle.isOn = true;
            }
            
        }
    }
}