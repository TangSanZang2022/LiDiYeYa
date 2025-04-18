using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MouseSelectObjs;
using DFDJ;
using Common;
using DG.Tweening;
using System;
using IntelligentManufacturingPracticePlatform;
using ShowHideObj;
using LiDi.CKP;
using UIFramework;

public class GameFacade : MonoSingleton<GameFacade>
{
    /// <summary>
    /// 配置控制器
    /// </summary>
    private XmlController xmlController;
    /// <summary>
    /// 材质球控制器
    /// </summary>
    private MaterialsCntroller materialsCntroller;
    /// <summary>
    /// socket客户端控制器
    /// </summary>
   // private ClientController clientController;
    /// <summary>
    /// 响应控制器
    /// </summary>
    private RequestController requestController;
    /// <summary>
    /// http客户端控制器
    /// </summary>
    private HttpClientController httpClientController;
  
    /// <summary>
    /// 音频控制器
    /// </summary>
    private AudioController audioController;
    /// <summary>
    /// UI面板控制器
    /// </summary>
    private UIController uiController;
    /// <summary>
    /// 内存池
    /// </summary>
    private MemoryPoolController poolController;
    /// <summary>
    /// 鼠标多选物体中鼠标移动控制
    /// </summary>
    //private MouseMoveController mouseMoveController;
   
  
   
    /// <summary>
    /// 相机位置控制器
    /// </summary>
    private PosController camPosController;
    /// <summary>
    /// 相机移动控制
    /// </summary>
    private CameraMoveController cameraMoveController;

    /// <summary>
    /// 设备管理器
    /// </summary>
    private EquipmentController equipmentController;

    ShowHideObjController showHideObjController;
    //private StepController stepController;
    /// <summary>
    /// 创建模型控制器
    /// </summary>
    // private CreateModelController createModelController;
    /// <summary>
    /// 保存场景控制器
    /// </summary>
    // private SaveSceneController saveSceneController;


    /// <summary>
    /// 实训控制器
    /// </summary>
    public OperationController operationController;
    /// <summary>
    /// 热点控制器
    /// </summary>
    private HotPointController hotPointController;

    /// <summary>
    /// 压力表控制器
    /// </summary>
    MeterController meterController;

    /// <summary>
    /// 动画播放控制器
    /// </summary>
    AnimatorPlayerController animatorPlayerController;
    /// <summary>
    /// 操作工具管理器
    /// </summary>
    ToolsController toolsController;
    //AudioController audioController;
    /// <summary>
    /// 是否已经初始化
    /// </summary>
    private bool isInit = false;

