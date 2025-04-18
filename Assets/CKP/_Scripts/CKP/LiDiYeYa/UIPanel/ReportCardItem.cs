using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace LiDi.CKP
{
    /// <summary>
    /// 成绩单每条数据
    /// </summary>
    public class ReportCardItem : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

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

        private Text _scoreText;
        /// <summary>
        /// 分数Text
        /// </summary>
        protected Text ScoreText
        {

            get
            {
                if (_scoreText == null)
                {
                    _scoreText = transform.FindChildForName<Text>("ScoreText");
                }
                return _scoreText;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public  void Init(Step step)
        {
          
            //序号
            NumText.text = step.StepIndex.ToString();
            //步骤名称
            TipsLabel.text = step.StepName;
            //Debug.Log(TipsLabel.GetComponent<RectTransform>().sizeDelta.y);
            GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, TipsLabel.preferredHeight + 25);

            if (step.isCorrect)
            {
                ScoreText.text = "+" + step.Score;
            }
            else
            {
                ScoreText.text = 0.ToString();//"-" + step.Score;
            }
        }
    }
}