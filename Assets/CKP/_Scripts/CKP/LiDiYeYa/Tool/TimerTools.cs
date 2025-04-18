using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Tools.CKP
{
    /// <summary>
    /// 计时器工具
    /// </summary>
    public class TimerTools : MonoSingleton<TimerTools>
    {
       
        // Start is called before the first frame update
        void Start()
        {

        }

        public void StopAllIe()
        {
            StopAllCoroutines();
        }
        /// <summary>
        /// 开始等待执行
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="action"></param>
        public void StartToWaitDoSomething(float waitTime, Action action)
        {
            StartCoroutine(IStartToWaitDoSomething(waitTime, action));
        }
        /// <summary>
        /// 开始等待执行
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="action"></param>
        public void StartToWaitDoSomething(Func<bool> condition, Action action)
        {
            StartCoroutine(IStartToWaitDoSomething(condition, action));
        }

        IEnumerator IStartToWaitDoSomething(float waitTime, Action action)
        {
            yield return new WaitForSeconds(waitTime);
            action();

        }

        IEnumerator IStartToWaitDoSomething(Func<bool> condition, Action action)
        {
            yield return new WaitUntil(()=> condition());
            action();

        }

    }
}