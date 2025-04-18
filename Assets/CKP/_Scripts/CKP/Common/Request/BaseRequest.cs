using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 请求的基类
/// </summary>
public class BaseRequest : MonoBehaviour
{
    protected RequestCode requestCode = RequestCode.None;
    protected ActionCode actionCode = ActionCode.None;
    private GameFacade _gameFacade;
    protected GameFacade gameFacade
    {
        get
        {
          
                _gameFacade = GameFacade.Instance;
            
            return _gameFacade;
        }
    }
    protected virtual void Awake()
    {
        gameFacade.AddRequest(actionCode, this);
    }
    /// <summary>
    /// 发送请求
    /// </summary>
    /// <param name="data"></param>
    public virtual void SendRequest(string data)
    {
        gameFacade.SendRequest(data); 
    }

    /// <summary>
    /// 响应
    /// </summary>
    /// <param name="data"></param>
    public virtual void OnResponce(string data)
    { 
    
    }

    public virtual void OnDestroy()
    {
        if (gameFacade!=null)
        {
            gameFacade.RemoveRequest(actionCode);
        }
    }



}
