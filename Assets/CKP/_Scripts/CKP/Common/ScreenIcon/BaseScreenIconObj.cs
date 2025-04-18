using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 可以创建屏幕Icon的物体
/// </summary>
public class BaseScreenIconObj : MonoBehaviour
{
    [SerializeField]
    /// <summary>
    /// 此物体屏幕Icon预制体
    /// </summary>
    protected BaseScreenIcon iconPrefab;
    [SerializeField]
    /// <summary>
    /// 此物体屏幕Icon预制体路径
    /// </summary>
    protected string iconPrefabPath;
    [SerializeField]
    /// <summary>
    /// 创建出来的Icon父物体
    /// </summary>
    protected Transform parentTrans;
    /// <summary>
    /// 创建出来的Icon
    /// </summary>
    public BaseScreenIcon baseScreenIcon
    {
        get;
        protected set;
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        baseScreenIcon = CreateIcon();
        InitIcon();
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    /// <summary>
    /// 创建icon
    /// </summary>
    /// <returns></returns>
    private BaseScreenIcon CreateIcon()
    {
        if (iconPrefab!=null)
        {
            return Instantiate(iconPrefab, parentTrans);
        }
        return Instantiate(Resources.Load<BaseScreenIcon>(iconPrefabPath), parentTrans);
    }

    /// <summary>
    /// 初始化Icon
    /// </summary>
    private void InitIcon()
    {
        baseScreenIcon.OnInit(); //初始化
        Transform UITargetTrans = transform.FindChildForName("UITargetTrans");
        if (UITargetTrans==null)
        {
            UITargetTrans = transform;
        }
        baseScreenIcon.SetTargetObj(UITargetTrans);
        baseScreenIcon.SetIScreenIconHandle(GetComponent<IScreenIconHandle>());
    }
    /// <summary>
    /// 当物体在摄像机视野以外
    /// </summary>
    protected virtual void OnBecameInvisible()
    {
        if (baseScreenIcon!=null)
        {
            baseScreenIcon.gameObject.SetActive(false);
        }
       
    }
    /// <summary>
    /// 在摄像机视野内
    /// </summary>
    protected virtual void OnBecameVisible()
    {
        baseScreenIcon.gameObject.SetActive(true);
    }
}
