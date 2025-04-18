using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Tool
{
    public static class TimeTool
    {
        public static string GetNowTime(string format= "yyyy/MM/dd HH:mm:ss")
        {
            return DateTime.Now.ToString(format);
        }

        /// <summary>
        /// 时间戳转C#时间
        /// </summary>
        public static DateTime TimeStamp2DateTime(long timeStamp, int timeZone = 0, bool isSecond = true)
        {
            DateTime startTime = new DateTime(1970, 1, 1, timeZone, 0, 0);
            DateTime dt = isSecond
                ? startTime.AddSeconds(timeStamp)
                : startTime.AddMilliseconds(timeStamp);
            return dt;
        }

        /// <summary>
        /// C#时间转时间戳
        /// </summary>
        public static long DateTime2TimeStamp(DateTime now, int timeZone = 0, bool getSecond = true)
        {
           
            DateTime startTime = new DateTime(1970, 1, 1, timeZone, 0, 0);
            TimeSpan ts = now - startTime;
            Debug.Log(now);
            return getSecond
                ? (long)ts.TotalSeconds
                : (long)ts.TotalMilliseconds;
        }


        /// <summary>
        /// 时间戳转C#时间string 
        /// </summary>
        public static string TimeStamp2DateTime_String(long timeStamp, int timeZone = 0, bool isSecond = true)
        {
            DateTime startTime = new DateTime(1970, 1, 1, timeZone, 0, 0);
            DateTime dt = isSecond
                ? startTime.AddSeconds(timeStamp)
                : startTime.AddMilliseconds(timeStamp);
            return dt.ToString("yyyy/MM/dd   HH:MM:ss");
        }
        /// <summary>
        /// 根据分钟得到时间，**小时**分钟
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static string GetTimeString_minutes(int minutes)
        {
            int hd = minutes / 60;
            //Debug.Log(stayTime+"："+ hd);
            string h = hd.ToString() != "0" ? hd.ToString() + "小时" : "";//小时
            int md = minutes % 60;
            //Debug.Log(stayTime + "：" + md);
            string m = md.ToString()+ "分钟" ;//分钟
            
           
            return h + m ;
        }

        /// <summary>
        /// 根据分钟得到时间，**小时**分钟**秒
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static string GetTimeString_Second(int seconds)
        {
            int hd = seconds / 3600;
            //Debug.Log(stayTime+"："+ hd);
            string h = hd.ToString() != "0" ? hd.ToString() + "小时" : "";//小时
            int md = (seconds % 3600) / 60;
            //Debug.Log(stayTime + "：" + md);
            string m=""; //= md.ToString() != "0" ? md.ToString() + "分钟" : "";//分钟
            if (h=="")
            {
                if (md != 0)
                {
                    m = md.ToString() + "分钟";
                }
               
            }
            else
            {
                m = md.ToString() + "分钟";
            }
           
            int sd = ((seconds % 3600) % 60);
            string s = sd.ToString()+"秒";
            //if (h == "" && m == "")//只有秒
            //{
            //    s = seconds.ToString() + "秒";
            //}
            return h + m + s;
        }
    }
}
