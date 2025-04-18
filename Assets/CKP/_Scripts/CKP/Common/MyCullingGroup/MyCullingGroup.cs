using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace DFDJ
{
    /// <summary>
    /// 根据距离来判断的物体可见性
    /// </summary>
    public class MyCullingGroup : MonoBehaviour
    {
        /// <summary>
        /// 在可见范围内看到物体
        /// </summary>
        public UnityAction  SeeObjOnDistance;
        /// <summary>
        /// 从看见到不看见
        /// </summary>
        public UnityAction LoseSightObjOnDistance;
        /// <summary>
        /// 目标相机
        /// </summary>
        public Camera targetCamera;
        /// <summary>
        /// 相机距离多远时判定为可见
        /// </summary>
        public float distance = 10f;
        /// <summary>
        /// 包围球的个数
        /// </summary>
        public int boundingSphereNum = 1;
        /// <summary>
        /// 包围球半径
        /// </summary>
        public float radius = 1f;
        CullingGroup cullingGroup;
        BoundingSphere[] boundingSpheres ;

        public bool isCanSee;
        private void OnEnable()
        {
            if (boundingSpheres==null)
            {
                boundingSpheres = new BoundingSphere[boundingSphereNum];
                cullingGroup = new CullingGroup();
                for (int i = 0; i < boundingSpheres.Length; i++)
                {
                    boundingSpheres[i] = new BoundingSphere(transform.position, radius);
                }
                cullingGroup.SetBoundingSpheres(boundingSpheres);
                cullingGroup.SetBoundingSphereCount(boundingSpheres.Length);
                cullingGroup.SetBoundingDistances(new float[] { distance });
                cullingGroup.targetCamera = targetCamera;
                cullingGroup.SetDistanceReferencePoint(targetCamera.transform);
                cullingGroup.onStateChanged = StateChangeFunc;
            }
        }
        // Start is called before the first frame update
        void Start()
        {
           
        }

        private void StateChangeFunc(CullingGroupEvent sphere)
        {
            int[] resultIndices = new int[boundingSphereNum];
            int num = 0;
            num = cullingGroup.QueryIndices(true, resultIndices, 0);

            if (num>0)//看见
            {
                 isCanSee = true;
                if (SeeObjOnDistance!=null)
                {
                    SeeObjOnDistance(); 
                }
            }
            else
            {
                isCanSee = false;
                if (SeeObjOnDistance != null)
                    LoseSightObjOnDistance();
            }
        }

        
       
        private void OnDestroy()
        {
            if (cullingGroup!=null)
            {
                cullingGroup.Dispose(); 
            }
        }
    }
}
