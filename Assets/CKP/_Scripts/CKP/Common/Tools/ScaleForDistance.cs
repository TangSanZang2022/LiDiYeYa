using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DFDJ;
namespace Tool
{
    /// <summary>
    /// 根据距离缩放
    /// </summary>
    public class ScaleForDistance : MonoBehaviour
    {
        [SerializeField]
        /// <summary>
        /// 最大距离
        /// </summary>
        private float maxDistance;
        [SerializeField]
        /// <summary>
        /// 最小距离
        /// </summary>
        private float minDistance;
        [SerializeField]
        /// <summary>
        /// 原始缩放，最大缩放
        /// </summary>
        private Vector3 originalScale=Vector3.one;
        [SerializeField]
        /// <summary>
        /// 目标物体
        /// </summary>
        private Transform targetObj;
        
        [SerializeField]
        /// <summary>
        /// 距离小于等于此值时隐藏
        /// </summary>
        private float hideDistance=-1;
        // Start is called before the first frame update
        void Start()
        {
            //targetObj = GetComponent<LookAtCamera>().Get_targetCam();
        }

        // Update is called once per frame
        void Update()
        {
            if (targetObj == null)
            {
                transform.DOScale(originalScale, 1);
                //targetObj = GetComponent<BaseWorldUIIcon>().targetCam.transform;
                return;
            }
            float disFromTargetObj = Vector3.Distance(transform.position, targetObj.position);
            if (disFromTargetObj > minDistance && disFromTargetObj < maxDistance)
            {
                //和最大距离的系数
                float coefficient = disFromTargetObj / maxDistance;
                transform.DOScale(new Vector3(originalScale.x * coefficient, originalScale.y * coefficient, originalScale.z), 1);
            }
            else if (disFromTargetObj >= maxDistance)
            {
                transform.DOScale(originalScale, 1);
            }
            else if (disFromTargetObj<=minDistance)
            {
                float coefficient = minDistance / maxDistance;
                transform.DOScale(new Vector3(originalScale.x * coefficient, originalScale.y * coefficient, originalScale.z), 1);
            }
            if (Vector3.Distance(transform.position, targetObj.position)<= hideDistance)
            {
                gameObject.SetActive(false);
            }

        }

        //private IEnumerator IHideTime
    }
}