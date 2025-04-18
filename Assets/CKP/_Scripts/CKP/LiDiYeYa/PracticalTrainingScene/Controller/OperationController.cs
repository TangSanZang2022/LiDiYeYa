using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using Tools.CKP;

namespace LiDi.CKP
{
    /// <summary>
    /// 模式
    /// </summary>
    public enum GameMode
    {
        /// <summary>
        /// 训练模式
        /// </summary>
        Training,
        /// <summary>
        /// 考核模式
        /// </summary>
        Appraisal
    }
    /// <summary>
    /// 实训控制器
    /// </summary>
    public class OperationController : BaseController
    {
        /// <summary>
        /// 模式
        /// </summary>
        private GameMode _gameMode;
        /// <summary>
        /// 模式
        /// </summary>
        public GameMode gameMode
        {
            get
            {
                return _gameMode;
            }
            set
            {
                _gameMode = value;
            }
        }
        /// <summary>
        /// 所有实训配置
        /// </summary>
        public Training trainingConfig;
        /// <summary>
        /// 选项配置
        /// </summary>
        private List<TopicData> topicDatasList = new List<TopicData>();
        /// <summary>
        /// 单独判断正确操作的列表
        /// </summary>
        private List<CorrectOperation> correctOperationsList = new List<CorrectOperation>();
        /// <summary>
        /// 所有实训项目物体
        /// </summary>
        private List<GameObject> trainingObjList = new List<GameObject>();
        /// <summary>
        /// 当前实训作业项目Index
        /// </summary>
        public int currentTrainingProjectIndex;
        /// <summary>
        /// 当前步骤index
        /// </summary>
        private int currentStepIndex = 0;


        /// <summary>
        /// 该步骤已操作的热点
        /// </summary>
        private List<BaseHotPoint> operatedHotPointList = new List<BaseHotPoint>();

        /// <summary>
        /// 此步骤正确的操作数据
        /// </summary>
        private CorrectOperation correctOperation;
        /// <summary>
        /// 已经操作的配置，用于单独操作时记录操作
        /// </summary>
        private CorrectOperation operationData;

        private BaseHotPoint selectedHotPoint;
        /// <summary>
        /// 已选中的热点
        /// </summary>
        public BaseHotPoint SelectedHotPoint
        {
            get
            {
                return selectedHotPoint;
            }
            set
            {
                if (selectedHotPoint != null && selectedHotPoint != value)
                {
                    selectedHotPoint.IsSelected = false;
                }
                selectedHotPoint = value;
            }
        }
        /// <summary>
        /// 当前步骤的子步骤列表
        /// </summary>
        private List<Step> currentStepChildStepsList = new List<Step>();



        private Step _currentStep;
        /// <summary>
        /// 当前步骤
        /// </summary>
        public Step CurrentStep
        {
            get
            {
                //_currentStep = trainingConfig.TrainingProjects[currentTrainingProjectIndex].Steps[currentStepIndex];
                //string indexStr = GetCurrentStepIndex(trainingConfig.TrainingProjects[currentTrainingProjectIndex].Steps);
                //currentStepIndex =int.Parse( indexStr.Split(new char[] { '_'})[0]);
                _currentStep = trainingConfig.TrainingProjects[currentTrainingProjectIndex].Steps[currentStepIndex];
                return _currentStep;
            }
        }
        private OperationErrorTip _operationErrorTip;

        private OperationErrorTip operationErrorTip
        {
            get
            {
                if (_operationErrorTip==null)
                {
                    _operationErrorTip = GameObject.Find("OperationCanvas").transform.FindChildForName<OperationErrorTip>("OperationErrorTip");
                }
                return _operationErrorTip;
            }
        }
        /// <summary>
        /// 当前子步骤，即正在执行的步骤，通过该步骤初始化场景
        /// </summary>
        //private Step CurrentChildStep
        //{
        //    get
        //    {

        //        return GetCurrentChildStep();
        //    }
        //}
        public OperationController(GameFacade gameFacade) : base(gameFacade)
        {
            trainingConfig = XmlController.ReadJsonForLitJson<Training>(Resources.Load<TextAsset>("Config/StepConfig").text);
            topicDatasList = XmlController.ReadJsonForLitJson<List<TopicData>>(Resources.Load<TextAsset>("Config/TopicConfig").text);
            correctOperationsList = XmlController.ReadJsonForLitJson<List<CorrectOperation>>(Resources.Load<TextAsset>("Config/CorrectOperationConfig").text);
            //Debug.Log(GetCurrentStepIndex(trainingConfig.TrainingProjects[currentTrainingProjectIndex].Steps));
            //InitSceneForStep();
            Init_trainingObjList();
        }
        /// <summary>
        /// 初始化实训物体列表
        /// </summary>

        private void Init_trainingObjList()
        {
            for (int i = 0; i < trainingConfig.TrainingProjects.Count; i++)
            {
                int index = i;
                GameObject trainingObj = GameObject.Find(trainingConfig.TrainingProjects[index].TrainingName);
                if (trainingObj != null)
                {
                    trainingObjList.Add(trainingObj);
                }
                TimerTools.Instance.StartToWaitDoSomething(2, () => SetTrainingObjState(trainingConfig.TrainingProjects[index].TrainingName, false));
            }
        }
        /// <summary>
        /// 设置所有训练项物体显示隐藏状态
        /// </summary>
        /// <param name="state"></param>
        public void SetAllTrainingObjState(bool state)
        {
            for (int i = 0; i < trainingObjList.Count; i++)
            {
                int index = i;
                trainingObjList[index].SetActive(state);
            }
        }
        /// <summary>
        /// 设置每个实训内容的显示隐藏
        /// </summary>
        /// <param name="TrainingName"></param>
        /// <param name="state"></param>
        public void SetTrainingObjState(string TrainingName, bool state)
        {
            //trainingConfig.TrainingProjects.Find((trainProject)=> trainProject.TrainingName)
           GameObject trainingObj= trainingObjList.Find((g)=>g.name== TrainingName);
            if (trainingObj!=null)
            {
                trainingObj.SetActive(state);
            }

        }

