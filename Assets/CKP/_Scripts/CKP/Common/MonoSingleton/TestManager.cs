using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
namespace MonoSingleton
{
    public class TestManager : MonoSingleton<TestManager>
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public override void Init()
        {
            Debug.Log("初始化" + typeof(TestManager));
        }

        public void Func()
        {
            Debug.Log("Fun");
        }
    }
}