    public override void Init()
    {
        base.Init();
        InitController();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        UpdateController();
    }
    private void InitController()
    {
        xmlController = new XmlController(this);
        materialsCntroller = new MaterialsCntroller(this);
        // clientController = new ClientController(this);
        animatorPlayerController = new AnimatorPlayerController(this);
        requestController = new RequestController(this);
        httpClientController = new HttpClientController(this);
        audioController = new AudioController(this);
        // gisPointTo3DPointController = new GisPointTo3DPointController(this);
        uiController = new UIController(this);
        //poolController = new MemoryPoolController(this);
        // mouseMoveController = new MouseMoveController(this);
        hotPointController = new HotPointController(this);
          camPosController = new PosController(this);
        operationController = new OperationController(this);
        cameraMoveController = new CameraMoveController(this);
        showHideObjController = new ShowHideObjController(this);
        meterController = new MeterController(this);
        toolsController = new ToolsController(this);
        // stepController = new StepController(this);


        // createModelController = new CreateModelController(this);

        // saveSceneController = new SaveSceneController(this);


        xmlController.OnInit();
        animatorPlayerController.OnInit();
        materialsCntroller.OnInit();
        audioController.OnInit();
        //clientController.OnInit();
       
        requestController.OnInit();
        httpClientController.OnInit();
        //gisPointTo3DPointController.OnInit();
        uiController.OnInit();
        //poolController.OnInit();
       // mouseMoveController.OnInit();
      
        camPosController.OnInit();
        cameraMoveController.OnInit();
        showHideObjController.OnInit();
        // equipmentController.OnInit();
        //stepController.OnInit();
        //createModelController.OnInit();
        // saveSceneController.OnInit();
        hotPointController.OnInit();
        operationController.OnInit();
        meterController.OnInit();
        toolsController.OnInit();
        isInit = true;
    }
    /// <summary>
    /// 更新所有Controller
    /// </summary>
    private void UpdateController()
    {
        xmlController.OnUpdate();
        materialsCntroller.OnUpdate();
        //clientController.OnUpdate();
        
        requestController.OnUpdate();
        httpClientController.OnUpdate();
        uiController.OnUpdate();
       // mouseMoveController.OnUpdate();
        audioController.OnUpdate();
        
        camPosController.OnUpdate();
        cameraMoveController.OnUpdate();
        //equipmentController.OnUpdate();

        // createModelController.OnUpdate();
        // saveSceneController.OnUpdate();
       // hotPointController.OnInit();
       // operationController.OnInit();
    }
    /// <summary>
    /// 删除所有Controller
    /// </summary>
    private void DestoryController()
    {
        if (!isInit) //如果为多个场景，在本场景第一帧直接切换到第一个场景时，可能会报空
        {
            return;
        }
        xmlController.OnDestory();
        materialsCntroller.OnDestory();
        animatorPlayerController.OnDestory();
        //clientController.OnDestory();

        requestController.OnDestory();
        httpClientController.OnDestory();
        uiController.OnDestory();
       // mouseMoveController.OnDestory();
        audioController.OnDestory();
      
        camPosController.OnDestory();
        cameraMoveController.OnDestory();
        //equipmentController.OnDestory();

        //createModelController.OnDestory();
        //saveSceneController.OnDestory();
        hotPointController.OnDestory();
        operationController.OnDestory();
        showHideObjController.OnDestory();
        meterController.OnDestory();
        toolsController.OnDestory();
    }
    /// <summary>
    /// 处理从服务器接收的消息
    /// </summary>
    /// <param name="msg"></param>
    public void HandleMsg(string msg)
    {
        Debug.Log(string.Format("接收到数据：{0}", msg));
    }
    /// <summary>
    /// 添加ActionCode的时候处理消息
    /// </summary>
    /// <param name="actionCode"></param>
    /// <param name="data"></param>
    public void HandleMsg(ActionCode actionCode, string data)
    {
        requestController.HandleResponse(actionCode, data);
    }
   

  
   
    /// <summary>
    /// 将json写入到本地
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <param name="path"></param>
    public void WriteAllJsonTo_streamingAssetsPath(string json, string path)
    {
        xmlController.WriteAllJsonTo_streamingAssetsPath(json, path);
    }
    /// <summary>
    /// 写入一行，如果有相同的就替换
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <param name="path"></param>
    /// <param name="SameLinecondition"></param>
    public void WriteJsonLineTo_streamingAssetsPath<T>(string json, string path, Func<T, T, bool> SameLinecondition, bool replace = true)
    {
        xmlController.WriteJsonLineTo_streamingAssetsPath<T>(json, path, SameLinecondition, replace);
    }
   
    /// <summary>
    /// 根据材质球的名称得到
    /// </summary>
    /// <param name="materialName"></param>
    /// <returns></returns>
    public Material GetMaterialForName(string materialName)
    {
        return materialsCntroller.GetMaterialForName(materialName);
    }
   
   
    /// <summary>
    /// 将request添加到字典
    /// </summary>
    /// <param name="actionCode"></param>
    /// <param name="baseRequest"></param>
    public void AddRequest(ActionCode actionCode, BaseRequest baseRequest)
    {
        requestController.AddRequest(actionCode, baseRequest);
    }
    /// <summary>
    /// 将Request从字典中移出
    /// </summary>
    /// <param name="actionCode"></param>
    public void RemoveRequest(ActionCode actionCode)
    {
        requestController.removeRequest(actionCode);
    }
    /// <summary>
    /// 发送请求
    /// </summary>
    public void SendRequest(string data)
    {
        // clientController.SendRequest(data);
    }
    /// <summary>
    ///播放背景音乐
    /// </summary>
    /// <param name="soundName"></param>
    public void PlayBGSound(string soundName)
    {
        audioController.PlayBGSound(soundName);
    }
    /// <summary>
    /// 播放一般音乐
    /// </summary>
    /// <param name="soundName"></param>
    public void PlayNormalSound(string soundName, bool isLoop = false)
    {
        audioController.PlayNormalSound(soundName, isLoop);
    }
    /// <summary>
    /// 面板入栈，将面板显示出来
    /// </summary>
    /// <param name="panelType"></param>
    public BasePanel PushPanel(UIPanelType panelType)
    {
        return uiController.PushPanel(panelType);
    }
    /// <summary>
    /// 面板出栈，将面板隐藏起来
    /// </summary>
    public void PopPanel()
    {
        
        uiController.PopPanel();

    }