        public override void OnInit()
        {
            base.OnInit();


        }

        public override void OnUpdate()
        {
            base.OnUpdate();


        }

        public override void OnDestory()
        {
            base.OnDestory();
        }



        // Start is called before the first frame update
        void Start()
        {

        }

        /// <summary>
        /// 获取当前正在执行的步骤Index
        /// </summary>
        /// <returns></returns>
        //private string GetCurrentStepIndex(List<Step> steps)
        //{
        //    string res = "";
        //    for (int i = 0; i < steps.Count; i++)
        //    {
        //        int index = i;
        //        if (!steps[index].isFinished)
        //        {
        //            res += (index.ToString() + "_");
        //            res += GetCurrentStepIndex(steps[index].ChildSteps);
        //            return res;
        //        }
        //    }
        //    return res;
        //}

        //private Step GetChildStep(Step parentStep, List<string> indexStrList)
        //{
        //    Step step = parentStep;
        //    for (int i = 0; i < indexStrList.Count; i++)
        //    {
        //        int index = i;
        //        if ( !string.IsNullOrEmpty(indexStrList[index]))
        //        {
        //            step = step.ChildSteps[int.Parse(indexStrList[index])];

        //        }
        //    }
        //    return step;
        //}
        /// <summary>
        /// 获取当前执行子的步骤
        /// </summary>
        /// <returns></returns>
        //private Step GetCurrentChildStep()
        //{

        //    string indexStr = GetCurrentStepIndex(trainingConfig.TrainingProjects[currentTrainingProjectIndex].Steps);
        //    List<string> indexStrList =new List<string>( indexStr.Split(new char[] { '_' }));
        //    Step parentStep= trainingConfig.TrainingProjects[currentTrainingProjectIndex].Steps[int.Parse(indexStrList[0])];
        //    indexStrList.RemoveAt(0);
        //    return GetChildStep(parentStep, indexStrList);
        //}







        /// <summary>
        /// 开始任务
        /// </summary>
        public void StartTask(int trainingProjectIndex, GameMode gameMode,float limitTime=-1)
        {
           
            SetAllTrainingObjState(false);
            operationData = null;
            SetTrainingObjState(trainingConfig.TrainingProjects[trainingProjectIndex].TrainingName, true);
            this.gameMode = gameMode;
            currentTrainingProjectIndex = trainingProjectIndex;
            facade.SetShouHideObjToStartState();
            TrainingProject trainingProject = trainingConfig.TrainingProjects[currentTrainingProjectIndex];
            //SceneManager.Instance.FPC.gameObject.SetActive(false);
            //SceneManager.Instance.FPC.transform.position = new Vector3((float)trainingProject.CamPosRot[0], (float)trainingProject.CamPosRot[1], (float)trainingProject.CamPosRot[2]);
            //SceneManager.Instance.FPC.transform.localEulerAngles = new Vector3(SceneManager.Instance.FPC.transform.localEulerAngles.x, (float)trainingProject.CamPosRot[3],SceneManager.Instance.FPC.transform.localEulerAngles.z);
           // SceneManager.Instance.FPC.transform.GetChild(0).transform.localEulerAngles = new Vector3((float)trainingProject.CamPosRot[4],0,0);
           // SceneManager.Instance.FPC.gameObject.SetActive(true);
            BaseTaskPanel baseTaskPanel = null;
            switch (gameMode)
            {
                case GameMode.Training:
                    baseTaskPanel = GameFacade.Instance.PushPanel(UIPanelType.TrainPanel) as BaseTaskPanel;
                    break;
                case GameMode.Appraisal:
                    baseTaskPanel = GameFacade.Instance.PushPanel(UIPanelType.AppraisalPanel) as BaseTaskPanel;
                    break;
                default:
                    break;
            }

            baseTaskPanel.Init(trainingConfig.TrainingProjects[currentTrainingProjectIndex], limitTime);
            //currentStepIndex = 0;
            InitSceneForStep();
            if (gameMode == GameMode.Training)
            {
                //facade.PlayAudio(currentTrainingProjectIndex, currentStepIndex);
            }
            ResetAllTaskStepRes();
            //
            if (operationData!=null)
            {
                operationData = null;
                
                correctOperation = correctOperationsList.Find((co) => co.TrainingProjectStepID == trainingConfig.TrainingProjects[currentTrainingProjectIndex].TrainingName + 1.ToString());
            }
         
            //operationData = null;
            //correctOperation = correctOperationsList.Find((co) => co.TrainingProjectStepID == trainingConfig.TrainingProjects[currentTrainingProjectIndex].TrainingName + (currentStepIndex + 1).ToString());
            operatedHotPointList.Clear();
            //operatedHotPointList
        }
        /// <summary>
        /// 准备开始任务
        /// </summary>
        /// <param name="trainingProjectIndex"></param>
        /// <param name="gameMode"></param>
        public void ReadToStartTask(int trainingProjectIndex, GameMode gameMode)
        {
            this.gameMode = gameMode;
            currentTrainingProjectIndex = trainingProjectIndex;
            ReadyPanel baseTaskPanel = GameFacade.Instance.PushPanel(UIPanelType.ReadyPanel) as ReadyPanel;
            baseTaskPanel.Init(trainingProjectIndex, gameMode);
            
           

        }
        /// <summary>
        /// 获取当前步骤的所有子步骤
        /// </summary>
        private void Set_currentStepChildStepsList()
        {
            // CurrentStep
        }

        /// <summary>
        /// 根据当前步骤初始化场景
        /// </summary>
        private void InitSceneForStep()
        {
            SetShowHideObjStateForStep();
            //Step step = GetCurrentChildStep();
           

            if (gameMode == GameMode.Training)
            {
                SetCorrectHotPointStateForStepForTraining();
                SetSelectableHotPointForTrainStateForStep();
            }
            if (gameMode == GameMode.Appraisal)
            {
               // SetCorrectHotPointStateForStepForAppraisal();

                SetSelectableToolPointStateForStep();
                SetSelectableHotPointStateForStep();
            }
            //Debug.Log(step.StepName);
        }

