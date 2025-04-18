using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;
/// <summary>
/// 加载场景进度条界面
/// </summary>
public class LoadingCanvas :MonoBehaviour
{
    
    private Slider progressSlider;
    private Text progressText;
    /// <summary>
    /// 当前进度
    /// </summary>
    private int currentProgress;

    /// <summary>
    /// 目标进度
    /// </summary>
    private int targetProgress;

    /// <summary>
    /// 加载速度
    /// </summary>
    private int progressSpeed = 3;
    private void Awake()
    {
        // DontDestroyOnLoad(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        StartSlider();
    }
    /// <summary>
    /// 开始进度条
    /// </summary>
    private void StartSlider()
    {
        gameObject.SetActive(true);
        progressSlider = transform.FindChildForName("ProgressSlider").GetComponent<Slider>();
        progressText = transform.FindChildForName("ProgressText").GetComponent<Text>();
        StartCoroutine(ILoading());
    }
   
    private IEnumerator ILoading()
    {
        Debug.Log("开始加载");
        MySceneManager.Ao.allowSceneActivation = false;
        while (!MySceneManager.Ao.isDone && MySceneManager.Ao.progress < 0.9f)
        {
            Debug.Log("正在加载"+ MySceneManager.Ao.progress);
            targetProgress = (int)(MySceneManager.Ao.progress * 100);
            yield return ILoadProgess();
        }
        targetProgress = 100;
        yield return ILoadProgess();
        MySceneManager.Ao.allowSceneActivation = true;
        MySceneManager.IsLoading = false;
    }

    /// <summary>
    /// 加载进度条
    /// </summary>
    /// <returns></returns>
    private IEnumerator ILoadProgess()
    {
        while (currentProgress < targetProgress)
        {
            Debug.Log(string.Format("targetProgress为：{0}，currentProgress为：{1}", targetProgress, currentProgress));
            currentProgress +=progressSpeed;
            currentProgress = Mathf.Clamp(currentProgress, 0, 100);
            progressSlider.value = (float)currentProgress / 100;
            progressText.text = currentProgress.ToString() + "%";
            yield return new WaitForEndOfFrame();
        }
    }
}
