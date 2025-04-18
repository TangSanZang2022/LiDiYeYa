using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hydrexia.CKP
{
    /// <summary>
    /// 动画事件
    /// </summary>
    public class AniEvent : MonoBehaviour
    {
        /// <summary>
        /// 设置显示隐藏物体
        /// </summary>
        /// <param name="showHideObjIDList"></param>
        public void SetShowHideObj(string showHideObjIDList)
        {
            Debug.Log(showHideObjIDList);
            string[] idArray = showHideObjIDList.Split("|".ToCharArray());

            for (int i = 0; i < idArray.Length; i++)
            {
                int index = i;
                if (index==0&& idArray[index]!="null")//要显示的所有物体ID
                {
                    string[] showIdArray = idArray[index].Split("&".ToCharArray());
                    for (int j = 0; j < showIdArray.Length; j++)
                    {
                        int showIndex = j;
                        if (!string.IsNullOrEmpty(showIdArray[showIndex]))
                        {
                            GameFacade.Instance.ShowObjForID(showIdArray[showIndex]); 
                        }
                    }
                }
                else//要隐藏的所有物体ID
                {
                    if (idArray[index] != "null")//要隐藏的所有物体ID
                    {
                        string[] hideIdArray = idArray[index].Split("&".ToCharArray());
                        for (int j = 0; j < hideIdArray.Length; j++)
                        {
                            int hideIndex = j;
                            if (!string.IsNullOrEmpty(hideIdArray[hideIndex]))
                            {
                                GameFacade.Instance.HideObjForID(hideIdArray[hideIndex]); 
                            }
                        }
                    }
                }
            }
        }

        public void ShowOnlyObj(string id)
        {
            GameFacade.Instance.ShowOnlyObj(id);
        }
        public void HideOnlyObj(string id)
        {
            GameFacade.Instance.HideOnlyObj(id);
        }
        public void MoveCam(string posID)
        {
            
            GameFacade.Instance.SetTarnsToPos(posID, Camera.main.transform);
        }
       
    }
}