        /// <summary>
        /// 根据当前子步骤，设置正确热点的状态，包括可点击，以及得显示,用于考核模式
        /// </summary>
        private void SetCorrectHotPointStateForStepForAppraisal()
        {

          
            //int index = operatedHotPointList.Count;

            ////====2025.04.09====修改，应该先显示工具
            //if (GameFacade.Instance.SetBaseToolActive(CurrentStep.TrainToolID[index], true))//如果应该操作的热点需要工具，则先设置工具的状态
            //{
            //    GameFacade.Instance.SetToolOperability(CurrentStep.TrainToolID[index], true);
            //}
          
        }
        /// <summary>
        /// 根据当前子步骤，设置正确热点的状态，包括可点击，以及得显示,用于训练模式
        /// </summary>
        private void SetCorrectHotPointStateForStepForTraining()
        {

            //GameFacade.Instance.SetHotPointActive(CurrentStep.TrainHotPointID, true);
            //GameFacade.Instance.SetHotPointOperability(CurrentStep.TrainHotPointID, true);

            int index = operatedHotPointList.Count;

            //====2025.04.09====修改，应该先显示工具
            if (GameFacade.Instance.SetBaseToolActive(CurrentStep.TrainToolID[index], true))//如果应该操作的热点需要工具，则先设置工具的状态
            {
                GameFacade.Instance.SetToolOperability(CurrentStep.TrainToolID[index], true);
            }
            //下方因该修改到选中了工具之后再显示热点
            else//否则直接设置热点状态
            {
                SetCurrentHotPointActiveStateForTraining(true);
            }


        }
        /// <summary>
        /// 训练状态时，选中了当前工具时，设置接下来要操作的热点
        /// </summary>
        public void SetCurrentHotPointActiveStateForTraining(bool active)
        {
            int index = operatedHotPointList.Count;
            GameFacade.Instance.SetHotPointActive(CurrentStep.TrainHotPointID[index], active);
            GameFacade.Instance.SetHotPointOperability(CurrentStep.TrainHotPointID[index], active);
          
        }
        /// <summary>
        ///考核状态时，选中了当前工具时，设置接下来要操作的热点
        /// </summary>
        //public void SetCurrentHotPointActiveStateForAppraisal(bool active)
        //{
        //    int index = operatedHotPointList.Count;
        //    GameFacade.Instance.SetHotPointActive(CurrentStep.TrainHotPointID[index], active);
        //    GameFacade.Instance.SetHotPointOperability(CurrentStep.TrainHotPointID[index], active);

        //}
        /// <summary>
        /// 根据当前步骤，设置可操作热点的状态，包括是否可点击，以及显示隐藏状态，用于训练模式
        /// </summary>
        private void SetSelectableHotPointForTrainStateForStep()
        {
            GameFacade.Instance.SetHotPointActive(CurrentStep.SelectableHotPointIDForTrain, true);
            GameFacade.Instance.SetHotPointOperability(CurrentStep.SelectableHotPointIDForTrain, true);
        }
        /// <summary>
        /// 根据当前步骤，设置可操作热点的状态，包括是否可点击，以及显示隐藏状态，用于考核模式
        /// </summary>
        private void SetSelectableHotPointStateForStep()
        {
            GameFacade.Instance.SetHotPointActive(CurrentStep.SelectableHotPointID, true);
            GameFacade.Instance.SetHotPointOperability(CurrentStep.SelectableHotPointID, true);
            
        }
        /// <summary>
        /// 根据当前步骤，设置可操作工具的状态，包括是否可点击，以及显示隐藏状态，用于考核模式
        /// </summary>
        private void SetSelectableToolPointStateForStep()
        {
            GameFacade.Instance.SetBaseToolActive(CurrentStep.SelectableToolID, true);
            GameFacade.Instance.SetToolOperability(CurrentStep.SelectableToolID, true);
        }
        /// <summary>
        /// 根据当前步骤，显示出需要显示的物体
        /// </summary>
        private void SetShowHideObjStateForStep()
        {
            GameFacade.Instance.ShowObjForIDList(CurrentStep.ShowHideObjID);
        }

        /// <summary>
        /// 将热点从列表中移除
        /// </summary>
        /// <param name="baseHotPoint"></param>
        public void RemoveOperatedHotPointTo_operatedHotPointList(BaseHotPoint baseHotPoint)
        {
            baseHotPoint.SelectedToolID = "";
            operatedHotPointList.Remove(baseHotPoint);
        }
        /// <summary>
        /// 将已经操作的热点添加到列表中
        /// </summary>
        /// <param name="baseHotPoint"></param>
        public void AddOperatedHotPointTo_operatedHotPointList(BaseHotPoint baseHotPoint)
        {
            operatedHotPointList.Add(baseHotPoint);
            if (!string.IsNullOrEmpty(baseHotPoint.SelectedToolID))//存在选中的工具
            {
                GameFacade.Instance.GetBaseToolForID(baseHotPoint.SelectedToolID).PutDownAfterUse();
            }
            if (gameMode == GameMode.Training)
            {
                if (JudgeSelectableHotPointForTrain(baseHotPoint.id))
                {
                    operationErrorTip.gameObject.SetActive(true);
                    operatedHotPointList.Remove(baseHotPoint);
                    //ToDo
                    SetCorrectHotPointStateForStepForTraining();
                    return;
                }

                if (!baseHotPoint.id.ToLower().Contains("topic"))
                {
                    baseHotPoint.SetNeedClick(false); 
                }
                if ( operatedHotPointList.Count < CurrentStep.TrainHotPointID.Count)
                {
                    TimerTools.Instance.StartToWaitDoSomething(1, () => SetCorrectHotPointStateForStepForTraining());
                }
            }
            if (CurrentStep.CorrectHotPointID.Count==0)//如果步骤配置中，正确热点长度为0，则说明需要单独记录来判断
            {
                if (operationData==null)
                {
                    operationData = new CorrectOperation();
                    //Debug.Log(trainingConfig.TrainingProjects[currentTrainingProjectIndex].TrainingName + currentStepIndex.ToString());
                    correctOperation = correctOperationsList.Find((co) => co.TrainingProjectStepID == trainingConfig.TrainingProjects[currentTrainingProjectIndex].TrainingName + (currentStepIndex+1).ToString());
                }
                Set_operationData();
            }
            Debug.Log(operatedHotPointList.Count);
            if (JudgeToNextStep())//如果可以进行下一步
            {

                GameFacade.Instance.SetHotPointOperability(CurrentStep.TrainHotPointID, false);
                ResetAllMeterReadingValueToCorrect();
                ResetAllClickableHotPointValueStateToCorrect();
                ResetAllSelectableHotPointValueStateToCorrect();

                if (gameMode == GameMode.Appraisal)
                {
                    SetThisStepSelectableHotPointToStartState();
                }
                ResetHotPointIn_operatedHotPointList();
                PlayCurrentStepAni();
                // NextStep();
            }
            else
            {

                Debug.Log("无法进行下一步");

            }
           
        }

