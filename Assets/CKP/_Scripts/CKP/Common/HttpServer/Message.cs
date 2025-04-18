using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Linq;

namespace UnityHttpServer
{
    /// <summary>
    /// 处理从服务器接收的数据类
    /// </summary>
    public class Message
    {
        /// <summary>
        /// 用于存储接收的数据
        /// </summary>
        private byte[] data = new byte[1024];

        /// <summary>
        /// 存储了多少个字节在data里面
        /// </summary>
        private int startIndex = 0;

        /// <summary>
        /// 存取数据的数组
        /// </summary>
        public byte[] Data
        {
            get
            {
                return data;
            }
        }

        /// <summary>
        /// 开始存储的位置
        /// </summary>
        public int StartIndex
        {
            get
            {
                return startIndex;
            }
        }

        /// <summary>
        /// data中剩余的存储位置
        /// </summary>
        public int RemainSize
        {
            get
            {
                return data.Length - startIndex;
            }
        }

        /// <summary>
        /// 读取从服务器接收的数据
        /// </summary>
        /// <param name="newDataAmount">接收到的byte[]数据长度</param>
        /// <param name="processDataCallback">处理过程数据回调</param>
        public void ReadMessage(int newDataAmount, Action<ActionCode, string> processDataCallback)
        {
            startIndex += newDataAmount;
            while (true)
            {
                if (startIndex <= 4)// 当小于4个的时候需要继续接收，还没有完全接收到此条数据的长度
                {
                    return;
                }
                int count = BitConverter.ToInt32(data, 0);//先读取前4个字节，知道此条数据真正的长度
                if ((startIndex - 4) >= count)//已经全部接收了，可以开始解析数据
                {
                    ActionCode actionCode = (ActionCode)BitConverter.ToInt32(data, 4);
                    string res = Encoding.UTF8.GetString(Data, 8, count - 4); //将byte[]转换为string，从第8位开始，长度也减去4，
                                                                              //因为前4位为这条数据的长度，
                    processDataCallback(actionCode, res);//通过传进来的事件来处理解析后的数据                                                        
                    Array.Copy(data, count + 4, data, 0, startIndex - count - 4); //将data中已经转换为res的数据移出data
                    startIndex -= (count + 4); //开始接收的长度减去已经解析了的长度
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 当为Http协议的时候，没有分包和粘包问题
        /// </summary>
        /// <param name="str"></param>
        /// <param name="proccessDataCallBack"></param>
        public void ReadMessageForHttp(string str, Action<ActionCode, string> proccessDataCallBack)
        {
            BaseData baseData = JsonUtility.FromJson<BaseData>(str);
            //TODO 这里要将str 转换为一个ActionCode,和要处理的信息
            ActionCode actionCode = (ActionCode)(int.Parse(baseData.ActionCode));
            Debug.Log(actionCode);
            proccessDataCallBack(actionCode, baseData.data);
        }
        /// <summary>
        /// 测试用，解析本地字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="proccessDataCallBack"></param>
        public void ReadMessageForTest(string str, Action<ActionCode, string> proccessDataCallBack)
        {
            Debug.Log(str);
            byte[] bs = Encoding.UTF8.GetBytes(str);
            string s = Encoding.UTF8.GetString(bs);
            BaseData baseData = JsonUtility.FromJson<BaseData>(s);
            //TODO 这里要将str 转换为一个ActionCode,和要处理的信息
            ActionCode actionCode = (ActionCode)(int.Parse(baseData.ActionCode));
            Debug.Log(actionCode);
            proccessDataCallBack(actionCode, baseData.data);
        }
        /// <summary>
        /// 将字符串转换为byte[],并在前面追加此条数据的长度
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] PackData(string data)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            int count = dataBytes.Length;
            byte[] countBytes = BitConverter.GetBytes(count);
            return countBytes.Concat(dataBytes).ToArray<byte>();
        }
    }
}
/// <summary>
/// 测试用，基础数据，先解析出来ActionCode，然后再做处理
/// </summary>
[Serializable]
public class BaseData
{
    public string ActionCode;

    public string data;
}
