using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LiDi.CKP
{
    public class OperationErrorTip : MonoBehaviour
    {
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
        private void OnEnable()
        {
            StartCoroutine(IShowErrorTip());
        }
        IEnumerator IShowErrorTip()
        {

            int time = 3;

            CountDownText.text = time.ToString() + "秒后请继续答题";
            yield return new WaitForSeconds(1);
            time--;
            CountDownText.text = time.ToString() + "秒后请继续答题";
            yield return new WaitForSeconds(1);
            time--;
            CountDownText.text = time.ToString() + "秒后请继续答题";
            yield return new WaitForSeconds(1);
            gameObject.SetActive(false);


        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }
    }
}