        /// <summary>
        /// 检测是否是点击了可选择热点
        /// </summary>
        private bool JudgeSelectableHotPointForTrain(string id)
        {
           
            if (CurrentStep.SelectableHotPointIDForTrain.Contains(id))
            {
                BaseHotPoint baseHotPoint = facade.GetBaseHotPointForID(id);

                TimerTools.Instance.StartToWaitDoSomething(2, () => baseHotPoint.ResetToStateBeforeClick());
                return true;
            }
            return false;
        }
        /// <summary>
        /// 重置已经操作的热点,用于下一步的重置
        /// </summary>
        public void ResetHotPointIn_operatedHotPointList()
        {
            for (int i = 0; i < operatedHotPointList.Count; i++)
            {
                int index = i;
                if (!operatedHotPointList[index].id.Contains("Topic"))
                {
                    operatedHotPointList[index].ResetBeforeNextStep();
                }
            }
            operatedHotPointList.Clear();
        }

        /// <summary>
        /// 在进行下一步之前将所有可选择阀门设置为正确状态,仅用于训练模式下中途退出
        /// </summary>
        private void StopllCurrentStepHotPointHighlight_Train()
        {
            for (int i = 0; i < CurrentStep.TrainHotPointID.Count; i++)
            {
                int index = i;

              
                facade.GetBaseHotPointForID(CurrentStep.TrainHotPointID[index]).SetToStartState() ;

            }
        }
        /// <summary>
        /// 在进行下一步之前将所有可选择阀门设置为正确状态
        /// </summary>
        private void ResetAllSelectableHotPointValueStateToCorrect()
        {
            for (int i = 0; i < CurrentStep.SelectableHotPointValueState.Count; i++)
            {
                int index = i;

                bool state = CurrentStep.SelectableHotPointValueState[index].ClickableHotPointValueState.ToLower() == "open" ? true : false;
                (facade.GetBaseHotPointForID(CurrentStep.SelectableHotPointValueState[index].ClickableHotPointValueID) as ClickableHotPointValue).SetToTargetState(state);

            }
        }
      
        /// <summary>
        /// 在进行下一步之前将所有阀门设置为正确状态
        /// </summary>
        private void ResetAllClickableHotPointValueStateToCorrect()
        {
            for (int i = 0; i < CurrentStep.CorrectClickableHotPointValueState.Count; i++)
            {
                int index = i;

                bool state = CurrentStep.CorrectClickableHotPointValueState[index].ClickableHotPointValueState.ToLower() == "open" ? true : false;
                (facade.GetBaseHotPointForID(CurrentStep.CorrectClickableHotPointValueState[index].ClickableHotPointValueID) as ClickableHotPointValue).SetToTargetState(state);
               
            }
        }

