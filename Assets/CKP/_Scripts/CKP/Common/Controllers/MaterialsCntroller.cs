using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialsCntroller : BaseController
{
    #region//材质球路径相关
    private const string path = "Materials/";

    public const string Red = "Red";

    public const string Black = "Black";

    public const string Blue = "Blue";
    public const string Green = "Green";
    public const string Purple = "Purple";
    public const string White = "White";
    public const string Yellow = "Yellow";
    #endregion
   
     /// <summary>
     /// 存放所有材质球的字典用于通过名字查找对应材质球
     /// </summary>
    private Dictionary<string, Material> allMaterialsDic=new Dictionary<string, Material>();

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="gameFacade"></param>
    public MaterialsCntroller(GameFacade gameFacade) : base(gameFacade) { }
   


  
    /// <summary>
    /// 初始化
    /// </summary>
    public override void OnInit()
    {
        base.OnInit();
        SetListAndDic();
    }

    /// <summary>
    /// 根据路径从Resources文件夹加载材质球
    /// </summary>
    /// <param name="path"></param>
    private Material LoadMatrrialFromResources(string materialName)
    {
        Material m = Resources.Load<Material>(path+materialName);
        if (m==null)
        {
            Debug.LogWarning(string.Format("在{0}路径下没有找到对应材质球", path));
        }
        return m;
    }

   
    /// <summary>
    /// 给装材质球的列表和字典赋值
    /// </summary>
    private void SetListAndDic()
    {
        allMaterialsDic.Clear();
        allMaterialsDic.Add(Red, LoadMatrrialFromResources(Red));
        allMaterialsDic.Add(Black, LoadMatrrialFromResources(Black));
        allMaterialsDic.Add(Blue, LoadMatrrialFromResources(Blue));
        allMaterialsDic.Add(Green, LoadMatrrialFromResources(Green));
        allMaterialsDic.Add(Purple, LoadMatrrialFromResources(Purple));
        allMaterialsDic.Add(White, LoadMatrrialFromResources(White));
        allMaterialsDic.Add(Yellow, LoadMatrrialFromResources(Yellow));
    }
    /// <summary>
    /// 清除的时候运行
    /// </summary>
    public override void OnDestory()
    {
        base.OnDestory();
    }
   
    /// <summary>
    /// 通过名称得到材质球
    /// </summary>
    /// <param name="materialName"></param>
    /// <returns></returns>
    public Material GetMaterialForName(string materialName)
    {
        Material m;
        if (!allMaterialsDic.TryGetValue(materialName, out m))
        {
            Debug.LogError(string.Format("找不到对应名称：{0}的材质球", materialName));
        }
        return m;
        
    }
}
