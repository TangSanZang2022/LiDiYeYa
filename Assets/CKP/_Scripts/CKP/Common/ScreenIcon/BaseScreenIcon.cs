using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// 物体的屏幕UI标识
/// </summary>

public class BaseScreenIcon : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// 
    /// </summary>
    private IScreenIconHandle screenIconHandle;

    /// <summary>
    /// 此Icon对应的物体，用于实时同步屏幕坐标位置
    /// </summary>
    private Transform targetObj;
    // Start is called before the first frame update
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (targetObj==null)
        {
            return;
        }
        transform.position = Camera.main.WorldToScreenPoint(targetObj.position);//实时更新Icon位置
    }
    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void OnInit()
    {

    }
    /// <summary>
    /// 设置UI对应的物体
    /// </summary>
    /// <param name="targetObj"></param>
    public void SetTargetObj(Transform targetObj)
    {
        this.targetObj = targetObj;

    }
    /// <summary>
    /// 设置鼠标操作之后的事件接口
    /// </summary>
    /// <param name="screenIconHandle"></param>
    public void SetIScreenIconHandle(IScreenIconHandle screenIconHandle)
    {
        this.screenIconHandle = screenIconHandle;
    }
    /// <summary>
    /// 鼠标点击
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (screenIconHandle==null)
        {
            return;
        }
        screenIconHandle.OnMouseClickIcon();
    }
    /// <summary>
    /// 鼠标进入
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (screenIconHandle == null)
        {
            return;
        }
        screenIconHandle.OnMouseEnterIcon();
    }
    /// <summary>
    /// 鼠标移出
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (screenIconHandle == null)
        {
            return;
        }
        screenIconHandle.OnMouseExitIcon();
    }
    /// <summary>
    /// 设置ICon数据
    /// </summary>
    /// <param name="data"></param>
    public virtual void SetIconData(object data)
    {

    }

}