        /// <summary>
        /// 在进行下一步之前将所有表读数设置为正确状态
        /// </summary>
        private void ResetAllMeterReadingValueToCorrect()
        {
                for (int i = 0; i < CurrentStep.CorrectReadingValue.Count; i++)
                {
                    int index = i;
                float targetValue = (CurrentStep.CorrectReadingValue[index].MeterReadingValue[0] + CurrentStep.CorrectReadingValue[index].MeterReadingValue[1]) / 2;
                 facade.GetPressureMeterForID(CurrentStep.CorrectReadingValue[index].MeterID).SetToTargetValue(targetValue);
               
                }
              
            
        }
        /// <summary>
        /// 设置本步骤热点到初始状态，用于考核模式
        /// </summary>
        public void SetThisStepSelectableHotPointToStartState()
        {
            for (int i = 0; i < CurrentStep.SelectableHotPointID.Count; i++)
            {
                int index = i;
                if (!CurrentStep.SelectableHotPointID[index].Contains("Topic"))
                {
                    Debug.Log(CurrentStep.SelectableHotPointID[index]);
                    facade.GetBaseHotPointForID(CurrentStep.SelectableHotPointID[index]).SetToStartState();

                  
                }
            }

          
        }
        /// <summary>
        /// 判断是否进入下一步
        /// </summary>
        /// <returns></returns>
        private bool JudgeToNextStep()
        {
            //如果正确热点列表长度不为0，则只需要判断热点顺序
            if (CurrentStep.CorrectHotPointID.Count != 0)
            {
                //当操作的热点数量和当前步骤正确热点数一样时，代表该步骤已经操作完成
                if (operatedHotPointList.Count == CurrentStep.CorrectHotPointID.Count)
                {
                    for (int i = 0; i < operatedHotPointList.Count; i++)
                    {
                        int index = i;
                        //当前操作热点ID不对应或者操作热点的工具不对
                        if (operatedHotPointList[index].id != CurrentStep.CorrectHotPointID[index].HotPointID || operatedHotPointList[index].SelectedToolID != CurrentStep.CorrectHotPointID[index].ToolID) 
                        {
                            CurrentStep.isCorrect = false;

                            Debug.Log(CurrentStep.StepName + "步骤：错误");
                            break;
                        }
                        else if (index == operatedHotPointList.Count - 1)
                        {
                            if (gameMode == GameMode.Appraisal)
                            {
                                if (CurrentStep.CorrectClickableHotPointValueState.Count != 0 && CurrentStep.CorrectReadingValue.Count != 0)
                                {
                                    CurrentStep.isCorrect = (JudgeClickableHotPointValueState() && JudgeMeterReadingValue());
                                }

                                else if (CurrentStep.CorrectReadingValue.Count != 0)//训练模式下如果有测量表读数限制，则需要判断测量表读数是否正确
                                {
                                    CurrentStep.isCorrect = JudgeMeterReadingValue();
                                }
                                else
                                {
                                    CurrentStep.isCorrect = true;

                                }

                                Debug.Log(CurrentStep.StepName + "步骤：" + CurrentStep.isCorrect.ToString()); 
                            }
                            else
                            {
                                CurrentStep.isCorrect = true;
                            }
                        }
                    }
                    CurrentStep.isFinished = true;

                    return true;
                }
                else 
                {
                    if (operatedHotPointList[operatedHotPointList.Count-1].id== CurrentStep.CorrectHotPointID[operatedHotPointList.Count - 1].HotPointID&&
                        operatedHotPointList[operatedHotPointList.Count - 1].SelectedToolID == CurrentStep.CorrectHotPointID[operatedHotPointList.Count - 1].ToolID)//ID相同则进行继续操作，说明当前还是正确的
                    {
                        Debug.Log("可以继续操作");
                        return false;
                    }
                    else
                    {
                        Debug.Log("操作热点错误，当前步骤错误");
                        CurrentStep.isFinished = true;
                        CurrentStep.isCorrect = false;
                        return true;
                    }
                }
            }
            //否则需要单独判断
            else
            {
                if (operatedHotPointList.Count== correctOperation.HotPointOrder.Count)
                {
                    CurrentStep.isCorrect = correctOperation.IsCorrect(operationData); 
                    CurrentStep.isFinished = true;
                    return true;
                }
                else
                {
                    if (operatedHotPointList[operatedHotPointList.Count - 1].id== correctOperation.HotPointOrder[operatedHotPointList.Count - 1].HotPointID&&
                        operatedHotPointList[operatedHotPointList.Count - 1].SelectedToolID == correctOperation.HotPointOrder[operatedHotPointList.Count - 1].ToolID
                        )//ID相同则进行继续操作，说明当前还是正确的
                    {
                        Debug.Log("可以继续操作");
                        return false;
                    }
                    else
                    {
                        Debug.Log("操作热点错误，当前步骤错误");
                        CurrentStep.isCorrect =false;
                        CurrentStep.isFinished = true;
                        return true;
                    }
                }
               // return operationData.IsCorrect(correctOperation);
            }

            return false;
        }


