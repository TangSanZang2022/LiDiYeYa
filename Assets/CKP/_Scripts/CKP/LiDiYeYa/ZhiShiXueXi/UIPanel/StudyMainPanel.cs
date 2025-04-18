using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using MyTreeView;
using UIFramework;
using Common;
using System;
using UnityEngine.EventSystems;
namespace LiDi
{
    /// <summary>
    /// 离地液压知识学习主页面
    /// </summary>
    public class StudyMainPanel : BasePanel
    {

        private Button returnMainSceneButton;
        /// <summary>
        /// 返回开始页面按钮
        /// </summary>
        public Button ReturnMainSceneButton
        {
            get
            {
                if (returnMainSceneButton == null)
                {
                    returnMainSceneButton = transform.FindChildForName<Button>("ReturnMainSceneButton");
                }
                return returnMainSceneButton;
            }
        }
        /// <summary>
        /// 所有点击之后有响应的菜单
        /// </summary>
        private List<TreeViewItem> treeViewItemsList = new List<TreeViewItem>();
        /// <summary>
        /// 当前所处在的节点
        /// </summary>
        public TreeViewItem currentTreeViewItem;
        private TreeViewControl treeView;
        /// <summary>
        /// 目录树形结构
        /// </summary>
        private TreeViewControl TreeView
        {
            get
            {
                if (treeView == null)
                {
                    treeView = transform.FindChildForName("TreeView").GetComponent<TreeViewControl>();
                }
                return treeView;
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            List<TreeViewData> treeViewDatas = XmlController.ReadJsonForLitJson<List<TreeViewData>>(Resources.Load<TextAsset>("TreeViewDataConfig").text);
            TreeView.Data = treeViewDatas;
            TreeView.GenerateTreeView_New();
            //刷新树形菜单
            TreeView.RefreshTreeView();
            //注册子元素的鼠标点击事件
            TreeView.ClickItemEvent += CallBack;
            Set_treeViewItemsList();
            TreeView.CloseAllItem();
            AddAllListener();
            //MaskImage.SetActive(false);
        }
        /// <summary>
        /// 点击目录之后回调
        /// </summary>
        /// <param name="item"></param>
        private void CallBack(GameObject item)
        {

            TreeViewItem treeViewItem = item.GetComponent<TreeViewItem>();
            if (treeViewItem.TreeViewToggle.isOn)
            {
                treeViewItem.TreeViewToggle.isOn = false;
                if (treeViewItem.GetChildrenNumber() != 0)
                {
                    treeView.ResetAllToggle();
                    treeView.CloseAllItem();
                    if (treeViewItem.GetParent() != null)
                    {
                        treeViewItem.GetParent().ExpandAll();
                        treeViewItem.GetParent().TreeViewToggle.isOn = true;
                    }
                    //treeViewItem.SetAllParentSelected(true);
                }
                currentTreeViewItem = null;
            }
            else
            {
                currentTreeViewItem = null;
                if (item.GetComponent<TreeViewItem>().GetChildrenNumber() != 0)
                {

                    treeView.ResetAllToggle();
                    treeView.CloseAllItem();
                    item.GetComponent<TreeViewItem>().SetAllParentSelected(false);
                    item.GetComponent<TreeViewItem>().TreeViewToggle.isOn = true;


                    item.GetComponent<TreeViewItem>().ContextButtonClick();
                    ExpandParent(item.GetComponent<TreeViewItem>());
                }
                else
                {
                    GameFacade.Instance.SetStepForStepID(item.transform.FindChildForName("TreeViewText").GetComponent<Text>().text);
                }
            }
            // treeViewItem.TreeViewToggle.isOn = true;

        }
        /// <summary>
        /// 展开所有父物体
        /// </summary>
        /// <param name="treeViewItem"></param>
        private void ExpandParent(TreeViewItem treeViewItem)
        {
            if (treeViewItem != null && treeViewItem.GetParent() != null)
            {
                if (!treeViewItem.GetParent().IsExpanding)
                {
                    treeViewItem.GetParent().ContextButtonClick();
                }
                ExpandParent(treeViewItem.GetParent());
            }
        }
        /// <summary>
        /// 设置可点击菜单列表
        /// </summary>
        private void Set_treeViewItemsList()
        {
            TreeViewItem[] treeViewItems = transform.GetComponentsInChildren<TreeViewItem>();
            foreach (var item in treeViewItems)
            {
                if (item.GetChildrenNumber() == 0)
                {
                    treeViewItemsList.Add(item);
                }
            }
            Debug.Log("可点击的菜单长度为" + treeViewItemsList.Count);
        }
        private void AddAllListener()
        {
            UIEventListener.GetUIEventListener(ReturnMainSceneButton.transform).pointClickHandler += On_ReturnMainSceneButton_Click;
        }
        /// <summary>
        /// 返回主场景按钮点击
        /// </summary>
        /// <param name="eventData"></param>
        private void On_ReturnMainSceneButton_Click(PointerEventData eventData)
        {
            MySceneManager.LoadSceneSync("StartScene");
        }
    }
}