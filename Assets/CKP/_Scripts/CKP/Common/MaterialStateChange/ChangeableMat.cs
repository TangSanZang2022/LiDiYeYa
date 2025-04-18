using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ChangeableMat : MonoBehaviour
{
    public List<Material> allNormalMats = new List<Material>();

    public List<Material> allNewMats = new List<Material>();

    List<Tweener> tweeners = new List<Tweener>();

    MeshRenderer renderer;
    /// <summary>
    /// 正在变透明
    /// </summary>
    private bool isFadeing;

    /// <summary>
    /// 正在变为原来的颜色
    /// </summary>
    private bool isReturning;
    // Start is called before the first frame update
    void Start()
    {

        renderer = GetComponent<MeshRenderer>();
        allNormalMats.AddRange(renderer.materials);

        for (int i = 0; i < allNormalMats.Count; i++)
        {
            int index = i;
            Material m_New = new Material(allNormalMats[index]);
            //m_New.color = new Color(0, 0, 0, 0.1f);
            m_New.SetMaterialRenderingMode(RenderingMode.Transparent);
            allNewMats.Add(m_New);
        }


        //for (int i = 0; i < GetComponent<MeshRenderer>().materials.Length; i++)
        //{
        //    int index = i;
        //    GetComponent<MeshRenderer>().materials[index] = allNewMats[index];
        //}
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    //renderer.material = allNewMats[0];
        //    //tweeners.Add(renderer.material.DOColor(new Color(0, 0, 0, 0.5f), 5));
        //    Fade();
        //}
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    //renderer.material = allNormalMats[0];
        //    Return();
        //}
    }
    /// <summary>
    /// 销毁所有Dotween
    /// </summary>
    private void KillAll_tweeners()
    {
        for (int i = 0; i < tweeners.Count; i++)
        {
            int index = i;
            tweeners[index].Kill();
        }

    }
    /// <summary>
    /// 逐渐变透明
    /// </summary>
    /// <param name="alpha"></param>
    /// <param name="fadeTime"></param>
    public void Fade(float alpha = 0.5f, float fadeTime = 5f)
    {
        if (isFadeing)
        {
            return;
        }
        isFadeing = true;
        isReturning = false;
        KillAll_tweeners();
        if (renderer.materials.Length == 1)
        {
            renderer.material = allNewMats[0];
            Color normalColor = renderer.material.color;
            tweeners.Add(renderer.material.DOColor(new Color(normalColor.r, normalColor.g, normalColor.b, alpha), fadeTime).OnComplete(()=>isFadeing=false));
        }
        if (renderer.materials.Length > 1)
        {
            renderer.materials = allNewMats.ToArray();
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                int index = i;
                renderer.materials[index] = allNewMats[index];
                
                Color normalColor = renderer.material.color;
                if (index == renderer.materials.Length-1)
                {
                    tweeners.Add(renderer.materials[index].DOColor(new Color(normalColor.r, normalColor.g, normalColor.b, alpha), fadeTime).OnComplete(() => isFadeing = false));
                }
                else
                {
                    tweeners.Add(renderer.materials[index].DOColor(new Color(normalColor.r, normalColor.g, normalColor.b, alpha), fadeTime));
                }
                
            }
        }

    }
    /// <summary>
    /// 还原为原来的颜色
    /// </summary>
    /// <param name="returnTime"></param>
    public void Return(float returnTime = 5f)
    {
        if (isReturning)
        {
            return;
        }
        isFadeing = false;
        isReturning = true;
        KillAll_tweeners();
        if (renderer.materials.Length == 1)
        {
            //renderer.material = allNormalMats[0];
            Color normalColor = allNormalMats[0].color;
            tweeners.Add(renderer.material.DOColor(new Color(normalColor.r, normalColor.g, normalColor.b, normalColor.a), returnTime).OnComplete(() => { isReturning = false; renderer.material = allNormalMats[0]; }));
        }
        if (renderer.materials.Length > 1)
        {
            //renderer.materials = allNormalMats.ToArray();
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                int index = i;
                //renderer.materials[index] = allNormalMats[index];
                Color normalColor = allNormalMats[index].color;
                if (index == renderer.materials.Length - 1)
                {
                    tweeners.Add(renderer.material.DOColor(new Color(normalColor.r, normalColor.g, normalColor.b, normalColor.a), returnTime).OnComplete(() => { isReturning = false; renderer.materials = allNormalMats.ToArray(); }));
                }
                else
                {
                    tweeners.Add(renderer.material.DOColor(new Color(normalColor.r, normalColor.g, normalColor.b, normalColor.a), returnTime));
                }

            }
        }
    }
}