        /// <summary>
        /// 完成了所有步骤
        /// </summary>
        private void FinishedAllSteps()
        {
            facade.ResetAllAni();
            facade.SetAllHotPointToStartState();
            BaseTaskPanel baseTaskPanel = facade.GetTopPanel() as BaseTaskPanel;
            baseTaskPanel.FinishedTask();
            //throw new NotImplementedException();
            switch (facade.GetGameMode())
            {
                case GameMode.Training:
                    break;
                case GameMode.Appraisal:
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// 下一步
        /// </summary>
        private void NextStep()
        {
            if (currentTrainingProjectIndex==-1)
            {
                return;
            }
            Debug.Log("下一步");
            operationData = null;
            BaseTaskPanel baseTaskPanel = facade.GetTopPanel() as BaseTaskPanel;
            if (baseTaskPanel != null)
            {
                baseTaskPanel.SetStepItemState(currentStepIndex, (CurrentStep.isCorrect ? 1 : 0));
            }

            currentStepIndex++;//当前步骤+1
            Debug.Log("当前步骤序号currentStepIndex：" + currentStepIndex);
            Debug.Log(currentTrainingProjectIndex);
            if (currentStepIndex == trainingConfig.TrainingProjects[currentTrainingProjectIndex].Steps.Count)//所有步骤已经完成
            {
                FinishedAllSteps();
                currentStepIndex = 0;
            }
            else//进行下一步
            {
               
                InitSceneForStep();
                if (gameMode == GameMode.Training)
                {
                   // facade.PlayAudio(currentTrainingProjectIndex, currentStepIndex);
                }
                //facade.PlayAudio(currentTrainingProjectIndex, currentStepIndex);
            }
        }
        /// <summary>
        /// 播放该步骤的动画
        /// </summary>
        private void PlayCurrentStepAni()
        {
            Debug.Log("开始播放步骤：" + CurrentStep.StepName + "的动画，动画时间为：" + CurrentStep.AniTime + "秒");
            facade.PlayAniForAniNameList(CurrentStep.AniName);
            TimerTools.Instance.StartToWaitDoSomething(CurrentStep.AniTime + 1, () => NextStep());
        }

        /// <summary>
        /// 根据ID获取题目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TopicData GetTopicDataForID(string id)
        {
            TopicData topicData = topicDatasList.Find((data) => data.ID == id);
            return topicData;
        }
        /// <summary>
        /// 获取当前步骤的题目配置
        /// </summary>
        /// <returns></returns>
        public TopicData GetTopicDataForCurrentStep()
        {
            TopicData topicData = topicDatasList.Find((data) => data.ID == trainingConfig.TrainingProjects[currentTrainingProjectIndex].TrainingName + CurrentStep.StepIndex.ToString());
            return topicData;
        }

        /// <summary>
        /// 根据Index获取任务
        /// </summary>
        /// <param name="index">如果为-1，则获取当前任务</param>
        /// <returns></returns>
        public TrainingProject GetTrainingProjectForIndex(int index = -1)
        {
            if (index == -1)
            {
                index = currentTrainingProjectIndex;
            }
            if (index==-1)
            {
                return null;
            }
            return trainingConfig.TrainingProjects[index];
        }

        /// <summary>
        /// 重置所有任务步骤的得分和完成情况
        /// </summary>
        public void ResetAllTaskStepRes()
        {
            TrainingProject trainingProject = GetTrainingProjectForIndex();
            for (int i = 0; i < trainingProject.Steps.Count; i++)
            {
                int index = i;
                trainingProject.Steps[index].isFinished = false;
                trainingProject.Steps[index].isCorrect = false;
            }
        }

        /// <summary>
        /// 判断测量表读数
        /// </summary>
        /// <returns></returns>
        private bool JudgeMeterReadingValue()
        {
            bool isCorrect = false;
            for (int i = 0; i < CurrentStep.CorrectReadingValue.Count; i++)
            {
                int index = i;
                float value = facade.GetPressureMeterReadingValueForID(CurrentStep.CorrectReadingValue[index].MeterID);
                if (value < CurrentStep.CorrectReadingValue[index].MeterReadingValue[0] || value > CurrentStep.CorrectReadingValue[index].MeterReadingValue[1])
                {
                    isCorrect = false;
                    break;
                }
                else if (index == CurrentStep.CorrectReadingValue.Count - 1)
                {
                    isCorrect = true;
                }
            }
            return isCorrect;
        }



        /// <summary>
        /// 判断阀门状态是否正确
        /// </summary>
        /// <returns></returns>
        private bool JudgeClickableHotPointValueState()
        {
            bool isCorrect = false;
            for (int i = 0; i < CurrentStep.CorrectClickableHotPointValueState.Count; i++)
            {
                int index = i;

                bool state = CurrentStep.CorrectClickableHotPointValueState[index].ClickableHotPointValueState.ToLower() == "open" ? true : false;
                if ((facade.GetBaseHotPointForID(CurrentStep.CorrectClickableHotPointValueState[index].ClickableHotPointValueID) as ClickableHotPointValue).GetState() != state)
                {
                    isCorrect = false;
                    break;
                }
                else if (index == CurrentStep.CorrectClickableHotPointValueState.Count - 1)
                {
                    isCorrect = true;
                }
            }
            return isCorrect;
        }

        /// <summary>
        /// 设置操作记录数据
        /// </summary>
        private void Set_operationData()
        {

            //已经操作的热点个数
            int operatedCount = operatedHotPointList.Count;
            string toolID =new string( operatedHotPointList[operatedCount - 1].SelectedToolID.ToCharArray());

            //operationData.HotPointOrder.Add(operatedHotPointList[operatedCount - 1].id);
            operationData.HotPointOrder.Add(new CorrectHotPointTool() {
                ToolID = toolID,
                HotPointID= operatedHotPointList[operatedCount - 1].id
            });
            //需要记录的表读数ID
            string meterID = correctOperation.CorrectReadingValue[operatedCount - 1].MeterID;

            operationData.CorrectReadingValue.Add(new MeterData()
            {
                Comment = correctOperation.CorrectReadingValue[operatedCount - 1].Comment + "记录",
                MeterID = meterID,
                MeterReadingValue = new List<float> { facade.GetPressureMeterReadingValueForID(meterID) }
            });

            Debug.Log("记录一条数据：Comment:" + operationData.CorrectReadingValue[operatedCount - 1].Comment +
                "---MeterID:" + operationData.CorrectReadingValue[operatedCount - 1].MeterID +
                "---MeterReadingValue:" + operationData.CorrectReadingValue[operatedCount - 1].MeterReadingValue[0]

                );
        }

        /// <summary>
        /// 重置所有已经操作的热点，用于中途退出
        /// </summary>
        private void ResetHotPointIn_operatedHotPointList_ForQuitMidway()
        {
            for (int i = 0; i < operatedHotPointList.Count; i++)
            {
                int index = i;
                if (!operatedHotPointList[index].id.Contains("Topic"))
                {
                    operatedHotPointList[index].SetToStartState();
                }
            }
        }
        /// <summary>
        /// 中途退出
        /// </summary>
        public void QuitMidway()
        {
            if (gameMode== GameMode.Training)
            {
                PostUpdateProgress();
            }
            //PostUpdateProgress();
            SetShowHideObjStateForStep();
            
            facade.ResetAllAni();
            
            facade.SetAllHotPointToStartState_ForQuite();
            //ResetHotPointIn_operatedHotPointList();

            //ResetAllSelectableHotPointValueStateToCorrect();
            //ResetAllClickableHotPointValueStateToCorrect();
            //ResetAllMeterReadingValueToCorrect();
            switch (gameMode)
            {
                case GameMode.Training:
                    StopllCurrentStepHotPointHighlight_Train();
                    //SetSelectableHotPointForTrainStateForStep();
                    break;
                case GameMode.Appraisal:
                    SetThisStepSelectableHotPointToStartState();
                    SetSelectableHotPointStateForStep();
                    SetSelectableToolPointStateForStep();
                    ResetAllTaskStepRes();
                    // UploadRes();
                    break;
                default:
                    break;
            }
            ResetAllTaskStepRes();
            operatedHotPointList.Clear();
            currentTrainingProjectIndex = -1;
            currentStepIndex = 0;
        }



        /// <summary>
        /// 上传训练结果
        /// </summary>
        public void UploadRes()
        {
            //todo，上传逻辑

            string res = XmlController.ObjToJsonForLitjson(GetUpLoadData());
            Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
            res = reg.Replace(res, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });


            Debug.Log(res);
            UpLoadData upLoadData = GetUpLoadData();
            string[,] scores = new string[upLoadData.gradeDetailList.Count, 3];

            for (int i = 0; i < upLoadData.gradeDetailList.Count; i++)
            {
                int index = i;
                scores[index, 0] = "";
                scores[index, 1] = upLoadData.gradeDetailList[index].score.ToString();
                scores[index, 2] = upLoadData.gradeDetailList[index].taskContent;
            }
            string uploadName = upLoadData.trainContent;//+ "考核";



            //进度
            //string progress = GetProgress();
            //PostUpdateProgress(currentTrainingProjectIndex + 2, progress);
           // Debug.Log(uploadName);
            PostUploadScores(uploadName, scores);

           
            //Debug.Log("上传结果");

            //上传完成之后重置步骤得分
            ResetAllTaskStepRes();
        }

        /// <summary>
        /// 获取成绩数据
        /// </summary>
        /// <returns></returns>
        private UpLoadData GetUpLoadData()
        {
            UpLoadData upLoadDataRes = new UpLoadData();
            upLoadDataRes.examEndDate=DateTime.Now.ToString("yyyy/MM/dd   HH:mm:ss");
           
            TrainingProject trainingProject = trainingConfig.TrainingProjects[currentTrainingProjectIndex];
            upLoadDataRes.trainContent = trainingConfig.TrainingProjects[currentTrainingProjectIndex].Comment;
            for (int i = 0; i < trainingProject.Steps.Count; i++)
            {
                int index = i;
                upLoadDataRes.gradeDetailList.Add(new GradeDeta()
                {
                    remark = "",
                    score = trainingProject.Steps[index].isCorrect ? trainingProject.Steps[index].Score : 0,
                    taskContent = trainingProject.Steps[index].StepName
                });
            }

            return upLoadDataRes;

        }
        /// <summary>
        /// 获取考核进度
        /// </summary>
        /// <returns></returns>
        private string GetProgress()
        {
            TrainingProject trainingProject = trainingConfig.TrainingProjects[currentTrainingProjectIndex];
            float totalStepCount = trainingProject.Steps.Count;
            float finishedStepCount = 0;
            for (int i = 0; i < totalStepCount; i++)
            {
                int index = i;
                if (trainingProject.Steps[index].isFinished)
                {
                    finishedStepCount++;
                }
            }
            return ((finishedStepCount / totalStepCount) * 100).ToString("F1");
        }
        /// <summary>
        /// 上传进度
        /// </summary>
        /// <param name="currentIndex"></param>
        /// <param name="currentProgress"></param>
        public void PostUpdateProgress()
        {
            int currentIndex = currentTrainingProjectIndex + 2;
            string currentProgress = GetProgress();
            Debug.Log("上传：" + currentIndex.ToString() + "进度为：" + currentProgress);
            //APIManager.PostUpdateProgress(currentIndex, currentProgress, (string result) => Debug.Log(result));
            SetAllTrainingObjState(false);
        }
        /// <summary>
        /// 上传成绩
        /// </summary>
        /// <param name="uploadName"></param>
        /// <param name="scores"></param>
        private void PostUploadScores(string uploadName, string[,] scores)
        {
            Debug.Log("上传：" + uploadName + "成绩");
            for (int i = 0; i < scores.GetLength(0); i++)//行
            {
                for (int j = 0; j < scores.GetLength(1); j++)//列
                {
                    Debug.Log(scores[i, j]);
                }
            }

            SetAllTrainingObjState(false);
            //APIManager.PostUploadScores(uploadName, scores, (string result)=>Debug.Log(result));
        }
    }
    /// <summary>
    /// 步骤
    /// </summary>
    [Serializable]
    public class Training
    {
        public List<TrainingProject> TrainingProjects = new List<TrainingProject>();
    }
    /// <summary>
    /// 实训项目
    /// </summary>
    [Serializable]
    public class TrainingProject
    {
        /// <summary>
        /// 说明
        /// </summary>
        public string Comment;
        /// <summary>
        /// 考核模式时
        /// </summary>
        public string CommentForAppraisal;
        /// <summary>
        /// 实训名称
        /// </summary>
        public string TrainingName;
        /// <summary>
        /// 相机初始位置旋转
        /// </summary>
        public List<double> CamPosRot = new List<double>();
        /// <summary>
        /// 该实训的步骤
        /// </summary>
        public List<Step> Steps = new List<Step>();
    }
    /// <summary>
    /// 每一步操作热点对应的工具类，用于判断此步骤操作热点的工具是否正确
    /// </summary>
    [Serializable]
    public class CorrectHotPointTool
    {
        /// <summary>
        /// 工具ID
        /// </summary>
        public string ToolID;
        /// <summary>
        /// 热点ID
        /// </summary>
        public string HotPointID;
    }
    [Serializable]
    /// <summary>
    /// 实训步骤
    /// </summary>
    public class Step
    {
        /// <summary>
        /// 步骤序号
        /// </summary>
        public int StepIndex;
        /// <summary>
        /// 步骤名称    
        /// </summary>
        public string StepName;
        /// <summary>
        /// 提示文字
        /// </summary>
        public string HintText;
        /// <summary>
        /// 相机位置坐标
        /// </summary>
        public List<float> CamPos = new List<float>();
        /// <summary>
        /// 正确触发热点ID列表,用于检测是否正确
        /// </summary>
        public List<CorrectHotPointTool> CorrectHotPointID = new List<CorrectHotPointTool>();
        /// <summary>
        /// 正确阀门状态
        /// </summary>
        public List<CorrectClickableHotPointValueStateData> CorrectClickableHotPointValueState = new List<CorrectClickableHotPointValueStateData>();


