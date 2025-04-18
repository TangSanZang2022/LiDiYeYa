using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

namespace MyTreeView
{
    /// <summary>
    /// 树形菜单元素
    /// </summary>
    public class TreeViewItem : MonoBehaviour
    {
        /// <summary>
        /// 当前选项对应的数据
        /// </summary>
        private TreeViewData treeViewData;
        /// <summary>
        /// 树形菜单控制器
        /// </summary>
        public TreeViewControl Controler;
        /// <summary>
        /// 当前元素的子元素是否展开（展开时可见）
        /// </summary>
        public bool IsExpanding = false;

        //当前元素在树形图中所属的层级
        private int _hierarchy = 0;
        //当前元素指向的父元素
        private TreeViewItem _parent;
        //当前元素的所有子元素
        private List<TreeViewItem> _children;
        //正在进行刷新
        private bool _isRefreshing = false;

        private Toggle treeViewToggle;
        /// <summary>
        /// 选项的选中框
        /// </summary>
        public Toggle TreeViewToggle
        {
            get
            {
                if (treeViewToggle == null)
                {
                    treeViewToggle = transform.FindChildForName("TreeViewToggle").GetComponent<Toggle>();
                }
                return treeViewToggle;
            }
        }
        private Button treeViewButton;
        /// <summary>
        /// 选项的按钮
        /// </summary>
        public Button TreeViewButton
        {
            get
            {
                if (treeViewButton == null)
                {
                    treeViewButton = transform.FindChildForName("TreeViewButton").GetComponent<Button>();
                }
                return treeViewButton;
            }
        }

        private Button contextButton;
        /// <summary>
        /// 选项的展开按钮
        /// </summary>
        public Button ContextButton
        {
            get
            {
                if (contextButton == null)
                {
                    contextButton = transform.FindChildForName("ContextButton").GetComponent<Button>();
                }
                return contextButton;
            }
        }

        private Text treeViewText;
        /// <summary>
        /// 选项的文字
        /// </summary>
        public Text TreeViewText
        {
            get
            {
                if (treeViewText == null)
                {
                    treeViewText = transform.FindChildForName("TreeViewText").GetComponent<Text>();
                }
                return treeViewText;
            }
        }
        [SerializeField]
        private Color normalColor;
        [SerializeField]
        private Color clickColor;
        /// <summary>
        /// 获取是否在刷新
        /// </summary>
        /// <returns></returns>
        public bool Get_isRefreshing()
        {
            return _isRefreshing;
        }
        void Awake()
        {
            //上下文按钮点击回调
            transform.Find("ContextButton").GetComponent<Button>().onClick.AddListener(ContextButtonClick);
            transform.Find("TreeViewButton").GetComponent<Button>().onClick.AddListener(delegate ()
            {
                Controler.ClickItem(gameObject);
            });
            TreeViewToggle.onValueChanged.AddListener(delegate (bool b)
            {
                OnTreeViewToggleChange(b);
            });

        }

        private void OnTreeViewToggleChange(bool b)
        {
            TreeViewText.color = b ? clickColor : normalColor;
            transform.Find("ContextButton").GetComponent<Image>().color = b ? clickColor : normalColor;

        }

        public void CloseSelf()
        {
            StartCoroutine(ICloseSelf());
        }
        IEnumerator ICloseSelf()
        {
            yield return new WaitUntil(() => !_isRefreshing);
            Debug.Log(_isRefreshing);
            ContextButtonClick();
        }
        /// <summary>
        /// 点击上下文菜单按钮，元素的子元素改变显示状态
        /// </summary>
        public void ContextButtonClick()
        {

            //上一轮刷新还未结束
            if (_isRefreshing)
            {

                return;
            }
            _isRefreshing = true;
            if (IsExpanding)
            {
                transform.Find("ContextButton").GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 90);
                IsExpanding = false;
                ChangeChildren(this, false);
            }
            else
            {
                transform.Find("ContextButton").GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
                IsExpanding = true;
                ChangeChildren(this, true);
            }

