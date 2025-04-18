using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityOprationalObj;
/// <summary>
/// 当物体挂载此脚本时，当鼠标放在物体上的时候创建介绍UI
/// </summary>
public class CreateIntroductionIcon : MonoBehaviour
{
    /// <summary>
    /// 介绍UI
    /// </summary>
    private WorldUIIcon worldUIIcon;

    /// <summary>
    /// 可操作的物体
    /// </summary>
    private OperationalObj operationalObj;

    /// <summary>
    /// 物体名称
    /// </summary>
    private string objName;
    // Start is called before the first frame update
    void Start()
    {
        operationalObj = GetComponent<OperationalObj>();
        if (operationalObj == null)
        {
            Debug.Log(string.Format("{0}物体上的CreateIntroductionIcon中无法获取OperationalObj对象，请检查", name));
            operationalObj = gameObject.AddComponent<OperationalObj>();
        }
        objName = operationalObj.GetObjName();
        operationalObj.MouseEnterHandleAction += () => MouseEnterHandle();
        operationalObj.MouseExitHandleAction += () => MouseExitHandle();
    }
    /// <summary>
    /// 鼠标移入
    /// </summary>
    private void MouseEnterHandle()
    {
        BaseMemoryObj obj = GameFacade.Instance.GetObjForObjTypeInPool(MemoryPoolObjType.WorldUIIcon);
        obj.OutPool(new object[] { objName, transform });
        worldUIIcon = obj.GetComponent<WorldUIIcon>();
        //worldUIIcon = GameObject.Instantiate(Resources.Load<WorldUIIcon>("Prefabs/UIPrefab/WorldUIIcon"), transform);
        //worldUIIcon.transform.localPosition = Vector3.zero;
        //worldUIIcon.UpdateNameText(objName);
    }

    /// <summary>
    /// 鼠标移出
    /// </summary>
    private void MouseExitHandle()
    {
        if (worldUIIcon != null)
        {
            worldUIIcon.GetComponent<BaseMemoryObj>().InPool();
            worldUIIcon = null;
        }

    }

}