        /// <summary>
        /// 测量表正确读数
        /// </summary>
        public List<MeterData> CorrectReadingValue = new List<MeterData>();

        
        /// <summary>
        /// 训练模式显示的工具
        /// </summary>
        public List<string> TrainToolID = new List<string>();
        /// <summary>
        /// 训练模式显示的热点
        /// </summary>
        public List<string> TrainHotPointID = new List<string>();
        /// <summary>
        /// 可选择的热点列表，用于训练模式
        /// </summary>
        public List<string> SelectableHotPointIDForTrain = new List<string>();
        /// <summary>
        /// 可选择的工具列表，用于训练模式
        /// </summary>
        public List<string> SelectableToolIDForTrain = new List<string>();

        
        /// <summary>
        /// 可选择的热点列表，用于考核模式
        /// </summary>
        public List<string> SelectableHotPointID = new List<string>();
        /// <summary>
        /// 可选择的工具列表，用于考核模式
        /// </summary>
        public List<string> SelectableToolID = new List<string>();
        /// <summary>
        /// 可选择阀门正确状态，用于初始化下一步
        /// </summary>
        public List<CorrectClickableHotPointValueStateData> SelectableHotPointValueState = new List<CorrectClickableHotPointValueStateData>();
        /// <summary>
        /// 本步骤分数
        /// </summary>
        public float Score;
        /// <summary>
        /// 动画名称
        /// </summary>
        public List<string> AniName = new List<string>();
        /// <summary>
        /// 动画持续时间，等待播放完动画之后才能进入下一步
        /// </summary>
        public float AniTime;
        /// <summary>
        /// 显示隐藏物体ID
        /// </summary>
        public List<string> ShowHideObjID = new List<string>();
        /// <summary>
        /// 是否正确
        /// </summary>
        public bool isCorrect;

