using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Security.Cryptography;
using System;

namespace Tool
{
    /// <summary>
    /// 加密，解密工具
    /// </summary>
    public static class KeyTool 
    {
        /// <summary>
        /// HmacSHA256 Base64算法,返回的结果始终是32位
        /// </summary>
        /// <param name="message">待加密的明文字符串</param>
        /// <returns>HmacSHA256算法加密之后的密文</returns>
        public static string HmacSHA256(string message, string _appSecret)
        {
            byte[] keyByte = Encoding.GetEncoding("utf-8").GetBytes(_appSecret);
            byte[] messageBytes = Encoding.GetEncoding("utf-8").GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }
        /// <summary>
        /// 得到随机字符串
        /// </summary>
        /// <returns></returns>
        public static string CreateRandomCode(int count)
        {
            string str = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            char[] chars = str.ToCharArray();
            System.Text.StringBuilder strRan = new System.Text.StringBuilder();
            System.Random ran = new System.Random();
            for (int i = 0; i < count; i++)
            {
                strRan.Append(chars[ran.Next(0, 36)]);
            }
            return strRan.ToString();
        }

    }
}
