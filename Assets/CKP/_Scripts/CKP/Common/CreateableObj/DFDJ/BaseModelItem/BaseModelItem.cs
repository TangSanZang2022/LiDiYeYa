using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace DFDJ
{
    /// <summary>
    /// 创建模型选项
    /// </summary>
    /// <param name="baseModelItem"></param>
    public delegate void BaseModelItemToggleOn(BaseModelItem baseModelItem);
    /// <summary>
    /// 可编辑部件选项基类
    /// </summary>
    public class BaseModelItem : MonoBehaviour
    {

        public GameObject currentCreatingObj;
        /// <summary>
        /// 选择了选项之后回调
        /// </summary>
        public BaseModelItemToggleOn baseModelItemToggleOn;
        [SerializeField]
        /// <summary>
        /// 此选项对应的数据
        /// </summary>
        protected BaseModelItemData baseModelItemData;

        /// <summary>
        /// 创建的模型类型
        /// </summary>
        public ModelType CreatedModelType
        {
            get;
            set;
        }
        private Toggle itemToggle;
        /// <summary>
        /// 选项Toggle
        /// </summary>
        public Toggle ItemToggle
        {
            get
            {
                if (itemToggle == null)
                {
                    itemToggle = GetComponent<Toggle>();
                }
                return itemToggle;
            }
        }
        private Text nameText;
        /// <summary>
        /// 展示的名称Text
        /// </summary>
        public Text NameText
        {
            get
            {
                if (nameText == null)
                {
                    nameText = transform.FindChildForName("NameText").GetComponent<Text>();
                }
                return nameText;
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            baseModelItemToggleOn += delegate { GameFacade.Instance.CreateModelOnClickModelItem(this); };
            AddListener();
        }
        /// <summary>
        /// 获取选项包含的数据
        /// </summary>
        /// <returns></returns>
        public BaseModelItemData Get_baseModelItemData()
        {
            return baseModelItemData;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Init(BaseModelItemData data, ToggleGroup toggleGroup)
        {
            this.baseModelItemData = data;
            //NameText.text = baseModelItemData.modelName;
            NameText.text = baseModelItemData.modelNameInUIUnity;
            ItemToggle.group = toggleGroup;
            AddListener();
        }

        private void AddListener()
        {
            ItemToggle.onValueChanged.AddListener(delegate { On_ItemToggle_ValueChange(ItemToggle.isOn); });
        }

        private void On_ItemToggle_ValueChange(bool isOn)
        {
            if (isOn)
            {
                if (baseModelItemToggleOn != null)
                {
                    baseModelItemToggleOn(this);
                }
            }
            else
            {
                if (currentCreatingObj != null)//如果鼠标上正在创建物体，则销毁
                {
                    Destroy(currentCreatingObj);
                }
            }

        }
    }
    [Serializable]
    /// <summary>
    /// 所有选项的数据解析类列表
    /// </summary>
    public class AllModelItemDataList
    {
        public int code;
        public string msg;
        public AllModelItemData data;
    }
    [Serializable]
    /// <summary>
    /// 所有选项的数据
    /// </summary>
    public class AllModelItemData
    {
        public List<BaseModelItemData> baseModelItemDatas = new List<BaseModelItemData>();
    }

    [Serializable]
    /// <summary>
    /// 可编辑模型选项数据基类
    /// </summary>
    public class BaseModelItemData
    {
        /// <summary>
        /// 模型在UnityUI上展示的名称
        /// </summary>
        public string modelNameInUIUnity;
        /// <summary>
        /// 可编辑模型名称
        /// </summary>
        public string modelName;
        /// <summary>
        /// 可编辑模型ID
        /// </summary>
        public string modelID;
        /// <summary>
        /// 加载路径
        /// </summary>
        public string loadPath;
    }
}
