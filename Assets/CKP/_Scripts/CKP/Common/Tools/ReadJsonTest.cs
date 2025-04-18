using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadJsonTest : MonoBehaviour
{
    [TextArea (1,20)]
    public string strTest;
    // Start is called before the first frame update
    void Start()
    {
       
    }
    private void OnGUI()
    {
        if (GUI.Button(new Rect(100,100,100,50),"解析Json"))
        {
            Root root = XmlController.ReadJsonForLitJson<Root>(strTest);
            Debug.Log(root.Dates[0].Chart_y[0]);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
public class Dates
{
    /// <summary>
    /// 
    /// </summary>
    public string Chart_x { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<double> Chart_y { get; set; }
}

public class Root
{
    /// <summary>
    /// 
    /// </summary>
    public List<Dates> Dates { get; set; }
}
