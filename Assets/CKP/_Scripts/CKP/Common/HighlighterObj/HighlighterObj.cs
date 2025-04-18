using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightingSystem;
using System;
using UnityOprationalObj;
using DFDJ;
using Tool;
/// <summary>
/// 当物体挂载此脚本时，鼠标放到此物体上，物体会高亮
/// </summary>
public class HighlighterObj : HighlighterBase
{
    protected bool isHighLighting;
  
    [SerializeField]
    /// <summary>
    /// 闪烁颜色1
    /// </summary>
    protected Color color1 = Color.red; 
    [SerializeField]
    /// <summary>
    /// 闪烁颜色2
    /// </summary>
    protected Color color2 = Color.green;

    protected override void Start()
    {
        base.Start();
       

    }
    protected override void OnValidate()
    {
        base.OnValidate();
    }

    public bool IsHighLighting()
    {
        return isHighLighting;
    }
    


}
