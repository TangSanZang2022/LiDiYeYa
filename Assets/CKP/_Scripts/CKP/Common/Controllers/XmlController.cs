using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using LitJson;
using System;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Text;

/// <summary>
/// 配置文件控制器
/// </summary>
public class XmlController : BaseController
{
    public XmlController(GameFacade gameFacade) : base(gameFacade)
    { }


    /// <summary>
    /// 初始化
    /// </summary>
    public override void OnInit()
    {
        base.OnInit();
        //ReadXml();
        //ReadConveyorBeltMonitorConfig();
        //ReadMoveObjDataConfig();
        //ReadPoliceInfos();
        //sdsd;
    }

    /// <summary>
    /// 创建配置文件
    /// </summary>
    private void CreateXml()
    {
        string path = Application.streamingAssetsPath + @"/NetConfig.xml";
        if (!File.Exists(path))
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement root = xmlDoc.CreateElement("NetPath");
            XmlElement httpServerPath = xmlDoc.CreateElement("HttpServerPath");
            //httpServerPath.InnerText = "1aa";
            httpServerPath.SetAttribute("HttpPath", "www");
            httpServerPath.SetAttribute("Other", "aaa");

            XmlElement monitorPath = xmlDoc.CreateElement("MonitorPath");
            monitorPath.SetAttribute("Path", "www.ww");
            monitorPath.SetAttribute("bodyJson", "sasada");
            root.AppendChild(httpServerPath);
            root.AppendChild(monitorPath);
            xmlDoc.AppendChild(root);
            xmlDoc.Save(path);
            Debug.Log("XML保存成功" + path);
        }
    }

    public static string ReadStringFromStreamingAssets(string path)
    {
        if (path != null) //如果有路径，则为本地读取
        {
            string p = Application.streamingAssetsPath+"/"+ path;
            string localJson = File.ReadAllText(p);
            return localJson;

        }
        return string.Empty;
    }
    /// <summary>
    /// 读取Json泛型方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static T ReadJsonForJsonUtility<T>(string json, string path = null)
    {
        if (path != null) //如果有路径，则为本地读取
        {
            string localJson = File.ReadAllText(path);
            json = localJson;

        }
        Debug.Log(json);
        T resT = JsonUtility.FromJson<T>(json.Trim());
        return resT;
    }

    public static T ReadJsonForLitJson<T>(string json, string path = null)
    {
        if (path != null) //如果有路径，则为本地读取
        {
            string localJson = File.ReadAllText(path);
            json = localJson;

        }
        Debug.Log(json);
        //T resT = JsonUtility.FromJson<T>(json.Trim());
        T resT = JsonMapper.ToObject<T>(json);
        return resT;
    }
    public static T ReadJsonForLitJson<T>(JsonData json)
    {
        //T resT = JsonUtility.FromJson<T>(json.Trim());
        T resT = JsonMapper.ToObject<T>(json.ToJson());
        return resT;
    }
    /// <summary>
    /// 泛型类将Json转换为需要用到的对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    public static T ReadJson<T>(string json)
    {
        T resT = JsonUtility.FromJson<T>(json.Trim());
        return resT;
    }



    /// <summary>
    /// 将json写入到本地,替换原有Json
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <param name="path"></param>
    public void WriteAllJsonTo_streamingAssetsPath(string json, string path)
    {
        path = Application.streamingAssetsPath + path;
        if (!File.Exists(path))
        {
            FileStream fs = File.Create(path);
            fs.Close();
        }
        if (File.ReadAllText(path) == json)
        {
            Debug.Log(string.Format("json相同，无需更新"));
            return;
        }
        File.WriteAllText(path, json);
    }
    /// <summary>
    /// 写入一行，如果有相同的就替换
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <param name="path"></param>
    /// <param name="SameLinecondition"></param>
    public void WriteJsonLineTo_streamingAssetsPath<T>(string json, string path, Func<T, T, bool> SameLinecondition, bool replace = true)
    {
        path = Application.streamingAssetsPath + path;
        if (!File.Exists(path))
        {
            FileStream fs = File.Create(path);
            fs.Close();
        }
        List<string> allLinesNew = new List<string>();

        string[] allLines = File.ReadAllLines(path);
        allLinesNew.AddRange(allLines);

        for (int i = 0; i < allLines.Length; i++)
        {
            int index = i;
            T t_Old;
            try
            {
                t_Old = JsonMapper.ToObject<T>(allLines[index]);
                T jsonT = JsonMapper.ToObject<T>(json);
                if (SameLinecondition(t_Old, jsonT))
                {
                    if (replace)//替换
                    {
                        allLinesNew[index] = json;
                        //allLinesNew.AddRange(allLines);

                    }
                    else//否则删除
                    {
                        allLinesNew.RemoveAt(index);
                    }
                    File.WriteAllText("null", path);
                    File.WriteAllLines(path, allLinesNew, System.Text.Encoding.UTF8);
                    return;

                }
            }
            catch (Exception)
            {

                Debug.Log("解析" + allLines[index] + "失败");
            }
        }

        allLinesNew.Add(json);
        File.WriteAllText("null", path);
        File.WriteAllLines(path, allLinesNew, System.Text.Encoding.UTF8);
    }
    /// <summary>
    /// 根据JsonData的简直类型解析得到字典
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Q"></typeparam>
    /// <param name="jsonData"></param>
    /// <returns></returns>
    public static Dictionary<string, string> ReadJsonDataForKeyValue(JsonData jsonData)
    {
        IDictionary dictionary = jsonData as IDictionary;
        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
        foreach (string item in dictionary.Keys)
        {
            Debug.Log(item + ":" + dictionary[item].GetType());
            keyValuePairs.Add(item, dictionary[item].ToString());
        }
        return keyValuePairs;
    }
    /// <summary>
    /// 通过Litjson将Json转化为string
    /// </summary>
    /// <param name="o"></param>
    /// <returns></returns>
    public static string ObjToJsonForLitjson(object o)
    {
       return JsonMapper.ToJson(o);
    }
    /// <summary>
    ///  通过Newtonsoft.Json将对象转换为Json，其中为空的字段自动剔除
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    public static string ObjToJsonForNetjson(object response)
    {
        var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
        var json = JsonConvert.SerializeObject(response, Newtonsoft.Json.Formatting.Indented, jSetting);
        return json;
    }
    /// <summary>
    /// Unicode转字符串
    /// </summary>
    /// <param name="source">经过Unicode编码的字符串</param>
    /// <returns>正常字符串</returns>
    public static string Unicode2String(string source)
    {
        return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                     source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
    }
    /// <summary>
    /// <summary>
    /// 字符串转Unicode
    /// </summary>
    /// <param name="source">源字符串</param>
    /// <returns>Unicode编码后的字符串</returns>
    public static string String2Unicode(string source)
    {
        byte[] bytes = Encoding.Unicode.GetBytes(source);
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < bytes.Length; i += 2)
        {
            stringBuilder.AppendFormat("\\u{0}{1}", bytes[i + 1].ToString("x").PadLeft(2, '0'), bytes[i].ToString("x").PadLeft(2, '0'));
        }
        return stringBuilder.ToString();
    }
    /// <summary>
    /// 通过Litjson将Json转化为string
    /// </summary>
    /// <param name="o"></param>
    /// <returns></returns>
    public static string ObjToJsonForJsonUtility(object o)
    {
        return JsonUtility.ToJson(o);
    }
}



