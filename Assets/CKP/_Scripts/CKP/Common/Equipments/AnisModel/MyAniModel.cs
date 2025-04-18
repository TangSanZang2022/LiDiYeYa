using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DFDJ
{
    /// <summary>
    /// 设备动画模型
    /// </summary>
    public class MyAniModel : MonoBehaviour
    {
        [SerializeField]
        /// <summary>
        /// ID
        /// </summary>
        private string id;

        /// <summary>
        ///子物体模型动画
        /// </summary>
        protected List<Animator> childModelAnis = new List<Animator>();
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            if (string.IsNullOrEmpty(id))
            {
                id = name;
            }
            childModelAnis.AddRange(GetComponentsInChildren<Animator>());

        }
        /// <summary>
        /// 获取ID
        /// </summary>
        /// <returns></returns>
        public string GetID()
        {
            return id;
        }
        /// <summary>
        /// 是否播放动画
        /// </summary>
        /// <param name="play"></param>
        public void Play(bool play)
        {
            for (int i = 0; i < childModelAnis.Count; i++)
            {
                if (play)
                {
                    childModelAnis[i].ResetTrigger("Stop");
                    childModelAnis[i].SetTrigger("Play");
                }
                else
                {
                    childModelAnis[i].ResetTrigger("Play");
                    childModelAnis[i].SetTrigger("Stop");
                }
            }
        }
    }
}