    /// <summary>
    /// 得到栈顶面板
    /// </summary>
    /// <returns></returns>
    public BasePanel GetTopPanel()
    {
       return uiController.GetTopPanel();
    }
    /// <summary>
    /// 根据UIPanelType获取UI面板
    /// </summary>
    /// <param name="uIPanelType"></param>
    /// <returns></returns>
    public BasePanel GetUIPanel(UIPanelType uiPanelType)
    {
        return uiController.GetUIPanel(uiPanelType);
    }
    /// <summary>
    /// 根据物体类型的到内存池
    /// </summary>
    /// <param name="memoryPoolObjType">物体类型</param>
    /// <returns></returns>
    public BaseMemoryPool GetMemoryPool(MemoryPoolObjType memoryPoolObjType)
    {
        return poolController.GetMemoryPool(memoryPoolObjType);
    }
    /// <summary>
    /// 根据物体类型得到内存池中对应的物体
    /// </summary>
    /// <param name="memoryPoolObjType">物体类型</param>
    /// <returns></returns>
    public BaseMemoryObj GetObjForObjTypeInPool(MemoryPoolObjType memoryPoolObjType)
    {
        return poolController.GetObjForObjType(memoryPoolObjType);
    }

  
  
    /// <summary>
    /// 设置物体到目标点
    /// </summary>
    /// <param name="id"></param>
    /// <param name="targetCam"></param>
    public BasePos SetTarnsToPos(string id, Transform targetCam)
    {
       return camPosController.SetTarnsToPos(id, targetCam);
    }
  
    /// <summary>
    /// 获取当前获取当前相机所在位置
    /// </summary>
    /// <returns></returns>
    public BasePos GetCurrentCamPos()
    {
        return camPosController.GetCurrentCamPos();
    }
    /// <summary>
    /// 设置缩放速率
    /// </summary>
    /// <param name="zoomRate"></param>
    public void SetZoomRate(int zoomRate)
    {
        cameraMoveController.SetZoomRate(zoomRate);
    }

    /// <summary>
    /// 设置最远观察距离
    /// </summary>
    /// <param name="zoomRate"></param>
    public void SetMaxObservationDis(float maxObservationDis)
    {
        cameraMoveController.SetMaxObservationDis(maxObservationDis);
    }
    /// <summary>
    /// 设置最近观察距离
    /// </summary>
    /// <param name="zoomRate"></param>
    public void SetMinObservationDis(float minObservationDis)
    {
        cameraMoveController.SetMinObservationDis(minObservationDis);
    }

    /// <summary>
    /// 设置水平方向最大移动距离
    /// </summary>
    /// <param name="zoomRate"></param>
    public void SetMaxDisH(float maxDisH)
    {
        cameraMoveController.SetMaxDisH(maxDisH);
    }
    /// <summary>
    /// 设置竖直方向最大移动距离
    /// </summary>
    /// <param name="zoomRate"></param>
    public void SetMaxDisV(float maxDisV)
    {
        cameraMoveController.SetMaxDisV(maxDisV);
    }
    /// <summary>
    /// 设置水平竖直平面移动速度
    /// </summary>
    /// <param name="zoomRate"></param>
    public void SetDeltaMoveSpeed(float deltaMoveSpeed)
    {
        cameraMoveController.SetDeltaMoveSpeed(deltaMoveSpeed);
    }

    /// <summary>
    /// 设置水平转动时的速度
    /// </summary>
    /// <param name="zoomRate"></param>
    public void SetRotSpeedH(float rotSpeedH)
    {
        cameraMoveController.SetRotSpeedH(rotSpeedH);
    }
    /// <summary>
    /// 设置竖直转动时的速度
    /// </summary>
    /// <param name="zoomRate"></param>
    public void SetRotSpeedV(float rotSpeedV)
    {
        cameraMoveController.SetRotSpeedV(rotSpeedV);
    }
    /// <summary>
    /// 设置相机水平移动时的速度
    /// </summary>
    /// <param name="zoomRate"></param>
    public void SetMoveSpeed(float moveSpeed)
    {
        cameraMoveController.SetMoveSpeed(moveSpeed);
    }

