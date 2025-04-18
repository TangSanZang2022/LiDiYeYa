using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LiDi.CKP
{
    /// <summary>
    /// 设备屏幕
    /// </summary>
    public class DeviceScreen : MonoBehaviour
    {
        public string ID;

        public string deviceScreenName;

        public string description;
        /// <summary>
        /// 开始时的状态
        /// </summary>
        public bool startState;
        /// <summary>
        /// 目标值
        /// </summary>
        public string startValue;

        /// <summary>
        /// 目标值
        /// </summary>
        public string targetValue;
        // Start is called before the first frame update
        protected virtual void Start()
        {
            if (GetComponent<ShowHideObj.BaseShowHideObj>()!=null)
            {
                return;
            }
            gameObject.SetActive(startState);
        }

        
        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void InitDeviceScreen()
        { }
        /// <summary>
        /// 向目标值移动
        /// </summary>
        public virtual void ChangeToTargetValue()
        {
            Debug.Log("设备屏幕改变到目标值：" + targetValue);
        }
        /// <summary>
        /// 暂停
        /// </summary>
        public virtual void Pause()
        { }
        /// <summary>
        /// 返回到开始值
        /// </summary>
        public virtual void ResetToStartValue()
        {
            Debug.Log("设备屏幕改变到初始：" + startValue);
        }

        protected virtual void OnDisable()
        {
            
        }
    }
}