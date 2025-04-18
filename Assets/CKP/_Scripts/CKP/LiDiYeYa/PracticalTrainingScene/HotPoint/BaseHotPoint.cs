using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LiDi.CKP
{
    /// <summary>
    /// 热点基类
    /// </summary>
    public class BaseHotPoint : HighlighterObj
    {
        /// <summary>
        /// 热点ID
        /// </summary>
        public string id;
        /// <summary>
        /// 描述
        /// </summary>
        public string description;
        [SerializeField]
        /// <summary>
        /// 点击之后需要显示的物体
        /// </summary>
        protected List<GameObject> showObjAfterClick = new List<GameObject>();
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

        private string _selectedToolID;
        /// <summary>
        /// 操作此热点时所选择的工具
        /// </summary>
        public string SelectedToolID
        {
            get
            {
                return _selectedToolID;
            }
            set {
                print("aaa:" + value+"bbb");
                _selectedToolID = value;
            }

        }
        private List<BaseHotPointChild> _baseHotPointChildrenList ;
        /// <summary>
        /// 热点操作之后子物体
        /// </summary>
        [SerializeField]
        protected List<BaseHotPointChild> baseHotPointChildrenList
        {
            get
            {
                if (_baseHotPointChildrenList == null)
                {
                    _baseHotPointChildrenList = new List<BaseHotPointChild>();
                    _baseHotPointChildrenList.AddRange(GetComponentsInChildren<BaseHotPointChild>());
                }
                return _baseHotPointChildrenList;


            }
        }

        private bool _isClicked;
        /// <summary>
        /// 是否已经点击
        /// </summary>
        public bool isClicked
        {
            get
            {
                return _isClicked;
            }
            set
            {
                _isClicked = value;
                if (value)
                {
                    ClickedHandel();
                   
                }


            }
        }

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
                    GameFacade.Instance.Set_SelectedHotPoint(this);
                    //h.tweenGradient = new Gradient()
                    //{
                    //    colorKeys = new GradientColorKey[] { new GradientColorKey(selcectedColor, 0)},
                    //};
                    //h.TweenStart();
                    HighlightOn_selcectedColor();
                    Set_showObjAfterClick_State(true);
                    Set_SelectedToolID();
                    GameFacade.Instance.AddOperatedHotPointTo_operatedHotPointList(this);
                    print("添加");
                }
                else
                {
                   // h.TweenStop();
                    h.FlashingOff();
                    Set_showObjAfterClick_State(false);
                   GameFacade.Instance.RemoveOperatedHotPointTo_operatedHotPointList(this);
                    print("移除");
                }
            }
        }

        protected override void Start()
        {
            base.Start();
            GameFacade.Instance.AddHotPointIntoDic(this);
            _selectedToolID = "";
            //训练模式时才需要设置开始可操作性
           // if (GameFacade.Instance.GetGameMode()==GameMode.Training)
            //{
                Operability = startOperability;
           // }
           
            gameObject.SetActive(startActiveState);
        }
        /// <summary>
        /// 设置热点为初始状态
        /// </summary>
        public virtual void SetToStartState()
        {
            ResetBeforeNextStep();
            Operability = startOperability;
            SelectedToolID = "";
            Set_showObjAfterClick_State(false);
            gameObject.SetActive(startActiveState);
        }
        /// <summary>
        /// 设置热点为初始状态,用于退出时调用
        /// </summary>
        public virtual void SetToStartState_ForQuite()
        {
            _isClicked = false;
            _isSelected = false;
            h.Off();
            SelectedToolID = "";
            Operability = startOperability;
            Set_showObjAfterClick_State(false);
            gameObject.SetActive(startActiveState);
        }
        /// <summary>
        /// 重置热点，用于进入下一步时，之前操作过的热点重置
        /// </summary>
        public virtual void ResetBeforeNextStep()
        {
            _isClicked = false;
            _isSelected = false;
            SelectedToolID = "";
            h.Off();
            Debug.Log(id + "停止闪烁");
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
        { }

        protected virtual void OnMouseExit()
        {
            if (IsSelected || !Operability)
            {
                return;
            }
            h.Off();

            if (GameFacade.Instance.GetGameMode() == GameMode.Training && hotPointCollider.enabled)
            {
                //print(222);
                HighlightOn_cueColor();
                if (GameFacade.Instance.GetCurrentStep().SelectableHotPointIDForTrain.Contains(id))
                {
                    h.Off();
                }
            }

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
        /// <summary>
        /// 提示高亮
        /// </summary>
        private void HighlightOn_cueColor()
        {
            h.Off();
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
            h.Off();

            //h.tweenGradient = new Gradient()
            //{
            //    colorKeys = new GradientColorKey[] { new GradientColorKey(mouseEnterColor[0], 0), new GradientColorKey(mouseEnterColor[1], 1) },
            //};
            //h.TweenStart();

            h.FlashingOn(mouseEnterColor[0], mouseEnterColor[1]);
        }

        /// <summary>
        ///选中高亮
        /// </summary>
        private void HighlightOn_selcectedColor()
        {
            h.Off();
            //h.tweenGradient = new Gradient()
            //{
            //    colorKeys = new GradientColorKey[] { new GradientColorKey(selcectedColor, 0) },
            //};
            //h.TweenStart();
            h.FlashingOn(selcectedColor[0]);
        }

        /// <summary>
        /// 设置所有子物体状态
        /// </summary>
        /// <param name="isOpen">是否打开</param>
        protected virtual void SetAllChildState(bool isOpen)
        {
            for (int i = 0; i < baseHotPointChildrenList.Count; i++)
            {
                int index = i;
                
                baseHotPointChildrenList[index].SetState(isOpen);
            }
        }
        /// <summary>
        /// 点击之后操作
        /// </summary>
        protected virtual void ClickedHandel()
        {
            Set_SelectedToolID();
            Set_showObjAfterClick_State(true);
            GameFacade.Instance.AddOperatedHotPointTo_operatedHotPointList(this);
            hotPointCollider.enabled = false;
            h.Off();
        }
        /// <summary>
        /// 设置热点为需点击热点
        /// </summary>
        public void SetNeedClick(bool isCanClick)
        {
            if (isCanClick)
            {
                hotPointCollider.enabled = true;
                HighlightOn_cueColor();
            }
            else
            {
                hotPointCollider.enabled = false;
                h.Off(); 
            }
        }


        public virtual void ResetToStateBeforeClick()
        { }
        /// <summary>
        /// 设置操作该热点时的工具
        /// </summary>
        public void Set_SelectedToolID()
        {
            string toolid = "";
            if (GameFacade.Instance.Get_currentSelectedTool() != null)
            {
                toolid = GameFacade.Instance.Get_currentSelectedTool().id;
            }
            SelectedToolID = new string(toolid.ToCharArray());
            Debug.Log("热点：" + id + "操作工具为：" + SelectedToolID);
        }
    }
}