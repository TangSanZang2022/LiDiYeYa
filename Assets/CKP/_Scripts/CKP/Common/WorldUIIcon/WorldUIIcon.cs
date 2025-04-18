using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 展示物体名称的世界UI
/// </summary>
public class WorldUIIcon : MonoBehaviour
{
   
    private Text nameText;
    /// <summary>
    /// 展示名称的Text
    /// </summary>
    private Text NameText
    {
        get
        {
            if (nameText==null)
            {
                nameText = transform.Find("Canvas/BGImage/Text").GetComponent<Text>();
            }
            return nameText;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform);//始终看向相机
    }

    /// <summary>
    /// 更新展示的名称
    /// </summary>
    /// <param name="name"></param>
    public void UpdateNameText(string name)
    {
        NameText.text = name;
    }
}