    /// <summary>
    /// 设置竖直方向最大旋转角度
    /// </summary>
    /// <param name="zoomRate"></param>
    public void SetMaxRotV(float maxRotV)
    {
        cameraMoveController.SetMaxRotV(maxRotV);
    }
    /// <summary>
    /// 设置是否可以水平移动
    /// </summary>
    /// <param name="canMoveHV"></param>
    public void Set_canMoveHV(bool canMoveHV)
    {
        cameraMoveController.Set_canMoveHV(canMoveHV);
    }
    /// <summary>
    /// 设置是否可以缩放
    /// </summary>
    /// <param name="canMoveHV"></param>
    public void Set_canZoom(bool canZoom)
    {
        cameraMoveController.Set_canZoom(canZoom);
    }
    /// <summary>
    /// 设置是否可以旋转
    /// </summary>
    /// <param name="canMoveHV"></param>
    public void Set_canRot(bool canRot)
    {
        cameraMoveController.Set_canRot(canRot);
    }
    /// <summary>
    /// 设置是否围绕目标点寻转
    /// </summary>
    /// <param name="isRotAroundTarget"></param>
    public void Set_isRotAroundTarget(bool isRotAroundTarget)
    {
        cameraMoveController.Set_isRotAroundTarget(isRotAroundTarget);
    }
    /// <summary>
    /// 设置相机旋转中心
    /// </summary>
    /// <param name="pos"></param>
    public void Set_rotTarget(Vector3 pos)
    {
        cameraMoveController.Set_rotTarget(pos);
    }
    /// <summary>
    /// 设置旋转中心点
    /// </summary>
    /// <param name="pos"></param>
    public void Set_zoomTarget(Vector3 pos)
    {
        cameraMoveController.Set_zoomTarget(pos);
    }
    /// <summary>
    /// 设置lock_rotTarget
    /// </summary>
    /// <param name="isLock"></param>
    public void Set_lock_rotTarget(bool isLock)
    {
        cameraMoveController.Set_lock_rotTarget(isLock);
    }
    /// <summary>
    /// 设置实现缩放的方式，如果为False,则为用设置相机Field of View来实现缩放
    /// </summary>
    /// <param name="isMoveForZoom"></param>
    public void Set_moveForZoom(bool isMoveForZoom)
    {
        cameraMoveController.Set_moveForZoom(isMoveForZoom);
    }
    /// <summary>
    /// 设置相机FieldOfView到初始状态
    /// </summary>
    public void Set_myCameraFieldOfViewToNormal()
    {
        cameraMoveController.Set_myCameraFieldOfViewToNormal();
    }
  
   
    /// <summary>
    /// 根据设备ID，前往该设备的最佳视角位置
    /// </summary>
    /// <param name="id"></param>
    /// <param name="moveTrans"></param>
    public void GoToEquipmentBestViewPosForID(string id, Transform moveTrans)
    {
        equipmentController.GoToEquipmentBestViewPosForID(id, moveTrans);
    }
    /// <summary>
    /// 重置所有isAtBestViewPos
    /// </summary>
    public void ResetAll_isAtBestViewPos()
    {
        equipmentController.ResetAll_isAtBestViewPos();
    }
   
  
    /// <summary>
    /// 更换模型
    /// </summary>
    /// <param name="loadPath">路径</param>
    /// <param name="modelName">模型名称</param>
    /// <param name="CreatedModel">之前的模型</param>
    public void ChangeModel(string loadPath, string modelName, Transform CreatedModel)
    {
        //createModelController.ChangeModel(loadPath, modelName, CreatedModel);
    } /// <summary>
      /// 在点击模型选项时创建模型
      /// </summary>
      /// <param name="baseModelItem"></param>
    public void CreateModelOnClickModelItem(BaseModelItem baseModelItem)
    {
        //createModelController.CreateModelOnClickModelItem(baseModelItem);
    }
    /// <summary>
    /// 根据保存场景的数据来初始化场景
    /// </summary>
    /// <param name="saveSceneData">保存场景的全部数据</param>
    public void InitSceneFor_SaveSceneData(SaveSceneData saveSceneData)
    {
        //createModelController.InitSceneFor_SaveSceneData(saveSceneData);
    }
    /// <summary>
    /// 根据每一条数据来更新已经创建的模型
    /// </summary>
    /// <param name="objSaveSceneData"></param>
    public void UpdateModelForObjSaveSceneData(ObjSaveSceneData objSaveSceneData)
    {
        //createModelController.UpdateModelForObjSaveSceneData(objSaveSceneData);
    }
    /// <summary>
    /// 将所有场景数据发送到服务器
    /// </summary>
    public void SendAllSaveSceneDataToServer()
    {
        //saveSceneController.SendAllSaveSceneDataToServer();
    }

