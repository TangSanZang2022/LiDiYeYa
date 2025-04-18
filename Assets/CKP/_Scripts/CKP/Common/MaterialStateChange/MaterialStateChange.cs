using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialStateChange : MonoBehaviour
{
  public  List<ChangeableMat> allChildMaterialList = new List<ChangeableMat>();
    // Start is called before the first frame update
    void Start()
    {
        List<MeshRenderer> meshRenderers = new List<MeshRenderer>();
        meshRenderers.AddRange(transform.GetComponentsInChildren<MeshRenderer>());
        for (int i = 0; i < meshRenderers.Count; i++)
        {
            int index = i;
            ChangeableMat changeableMat = meshRenderers[index].gameObject.GetComponent<ChangeableMat>();
            if (changeableMat==null)
            {
                changeableMat = meshRenderers[index].gameObject.AddComponent<ChangeableMat>();
            }
            allChildMaterialList.Add(changeableMat);
        }
    }
    /// <summary>
    /// 变透明
    /// </summary>
    /// <param name="alpha"></param>
    /// <param name="fadeTime"></param>
    public void Fade(float alpha = 0.5f, float fadeTime = 5f)
    {
        for (int i = 0; i < allChildMaterialList.Count; i++)
        {
            int index = i;
            allChildMaterialList[index].Fade(alpha, fadeTime);
        }
    }
    /// <summary>
    /// 变回原来的颜色
    /// </summary>
    /// <param name="returnTime"></param>
    public void Return(float returnTime = 5f)
    {
        for (int i = 0; i < allChildMaterialList.Count; i++)
        {
            int index = i;
            allChildMaterialList[index].Return(returnTime);
        }
    }
}
