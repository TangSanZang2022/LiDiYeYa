using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace ShowHideObj
{
    /// <summary>
    /// 显示隐藏物体基类
    /// </summary>
    public class BaseShowHideObj : MonoBehaviour
    {
        [SerializeField]
        /// <summary>
        /// 物体ID
        /// </summary>
        protected string id;
        [SerializeField]
        /// <summary>
        /// 是否需要在开始的时候隐藏
        /// </summary>
        protected bool isHideOnStart = false;
        protected virtual void OnEnable()
        {

        }
        // Start is called before the first frame update
        protected virtual void Start()
        {

        }
        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Oninit()
        {
            if (isHideOnStart)
            {
                gameObject.SetActive(false);
            }
        }
        // Update is called once per frame
        protected virtual void Update()
        {

        }


        /// <summary>
        /// 设置ID
        /// </summary>
        /// <param name="id"></param>
        public void SetID(string id)
        {
            this.id = id;
        }
        /// <summary>
        /// 获取ID
        /// </summary>
        /// <returns></returns>
        public string GetID()
        {
            if (string.IsNullOrEmpty(id))
            {
                SetID(gameObject.name);
            }
            return id;
        }
        /// <summary>
        /// 显示
        /// </summary>
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }
        /// <summary>
        /// 隐藏
        /// </summary>
        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
        public bool Get_isHideOnStart()
        {
            return isHideOnStart;
        }
    }
}
