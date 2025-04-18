using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace MoveObjTool
{
    /// <summary>
    /// 移动物体基类
    /// </summary>
    public class BaseMoveObj : MonoBehaviour
    {
        /// <summary>
        /// 当前目标点
        /// </summary>
        [SerializeField]
        protected int currentPointIndex;
        /// <summary>
        /// 当前所在点
        /// </summary>
        protected BaseRoadPoint currentBaseRoadPoint;
        /// <summary>
        /// 移动速度
        /// </summary>
        [SerializeField]
        protected float moveSpeed;

        /// <summary>
        /// 移动的路径点
        /// </summary>
        [SerializeField]
        protected List<BaseRoadPoint> roadPointList = new List<BaseRoadPoint>();
        /// <summary>
        /// 是否循环
        /// </summary>
        [SerializeField]
        protected bool isLoop;
        public Tweener tweener;

        // Start is called before the first frame update
        void Start()
        {
            //OnInit();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void OnInit()
        {
            currentPointIndex = 0;
            currentBaseRoadPoint = null;
            if (tweener!=null)
            {
                tweener.Kill();
            }
            tweener = null;
            //MoveToNextPoint();
        }
       
        /// <summary>
        /// 移动到目标点
        /// </summary>
        /// <param name="index"></param>
        public virtual void MoveToNextPoint()
        {
            if (roadPointList.Count <= currentPointIndex && !isLoop)//到了最后一个点且不循环
            {
                currentBaseRoadPoint = null;
                currentPointIndex = 0;
                tweener = null;
                return;
            }
            if (roadPointList.Count <= currentPointIndex && isLoop)//到了最后一个点且循环
            {
                currentBaseRoadPoint = null;
                currentPointIndex = 0;
                tweener = null;
            }
            currentBaseRoadPoint = roadPointList[currentPointIndex];
            float distance = Vector3.Distance(transform.position, currentBaseRoadPoint.transform.position);
            float time = distance / moveSpeed;

           
           
            tweener = transform.DOMove(currentBaseRoadPoint.transform.position, time, false).OnComplete(() =>
            {
                //print(name + "----" + currentPointIndex);
                currentBaseRoadPoint.ArrivedPoint(this);
                if (currentBaseRoadPoint.IsMoveNext())
                {
                    currentBaseRoadPoint.LeavedPoint();
                    currentPointIndex++;
                    MoveToNextPoint();
                }
               

            }
            );
            tweener.SetEase(Ease.Linear);
        }
        /// <summary>
        /// 继续移动
        /// </summary>
        public virtual void ContinueMove()
        {
            currentBaseRoadPoint.LeavedPoint();
            currentPointIndex++;
            MoveToNextPoint();
        }
        /// <summary>
        /// 暂停播放移动tweener
        /// </summary>
        public void PauseMove()
        {
            if (tweener != null)
            {
                tweener.Pause();
            }

        }
        /// <summary>
        /// 开始播放移动tweener
        /// </summary>
        public void PlayMove()
        {
            if (tweener != null)
            {
                tweener.Play();
            }
            else
            {
                MoveToNextPoint();
            }
        }

        public List<BaseRoadPoint> Get_roadPointList()
        {
            return roadPointList;
        }
        private void OnDestroy()
        {
            tweener = null;
            currentBaseRoadPoint = null;
        }
    }
}
