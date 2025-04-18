using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LiDi.CKP
{
    /// <summary>
    /// 动画播放控制器
    /// </summary>
    public class AnimatorPlayerController : BaseController
    {
        public AnimatorPlayerController(GameFacade gameFacade) : base(gameFacade)
        { }
        /// <summary>
        /// 所有动画
        /// </summary>
        List<HydrexiaAni> allAnimators = new List<HydrexiaAni>();

        public override void OnInit()
        {
            base.OnInit();
            Set_allAnimators();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnDestory()
        {
            base.OnDestory();
        }
        /// <summary>
        /// 将所有动画添加到列表
        /// </summary>
        private void Set_allAnimators()
        {
            allAnimators.AddRange(GameObject.FindObjectsOfType<HydrexiaAni>());
        }

        /// <summary>
        /// 将动画添加到列表
        /// </summary>
        /// <param name="hydrexiaAni"></param>
        public void Add_allAnimators(HydrexiaAni hydrexiaAni)
        {
            if (!allAnimators.Contains(hydrexiaAni) )
            {
                allAnimators.Add(hydrexiaAni);
            }
           
        }
        /// <summary>
        /// 根据动画名称播放动画
        /// </summary>
        /// <param name="aniNameList"></param>
        public void PlayAniForAniNameList(List<string> aniNameList)
        {
            for (int i = 0; i < aniNameList.Count; i++)
            {
                int index = i;
                for (int j = 0; j < allAnimators.Count; j++)
                {
                    int indexAnimators = j;
                    allAnimators[indexAnimators].PlayAni(aniNameList[index]);
                }
            }
        }
        /// <summary>
        /// 重置所有动画，即让所有动画播放Idel动画
        /// </summary>
        public void ResetAllAni()
        {
            for (int i = 0; i < allAnimators.Count; i++)
            {
                int index = i;
                allAnimators[index].gameObject.SetActive(true);
                allAnimators[index].PlayAni("Idel");
            }
        }
    }
}