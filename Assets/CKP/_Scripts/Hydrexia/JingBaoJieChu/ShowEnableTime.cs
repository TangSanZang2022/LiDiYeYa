using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShowEnableTime : MonoBehaviour
{

    private void OnEnable()
    {
      GetComponent<Text>().text=  DateTime.Now.ToString("yyyy/MM/dd   HH:mm:ss");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
