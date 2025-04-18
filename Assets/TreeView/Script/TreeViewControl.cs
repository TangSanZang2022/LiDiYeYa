﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
namespace MyTreeView
{
    /// <summary>
    /// 树形菜单控制器
    /// </summary>
    public class TreeViewControl : MonoBehaviour
    {
        /// <summary>
        /// 当前树形菜单的数据源
        /// </summary>
        [HideInInspector]
        public List<TreeViewData> Data = null;
        /// <summary>
        /// 树形菜单中元素的模板
        /// </summary>
        public GameObject Template;
        /// <summary>
        /// 树形菜单中元素的根物体
        /// </summary>
        public Transform TreeItems;
        /// <summary>
        /// 树形菜单的纵向排列间距
        /// </summary>
        public int VerticalItemSpace = 2;
        /// <summary>
        /// 树形菜单的横向排列间距
        /// </summary>
        public int HorizontalItemSpace = 25;
        /// <summary>
        /// 树形菜单中元素的宽度
        /// </summary>
        public int ItemWidth = 230;
        /// <summary>
        /// 树形菜单中元素的高度
        /// </summary>
        public int ItemHeight = 35;
        /// <summary>
        /// 所有子元素的鼠标点击回调事件
        /// </summary>
        public delegate void ClickItemdelegate(GameObject item);
        public event ClickItemdelegate ClickItemEvent;

        /// <summary>
        /// 当前树形菜单中的所有元素
        /// </summary>
        private List<GameObject> _treeViewItems;
        /// <summary>
        /// 当前树形菜单中的所有元素克隆体（刷新树形菜单时用于过滤计算）
        /// </summary>
        private List<GameObject> _treeViewItemsClone;
        /// <summary>
        /// 树形菜单当前刷新队列的元素位置索引
        /// </summary>
        private int _yIndex = 0;
        /// <summary>
        /// 树形菜单当前刷新队列的元素最大层级
        /// </summary>
        private int _hierarchy = 0;
        /// <summary>
        /// 正在进行刷新
        /// </summary>
        private bool _isRefreshing = false;

        void Awake()
        {
            ClickItemEvent += ClickItemTemplate;
        }
        /// <summary>
        /// 鼠标点击子元素事件
        /// </summary>
        public void ClickItem(GameObject item)
        {
            ClickItemEvent(item);
        }
        void ClickItemTemplate(GameObject item)
        {
            //空的事件，不这样做的话ClickItemEvent会引发空引用异常
        }



