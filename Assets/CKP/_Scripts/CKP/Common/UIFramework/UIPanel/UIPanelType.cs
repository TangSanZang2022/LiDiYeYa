using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// UI面板类型枚举
/// </summary>
public enum UIPanelType
{
    /// <summary>
    /// 默认
    /// </summary>
    None,
    /// <summary>
    /// 开始界面主面板
    /// </summary>
    StartSceneMainPanel,
    /// <summary>
    /// 主界面
    /// </summary>
    StudyMainPanel,
    /// <summary>
    /// 文字介绍界面
    /// </summary>
    TextContentPanel,
    /// <summary>
    /// 图片展示界面
    /// </summary>
    ImageContentPanel,
    /// <summary>
    /// 播放视频界面
    /// </summary>
    VideoContentPanel,

    /// <summary>
    /// 准备界面
    /// </summary>
    ReadyPanel,
    /// <summary>
    /// 训练面板
    /// </summary>
    TrainPanel,
    /// <summary>
    /// 考核面板
    /// </summary>
    AppraisalPanel,
    /// <summary>
    /// 退出界面
    /// </summary>
    ExitPanel
}
