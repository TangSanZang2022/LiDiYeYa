using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LiDi.CKP
{
    /// <summary>
    /// 氢枫动画
    /// </summary>
    public class HydrexiaAni : MonoBehaviour
    {

        private Animator _ani;

        private Animator ani
        {
            get
            {
                if (_ani == null)
                {
                    _ani = GetComponent<Animator>();
                }
                return _ani;
            }
        }

        private void OnEnable()
        {
            GameFacade.Instance.Add_allAnimators(this);
        }
        // Start is called before the first frame update
        void Start()
        {
            GameFacade.Instance.Add_allAnimators(this);
        }

        public void PlayAni(string aniName)
        {
           
            // 检查是否正在播放指定的动画
            //if (ani.GetCurrentAnimatorClipInfoCount(0) > 0)
            //{
            //    foreach (var clipInfo in ani.GetCurrentAnimatorClipInfo(0))
            //    {
            //        if (clipInfo.clip.name == aniName)
            //        {
            //            // 正在播放指定动画
            //            ani.Play(aniName, 0, 0);
            //        }
            //    }
            //}

            int stateid = Animator.StringToHash(aniName);
            bool hasAction = ani.HasState(0, stateid);
            if (hasAction)
            {
                //单一动作重复调用时需要使用Play方法而且需把所有参数填写完整
                ani.Play(aniName, 0, 0);
            }
          
        }
    }
}