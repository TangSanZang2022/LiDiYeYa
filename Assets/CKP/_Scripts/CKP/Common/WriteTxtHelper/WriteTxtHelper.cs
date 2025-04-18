using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
/// <summary>
/// 将string写在本地
/// </summary>
public static class WriteTxtHelper
{
    /// <summary>
    /// 当从服务器接收到无法解析的数据的时候，将此接收信息存放到该地址
    /// </summary>
    public const string errorMessageFromServerWritePath = "ErrorMessageFromServer.txt";
    /// <summary>
    /// 当需要将Txt写入本地时调用，默认在Streaming Assets下面
    /// </summary>
    /// <param name="path"></param>
    /// <param name="msg"></param>
    public static void WriteString(string path, string msg)
    {
        path = Application.streamingAssetsPath + @"//" + path;
        if (!File.Exists(path))
        {
            FileStream fileStream = File.Create(path);
            fileStream.Close();
        }
        StreamWriter sw = File.AppendText(path);
        string data = DateTime.Now.ToString("yyyy-MM-dd-[hh:mm:ss]") + msg + "\n";
        byte[] byteData = Encoding.UTF8.GetBytes(data);
        sw.Write(data);
        sw.Close();
        sw.Dispose();
    }
}