        /// <summary>
        /// 返回指定名称的子元素是否被勾选
        /// </summary>
        public bool ItemIsCheck(string itemName)
        {
            for (int i = 0; i < _treeViewItems.Count; i++)
            {
                if (_treeViewItems[i].transform.Find("TreeViewText").GetComponent<Text>().text == itemName)
                {
                    return _treeViewItems[i].transform.Find("TreeViewToggle").GetComponent<Toggle>().isOn;
                }
            }
            return false;
        }
        /// <summary>
        /// 返回树形菜单中被勾选的所有子元素名称
        /// </summary>
        public List<string> ItemsIsCheck()
        {
            List<string> items = new List<string>();

            for (int i = 0; i < _treeViewItems.Count; i++)
            {
                if (_treeViewItems[i].transform.Find("TreeViewToggle").GetComponent<Toggle>().isOn)
                {
                    items.Add(_treeViewItems[i].transform.Find("TreeViewText").GetComponent<Text>().text);
                }
            }

            return items;
        }
        /// <summary>
        /// 返回树形菜单中被勾选的所有子元素
        /// </summary>
        public List<TreeViewItem> GetSelectItems()
        {
            List<TreeViewItem> items = new List<TreeViewItem>();

            for (int i = 0; i < _treeViewItems.Count; i++)
            {
                if (_treeViewItems[i].transform.Find("TreeViewToggle").GetComponent<Toggle>().isOn)
                {
                    items.Add(_treeViewItems[i].GetComponent<TreeViewItem>());
                }
            }

            return items;
        }
        /// <summary>
        /// 重置所有toggle
        /// </summary>
        public void ResetAllToggle()
        {
            List<TreeViewItem> treeViewItems = GetSelectItems();
            for (int i = 0; i < treeViewItems.Count; i++)
            {
                treeViewItems[i].TreeViewToggle.isOn = false;
            }
        }
        /// <summary>
        /// 生成树形菜单
        /// </summary>
        public void GenerateTreeView()
        {
            //删除可能已经存在的树形菜单元素
            if (_treeViewItems != null)
            {
                for (int i = 0; i < _treeViewItems.Count; i++)
                {
                    Destroy(_treeViewItems[i]);
                }
                _treeViewItems.Clear();
            }
            //重新创建树形菜单元素
            _treeViewItems = new List<GameObject>();
            for (int i = 0; i < Data.Count; i++)
            {
                GameObject item = Instantiate(Template);

                if (Data[i].ParentIndex == -1)
                {
                    item.GetComponent<TreeViewItem>().SetHierarchy(0);
                    item.GetComponent<TreeViewItem>().SetParent(null);
                }
                else
                {
                    TreeViewItem tvi = _treeViewItems[Data[i].ParentIndex].GetComponent<TreeViewItem>();
                    item.GetComponent<TreeViewItem>().SetHierarchy(tvi.GetHierarchy() + 1);
                    item.GetComponent<TreeViewItem>().SetParent(tvi);
                    tvi.AddChildren(item.GetComponent<TreeViewItem>());
                }

                item.transform.name = "TreeViewItem";
                item.transform.Find("TreeViewText").GetComponent<Text>().text = Data[i].Name;
                item.transform.SetParent(TreeItems);
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                item.transform.localRotation = Quaternion.Euler(Vector3.zero);
                item.SetActive(true);

                _treeViewItems.Add(item);
            }
        }
        /// <summary>
        /// 生成树形菜单
        /// </summary>
        public void GenerateTreeView_New()
        {
            //删除可能已经存在的树形菜单元素
            if (_treeViewItems != null)
            {
                for (int i = 0; i < _treeViewItems.Count; i++)
                {
                    Destroy(_treeViewItems[i]);
                }
                _treeViewItems.Clear();
            }
            //重新创建树形菜单元素
            _treeViewItems = new List<GameObject>();
            for (int i = 0; i < Data.Count; i++)
            {
                int index = i;
                GameObject item = Instantiate(Template);
                item.GetComponent<TreeViewItem>().SetHierarchy(0);
                item.GetComponent<TreeViewItem>().SetParent(null);
                item.GetComponent<TreeViewItem>().Set_treeViewData(Data[index]);
                item.transform.name = "TreeViewItem";
                item.transform.Find("TreeViewText").GetComponent<Text>().text = Data[i].Name;
                item.transform.SetParent(TreeItems);
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                item.transform.localRotation = Quaternion.Euler(Vector3.zero);
                item.SetActive(true);

                _treeViewItems.Add(item);
                if (Data[index].childTreeViewData == null || Data[index].childTreeViewData.Count == 0)
                {
                    item.transform.Find("ContextButton").gameObject.SetActive(false);
                }
                else
                {
                    GenerateTreeViewChild_New(Data[index].childTreeViewData, item.GetComponent<TreeViewItem>());
                }
            }
        }
        /// <summary>
        /// 生成树形菜单
        /// </summary>
        public void GenerateTreeViewChild_New(List<TreeViewData> treeData, TreeViewItem parentTrans)
        {
            for (int i = 0; i < treeData.Count; i++)
            {
                int index = i;
                GameObject item = Instantiate(Template);
                item.GetComponent<TreeViewItem>().SetHierarchy(parentTrans.GetHierarchy() + 1);
                item.GetComponent<TreeViewItem>().SetParent(parentTrans);
                parentTrans.AddChildren(item.GetComponent<TreeViewItem>());

                item.transform.name = "TreeViewItem";
                item.transform.Find("TreeViewText").GetComponent<Text>().text = treeData[i].Name;
                item.transform.SetParent(TreeItems);
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                item.transform.localRotation = Quaternion.Euler(Vector3.zero);
                item.SetActive(true);
                item.GetComponent<TreeViewItem>().Set_treeViewData(treeData[index]);
                _treeViewItems.Add(item);
                if (treeData[index].childTreeViewData == null || treeData[index].childTreeViewData.Count == 0)
                {
                    item.transform.Find("ContextButton").gameObject.SetActive(false);
                }
                else
                {
                    GenerateTreeViewChild_New(treeData[index].childTreeViewData, item.GetComponent<TreeViewItem>());
                }

            }
        }
        /// <summary>
        /// 刷新树形菜单
        /// </summary>
        public void RefreshTreeView()
        {
            //上一轮刷新还未结束
            if (_isRefreshing)
            {
                return;
            }

            _isRefreshing = true;
            _yIndex = 0;
            _hierarchy = 0;

            //复制一份菜单
            _treeViewItemsClone = new List<GameObject>(_treeViewItems);

            //用复制的菜单进行刷新计算
            for (int i = 0; i < _treeViewItemsClone.Count; i++)
            {
                //已经计算过或者不需要计算位置的元素
                if (_treeViewItemsClone[i] == null || !_treeViewItemsClone[i].activeSelf)
                {
                    continue;
                }

                TreeViewItem tvi = _treeViewItemsClone[i].GetComponent<TreeViewItem>();

                _treeViewItemsClone[i].GetComponent<RectTransform>().localPosition = new Vector3(tvi.GetHierarchy() * HorizontalItemSpace, _yIndex, 0);
                _yIndex += (-(ItemHeight + VerticalItemSpace));
                if (tvi.GetHierarchy() > _hierarchy)
                {
                    _hierarchy = tvi.GetHierarchy();
                }

                //如果子元素是展开的，继续向下刷新
                if (tvi.IsExpanding)
                {
                    RefreshTreeViewChild(tvi);
                }

                _treeViewItemsClone[i] = null;
            }

            //重新计算滚动视野的区域
            float x = _hierarchy * HorizontalItemSpace + ItemWidth;
            float y = Mathf.Abs(_yIndex);
            transform.GetComponent<ScrollRect>().content.sizeDelta = new Vector2(x, y);

            //清空复制的菜单
            _treeViewItemsClone.Clear();

            _isRefreshing = false;
        }
        /// <summary>
        /// 刷新元素的所有子元素
        /// </summary>
        void RefreshTreeViewChild(TreeViewItem tvi)
        {
            for (int i = 0; i < tvi.GetChildrenNumber(); i++)
            {
                tvi.GetChildrenByIndex(i).gameObject.GetComponent<RectTransform>().localPosition = new Vector3(tvi.GetChildrenByIndex(i).GetHierarchy() * HorizontalItemSpace, _yIndex, 0);
                _yIndex += (-(ItemHeight + VerticalItemSpace));
                if (tvi.GetChildrenByIndex(i).GetHierarchy() > _hierarchy)
                {
                    _hierarchy = tvi.GetChildrenByIndex(i).GetHierarchy();
                }

                //如果子元素是展开的，继续向下刷新
                if (tvi.GetChildrenByIndex(i).IsExpanding)
                {
                    RefreshTreeViewChild(tvi.GetChildrenByIndex(i));
                }

                int index = _treeViewItemsClone.IndexOf(tvi.GetChildrenByIndex(i).gameObject);
                if (index >= 0)
                {
                    _treeViewItemsClone[index] = null;
                }
            }
        }

        public void CloseAllItem()
        {
            //Debug.Log(_treeViewItems[0].name);
            // _treeViewItems[0].GetComponent<TreeViewItem>().ContextButtonClick();
            for (int i = _treeViewItems.Count - 1; i >= 0; i--)
            {
                //Debug.Log(i);
                int index = i;
                if (_treeViewItems[index].GetComponent<TreeViewItem>().IsExpanding)
                {
                    _treeViewItems[index].GetComponent<TreeViewItem>().ContextButtonClick();
                    _treeViewItems[index].GetComponent<TreeViewItem>().Set_ContextButton();
                }
                //RefreshTreeView();
            }



        }
    }
}
