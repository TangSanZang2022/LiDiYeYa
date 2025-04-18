using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LiDi.CKP
{
    public class BaseTool : HighlighterObj
    {
        /// <summary>
        /// 热点ID
        /// </summary>
        public string id;
        /// <summary>
        /// 工具名称，用于初始化UI的工具选项
        /// </summary>
        public string toolName;
        /// <summary>
        /// 描述
        /// </summary>
        public string description;
        /// <summary>
        /// 初始父物体
        /// </summary>
        private Transform originalParent;
        /// <summary>
        /// 初始的相对位置
        /// </summary>
        private Vector3 originalPos;
        /// <summary>
        /// 初始的相对旋转
        /// </summary>
        private Quaternion originalRot;
        /// <summary>
        /// 初始的相对缩放
        /// </summary>
        private Vector3 originalScal;
        [SerializeField]
        /// <summary>
        /// 鼠标进入时热点闪烁颜色
        /// </summary>
        protected Color[] mouseEnterColor = new Color[] { Color.red, Color.green };
        [SerializeField]
        /// <summary>
        /// 训练模式时热点提示的闪烁颜色
        /// </summary>
        protected Color[] cueColor = new Color[] { Color.yellow, Color.grey };
        /// <summary>
        /// 选中的颜色
        /// </summary>
        protected Color selcectedColor = Color.blue;
        [SerializeField]
        /// <summary>
        ///开始时显示隐藏状态
        /// </summary>
        private bool startActiveState = false;

        /// <summary>
        /// 是否会因为其他热点选中和未选中时显示隐藏
        /// </summary>
        public bool isCanHideUnSelectOtherPoint = true;
        [SerializeField]
        /// <summary>
        ///开始时可点击状态
        /// </summary>
        private bool startOperability = false;
        [SerializeField]
        /// <summary>
        /// 点击之后需要显示的物体
        /// </summary>
        protected List<GameObject> showObjAfterClick = new List<GameObject>();
        /// <summary>
        ///拿起事件
        /// </summary>
        public Action PickUpAction;
        /// <summary>
        /// 放下事件
        /// </summary>
        public Action PutDownAction;
        private Collider _collider;
        /// <summary>
        /// 热点的碰撞
        /// </summary>
        private Collider hotPointCollider
        {
            get
            {
                if (_collider == null)
                {
                    _collider = GetComponent<Collider>();
                    if (_collider == null)
                    {
                        _collider = GetComponentInChildren<Collider>();
                    }
                }
                return _collider;
            }
        }

        public bool _operability;
        /// <summary>
        /// 热点是否可操作
        /// </summary>
        public bool Operability
        {
            get
            {
                return _operability;
            }
            set
            {
                _operability = value;
                hotPointCollider.enabled = value;
                //在训练模式时，如果设置了该热点为可操作，则需要高亮提示玩家
                if (value && GameFacade.Instance.GetGameMode() == GameMode.Training && !GameFacade.Instance.GetCurrentStep().SelectableHotPointIDForTrain.Contains(id))
                {
                    HighlightOn_cueColor();
                }
                else if (GameFacade.Instance.GetGameMode() == GameMode.Training)
                {
                    h.Off();
                }

            }
        }


        private bool _isSelected;
        /// <summary>
        /// 是否已经选中
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                if (value)
                {
                    //GameFacade.Instance.Set_SelectedHotPoint(this);
                    //h.tweenGradient = new Gradient()
                    //{
                    //    colorKeys = new GradientColorKey[] { new GradientColorKey(selcectedColor, 0)},
                    //};
                    //h.TweenStart();
                    HighlightOn_selcectedColor();
                    Set_showObjAfterClick_State(true);
                    transform.parent = null;
                    hotPointCollider.enabled = false;
                    GameFacade.Instance.Set_currentSelectedTool(this);
                    if (GameFacade.Instance.GetGameMode()==GameMode.Training&&!GameFacade.Instance.GetCurrentStep().SelectableToolIDForTrain.Contains(id))//如果是训练状态,选中的工具为正确的
                    {
                        GameFacade.Instance.SetCurrentHotPointActiveStateForTraining(true);
                    }
                    else//选了错误的工具
                    {

                    }
                    if (PickUpAction!=null)
                    {
                        PickUpAction();
                    }
                    print("拿起工具");
                }
                else
                {
                    // h.TweenStop();
                    h.FlashingOff();
                    Set_showObjAfterClick_State(false);
                    ResetToOriginalState();
                    if (PutDownAction!=null)
                    {
                        PutDownAction();
                    }
                    //GameFacade.Instance.RemoveOperatedHotPointTo_operatedHotPointList(this);
                    print("放下工具");
                }
            }
        }
        protected override void Start()
        {
            base.Start();
            Init();
        }

        protected override void Update()
        {
            base.Update();
            if (IsSelected)
            {
                FollowMouse();
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            originalParent = transform.parent;
            originalPos=transform.localPosition;
            originalRot = transform.localRotation;
            originalScal = transform.localScale;
            IsSelected = false;
            Operability = startOperability;
            gameObject.SetActive(startActiveState);
        }
        /// <summary>
        /// 重置到初始状态
        /// </summary>
        public void ResetToOriginalState()
        {
            transform.SetParent(originalParent);
            transform.localPosition = originalPos;
            transform.localScale = originalScal;
            transform.localRotation = originalRot;
            hotPointCollider.enabled = true;
        }
        /// <summary>
        /// 跟随鼠标，鼠标点击之后拿起物体
        /// </summary>
        private void FollowMouse()
        {
            Vector3 toolNewPos =Camera.main.ScreenToWorldPoint( new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
            transform.position = toolNewPos;
            if (Input.GetMouseButtonDown(2))
            {
                IsSelected = false;
                GameFacade.Instance.Set_currentSelectedTool(null);
            }
        }
        /// <summary>
        /// 提示高亮
        /// </summary>
        private void HighlightOn_cueColor()
        {
            h.FlashingOff();
            //h.tweenGradient = new Gradient()
            //{
            //    colorKeys = new GradientColorKey[] { new GradientColorKey(cueColor[0], 0), new GradientColorKey(cueColor[1], 1) },
            //};
            //h.TweenStart();
            h.FlashingOn(cueColor[0], cueColor[1]);
            Debug.Log(id + "开始提示闪烁");
        }

        /// <summary>
        /// 鼠标进入高亮
        /// </summary>
        private void HighlightOn_mouseEnterColor()
        {
            h.FlashingOff();

            //h.tweenGradient = new Gradient()
            //{
            //    colorKeys = new GradientColorKey[] { new GradientColorKey(mouseEnterColor[0], 0), new GradientColorKey(mouseEnterColor[1], 1) },
            //};
            //h.TweenStart();
            Debug.Log(111);
            h.FlashingOn(mouseEnterColor[0], mouseEnterColor[1],1);
            //h.On(mouseEnterColor[0]);
        }

        /// <summary>
        ///选中高亮
        /// </summary>
        private void HighlightOn_selcectedColor()
        {
            h.FlashingOff();
            //h.tweenGradient = new Gradient()
            //{
            //    colorKeys = new GradientColorKey[] { new GradientColorKey(selcectedColor, 0) },
            //};
            //h.TweenStart();
            h.FlashingOn(selcectedColor[0]);
        }

        /// <summary>
        /// 设置点击之后显示隐藏物体状态
        /// </summary>
        /// <param name="state"></param>
        protected void Set_showObjAfterClick_State(bool state)
        {
            for (int i = 0; i < showObjAfterClick.Count; i++)
            {
                int index = i;
                //showObjAfterClick[index].SetActive(state);

                BaseHotPoint hotPoint = showObjAfterClick[index].GetComponent<BaseHotPoint>();
                if (hotPoint != null)
                {
                    if (hotPoint.isCanHideUnSelectOtherPoint)//如果该热点一开始是显示状态，则不需要隐藏该热点
                    {
                        showObjAfterClick[index].SetActive(state);
                    }
                    else if (!state)
                    {
                        showObjAfterClick[index].SetActive(showObjAfterClick[index].activeSelf);
                    }
                    hotPoint.Operability = state;
                }
                else
                {
                    showObjAfterClick[index].SetActive(state);
                }
            }
        }

        protected virtual void OnMouseEnter()
        {
            if (IsSelected || !Operability)
            {
                return;
            }
            //h.TweenStart();
            //h.tweenGradient = new Gradient()
            //{
            //    colorKeys =new GradientColorKey[] { new GradientColorKey(mouseEnterColor[0], 0), new GradientColorKey(mouseEnterColor[1], 1) },
            //};
            HighlightOn_mouseEnterColor();
        }

        protected virtual void OnMouseDown()
        {
            IsSelected = !IsSelected;
        }

        protected virtual void OnMouseExit()
        {
            if (IsSelected || !Operability)
            {
                return;
            }
            h.FlashingOff();

            if (GameFacade.Instance.GetGameMode() == GameMode.Training && hotPointCollider.enabled)
            {
                //print(222);
                HighlightOn_cueColor();
                if (GameFacade.Instance.GetCurrentStep().SelectableToolIDForTrain.Contains(id))
                {
                   h.Off();
                }
            }

        }
        /// <summary>
        /// 设置热点为初始状态,用于退出时调用
        /// </summary>
        public virtual void SetToStartState_ForQuite()
        {
            _isSelected = false;
            h.Off();
            Operability = startOperability;
            Set_showObjAfterClick_State(false);
            gameObject.SetActive(startActiveState);
        }

        /// <summary>
        /// 使用之后放下工具
        /// </summary>
        public void PutDownAfterUse()
        {
            IsSelected = false;
            Operability = false;//不可操作了
        }
        /// <summary>
        /// 重置热点，用于进入下一步时，之前操作过的热点重置
        /// </summary>
        public virtual void ResetBeforeNextStep()
        {
            _isSelected = false;
            h.Off();
            Debug.Log(id + "停止闪烁");
        }
    }
}