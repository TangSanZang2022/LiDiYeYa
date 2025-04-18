using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace LiDi.CKP
{
    /// <summary>
    /// 选择题面板
    /// </summary>
    public class Topic : MonoBehaviour
    {

        protected TopicData topicData;

        private Text titleText;
        /// <summary>
        /// 题目
        /// </summary>
        protected Text TitleText
        {
            get
            {
                if (titleText == null)
                {
                    titleText = transform.FindChildForName<Text>("TitleText");
                }
                return titleText;
            }
        }
        private Text countDownText;
        /// <summary>
        /// 倒计时
        /// </summary>
        protected Text CountDownText
        {
            get
            {
                if (countDownText == null)
                {
                    countDownText = transform.FindChildForName<Text>("CountDownText");
                }
                return countDownText;
            }
        }
        private Transform _answerContent;
        /// <summary>
        /// 选项父物体
        /// </summary>
        protected Transform AnswerContent
        {
            get
            {
                if (_answerContent == null)
                {
                    _answerContent = transform.FindChildForName("AnswerContent");
                }
                return _answerContent;
            }
        }

        private List<Toggle> toggles;
        /// <summary>
        /// 所有选项
        /// </summary>
        protected List<Toggle> Toggles
        {
            get
            {
                if (toggles == null)
                {
                    toggles = new List<Toggle>();
                }
                if (toggles.Count == 0)
                {
                    toggles.AddRange(AnswerContent.GetComponentsInChildren<Toggle>());
                }
                return toggles;
            }
        }

        private Button ackButton;
        /// <summary>
        /// 确认选项
        /// </summary>
        protected Button AckButton
        {
            get
            {
                if (ackButton == null)
                {
                    ackButton = transform.FindChildForName<Button>("AckButton");
                }
                return ackButton;
            }
        }

        private GameObject errorTip;
        /// <summary>
        /// 错误提示
        /// </summary>
        protected GameObject ErrorTip
        {
            get
            {
                if (errorTip == null)
                {
                    errorTip = transform.FindChildForName("ErrorTip").gameObject;
                }
                return errorTip;
            }
        }

        protected virtual void OnEnable()
        {
            Set_topicData(GameFacade.Instance.GetTopicDataForCurrentStep());
        }
        // Start is called before the first frame update
        protected virtual void Start()
        {
            AckButton.onClick.AddListener(delegate { On_AckButton_Click(); });

            // Set_topicData( GameFacade.Instance.GetTopicDataForID("1"));
        }



        /// <summary>
        /// 重置所有选项
        /// </summary>
        protected virtual void ResetAllToggles()
        {
            for (int i = 0; i < Toggles.Count; i++)
            {
                int index = i;
                Toggles[index].isOn = false;
            }
        }
        /// <summary>
        /// 设置选项是否为多选
        /// </summary>
        /// <param name="multipleChoice"></param>
        protected virtual void SetToggleMultipleChoice(string multipleChoice)
        {
            if (multipleChoice.ToLower() == "true")//多选
            {
                for (int i = 0; i < Toggles.Count; i++)
                {
                    int index = i;
                    Toggles[index].group = AnswerContent.GetComponent<ToggleGroup>();
                }
            }
            else
            {
                for (int i = 0; i < Toggles.Count; i++)
                {
                    int index = i;
                    Toggles[index].group = null;
                }
            }
        }
        /// <summary>
        /// 设置题目
        /// </summary>
        /// <param name="topicData_New"></param>
        public virtual void Set_topicData(TopicData topicData_New)
        {
            //将自己设置到最前端
            transform.SetSiblingIndex(transform.parent.childCount - 1);
            ErrorTip.SetActive(false);
            ResetAllToggles();
            topicData = topicData_New;
            TitleText.text = topicData.Title;
            List<string> options = new List<string>(topicData.Options);
            int optionsNum = topicData.Options.Count;
            for (int i = optionsNum; i < Toggles.Count; i++)
            {
                int index = i;
                Toggles[index].gameObject.SetActive(false);
            }

            for (int i = 0; i < optionsNum; i++)
            {
                int index = i;
                //随机一个选项
                int rangeIndex = UnityEngine.Random.Range(0, options.Count);
                Debug.Log("index:" + index + "---rangeIndex:" + rangeIndex);
                Toggles[index].gameObject.SetActive(true);
                Toggles[index].transform.FindChildForName<Text>("Label").text = options[rangeIndex];
                options.RemoveAt(rangeIndex);
            }
        }
        /// <summary>
        /// 确认按钮点击
        /// </summary>
        protected virtual void On_AckButton_Click()
        {
            List<string> answers = new List<string>();

            for (int i = 0; i < Toggles.Count; i++)
            {
                int index = i;
                if (Toggles[index].isOn)
                {
                    answers.Add(Toggles[index].transform.FindChildForName<Text>("Label").text);
                }
            }
            if (answers.Count == 0)
            {
                return;
            }
            if (answers.Count != topicData.RightAnswers.Count)
            {
                Debug.Log("选择错误");

                StartCoroutine(IShowErrorTip());



            }
            else
            {
                for (int i = 0; i < answers.Count; i++)
                {
                    int index = i;
                    if (!topicData.RightAnswers.Contains(answers[index]))
                    {
                        Debug.Log("选择错误");
                        StartCoroutine(IShowErrorTip());
                        break;
                    }
                    else if (topicData.RightAnswers.Contains(answers[index])&& index == answers.Count - 1)
                    {
                        Debug.Log("选择正确");
                        GameFacade.Instance.AddOperatedHotPointTo_operatedHotPointList(new BaseHotPoint() { id = "TopicRight" });
                        gameObject.SetActive(false);
                        break;
                    }

                    else if (!topicData.RightAnswers.Contains(answers[index]) && index == answers.Count - 1)
                    {
                        Debug.Log("选择错误");
                        StartCoroutine(IShowErrorTip());
                        break;
                    }
                }
            }

        }

        IEnumerator IShowErrorTip()
        {
            if (GameFacade.Instance.GetGameMode() == GameMode.Appraisal)//如果是考核模式，则不需要弹出提示错误框
            {
                GameFacade.Instance.AddOperatedHotPointTo_operatedHotPointList(new BaseHotPoint() { id = "TopicError" });
                gameObject.SetActive(false);
            }
            else
            {
                int time = 3;
                ErrorTip.SetActive(true);
                CountDownText.text = time.ToString() + "秒后请继续答题";
                 yield return new WaitForSeconds(1);
                time--;
                CountDownText.text = time.ToString() + "秒后请继续答题";
                yield return new WaitForSeconds(1);
                time--;
                CountDownText.text = time.ToString() + "秒后请继续答题";
                yield return new WaitForSeconds(1);
                //CountDownText.text = time.ToString() + "秒后请继续答题";
                ErrorTip.SetActive(false);
                ResetAllToggles();
            }

        }


    }
    /// <summary>
    /// 题目数据
    /// </summary>
    [Serializable]
    public class TopicData
    {
        /// <summary>
        /// 题目ID
        /// </summary>
        public string ID;
        /// <summary>
        /// 题目
        /// </summary>
        public string Title;
        /// <summary>
        /// 是否是多选，"true"为多选，"false"或者空，为单选
        /// </summary>
        public string MultipleChoice;
        /// <summary>
        /// 正确答案
        /// </summary>
        public List<string> RightAnswers;
        /// <summary>
        /// 选项
        /// </summary>
        public List<string> Options;
    }
}