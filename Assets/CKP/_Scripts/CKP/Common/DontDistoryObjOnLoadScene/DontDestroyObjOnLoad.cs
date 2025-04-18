using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyObjOnLoad : MonoBehaviour
{    //不需要销毁的物体是否存在
    private static bool origional = true;
    private void Awake()
    {
        if (origional)
        {
            origional = false;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
       // DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
