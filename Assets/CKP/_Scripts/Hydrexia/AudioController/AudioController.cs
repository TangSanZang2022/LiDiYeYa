using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
namespace Hydrexia.CKP
{
    /// <summary>
    /// 音频控制器
    /// </summary>
    public class AudioController : BaseController
    {
        public AudioController(GameFacade gameFacade) : base(gameFacade) { }

        private AudioSource _audioSource;

        private AudioSource audioSource
        {
            get
            {
                if (_audioSource==null)
                {
                    _audioSource = new GameObject("AudioSource").AddComponent<AudioSource>();
                    _audioSource.playOnAwake = false;
                    _audioSource.loop = false;
                }
                return _audioSource;
            }
        }

        private AudioClip[] hydrogenationOperationAudioClip;
        /// <summary>
        /// 模拟加氢操作语音
        /// </summary>
        private AudioClip[] HydrogenationOperationAudioClip
        {
            get
            {
                if (hydrogenationOperationAudioClip==null)
                {
                    hydrogenationOperationAudioClip = new AudioClip[] { };
                    hydrogenationOperationAudioClip = Resources.LoadAll<AudioClip>("AudioClip/HydrogenationOperationAudioClip");
                }
                return hydrogenationOperationAudioClip;
            }
        }

        private AudioClip[] tTGasDischargeOperationAudioClip;
        /// <summary>
        /// 模拟卸气操作语音
        /// </summary>
        private AudioClip[] TTGasDischargeOperationAudioClip
        {
            get
            {
                if (tTGasDischargeOperationAudioClip == null)
                {
                    tTGasDischargeOperationAudioClip = new AudioClip[] { };
                    tTGasDischargeOperationAudioClip = Resources.LoadAll<AudioClip>("AudioClip/TTGasDischargeOperationAudioClip");
                }
                return tTGasDischargeOperationAudioClip;
            }
        }
        private AudioClip[] gengHuanZhuangXieCheAudioClip;
        /// <summary>
        /// 模拟装卸车操作语音
        /// </summary>
        private AudioClip[] GengHuanZhuangXieCheAudioClip
        {
            get
            {
                if (gengHuanZhuangXieCheAudioClip == null)
                {
                    gengHuanZhuangXieCheAudioClip = new AudioClip[] { };
                    gengHuanZhuangXieCheAudioClip = Resources.LoadAll<AudioClip>("AudioClip/GengHuanZhuangXieCheAudioClip");
                }
                return gengHuanZhuangXieCheAudioClip;
            }
        }
        private AudioClip[] yaLiJingBaoJieChuAudioClip;
        /// <summary>
        /// 模拟报警确认处理语音
        /// </summary>
        private AudioClip[] YaLiJingBaoJieChuAudioClip
        {
            get
            {
                if (yaLiJingBaoJieChuAudioClip == null)
                {
                    yaLiJingBaoJieChuAudioClip = new AudioClip[] { };
                    yaLiJingBaoJieChuAudioClip = Resources.LoadAll<AudioClip>("AudioClip/YaLiJingBaoJieChuAudioClip");
                }
                return yaLiJingBaoJieChuAudioClip;
            }
        }
        public override void OnInit()
        {
            base.OnInit();
           // PlayAudio(HydrogenationOperationAudioClip[0]);
        }
        /// <summary>
        /// 播放音频
        /// </summary>
        /// <param name="audioClip"></param>
        public void PlayAudio(AudioClip audioClip)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
                audioSource.clip = null;
            }
            audioSource.clip = audioClip;
            if (audioClip!=null)
            {
                audioSource.Play();
            }
          
        }
        /// <summary>
        /// 播放音频
        /// </summary>
        /// <param name="audioClip"></param>
        public void PlayAudio(int trainingProjectsIndex,int stepID)
        {
            AudioClip audioClip = null;
            switch (trainingProjectsIndex)
            {
                case 0://模拟加氢操作
                    audioClip = HydrogenationOperationAudioClip.Find((a)=>a.name== (stepID+1).ToString());
                    break;
                case 1://模拟卸气操作
                    audioClip = TTGasDischargeOperationAudioClip.Find((a) => a.name == (stepID + 1).ToString());
                    break;
                case 2://模拟装卸车
                    audioClip = GengHuanZhuangXieCheAudioClip.Find((a) => a.name == (stepID + 1).ToString());
                    break;
                case 3://模拟报警确认处理
                    audioClip = YaLiJingBaoJieChuAudioClip.Find((a) => a.name == (stepID + 1).ToString());
                    break;
                default:
                    break;
            }
            PlayAudio(audioClip);
        }
        /// <summary>
        /// 停止音频
        /// </summary>
        public void StopAudio()
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
                audioSource.clip = null;
            }
        }
    }
}