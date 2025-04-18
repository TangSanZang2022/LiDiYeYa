using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 /// <summary>
 /// 更新物体位置
 /// </summary>
public class UpdateObjPosRequest : BaseRequest
{
    private bool isSyncUpdateObjPos = false;
    /// <summary>
    /// 接收的数据
    /// </summary>
    private string data;
    protected override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.UpdateObjPos;
        Debug.Log(string.Format("创建{0}成功", actionCode.ToString()));
        base.Awake();
    }

    private void Update()
    {
        if (isSyncUpdateObjPos)
        {
            //gameFacade.UpdateAllMoveObjPos(data);
            isSyncUpdateObjPos = false;
        }  
    }
    public override void OnResponce(string data)
    {
        base.OnResponce(data);
        this.data = data;
        isSyncUpdateObjPos = true;//异步更新物体位置
    }
}
