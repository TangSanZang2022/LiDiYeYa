using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// 脚本单例类
    /// </summary>
    public class MonoSingleton<T> : MonoBehaviour where T: MonoSingleton<T>
    {
        static bool isQuite = false;
        private static T instance;

        public static T Instance
        {
            get
            {
                if (isQuite)
                {
                    return null;
                }
                if (instance==null)
                {
                    instance = FindObjectOfType<T>();
                    if (instance==null)
                    {
                        new GameObject("MonoSingleton of" + typeof(T)).AddComponent<T>();
                    }
                    else
                    {
                        instance.Init();
                    }
                }
                return instance;
            }
        }

        protected void Awake()
        {
            if (instance==null)
            {
                isQuite = false;
                instance = this as T;
                instance.Init();
            }
        }

        /// <summary>
        /// 初始化，由子类去实现
        /// </summary>
        public virtual void Init()
        { }

        private void OnDestroy()
        {
            isQuite = true;
            instance = null;
        }
    }
}
