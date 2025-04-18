using System.Collections;
using System.Collections.Generic;
using Tools.CKP;
using UnityEngine;
namespace LiDi.CKP
{
    public class PracticalTrainingScene_Test : MonoBehaviour
    {
        public bool isAppraisal;
        // Start is called before the first frame update
        void Start()
        {
            if (isAppraisal)
            {
                TimerTools.Instance.StartToWaitDoSomething(2, () => GameFacade.Instance.StartTask(0, GameMode.Appraisal));
               
            }
            else
            {
                TimerTools.Instance.StartToWaitDoSomething(2, () => GameFacade.Instance.StartTask(0, GameMode.Training));
                
            }
            
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}