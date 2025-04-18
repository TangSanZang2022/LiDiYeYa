using DFDJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
 /// <summary>
 /// 要创建屏幕ICon的枪型摄像头
 /// </summary>
public class GunMonitorScreenIconObj : BaseScreenIconObj, IScreenIconHandle
{
    MyCullingGroup myCullingGroup;
    // Start is called before the first frame update
   protected override void Start()
    {
       base.Start();
        myCullingGroup = GetComponent<MyCullingGroup>();
        if (myCullingGroup == null)
        {
            myCullingGroup = GetComponentInChildren<MyCullingGroup>();
        }
        myCullingGroup.SeeObjOnDistance += delegate { baseScreenIcon.gameObject.SetActive(true); };
        myCullingGroup.LoseSightObjOnDistance += delegate { baseScreenIcon.gameObject.SetActive(false); };
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    /// <summary>
    /// 鼠标点击
    /// </summary>
    public void OnMouseClickIcon()
    {
       BasePos baseCamPos = GameFacade.Instance.GetCurrentCamPos();
        if (baseCamPos != null)
        {
            IAnimControl animControl = baseCamPos.GetComponent<IAnimControl>();
            if (animControl!=null)
            {
                animControl.Pause();
            }
          
        }
        //GetComponent<GunMonitor>().MonitorOn();
    }
    /// <summary>
    /// 鼠标进入
    /// </summary>
    public void OnMouseEnterIcon()
    {
        
    }
    /// <summary>
    /// 鼠标移出
    /// </summary>
    public void OnMouseExitIcon()
    {
        
    }

    protected override void OnBecameVisible()
    {
       // base.OnBecameVisible();
    }

    protected override void OnBecameInvisible()
    {
        //base.OnBecameInvisible();
    }

    
}
