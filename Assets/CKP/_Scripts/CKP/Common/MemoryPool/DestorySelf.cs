using DFDJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class DestorySelf : MonoBehaviour
{
    /// <summary>
    /// 多长时间之后自己消失
    /// </summary>
    public float lifeTime;
    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnEnable()
    {
        if (lifeTime <= 0)
        {
            return;
        }
        BasePos basePos = GameFacade.Instance.GetCurrentCamPos();
        Debug.Log(basePos.GetID());
        if (basePos != null && basePos.GetID() == GameObjIDTool.HeavyEquipmentRoamtPos)//(basePanel.panelType == UIPanelType.HeavyEquipmentRoamtPanel)
        {
            Debug.Log(basePos.GetID());


            StartCoroutine(IDestorySelf());
        }
    }


    public void StartIDestorySelf()
    {
        StopAllCoroutines();
        StartCoroutine(IDestorySelf());
    }
    IEnumerator IDestorySelf()
    {
        yield return new WaitForSeconds(lifeTime);
        gameObject.SetActive(false);
        // GetComponent<BaseMemoryObj>().InPool();
    }
}