            //刷新树形菜单
            Controler.RefreshTreeView();

            _isRefreshing = false;
        }
        /// <summary>
        /// 改变某一元素所有子元素的显示状态
        /// </summary>
        void ChangeChildren(TreeViewItem tvi, bool value)
        {
            for (int i = 0; i < tvi.GetChildrenNumber(); i++)
            {
                tvi.GetChildrenByIndex(i).gameObject.SetActive(value);
                if (tvi.GetChildrenByIndex(i).IsExpanding)
                {
                    ChangeChildren(tvi.GetChildrenByIndex(i), value);
                }
            }
        }


        public void SetContextButtonColor(Color color, bool isCanClick = true)
        {
            transform.Find("ContextButton").GetComponent<Image>().color = color;

            transform.Find("ContextButton").GetComponent<Button>().enabled = isCanClick;

        }

        public void Set_ContextButton()
        {
            if (_children == null)
            {
                SetContextButtonColor(Color.gray, false);
                transform.Find("ContextButton").gameObject.SetActive(false);
                return;
            }
            if (_children.Count == 0)
            {
                SetContextButtonColor(Color.gray, false);
                transform.Find("ContextButton").gameObject.SetActive(false);
            }
            else
            {
                SetContextButtonColor(Color.white, true);
                transform.Find("ContextButton").gameObject.SetActive(true);
            }
        }


        /// <summary>
        /// 展开所有选项
        /// </summary>
        public void ExpandAll(bool isCloseOthers = false)
        {
            if (isCloseOthers)
            {
                Controler.CloseAllItem();
            }
            if (!IsExpanding)
            {
                ContextButtonClick();
            }
            if (GetParent() != null && !GetParent().IsExpanding)
            {
                GetParent().ContextButtonClick();
                GetParent().ExpandAll();
            }
        }
        /// <summary>
        /// 设置所有父节点为选中状态
        /// </summary>
        public void SetAllParentSelected(bool isResetAll = true)
        {
            if (isResetAll)
            {
                Controler.ResetAllToggle();
            }

            TreeViewToggle.isOn = true;

            if (GetParent() != null)
            {
                GetParent().TreeViewToggle.isOn = true;
                GetParent().SetAllParentSelected(false);
            }
        }
        #region 属性访问
        public int GetHierarchy()
        {
            return _hierarchy;
        }
        public void SetHierarchy(int hierarchy)
        {
            _hierarchy = hierarchy;
            if (hierarchy > 0)
            {
                TreeViewToggle.gameObject.SetActive(false);
                TreeViewText.fontSize = 18;
            }
            else
            {
                TreeViewToggle.gameObject.SetActive(true);
                TreeViewText.fontSize = 20;
            }
        }
        public TreeViewItem GetParent()
        {
            return _parent;
        }
        public void SetParent(TreeViewItem parent)
        {
            _parent = parent;
        }
        public void AddChildren(TreeViewItem children)
        {
            if (_children == null)
            {
                _children = new List<TreeViewItem>();
            }
            _children.Add(children);
        }
        public void RemoveChildren(TreeViewItem children)
        {
            if (_children == null)
            {
                return;
            }
            _children.Remove(children);
        }
        public void RemoveChildren(int index)
        {
            if (_children == null || index < 0 || index >= _children.Count)
            {
                return;
            }
            _children.RemoveAt(index);
        }
        public int GetChildrenNumber()
        {
            if (_children == null)
            {
                return 0;
            }

            return _children.Count;
        }
        public TreeViewItem GetChildrenByIndex(int index)
        {
            if (index >= _children.Count)
            {
                return null;
            }
            return _children[index];
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="data"></param>
        public void Set_treeViewData(TreeViewData data)
        {
            treeViewData = data;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public TreeViewData Get_treeViewData()
        {
            return treeViewData;
        }
        #endregion
    }
}