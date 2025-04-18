using HighlightingSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityOprationalObj;

namespace DFDJ
{
    /// <summary>
    /// 机床设备高亮
    /// </summary>
    public class EquipmentHighlighterObj : HighlighterObj
    {
        /// <summary>
        /// 可操作物体
        /// </summary>
        private OperationalObj operationalObj;
        protected override void Start()
        {
            base.Start();
            operationalObj = GetComponent<OperationalObj>();

            if (operationalObj == null)
            {
                Debug.Log(string.Format("{0}物体的HighlighterObj无法获取OperationalObj，已自动添加", name));
                operationalObj = gameObject.AddComponent<OperationalObj>();
            }
            operationalObj.MouseEnterHandleAction += () => {  GetComponent<Highlighter>().FlashingOn(color1, color2); isHighLighting = true; };
            operationalObj.MouseExitHandleAction += () => { GetComponent<Highlighter>().FlashingOff(); isHighLighting = false; };
            operationalObj.MouseOverHandleAction += () =>
            {
            //if (!isHighLighting)
            //{
            //    if (GetComponent<BaseEquipment>().isAtBestViewPos || (GetComponent<BaseEquipment>() != null && GetComponent<BaseEquipment>().CanClickFPS)) { GetComponent<Highlighter>().FlashingOn(color1, color2); isHighLighting = true; };
            //}
            //else
            //{
            //    if (GetComponent<BaseEquipment>() != null &&!GetComponent<BaseEquipment>().CanClickFPS&& !GetComponent<BaseEquipment>().isAtBestViewPos)
            //    {
            //        GetComponent<Highlighter>().FlashingOff();
            //        isHighLighting = false;
            //    }
            //}

            //2023.02.16修改，最佳视角下无法点击机床
            if (!isHighLighting)
                {
                    isHighLighting = true; };
               
            };


        }
        protected override void OnValidate()
        {
            base.OnValidate();
        }

      
    }
}