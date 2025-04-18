using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI.Extensions;
using UnityEngine.EventSystems;
namespace MouseSelectObjs
{
    /// <summary>
    /// 鼠标移动脚本，点击鼠标左键之后拖动鼠标会有个区域，用于选中此区域内能够选中的物体
    /// </summary>
    public class MouseMoveController : BaseController
    {
        public MouseMoveController(GameFacade gameFacade) : base(gameFacade)
        {
            Start();
        }
        /// <summary>
        /// 鼠标按下时的对应屏幕坐标
        /// </summary>
        private Vector3 mouseDownScreenPos;
        /// <summary>
        /// 鼠标抬起时对应屏幕坐标
        /// </summary>
        private Vector3 mouseUpScreenPos;
        /// <summary>
        /// UI画线组件
        /// </summary>
        private UILineRenderer lineRenderer;
        /// <summary>
        /// 场景中可以被选中的物体
        /// </summary>
        private List<BaseCanSelectObj> baseCanSelectObj = new List<BaseCanSelectObj>();
        // Start is called before the first frame update
        void Start()
        {
            GameObject go = GameObject.Find("UILineRendererCanvas");
            if (go == null)
            {
                go = GameObject.Instantiate(Resources.Load<GameObject>("Prefab/UILineRendererCanvas/UILineRendererCanvas"));
            }
            lineRenderer = go.GetComponentInChildren<UILineRenderer>();
            baseCanSelectObj.AddRange(GameObject.FindObjectsOfType<BaseCanSelectObj>());
        }
        public override void OnInit()
        {
            
        }
        public override void OnUpdate()
        {
            Update();
        }
        public override void OnDestory()
        {
            
        }
        // Update is called once per frame
        void Update()
        {

            if (EventSystem.current.currentSelectedGameObject)
            {
                return;
            }
            if (Input.GetMouseButtonDown(0))//按下鼠标左键
            {
                mouseDownScreenPos = Input.mousePosition;//当鼠标按下时，记录下鼠标按下的屏幕坐标
                //保证鼠标开始位置在屏幕内
                GetPosInScreen(ref mouseDownScreenPos);

            }
            if (Input.GetMouseButton(0))//鼠标左键一直按下
            {
                mouseUpScreenPos = Input.mousePosition;//每一帧得到鼠标位置
                GetPosInScreen(ref mouseUpScreenPos);
                Vector2[] mousePoints = new Vector2[] { mouseDownScreenPos,
                new Vector2(mouseUpScreenPos.x,mouseDownScreenPos.y),mouseUpScreenPos,
                new Vector2(mouseDownScreenPos.x,mouseUpScreenPos.y),mouseDownScreenPos
                };
                SetPoints(mousePoints);
                CanSelectObjSelectingHandle();

            }
            if (Input.GetMouseButtonUp(0))
            {
                CanSelectObjSelectedHandle();
                ClearPoints();
            }
        }
        /// <summary>
        /// 设置 lineRenderer的点来绘制鼠标拖动的矩形
        /// </summary>
        /// <param name="points"></param>
        private void SetPoints(Vector2[] points)
        {
            for (int i = 0; i < points.Length; i++)
            {
                int index = i;
                lineRenderer.Points.SetValue(points[index], index);
                lineRenderer.SetAllDirty();
            }
            lineRenderer.SetAllDirty();
        }
        /// <summary>
        /// 正在选中的处理
        /// </summary>
        private void CanSelectObjSelectingHandle()
        {
            float minX = Mathf.Min(mouseDownScreenPos.x, mouseUpScreenPos.x);
            float maxX = Mathf.Max(mouseDownScreenPos.x, mouseUpScreenPos.x);
            float minY = Mathf.Min(mouseDownScreenPos.y, mouseUpScreenPos.y);
            float maxY = Mathf.Max(mouseDownScreenPos.y, mouseUpScreenPos.y);
            foreach (BaseCanSelectObj item in baseCanSelectObj)
            {
                item.JudgeIsSelected(minX, maxX, minY, maxY, item.SelectingHandle);
            }
        }
        /// <summary>
        /// 已经选中的处理
        /// </summary>
        private void CanSelectObjSelectedHandle()
        {
            float minX = Mathf.Min(mouseDownScreenPos.x, mouseUpScreenPos.x);
            float maxX = Mathf.Max(mouseDownScreenPos.x, mouseUpScreenPos.x);
            float minY = Mathf.Min(mouseDownScreenPos.y, mouseUpScreenPos.y);
            float maxY = Mathf.Max(mouseDownScreenPos.y, mouseUpScreenPos.y);
            foreach (BaseCanSelectObj item in baseCanSelectObj)
            {
                item.JudgeIsSelected(minX, maxX, minY, maxY, item.SelectedHandle);
            }
        }
        /// <summary>
        /// 将一个点限定为一定在屏幕内
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        private void GetPosInScreen(ref Vector3 pos)
        {
            float realX;
            float realY;
            realX = pos.x < 0 ? 0 : (pos.x > Screen.width ? Screen.width : pos.x);
            realY = pos.y < 0 ? 0 : (pos.y > Screen.height ? Screen.height : pos.y);
            pos = new Vector3(realX, realY, pos.z);
        }
        /// <summary>
        /// 清楚lineRenderer的所有点
        /// </summary>
        private void ClearPoints()
        {
            SetPoints(new Vector2[5]);
        }
    }
}
