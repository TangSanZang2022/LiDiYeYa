using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 namespace LiDi.CKP
{
    /// <summary>
    /// 开始场景初始化
    /// </summary>
    public class StartSceneInit : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GameFacade.Instance.PushPanel(UIPanelType.StartSceneMainPanel);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}