    /// <summary>
    /// 根据ID来找到显示隐藏的物体
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public BaseShowHideObj GetShowHideObjForID(string id)
    {
       return showHideObjController.GetShowHideObjForID(id);
    }
    /// <summary>
    /// 根据ID显示物体
    /// </summary>
    /// <param name="id"></param>
    public GameObject ShowObjForID(string id)
    {
        return showHideObjController.ShowObjForID(id);
    }
    /// <summary>
    /// 根据ID隐藏物体
    /// </summary>
    /// <param name="id"></param>
    public void HideObjForID(string id)
    {
         showHideObjController.HideObjForID(id);
    }
    /// <summary>
    /// 根据ID列表显示物体
    /// </summary>
    /// <param name="ids"></param>
    public void ShowObjForIDList(List<string> ids)
    {
        showHideObjController.ShowObjForIDList(ids);


    }
   
    /// <summary>
    /// 根据ID列表隐藏物体
    /// </summary>
    /// <param name="ids"></param>
    public void HideObjForIDList(List<string> ids)
    {
        showHideObjController.HideObjForIDList(ids);

    }
    /// <summary>
    /// 设置所有显示隐藏物体到初始状态
    /// </summary>
    public void SetShouHideObjToStartState()
    {
        showHideObjController.SetShouHideObjToStartState();
    }
    public void ShowOnlyObj(string id)
    {
        showHideObjController.ShowObjForID(id);
    }

    public void HideOnlyObj(string id)
    {
        showHideObjController.HideObjForID(id);
    }
    /// <summary>
    /// 下一步
    /// </summary>
    public void GotoNextStep()
    {
        //stepController.GotoNextStep();
    }
    /// <summary>
    /// 上一步
    /// </summary>
    public void GotoLastStep()
    {
        //stepController.GotoLastStep();
    }

    /// <summary>
    /// 根据步骤ID来设置步骤
    /// </summary>
    /// <param name="StepID"></param>
    public void SetStepForStepID(string StepID)
    {
        //stepController.SetStepForStepID(StepID);
    }
    /// <summary>
    /// 设置当前步骤
    /// </summary>
    /// <param name="stepIndex"></param>
    public void Set_CurrentStepIndex(int stepIndex)
    {
       // stepController.Set_CurrentStepIndex(stepIndex);
    }


    /// <summary>
    /// 将热点添加到字典中
    /// </summary>
    /// <param name="baseHotPoint"></param>
    public void AddHotPointIntoDic(BaseHotPoint baseHotPoint)
    {
        hotPointController.AddHotPointIntoDic(baseHotPoint);

    }
    /// <summary>
    /// 设置热点可点击状态
    /// </summary>
    /// <param name="hotPointIdList"></param>
    /// <param name="operability"></param>
    public void SetHotPointOperability(List<string> hotPointIdList, bool operability)
    {
        hotPointController.SetHotPointOperability(hotPointIdList, operability);

    }
    /// <summary>
    /// 设置热点可点击状态
    /// </summary>
    /// <param name="hotPointIdList"></param>
    /// <param name="hotPointId"></param>
    public void SetHotPointOperability(string hotPointId, bool operability)
    {
        hotPointController.SetHotPointOperability(hotPointId, operability);
    }
    /// <summary>
    /// 设置热点显示隐藏状态
    /// </summary>
    /// <param name="hotPointIdList"></param>
    /// <param name="activeState"></param>
    public void SetHotPointActive(List<string> hotPointIdList, bool activeState)
    {
        hotPointController.SetHotPointActive(hotPointIdList, activeState);
    }
    /// <summary>
    /// 设置热点显示隐藏状态
    /// </summary>
    /// <param name="hotPointIdList"></param>
    /// <param name="activeState"></param>
    public void SetHotPointActive(string hotPointId, bool activeState)
    {
        //Debug.Log(hotPointId);
        hotPointController.SetHotPointActive(hotPointId, activeState);

    }
    /// <summary>
    /// 重置所有热点到初始状态
    /// </summary>
    public void SetAllHotPointToStartState()
    {
        hotPointController.SetAllHotPointToStartState();
    }

