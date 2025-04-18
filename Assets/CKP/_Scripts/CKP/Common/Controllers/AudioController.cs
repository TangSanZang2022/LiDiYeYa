using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 声音控制器
/// </summary>
public class AudioController : BaseController
{
    /// <summary>
    /// 声音文件存放路径前缀
    /// </summary>
    private const string SoundPerfix = "Sound/";
    /// <summary>
    /// 背景音乐文件名
    /// </summary>
    public const string SoundBG = "BGSound";
    /// <summary>
    /// 背景音乐播放器
    /// </summary>
    private AudioSource bgAudioSource;
    /// <summary>
    /// 普通音乐播放器
    /// </summary>
    private AudioSource normalAudioSource;
    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="gameFacade"></param>
    public AudioController(GameFacade gameFacade) : base(gameFacade)
    { }
    public override void OnInit()
    {
        GameObject go = new GameObject("BGAudioSourcePlayer");
        bgAudioSource = go.AddComponent<AudioSource>();
        bgAudioSource.playOnAwake = false;
        normalAudioSource = go.AddComponent<AudioSource>();
        bgAudioSource.playOnAwake = false;

    }
    /// <summary>
    /// 播放音频
    /// </summary>
    /// <param name="audioSource"></param>
    /// <param name="audioClip"></param>
    /// <param name="volume"></param>
    /// <param name="isLoop"></param>
    private void PlayerAudio(AudioSource audioSource, AudioClip audioClip, float volume, bool isLoop = false)
    {
        if (audioClip==null)
        {
            Debug.Log("播放音频为空");
        }
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.loop = isLoop;
        audioSource.Play();
    }
    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="soundName"></param>
    public void PlayBGSound(string soundName)
    {
        PlayerAudio(bgAudioSource, LoadSound(soundName), 1, true);
    }
    /// <summary>
    /// 播放其他音乐
    /// </summary>
    /// <param name="soundName"></param>
    public void PlayNormalSound(string soundName,bool isLoop=false)
    {
        if (string.IsNullOrEmpty(soundName))
        {
            Debug.Log("播放音频为空");
            return;
        }
        PlayerAudio(normalAudioSource, LoadSound(soundName), 1, isLoop);
    }
    /// <summary>
    /// 从Resources文件夹加载音频信息
    /// </summary>
    /// <param name="soundName"></param>
    /// <returns></returns>
    private AudioClip LoadSound(string soundName)
    {
        return Resources.Load<AudioClip>(SoundPerfix + soundName);
    }
}
