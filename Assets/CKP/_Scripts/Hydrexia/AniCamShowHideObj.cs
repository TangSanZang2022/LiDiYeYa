using ShowHideObj;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LiDi.CKP
{
    public class AniCamShowHideObj : BaseShowHideObj
    {
        public override void Show()
        {
            base.Show();
            transform.GetChild(0).gameObject.SetActive(true);
            transform.position = GameObject.Find("FirstPersonCharacter").transform.position;
            transform.rotation = GameObject.Find("FirstPersonCharacter").transform.rotation;
            GameFacade.Instance.SetTarnsToPos(ObjIDTool.QingQiLiuXIangXIangJiPos, transform);
        }

        public override void Hide()
        {
            base.Hide();
            transform.GetChild(0).gameObject.SetActive(false);
        }

        
    }
}