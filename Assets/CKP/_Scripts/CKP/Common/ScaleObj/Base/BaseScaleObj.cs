using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace BaseScaleObjTool
{
    /// <summary>
    /// 缩放的物体
    /// </summary>
    public class BaseScaleObj : MonoBehaviour
    {
        /// <summary>
        /// 正向缩放动画时常
        /// </summary>
        [SerializeField]
        private float scaleChangeTime_Forward;

        /// <summary>
        /// 反向缩放动画时常
        /// </summary>
        [SerializeField]
        private float scaleChangeTime_Reverse;
        /// <summary>
        /// 正向doTween的方式，线性匀速还是其他方式
        /// </summary>
        [SerializeField]
        private Ease ease_Forward;

        /// <summary>
        /// 反向doTween的方式，线性匀速还是其他方式
        /// </summary>
        [SerializeField]
        private Ease ease_Reverse;
        /// <summary>
        /// 是否循环播放
        /// </summary>
        [SerializeField]
        private bool isLoop;
        /// <summary>
        /// 开始的缩放大小
        /// </summary>
        [SerializeField]
        private Vector3 startScale;

        /// <summary>
        /// 结束时的缩放大小
        /// </summary>
        [SerializeField]
        private Vector3 endScale;
        /// <summary>
        /// 当前正在运行的Tweener
        /// </summary>
        private Tweener tweener;
        // Start is called before the first frame update
        void Start()
        {

        }
        /// <summary>
        /// 开始正向改变缩放
        /// </summary>
        public virtual void StartChangeScale_Forward()
        {
            if (tweener != null)
            {
                tweener.Kill();
            }
            transform.localScale = startScale;
            tweener = transform.DOScale(endScale, scaleChangeTime_Forward).OnComplete(() =>
            {
                On_tweener_Complete_Forward();
                if (isLoop)
                {
                    StartChangeScale_Reverse();
                };
            }
                );
            tweener.SetEase(ease_Forward);
        }
        /// <summary>
        /// 开始反向播放动画
        /// </summary>
        public virtual void StartChangeScale_Reverse()
        {
            if (tweener != null)
            {
                tweener.Kill();
            }
            transform.localScale = endScale;
            tweener = transform.DOScale(startScale, scaleChangeTime_Reverse).OnComplete(() =>
            {
                On_tweener_Complete_Reverse();
                if (isLoop)
                {
                    StartChangeScale_Reverse();
                };
            }
                );
            tweener.SetEase(ease_Reverse);
        }
        /// <summary>
        /// 每完成一次正向缩放之后的回调
        /// </summary>
        public virtual void On_tweener_Complete_Forward()
        {

        }

        /// <summary>
        /// 每完成一次反向缩放之后的回调
        /// </summary>
        public virtual void On_tweener_Complete_Reverse()
        {

        }
    }
}
