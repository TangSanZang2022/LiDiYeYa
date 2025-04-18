using System;
using System.Collections.Generic;
using UnityEngine;
namespace MyTreeView
{
    [Serializable]
    /// <summary>
    /// 树形菜单数据
    /// </summary>
    public class TreeViewData
    { /// <summary>
      /// 数据内容
      /// </summary>
        public string Name;
        /// <summary>
        /// 在列表中的索引
        /// </summary>
        public int index;
        /// <summary>
        /// ID
        /// </summary>
        public string id;
        /// <summary>
        /// 数据作为哪个索引的子物体,如果为-1，则是最上层
        /// </summary>
        public int ParentIndex;
        /// <summary>
        /// 子菜单
        /// </summary>
        public List<TreeViewData> childTreeViewData = new List<TreeViewData>();
    }



}
