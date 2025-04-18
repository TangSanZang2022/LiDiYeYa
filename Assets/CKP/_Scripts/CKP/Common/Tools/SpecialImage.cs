using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/// <summary>
/// 不规则按钮类
/// </summary>
public class SpecialImage : Image,IPointerEnterHandler,IPointerExitHandler
{
    Transform ShowImage;
    PolygonCollider2D polygonCollider;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (ShowImage != null)
        {
            ShowImage.gameObject.SetActive(false);
        }
    }
    protected override void Awake()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
        ShowImage = transform.Find("ShowImage");
        if (ShowImage!=null)
        {
            ShowImage.gameObject.SetActive(false);
        }
    }

    public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        bool inside = polygonCollider.OverlapPoint(screenPoint);
        return inside;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        if (ShowImage != null)
        {
            ShowImage.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ShowImage != null)
        {
            ShowImage.gameObject.SetActive(false);
        }
    }
}