    /// <summary>
    /// 重置所有热点到初始状态，用于退出时调用
    /// </summary>
    public void SetAllHotPointToStartState_ForQuite()
    {
        hotPointController.SetAllHotPointToStartState_ForQuite();

    }
    /// <summary>
    /// 根据ID获取热点
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public BaseHotPoint GetBaseHotPointForID(string id)
    {
        return hotPointController.GetBaseHotPointForID(id);
    }
    /// <summary>
    /// 训练状态时，选中了当前工具时，设置接下来要操作的热点
    /// </summary>
    public void SetCurrentHotPointActiveStateForTraining(bool active)
    {
        operationController.SetCurrentHotPointActiveStateForTraining (active);
    }
    /// <summary>
    /// 准备开始任务
    /// </summary>
    /// <param name="trainingProjectIndex"></param>
    /// <param name="gameMode"></param>
    public void ReadToStartTask(int trainingProjectIndex, GameMode gameMode)
    {
        operationController.ReadToStartTask(trainingProjectIndex, gameMode);
    }
    /// <summary>
    /// 根据Index获取任务
    /// </summary>
    /// <param name="index">>如果为-1，则获取当前任务</param>
    /// <returns></returns>
    public TrainingProject GetTrainingProjectForIndex(int index = -1)
    {
        return operationController.GetTrainingProjectForIndex(index);
    }
    /// <summary>
    /// 开始任务,0-3,
    /// </summary>
    public void StartTask(int trainingProjectIndex, GameMode gameMode, float limitTime = -1)
    {
        operationController.StartTask(trainingProjectIndex, gameMode, limitTime);
    }
    /// <summary>
    /// 获取模式，训练模式和考核模式
    /// </summary>
    /// <returns></returns>
    public GameMode GetGameMode()
    {
        return operationController.gameMode;
    }
    /// <summary>
    /// 设置选中热点
    /// </summary>
    /// <param name="baseHotPoint"></param>
    public void Set_SelectedHotPoint(BaseHotPoint baseHotPoint)
    {
        operationController.SelectedHotPoint = baseHotPoint;
    }

    /// <summary>
    /// 中途退出
    /// </summary>
    public void QuitMidway()
    {
        operationController.QuitMidway();
    }

    /// <summary>
    /// 上传训练结果
    /// </summary>
    public void UploadRes()
    {
        operationController.UploadRes();
    }
    /// <summary>
    /// 将热点从列表中移除
    /// </summary>
    /// <param name="baseHotPoint"></param>
    public void RemoveOperatedHotPointTo_operatedHotPointList(BaseHotPoint baseHotPoint)
    {
        operationController.RemoveOperatedHotPointTo_operatedHotPointList(baseHotPoint);
    }
    /// <summary>
    /// 将已经操作的热点添加到列表中
    /// </summary>
    /// <param name="baseHotPoint"></param>
    public void AddOperatedHotPointTo_operatedHotPointList(BaseHotPoint baseHotPoint)
    {
        operationController.AddOperatedHotPointTo_operatedHotPointList(baseHotPoint);
    }
    /// <summary>
    /// 当前步骤
    /// </summary>
    /// <returns></returns>
    public Step GetCurrentStep()
    {
        return operationController.CurrentStep;
    }
    /// <summary>
    /// 根据ID获取题目
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public TopicData GetTopicDataForID(string id)
    {
        return operationController.GetTopicDataForID(id);
    }
    /// <summary>
    /// 获取当前步骤的题目配置
    /// </summary>
    /// <returns></returns>
    public TopicData GetTopicDataForCurrentStep()
    {

        return operationController.GetTopicDataForCurrentStep();
    }

    /// <summary>
    /// 根据压力表ID获取压力表
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public BaseMeter GetPressureMeterForID(string id)
    {
        return meterController.GetPressureMeterForID(id);
    }

