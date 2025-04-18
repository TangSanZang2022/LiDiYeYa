using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
namespace Tool
{
    /// <summary>
    /// MD5加密工具
    /// </summary>
    public class EncryptMD5 
    {
        /// <summary>
        /// MD5 16位加密
        /// </summary>
        /// <param name="encryptContent">需要加密的内容</param>
        /// <returns></returns>
        public static string EncryptMD5_16(string encryptContent, bool isToLower = false)
        {
            var md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(encryptContent)), 4, 8);
            t2 = t2.Replace("-", "");
            if (isToLower)
            {
                t2 = t2.ToLower();
            }
            return t2;
        }
        /// <summary>
        /// MD5 32位加密
        /// </summary>
        /// <param name="encryptContent">需要加密的字符串</param>
        /// <param name="isToLower">是否是小写</param>
        /// <returns></returns>
        public static string EncryptMD5_32(string encryptContent, bool isToLower = false)
        {
            string normalContent = encryptContent.ToLower();
            string result = "";
            MD5 md5 = MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(normalContent));
            for (int i = 0; i < s.Length; i++)
            {
                result += s[i].ToString("X2");
            }
            if (isToLower)
            {
                result = result.ToLower();
            }
            return result;
        }
        /// <summary>
        /// MD5 64位加密
        /// </summary>
        /// <param name="encryptContent">需要加密的内容</param>
        /// <returns></returns>
        public static string EncryptMD5_64(string encryptContent, bool isToLower = false)
        {
            string content = encryptContent;
            MD5 md5 = MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(content));
            string result = Convert.ToBase64String(s);
            if(isToLower)
            {
                result = result.ToLower();
            }
            return result;
        }
    }
}
