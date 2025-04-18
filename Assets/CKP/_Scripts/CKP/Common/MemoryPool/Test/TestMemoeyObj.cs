using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMemoeyObj : BaseMemoryObj
{
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="obj"></param>
    protected override void Init(object[] obj=null)
    {
        transform.localPosition = (Vector3)obj[0];
        transform.localRotation = Quaternion.Euler((Vector3)obj[1]);
        transform.localScale = (Vector3)obj[2];
    }

    public override void InPool()
    {
        base.InPool();
    }

    public override void OutPool(object[] obj)
    {
        base.OutPool(obj);
    }

}
