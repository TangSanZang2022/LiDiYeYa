using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Common
{
    /// <summary>
    /// 数组助手类
    /// </summary>
    public static class ArrayHelper
    {
        /// <summary>
        /// 查找满足条件的单个元素
        /// </summary>
        /// <typeparam name="T">数组的类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="condition">查找条件，由调用者决定</param>
        /// <returns>返回的满足条件的对象</returns>
        public static T Find<T>(this T[] array, Func<T, bool> condition)
        {
            foreach (T item in array)
            {
                if (condition(item))
                {
                    return item;
                }
            }
            return default(T);
        }

        /// <summary>
        /// 查找满足条件的所有元素
        /// </summary>
        /// <typeparam name="T">数组的类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="condition">查找条件，由调用者决定</param>
        /// <returns>返回的满足条件的所有对象数组</returns>
        public static T[] FindAll<T>(this T[] array, Func<T, bool> condition)
        {
            List<T> resList = new List<T>();
            foreach (T item in array)
            {
                if (condition(item))
                {
                    resList.Add(item);
                }
            }
            return resList.ToArray();
        }
        /// <summary>
        /// 返回最大值
        /// </summary>
        /// <typeparam name="T">数组的类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="condition">查找最大值条件，由调用者决定</param>
        /// <returns></returns>
        public static T FindMax<T>(this T[] array, Func< T, T, T> condition)
        {
            T res = default(T);
            if (array.Length==0)
            {
                return res;
            }
            else
            {
                res = array[0];
                for (int i = 0; i < array.Length; i++)
                {
                    res = condition(res, array[i]);
                }
            }
            return res;
        }

        /// <summary>
        /// 返回最大值
        /// </summary>
        /// <typeparam name="T">数组的类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="condition">查找最大值条件，由调用者决定</param>
        /// <returns></returns>
        public static T GetMax<T,Q>(this T[] array, Func<T,Q> condition) where Q:IComparable
        {
            T res = default(T);
            if (array.Length == 0)
            {
                return res;
            }
            else
            {
                res = array[0];
                for (int i = 0; i < array.Length; i++)
                {
                    if (condition(res).CompareTo(condition(array[i]))<0)
                    {
                        res = array[i];
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// 返回最小值
        /// </summary>
        /// <typeparam name="T">数组的类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="condition">查找最小值条件，由调用者决定</param>
        /// <returns></returns>
        public static T GetMin<T, Q>(this T[] array, Func<T, Q> condition) where Q : IComparable
        {
            T res = default(T);
            if (array.Length == 0)
            {
                return res;
            }
            else
            {
                res = array[0];
                for (int i = 0; i < array.Length; i++)
                {
                    if (condition(res).CompareTo(condition(array[i])) > 0)
                    {
                        res = array[i];
                    }
                }
            }
            return res;
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void SortArray<T>( ref  T[] array) where T : IComparable<T>
        {
            List<T> list = new List<T>();
            list.AddRange(array);
            list.Sort();
            array = list.ToArray();
        }
        /// <summary>
        /// 升序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Q"></typeparam>
        /// <param name="array"></param>
        /// <param name="condition"></param>
        public static T[] AscendingOrder<T,Q>(this T[] array,Func<T,Q> condition)where Q: IComparable
        {
            for (int i = 0; i < array.Length; i++)
            {
                for (int j =0; j < array.Length-i-1; j++)
                {
                    //if (array[j]< array[j+1])
                    if(condition(array[j]).CompareTo(condition(array[j+1]))>0)
                    {
                        T item = array[j+1]; 
                        array[j+1] = array[j];
                        array[j] = item;
                    }
                }
            }
            return array;
        }
        /// <summary>
        /// 降序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Q"></typeparam>
        /// <param name="array"></param>
        /// <param name="condition"></param>
        public static T[] DescendingOrder<T, Q>(this T[] array, Func<T, Q> condition) where Q : IComparable
        {
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = 0; j < array.Length - i - 1; j++)
                {
                    //if (array[j]< array[j+1])
                    if (condition(array[j]).CompareTo(condition(array[j + 1]))<0)
                    {
                        T item = array[j + 1];
                        array[j + 1] = array[j];
                        array[j] = item;
                    }
                }
            }
            return array;
        }
        /// <summary>
        /// 筛选
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Q"></typeparam>
        /// <param name="array"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static Q[] Select<T,Q>(this T[] array,Func<T,Q> condition)
        {
            List<Q> result = new List<Q>(array.Length);
            for (int i = 0; i < array.Length; i++)
            {
                Q q = condition(array[i]);
                if (q!=null)
                {
                    result.Add(q);
                }
            }
            return result.ToArray();
        }
    }
}
