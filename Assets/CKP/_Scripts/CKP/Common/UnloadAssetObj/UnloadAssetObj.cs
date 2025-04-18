using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Common
{
    /// <summary>
    /// 在销毁时卸载所有引用的资源
    /// </summary>
    public class UnloadAssetObj : MonoBehaviour
    {
        private void OnDestroy()
        {
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }
    }
}
