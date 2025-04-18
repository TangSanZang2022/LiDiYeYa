using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 接收到服务器信息后，响应控制器
/// </summary>
public class RequestController : BaseController
{
    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="gameFacade"></param>
    public RequestController(GameFacade gameFacade) : base(gameFacade)
    { 
    
    }
    /// <summary>
    /// 存放所有响应类型的字典
    /// </summary>
    private Dictionary<ActionCode, BaseRequest> requestDict = new Dictionary<ActionCode, BaseRequest>();
    /// <summary>
    /// 添加响应到字典
    /// </summary>
    /// <param name="actionCode"></param>
    /// <param name="baseRequest"></param>
    public void AddRequest(ActionCode actionCode,BaseRequest baseRequest)
    {
        requestDict.Add(actionCode, baseRequest);
    }

    /// <summary>
    /// 从字典中移出对应响应
    /// </summary>
    /// <param name="actionCode"></param>
    public void removeRequest(ActionCode actionCode)
    {
        requestDict.Remove(actionCode);
    }
    /// <summary>
    /// 处理响应
    /// </summary>
    /// <param name="actionCode"></param>
    /// <param name="data"></param>
    public void HandleResponse(ActionCode actionCode,string data)
    {
        BaseRequest request;
        if (requestDict.TryGetValue(actionCode, out request))
        {
            Debug.Log(string.Format("找到了ActionCode[{0}]对应的Request", actionCode));
            request.OnResponce(data);
        }
        else
        {
            Debug.LogError(string.Format("没有找到ActionCode[{0}]对应的Request", actionCode));
        }
    }
    public override void OnInit()
    {
        base.OnInit();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnDestory()
    {
        base.OnDestory();
    }
}