    /// <summary>
    /// 根据压力表ID获取压力表读数
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public float GetPressureMeterReadingValueForID(string id)
    {
        return meterController.GetPressureMeterReadingValueForID(id);
    }

    /// <summary>
    /// 设置压力表到目标值
    /// </summary>
    /// <param name="id"></param>
    /// <param name="value"></param>
    public void SetPressureMeterToTargetReadingValue(string id, float value)
    {
        meterController.SetPressureMeterToTargetReadingValue(id, value);
    }

    /// <summary>
    /// 将动画添加到列表
    /// </summary>
    /// <param name="hydrexiaAni"></param>
    public void Add_allAnimators(HydrexiaAni hydrexiaAni)
    {
        animatorPlayerController.Add_allAnimators(hydrexiaAni);

    }
    /// <summary>
    /// 根据动画名称播放动画
    /// </summary>
    /// <param name="aniNameList"></param>
    public void PlayAniForAniNameList(List<string> aniNameList)
    {
        animatorPlayerController.PlayAniForAniNameList(aniNameList);
    }
    /// <summary>
    /// 重置所有动画，即让所有动画播放Idel动画
    /// </summary>
    public void ResetAllAni()
    {
        animatorPlayerController.ResetAllAni();
    }
    /// <summary>
    /// 将工具添加到字典中
    /// </summary>
    /// <param name="baseTool"></param>
    public void AddToolIntoDic(BaseTool baseTool)
    {
        toolsController.AddToolIntoDic(baseTool);
    }
    /// <summary>
    /// 设置工具可点击状态
    /// </summary>
    /// <param name="baseToolIdList"></param>
    /// <param name="operability"></param>
    public void SetToolOperability(List<string> baseToolIdList, bool operability)
    {

        toolsController.SetToolOperability(baseToolIdList, operability);
    }
    /// <summary>
    /// 设置工具可点击状态
    /// </summary>
    /// <param name="hotPointIdList"></param>
    /// <param name="toolId"></param>
    public void SetToolOperability(string toolId, bool operability)
    {
        toolsController.SetToolOperability(toolId, operability);
    }


    /// <summary>
    /// 设置工具显示隐藏状态
    /// </summary>
    /// <param name="baseToolIdList"></param>
    /// <param name="activeState"></param>
    public void SetBaseToolActive(List<string> baseToolIdList, bool activeState)
    {
        toolsController.SetBaseToolActive(baseToolIdList, activeState);
    }

    /// <summary>
    /// 设置工具显示隐藏状态
    /// </summary>
    /// <param name="hotPointId"></param>
    /// <param name="activeState"></param>
    /// <returns>返回是否存在该ID的热点</returns>
    public bool SetBaseToolActive(string hotPointId, bool activeState)
    {
       return toolsController.SetBaseToolActive(hotPointId, activeState);

    }
    /// <summary>
    /// 重置所有工具到初始状态
    /// </summary>
    public void SetAllBaseToolToStartState()
    {
        toolsController.SetAllBaseToolToStartState();
    }
    /// <summary>
    /// 重置所有工具到初始状态，用于退出时调用
    /// </summary>
    public void SetAllBaseToolToStartState_ForQuite()
    {
        toolsController.SetAllBaseToolToStartState_ForQuite();
    }

    /// <summary>
    /// 根据ID获取热点
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public BaseTool GetBaseToolForID(string id)
    {
        return toolsController.GetBaseToolForID(id);
    }
    /// <summary>
    /// 获取当前选中的热点
    /// </summary>
    /// <returns></returns>
    public BaseTool Get_currentSelectedTool()
    {
        return toolsController.Get_currentSelectedTool();
    }
    /// <summary>
    /// 设置当前选中的工具
    /// </summary>
    /// <param name="baseTool"></param>
    public void Set_currentSelectedTool(BaseTool baseTool)
    {
        toolsController.Set_currentSelectedTool(baseTool);
    }
    /// <summary>
    /// 根据工具ID选中工具
    /// </summary>
    /// <param name="ToolName"></param>
    public void SelectToolForToolID(string ToolID, bool isSelect)
    {
        toolsController.IsSelectToolForToolID(ToolID, isSelect);

    }
    private void OnDestroy()
    {
        DestoryController();
    }
}
