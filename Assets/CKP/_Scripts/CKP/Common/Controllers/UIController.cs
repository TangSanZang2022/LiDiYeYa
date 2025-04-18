using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UIFramework
{
    /// <summary>
    /// UI控制器
    /// </summary>
    public class UIController : BaseController
    {
        //  private static UIController _instance;
        /// <summary>
        /// 单例模式，如果不在GameFacade中使用，就用单例模式
        /// </summary>
        //public static UIController Instance
        //{
        //    get
        //    {
        //        if (_instance == null)
        //        {
        //            _instance = new UIController();
        //        }
        //        return _instance;
        //    }
        //}
        //public UIController()
        //{
        //    AddUIPanelPathDict();
        //}

        public UIController(GameFacade gameFacade) : base(gameFacade)
        {

        }
        /// <summary>
        /// UIController初始化
        /// </summary>
        public override void OnInit()
        {
            AddUIPanelPathDict();
            //PushPanel(UIPanelType.MainPanel);
        }
        /// <summary>
        /// UIController更新
        /// </summary>
        public override void OnUpdate()
        {

        }
        /// <summary>
        /// UIController销毁
        /// </summary>
        public override void OnDestory()
        {

        }
        private Transform canvasTrans;
        /// <summary>
        /// 场景中的Canvas，所有创建的面板都要是他的子物体
        /// </summary>
        private Transform CanvasTrans
        {
            get
            {
                if (canvasTrans == null)
                {
                    canvasTrans = GameObject.Find("UIFrameworkCanvas").transform;
                }
                return canvasTrans;
            }
        }
        /// <summary>
        /// 存放不同类型UI面板对应的预制体存放的位置字典
        /// </summary>
        private Dictionary<UIPanelType, string> uiPanelPathDict = new Dictionary<UIPanelType, string>();
        /// <summary>
        /// 存放所有已经创建的Ui面板，方便下次再显示此面板
        /// </summary>
        private Dictionary<UIPanelType, BasePanel> basePanelDict = new Dictionary<UIPanelType, BasePanel>();
        /// <summary>
        /// 存放所有显示的Ui界面
        /// </summary>
        private Stack<BasePanel> panelStack = new Stack<BasePanel>();

        /// <summary>
        /// 弹出UI界面，入栈
        /// </summary>
        /// <param name="uIPanelType">UI的类型</param>
        public BasePanel PushPanel(UIPanelType uIPanelType)
        {
            if (panelStack.Count > 0)
            {
                BasePanel topPanel = panelStack.Peek();//获得栈顶的面板，但是不让它出栈
                topPanel.OnPause();//让栈顶界面暂停，因为有新的页面要入栈
            }
            BasePanel panel = GetUIPanel(uIPanelType);
            panel.OnEnter();
            // if (!panelStack.Contains(panel))
            {
                panelStack.Push(panel);//将此面板存入栈中 
            }
            return panel;
        }
        /// <summary>
        /// 出栈，因为栈的结构为先进后出，所以出栈的永远是栈顶的面板
        /// </summary>
        public void PopPanel()
        {
            if (panelStack.Count < 1) //栈中没有面板
            {
                return;
            }
            BasePanel topPanel1 = panelStack.Pop(); //栈顶元素
            topPanel1.OnExit();
            if (panelStack.Count < 1)
            {
                return;
            }
            BasePanel topPanel2 = panelStack.Peek();//原来栈顶元素出栈之后，得到新的栈顶元素，但是不出栈
            topPanel2.OnResume();
        }
        /// <summary>
        /// 得到栈顶面板
        /// </summary>
        /// <returns></returns>
        public BasePanel GetTopPanel()
        {
            BasePanel topPanel = null;
            if (panelStack.Count > 0)
            {
                topPanel = panelStack.Peek();//获得栈顶的面板，但是不让它出栈
            }
            return topPanel;
        }
        /// <summary>
        /// 给uiPanelPathDict字典赋值
        /// </summary>
        private void AddUIPanelPathDict()
        {
            TextAsset textAsset = Resources.Load<TextAsset>("UIPanelType");
            UIPanelTypeJson uIPanelTypeJson = JsonUtility.FromJson<UIPanelTypeJson>(textAsset.text);
            foreach (UIPanelInfo info in uIPanelTypeJson.uiInfoList)
            {
                uiPanelPathDict.Add(info.panelType, info.path);
            }
        }
        /// <summary>
        /// 根据UIPanelType获取UI面板
        /// </summary>
        /// <param name="uIPanelType"></param>
        /// <returns></returns>
        public BasePanel GetUIPanel(UIPanelType uiPanelType)
        {
            BasePanel basePanel;
            if (!basePanelDict.TryGetValue(uiPanelType, out basePanel)) //如果字典中不存在此类型的UI面板则需要重新创建
            {
                Debug.Log(uiPanelType);
                basePanel = GameObject.Instantiate(Resources.Load<BasePanel>(uiPanelPathDict[uiPanelType]), CanvasTrans, false);
                Debug.Log(string.Format("创建预制体成功，预制体名称为：{0},父物体名称为", basePanel.name, basePanel.transform.parent.name));
                basePanel.SetUIPanelType(uiPanelType);//给创建出来的面板的类型赋值
                basePanelDict.Add(uiPanelType, basePanel);
            }
            return basePanel;

        }

    }
    [Serializable]
    /// <summary>
    /// UIPanelType的json解析类
    /// </summary>
    public class UIPanelTypeJson
    {
        /// <summary>
        /// 不同类型UI面板对应地址列表
        /// </summary>
        public List<UIPanelInfo> uiInfoList;
    }
}