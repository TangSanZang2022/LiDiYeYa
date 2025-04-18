using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LiDi.CKP
{
    /// <summary>
    /// 靠近类型的热点
    /// </summary>
    public class CloseToHotPoint : BaseHotPoint
    { protected override void OnEnable()
        {
            base.OnEnable();
        }
        protected override void Awake()
        {
            base.Awake();
        }
        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            
        }

        public override void ResetBeforeNextStep()
        {
            base.ResetBeforeNextStep();
        }

       
        protected override void OnMouseDown()
        {
           
        }

        protected override void OnMouseEnter()
        {
           
        }
        protected override void OnMouseExit()
        {
           
        }
        protected override void OnValidate()
        {
            
        }

        private void OnTriggerEnter(Collider other)
        {
            print(other.name);
            if (other.name== "FPSController")
            {
                GameFacade.Instance.Set_SelectedHotPoint(this);

                Set_showObjAfterClick_State(true);

                GameFacade.Instance.AddOperatedHotPointTo_operatedHotPointList(this);
                Operability = false;
                print("添加");


            }
        }

        
    }
}