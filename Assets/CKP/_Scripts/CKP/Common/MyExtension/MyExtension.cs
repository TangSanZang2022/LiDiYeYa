using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Common;
/// <summary>
/// 拓展类
/// </summary>
public static class MyExtension
{
    /// <summary>
    /// 获取子物体中T类型，除去父物体和子物体的子物体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="trans"></param>
    /// <returns></returns>
    public static T[] GetComponentsExceptParentAndChildedChild<T>(this Transform trans)
    {
        List<T> list = new List<T>();
        for (int i = 0; i < trans.childCount; i++)
        {
            int index = i;
            T t = trans.GetChild(index).GetComponent<T>();
            if (t != null)
            {
                list.Add(t);
            }
        }
        return list.ToArray();
    }

    /// <summary>
    /// 获取子物体中T类型，除去父物体和子物体的子物体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="go"></param>
    /// <returns></returns>
    public static T[] GetComponentsExceptParentAndChildedChild<T>(this GameObject go)
    {
        List<T> list = new List<T>();
        for (int i = 0; i < go.transform.childCount; i++)
        {
            int index = i;
            T t = go.transform.GetChild(index).GetComponent<T>();
            if (t != null)
            {
                list.Add(t);
            }

        }
        return list.ToArray();
    }

    /// <summary>
    /// 根据子物体的名称，找到子物体
    /// </summary>
    /// <param name="currentTransform">子物体所在层级</param>
    /// <param name="childName">子物体名称</param>
    /// <returns></returns>
    public static Transform FindChildForName(this Transform currentTransform, string childName)
    {
        Transform childTrans = currentTransform.Find(childName);
        if (childTrans != null)
        {
            return childTrans;
        }
        for (int i = 0; i < currentTransform.childCount; i++)
        {
            childTrans = FindChildForName(currentTransform.GetChild(i), childName);
            if (childTrans != null)
            {
                return childTrans;
            }
        }
        return null;
    }
    /// <summary>
    /// 根据子物体的名称，找到子物体
    /// </summary>
    /// <param name="currentTransform">子物体所在层级</param>
    /// <param name="childName">子物体名称</param>
    /// <returns></returns>
    public static T FindChildForName<T>(this Transform currentTransform, string childName)
    {
        Transform childTrans = currentTransform.Find(childName);
        if (childTrans != null)
        {
            return childTrans.GetComponent<T>();
        }
        for (int i = 0; i < currentTransform.childCount; i++)
        {
            childTrans = FindChildForName(currentTransform.GetChild(i), childName);
            if (childTrans != null)
            {
                return childTrans.GetComponent<T>();
            }
        }
        return default(T);
    }
    /// <summary>
    /// 围绕物体旋转
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="center"></param>
    /// <param name="axis"></param>
    /// <param name="angle"></param>
    public static void MyRotateAround(this Transform transform, Vector3 center, Vector3 axis, float angle, float t)
    {

        Vector3 pos = transform.position;
        Quaternion rot = Quaternion.AngleAxis(angle, axis);
        Vector3 dir = pos - center; //计算从圆心指向摄像头的朝向向量
        dir = rot * dir;            //旋转此向量
        transform.position = Vector3.Lerp(transform.position, center + dir, t);//移动摄像机位置
                                                                               // transform.DOMove(center + dir, t);
        Quaternion myrot = transform.rotation;
        //transform.rotation *= Quaternion.Inverse(myrot) * rot *myrot;//设置角度另一种方法
        transform.rotation = Quaternion.Lerp(transform.rotation, rot * myrot, t); //设置角度
                                                                                  // transform.DORotate((rot * myrot).eulerAngles, t);
    }
    /// <summary>
    /// 围绕物体旋转
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="center"></param>
    /// <param name="axis"></param>
    /// <param name="angle"></param>
    public static void DOMyRotateAround(this Transform transform, Vector3 center, Vector3 axis, float angle, float t)
    {

        Vector3 pos = transform.position;
        Quaternion rot = Quaternion.AngleAxis(angle, axis);
        Vector3 dir = pos - center; //计算从圆心指向摄像头的朝向向量
        dir = rot * dir;            //旋转此向量
                                    // transform.position = Vector3.Lerp(transform.position, center + dir, t);//移动摄像机位置
        transform.DOMove(center + dir, t);
        Quaternion myrot = transform.rotation;
        //transform.rotation *= Quaternion.Inverse(myrot) * rot *myrot;//设置角度另一种方法
        //transform.rotation = Quaternion.Lerp(transform.rotation, rot * myrot, t); //设置角度
        transform.DORotate((rot * myrot).eulerAngles, t);
    }
    /// <summary>
    /// 判断物体是否被隐藏，包括父物体
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    public static bool IsSelfActive(this GameObject go)
    {
        Transform parentTrans = go.transform.parent;

        if (go.activeSelf)
        {
            if (parentTrans != null)
            {
                if (!parentTrans.gameObject.activeSelf)
                {
                    return false;
                }
                else
                {
                    return parentTrans.gameObject.IsSelfActive();
                }
            }
            else
            {
                return go.activeSelf;
            }
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// string转换为float
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public static float StringToFloat(string content, float defultValue)
    {
        float f;
        if (!float.TryParse(content, out f))
        {
            f = defultValue;
        }
        return f;

    }

    /// <summary>
    /// 根据键从字典中获取对应的值
    /// </summary>
    /// <typeparam name="Q"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <param name="keyValuePairs"></param>
    /// <param name="keyStr"></param>
    /// <returns></returns>
    public static T GetValueInDictionary<Q, T>(this Dictionary<Q, T> keyValuePairs, Q keyStr) where T : UnityEngine.Object
    {
        if (!keyValuePairs.ContainsKey(keyStr))
        {
            return null;
        }
        return keyValuePairs[keyStr];
    }

    /// <summary>
    /// 给动画添加事件
    /// </summary>
    /// <param name="animator">动画状态机</param>
    /// <param name="ClipName">动画clip名称</param>
    /// <param name="functionName">事件方法名称</param>
    /// <param name="animationEventTime">触发事件的时间,如果为-1，则为动画结束前的0.1秒</param>
    public static void AddAnimEvent(this Animator animator, string ClipName, string functionName, float animationEventTime = -1)
    {

        RuntimeAnimatorController runtimeAnimatorController = animator.runtimeAnimatorController;
        AnimationClip currentAnimClip;
        for (int i = 0; i < runtimeAnimatorController.animationClips.Length; i++)
        {
            Debug.Log(runtimeAnimatorController.animationClips[i].name);
            if (runtimeAnimatorController.animationClips[i].name == ClipName)
            {
                currentAnimClip = runtimeAnimatorController.animationClips[i];
                AnimationEvent animationEvent = new AnimationEvent();
                animationEvent.functionName = functionName;
                if (animationEventTime == -1)
                {
                    animationEventTime = currentAnimClip.length - 0.1f;
                }
                Debug.Log("动画时间为：" + (currentAnimClip.length - 0.1f));
                animationEvent.time = animationEventTime;
                currentAnimClip.AddEvent(animationEvent);
                break;
            }
        }
    }
    /// <summary>
    /// 给动画添加事件
    /// </summary>
    /// <param name="animator">动画状态机</param>
    /// <param name="ClipName">动画clip名称</param>
    /// <param name="functionName">事件方法名称</param>
    /// <param name="stringParameter">string参数</param> 
    /// <param name="animationEventTime">触发事件的时间,如果为-1，则为动画结束前的0.1秒</param>
    public static void AddAnimEvent(this Animator animator, string ClipName, string functionName, string stringParameter, float animationEventTime = -1)
    {

        RuntimeAnimatorController runtimeAnimatorController = animator.runtimeAnimatorController;
        AnimationClip currentAnimClip;
        for (int i = 0; i < runtimeAnimatorController.animationClips.Length; i++)
        {
            Debug.Log(runtimeAnimatorController.animationClips[i].name);
            if (runtimeAnimatorController.animationClips[i].name == ClipName)
            {
                currentAnimClip = runtimeAnimatorController.animationClips[i];
                AnimationEvent animationEvent = new AnimationEvent();
                animationEvent.functionName = functionName;
                animationEvent.stringParameter = stringParameter;
                if (animationEventTime == -1)
                {
                    animationEventTime = currentAnimClip.length - 0.1f;
                }
                Debug.Log("动画时间为：" + (currentAnimClip.length - 0.1f));
                animationEvent.time = animationEventTime;
                currentAnimClip.AddEvent(animationEvent);
                break;
            }
        }
    }
    /// <summary>
    /// 给动画添加事件
    /// </summary>
    /// <param name="animator">动画状态机</param>
    /// <param name="ClipName">动画clip名称</param>
    /// <param name="functionName">事件方法名称</param>
    /// <param name="floatParameter">float参数</param>
    /// <param name="animationEventTime">触发事件的时间,如果为-1，则为动画结束前的0.1秒</param>
    public static void AddAnimEvent(this Animator animator, string ClipName, string functionName, float floatParameter, float animationEventTime = -1)
    {

        RuntimeAnimatorController runtimeAnimatorController = animator.runtimeAnimatorController;
        AnimationClip currentAnimClip;
        for (int i = 0; i < runtimeAnimatorController.animationClips.Length; i++)
        {
            Debug.Log(runtimeAnimatorController.animationClips[i].name);
            if (runtimeAnimatorController.animationClips[i].name == ClipName)
            {
                currentAnimClip = runtimeAnimatorController.animationClips[i];
                AnimationEvent animationEvent = new AnimationEvent();
                animationEvent.functionName = functionName;
                animationEvent.floatParameter = floatParameter;
                if (animationEventTime == -1)
                {
                    animationEventTime = currentAnimClip.length - 0.1f;
                }
                Debug.Log("动画时间为：" + (currentAnimClip.length - 0.1f));
                animationEvent.time = animationEventTime;
                currentAnimClip.AddEvent(animationEvent);
                break;
            }
        }
    }
    /// <summary>
    /// 给动画添加事件
    /// </summary>
    /// <param name="animator">动画状态机</param>
    /// <param name="ClipName">动画clip名称</param>
    /// <param name="functionName">事件方法名称</param>
    /// <param name="intParameter">int参数</param>
    /// <param name="animationEventTime">触发事件的时间,如果为-1，则为动画结束前的0.1秒</param>
    public static void AddAnimEvent(this Animator animator, string ClipName, string functionName, int intParameter, float animationEventTime = -1)
    {

        RuntimeAnimatorController runtimeAnimatorController = animator.runtimeAnimatorController;
        AnimationClip currentAnimClip;
        for (int i = 0; i < runtimeAnimatorController.animationClips.Length; i++)
        {
            Debug.Log(runtimeAnimatorController.animationClips[i].name);
            if (runtimeAnimatorController.animationClips[i].name == ClipName)
            {
                currentAnimClip = runtimeAnimatorController.animationClips[i];
                AnimationEvent animationEvent = new AnimationEvent();
                animationEvent.functionName = functionName;
                animationEvent.intParameter = intParameter;
                if (animationEventTime == -1)
                {
                    animationEventTime = currentAnimClip.length - 0.1f;
                }
                Debug.Log("动画时间为：" + (currentAnimClip.length - 0.1f));
                animationEvent.time = animationEventTime;
                currentAnimClip.AddEvent(animationEvent);
                break;
            }
        }
    }
    /// <summary>
    /// 设置材质球渲染模式
    /// </summary>
    /// <param name="material"></param>
    /// <param name="renderingMode"></param>
    public static void SetMaterialRenderingMode(this Material material, RenderingMode renderingMode)
    {
        switch (renderingMode)
        {
            case RenderingMode.Opaque:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
                break;
            case RenderingMode.Cutout:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.EnableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 2450;
                break;
            case RenderingMode.Fade:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
            case RenderingMode.Transparent:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.SetFloat("_BumpScale", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
        }
    }
    /// <summary>
    /// 添加反射材质
    /// </summary>
    /// <param name="meshRenderer"></param>
    public static void AddReflectMat(this MeshRenderer meshRenderer,float alpha=0.2f)
    {
        //Material[] reflectMatArray=
        List<Material> reflectMatList = new List<Material>();
        if (meshRenderer.materials.Length==1)
        {
            Material m = new Material(meshRenderer.material);
            m.name = m.name + "Reflect";
            Material m1 = new Material(meshRenderer.material);
           
            reflectMatList.Add(m1);
            m.shader = Shader.Find("Unlit/Reflect");
            m.SetFloat("_Alpha", alpha);
            reflectMatList.Add(m);
           
        }
        else if (meshRenderer.materials.Length >1)
        {
            for (int i = 0; i < meshRenderer.materials.Length; i++)
            {
                int index = i;
                Material m = new Material(meshRenderer.materials[index]);
                m.name = m.name + "Reflect";
                Material m1 = new Material( meshRenderer.materials[index]);
              
                reflectMatList.Add(m1);
                m.shader = Shader.Find("Unlit/Reflect");
                m.SetFloat("_Alpha", alpha);
                reflectMatList.Add(m);
            }
        }
        meshRenderer.materials = new Material[reflectMatList.Count];
        meshRenderer.materials = reflectMatList.ToArray();
    }
    /// <summary>
    /// 字符串转Enum
    /// </summary>
    /// <typeparam name="T">枚举</typeparam>
    /// <param name="str">字符串</param>
    /// <returns>转换的枚举</returns>
    public static T ToEnum<T>(this string str)
    {
        return (T)System.Enum.Parse(typeof(T), str);
    }
    /// <summary>
    /// 删除所有特定类型的子物体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tf"></param>
    /// <returns></returns>
    public static void DestoryAllChildForType<T>(this Transform tf) where T : MonoBehaviour
    {
        for (int i = 0; i < tf.childCount; i++)
        {
            int index = i;
            if (tf.GetChild(index).GetComponent<T>() != null)
            {
                GameObject.Destroy(tf.GetChild(index).gameObject);
            }
        }
    }
    /// <summary>
    /// 删除所有特定类型的子物体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tf"></param>
    /// <returns></returns>
    public static void DestoryAllChild(this Transform tf)
    {
        for (int i = 0; i < tf.childCount; i++)
        {
            int index = i;
            GameObject.Destroy(tf.GetChild(index).gameObject);
        }
    }

    /// <summary>
    /// 获取枚举的所有值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <returns></returns>
    public static string[] GetAllEnumName<T>(this T t) where T : Enum
    {
        string[] Names = System.Enum.GetNames(typeof(T));
        return Names;
    }
    /// <summary>
    /// 移除字典里面的值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Q"></typeparam>
    /// <param name="keyValuePairs"></param>
    /// <param name="removeValue"></param>
    public static void RemoveDictValue<T, Q>(this Dictionary<T, Q> keyValuePairs, Q removeValue) where Q : UnityEngine.Object
    {

        if (keyValuePairs.ContainsValue(removeValue))
        {

            foreach (var item in new List<T>(keyValuePairs.Keys))
            {
                if (keyValuePairs[item] == removeValue)
                {
                    keyValuePairs.Remove(item);
                    break;
                }
            }
        }
        else
        {
            Debug.LogWarning(string.Format("字典{0}中不存在Value为{1}的值", keyValuePairs.ToString(), removeValue.ToString()));
        }

    }

    /// <summary>
    /// 更新item信息
    /// </summary>
    /// <param name="datasList"></param>
    public static void Update_ItemInfo<T, Q>(this Transform itemParent, List<T> datasList, Func<T, Q, bool> condition, Q itemPrefab) where Q : MonoBehaviour, IBaseItem_CanUpdate
       where T : BaseItemData_CanUpdate
    {
        List<Q> allChild = new List<Q>();
        allChild.AddRange(itemParent.GetComponentsInChildren<Q>());
        List<int> allIndex = new List<int>();//存放所有没有用到的数据下标

        for (int i = 0; i < datasList.Count; i++)
        {
            int index = i;
            Q currentIDItem = allChild.ToArray().Find((item) => condition(datasList[index], item));
            if (currentIDItem != null)//查找到了对应ID的Item
            {
                datasList[index].CurrentIndex = currentIDItem.GetData().CurrentIndex;
                currentIDItem.SetData(datasList[index]);
                allChild.Remove(currentIDItem);//将已经更新的数据Item移除
            }
            else
            {
                allIndex.Add(index);//如果此条数据没有在已经创建的Item中查找到则需要保存
            }
        }

        for (int i = 0; i < allIndex.Count; i++)
        {
            int index = allIndex[i];
            T t = datasList[index];//获取要处理的数据
            if (allChild.Count != 0)//如果子物体还有
            {
                allChild[0].SetData(t);
                allChild.RemoveAt(0);
            }
            else//当展示数据Item不够时，就要重新创建
            {
                Q q_Created = GameObject.Instantiate<Q>(itemPrefab, itemParent);
                t.CurrentIndex = itemParent.childCount;
                q_Created.SetData(t);
            }
        }
        for (int i = 0; i < allChild.Count; i++)//删除多的数据
        {
            int index = i;
            GameObject.Destroy(allChild[index].gameObject);
        }

    }
}
public enum RenderingMode
{
    Opaque,
    Cutout,
    Fade,
    Transparent,
}
/// <summary>
/// 显示数据Item接口
/// </summary>
public interface IBaseItem_CanUpdate
{
    /// <summary>
    /// 获取数据
    /// </summary>
    /// <returns></returns>
     BaseItemData_CanUpdate GetData();
    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="baseItemData"></param>
     void SetData(BaseItemData_CanUpdate baseItemData);
}
/// <summary>
///  Item显示的数据接口
/// </summary>
public interface BaseItemData_CanUpdate
{
    /// <summary>
    /// 当前在第几个
    /// </summary>
     int CurrentIndex { get; set; }
}