        /// <summary>
        /// 是否完成
        /// </summary>
        public bool isFinished;

    }
    [Serializable]
    /// <summary>
    /// 表数据
    /// </summary>
    public class MeterData
    {
        /// <summary>
        /// 描述
        /// </summary>
        public string Comment;
        /// <summary>
        /// 压力表ID
        /// </summary>
        public string MeterID;
        /// <summary>
        /// 正确读数范围
        /// </summary>
        public List<float> MeterReadingValue = new List<float>();
    }
    [SerializeField]
    /// <summary>
    /// 正确阀门状态
    /// </summary>
    public class CorrectClickableHotPointValueStateData
    {
        /// <summary>
        /// 阀门ID
        /// </summary>
        public string ClickableHotPointValueID;
        /// <summary>
        /// 阀门正确状态open为开，close为关
        /// </summary>
        public string ClickableHotPointValueState;
    }
   
    [Serializable]
    /// <summary>
    /// 正确操作配置，需要单独判断
    /// </summary>
    public class CorrectOperation
    {
        /// <summary>
        /// 考核项目的步骤
        /// </summary>
        public string TrainingProjectStepID;
        /// <summary>
        /// 操作热点的顺序
        /// </summary>
        public List<CorrectHotPointTool> HotPointOrder=new List<CorrectHotPointTool>();
        /// <summary>
        /// 正确表的读数，每点击一次HotPointOrder中的热点之后就记录一次
        /// </summary>
       public List<MeterData> CorrectReadingValue = new List<MeterData>();
        /// <summary>
        /// 判断是否正确
        /// </summary>
        /// <param name="correctOperationOperationed"></param>
        /// <returns></returns>
        public bool IsCorrect(CorrectOperation correctOperationOperationed)
        {
            bool isCorrect = false;
            int count = correctOperationOperationed.HotPointOrder.Count;
            if (count == HotPointOrder.Count)//首先热点数量要相等
            {
                for (int i = 0; i < count; i++)
                {
                    int index = i;
                    if (correctOperationOperationed.HotPointOrder[index].HotPointID!= HotPointOrder[index].HotPointID||
                        correctOperationOperationed.HotPointOrder[index].ToolID != HotPointOrder[index].ToolID
                        )//有不一样的则说明操作错误,====2025.04.09====添加判断工具是否正确
                    {
                        Debug.Log("第"+ index + "热点顺序错误：" + correctOperationOperationed.HotPointOrder[index] + "----" + HotPointOrder[index]);
                        break;
                    }
                    else if (index== count-1)//说明操作的热点步骤正确了，再判断表的读数是否正确
                    {
                        if (GameFacade.Instance.GetGameMode()==GameMode.Appraisal)//只有在考核模式的时候才判断读数
                        {
                            int meterReadingValueCount = correctOperationOperationed.CorrectReadingValue.Count;//记录的表和读数长度
                            for (int j = 0; j < meterReadingValueCount; j++)
                            {
                                int meterIndex = j;
                                float value = correctOperationOperationed.CorrectReadingValue[meterIndex].MeterReadingValue[0];
                                if (value < CorrectReadingValue[meterIndex].MeterReadingValue[0] || value > CorrectReadingValue[meterIndex].MeterReadingValue[1])//读数不在区间内，则错误
                                {
                                    Debug.Log("读数错误：value：" + value + "----" + CorrectReadingValue[meterIndex].MeterReadingValue[0] + "~" + CorrectReadingValue[meterIndex].MeterReadingValue[1]);
                                    break;
                                }
                                else if (meterIndex == meterReadingValueCount - 1)
                                {
                                    isCorrect = true;
                                }
                            }
                        }
                        else
                        {
                            isCorrect = true;
                        }
                      
                    }
                } 
            }
            return isCorrect;
        }
    }
    [Serializable]
    /// <summary>
    /// 上传数据
    /// </summary>
    public class UpLoadData
    {
        /// <summary>
        /// 考核结束时间
        /// </summary>
        public string examEndDate;
        /// <summary>
        /// 每一项成绩列表
        /// </summary>
        public List<GradeDeta> gradeDetailList = new List<GradeDeta>();
        /// <summary>
        /// 学生ID
        /// </summary>
        public string studentId;
        /// <summary>
        /// 科目
        /// </summary>
        public string trainContent;
    }
    [Serializable]
    /// <summary>
    /// 每一项成绩
    /// </summary>
    public class GradeDeta
    {
        /// <summary>
        /// 备注
        /// </summary>
        public string remark;
        /// <summary>
        /// 得分
        /// </summary>
        public double score;
        /// <summary>
        /// 得分
        /// </summary>
        public string taskContent;
        
    }


}