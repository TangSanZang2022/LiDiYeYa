using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace LiDi.CKP
{
    /// <summary>
    /// 加氢机屏幕
    /// </summary>
    public class HydrogenScreen : DeviceScreen
    {

        private Text flowText;
        /// <summary>
        /// 
        /// </summary>
        protected Text FlowText
        {
            get
            {
                if (flowText == null)
                {
                    flowText = transform.FindChildForName<Text>("FlowText");
                }
                return flowText;
            }
        }
        private Text pressureText;
        /// <summary>
        /// 
        /// </summary>
        protected Text PressureText
        {
            get
            {
                if (pressureText == null)
                {
                    pressureText = transform.FindChildForName<Text>("PressureText");
                }
                return pressureText;
            }
        }
        private Text moneyText;
        /// <summary>
        /// 
        /// </summary>
        protected Text MoneyText
        {
            get
            {
                if (moneyText == null)
                {
                    moneyText = transform.FindChildForName<Text>("MoneyText");
                }
                return moneyText;
            }
        }
        private Text unitPriceText;
        /// <summary>
        /// 
        /// </summary>
        protected Text UnitPriceText
        {
            get
            {
                if (unitPriceText == null)
                {
                    unitPriceText = transform.FindChildForName<Text>("UnitPriceText");
                }
                return unitPriceText;
            }
        }
        public override void ChangeToTargetValue()
        {
            
            base.ChangeToTargetValue();
            StartCoroutine(IChangeToTargetValue());
        }

        public override void ResetToStartValue()
        {
            base.ResetToStartValue();
        }

        IEnumerator IChangeToTargetValue()
        {
            float targetValueFloat = float.Parse(targetValue.Remove(targetValue.Length - 1));
            float money = 0;
            float pressure = 0;
            float flow = 0;
            while (money< targetValueFloat)
            {
                FlowText.text = flow.ToString("f2");// + "kg/min";
                MoneyText.text = money.ToString("f2");// + "元";
                PressureText.text = pressure.ToString("f2");// + "MPa";
                yield return new WaitForSeconds(0.5f);
                money += 10;
                flow = UnityEngine.Random.Range(0.1f, 3.6f);
                pressure += 35 / (targetValueFloat / 10);
            }
             money = targetValueFloat;
             pressure = 0;
             flow = 0;
            FlowText.text = flow.ToString("f2");// + "kg/min";
            MoneyText.text = money.ToString("f2");// + "元";
            PressureText.text = pressure.ToString();// + "MPa";


        }

    }
}