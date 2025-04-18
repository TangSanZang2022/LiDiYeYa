using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DFDJ
{
    /// <summary>
    /// 动画控制
    /// </summary>
    public interface IAnimControl 
    {
        bool IsPlaying { get;}
        /// <summary>
        /// 重头开始播放
        /// </summary>
        void Replay();
        /// <summary>
        /// 播放
        /// </summary>
        void Play();
        /// <summary>
        /// 暂停
        /// </summary>
        void Pause();
        /// <summary>
        /// 停止，并且到最后一帧
        /// </summary>
        void Stop();
    }
}
