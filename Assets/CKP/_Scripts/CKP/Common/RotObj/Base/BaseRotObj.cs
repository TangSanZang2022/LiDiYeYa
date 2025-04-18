using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.Events;

namespace BaseRotObjTool
{
    /// <summary>
    /// 旋转物体
    /// </summary>
    public class BaseRotObj : MonoBehaviour
    {
        [SerializeField]
        /// <summary>
        /// 暂停旋转
        /// </summary>
        private bool pauseRot;
        /// <summary>
        /// 旋转点配置
        /// </summary>
        [SerializeField]
        private List<RotPointConfig> rotPointConfigs = new List<RotPointConfig>();
        /// <summary>
        /// 是否循环播放
        /// </summary>
        [SerializeField]
        private bool isLoop;
        /// <summary>
        /// 当前正在运行的Tweener
        /// </summary>
        private Tweener tweener;
        /// <summary>
        /// 当前的点序号
        /// </summary>
        [SerializeField]
        private int currentIndex;
        /// <summary>
        /// 原始的旋转
        /// </summary>
        [SerializeField]
        private Vector3 originallocalEulerAngles;
        /// <summary>
        /// 旋转一圈的时间
        /// </summary>
        private float rotSpeed;
        [SerializeField]
        /// <summary>
        /// 旋转方向
        /// </summary>
        private int rotDir=1;
        // Start is called before the first frame update
        void Start()
        {
            StartChangeRot();
        }
        /// <summary>
        /// 开始旋转
        /// </summary>
        public virtual void StartChangeRot()
        {
            if (tweener != null)
            {
                tweener.Kill();
            }
            if (currentIndex==0)
            {
                transform.localEulerAngles = originallocalEulerAngles;
            }
            if (currentIndex<= rotPointConfigs.Count-1)//还有点
            {
                RotPointConfig rotPointConfig = rotPointConfigs[currentIndex];
                // tweener = transform.DOLocalRotate(rotPointConfig.pointLocalRotation, rotPointConfig.rotChangeTime).OnComplete(() =>
                tweener = transform.DOLocalRotateQuaternion(Quaternion.Euler( rotPointConfig.pointLocalRotation* rotDir), rotPointConfig.rotChangeTime).OnComplete(() =>
                {
                    On_tweener_Complete_Everytime();
                    if (rotPointConfig.action!=null)
                    {
                        rotPointConfig.action();
                    }
                    currentIndex++;
                    StartChangeRot();
                }
               );
                tweener.SetEase(rotPointConfig.ease);
            }
            else//到了随后一个点
            {
                currentIndex = 0;
                if (isLoop)//循环
                {
                    StartChangeRot();
                }
            }
            
        }
       
        /// <summary>
        /// 每完成一次旋转之后的回调
        /// </summary>
        public virtual void On_tweener_Complete_Everytime()
        {

        }
        /// <summary>
        /// 恢复为初始状态时回调
        /// </summary>
        public virtual void On_SetTo_originallocalEulerAngles()
        {

        }
        /// <summary>
        /// 设置是否暂停
        /// </summary>
        /// <param name="isPause"></param>
        public void Set_pauseRot(bool isPause)
        {
            pauseRot = isPause;
            if (tweener==null&&isPause)
            {
                return;
            }
            else if (tweener == null)
            {
                StartChangeRot();
            }
            else if (isPause)
            {
                tweener.Pause();
            }
            else
            {
                tweener.Play();
            }
           
           
        }
        /// <summary>
        /// 设置为初始旋转值
        /// </summary>
        /// <param name="time"></param>
        public void SetTo_originallocalEulerAngles(float time = 0,Action action=null)
        {
            if (tweener != null)
            {
                tweener.Kill();
            }
            currentIndex = 0;
            if (currentIndex == 0)
            {
                RotPointConfig rotPointConfig = rotPointConfigs[currentIndex];
                // tweener = transform.DOLocalRotate(rotPointConfig.pointLocalRotation, rotPointConfig.rotChangeTime).OnComplete(() =>
                tweener = transform.DOLocalRotateQuaternion(Quaternion.Euler(rotPointConfig.pointLocalRotation), time).OnComplete(() =>
                {
                    On_SetTo_originallocalEulerAngles();
                   tweener = null;
                    if (action!=null)
                    {
                        action();
                    }
                    if (!pauseRot)
                    {
                        StartChangeRot();
                    }
                }
               );
                tweener.SetEase(rotPointConfig.ease);
            }
        }

        /// <summary>
        /// 设置旋转时间
        /// </summary>
        /// <param name="speed"></param>
        public void Set_RotSpeed(float speed)
        {
            rotSpeed = speed;
            for (int i = 0; i < rotPointConfigs.Count; i++)
            {
                int index = i;
                if (index!=0)
                {
                    rotPointConfigs[index].rotChangeTime = speed / (rotPointConfigs.Count-1);
                }
                
            }
        }
        /// <summary>
        /// 设置旋转时间
        /// </summary>
        /// <param name="speed"></param>
        public void Set_RotDir(int dir)
        {
            if (dir==1|| dir==-1)
            {
                rotDir = dir; 
            }
        }

    }
    /// <summary>
    /// 旋转点的配置
    /// </summary>
    [Serializable]
    public class RotPointConfig
    {
        /// <summary>
        /// 旋转动画时长
        /// </summary>
        public float rotChangeTime;
        /// <summary>
        /// doTween的方式，线性匀速还是其他方式
        /// </summary>
        public Ease ease;
        /// <summary>
        /// 这个点的角度开始的角度
        /// </summary>
        public Vector3 pointLocalRotation;
        /// <summary>
        /// 到达之后的事件
        /// </summary>
        public UnityAction action;
    }
}
