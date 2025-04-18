using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityHttpServer;
/// <summary>
/// Http客户端控制器
/// </summary>
public class HttpClientController : BaseController
{
    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="gameFacade"></param>
    public HttpClientController(GameFacade gameFacade) : base(gameFacade)
    {

    }

    /// <summary>
    /// 消息处理类
    /// </summary>
    private Message message = new Message();

    /// <summary>
    /// http 监听器
    /// </summary>
    HttpListener _Listerner = null;

    /// <summary>
    /// 
    /// </summary>
    private HttpListenerContext _HttpContext;
    private HttpListenerRequest _HttpRequest;
    private HttpListenerResponse _HttpResponse;

    /// <summary>
    /// 监听地址
    /// </summary>
    const string _ListernUrl = "";

    /// <summary>
    /// 是否监听
    /// </summary>
    bool _IsRunning = false;

    /// <summary>
    /// 接收的服务器发送过来的字符串
    /// </summary>
    public string ReceiveResult
    {
        get;
        private set;

    }
    /// <summary>
    /// 启动服务
    /// </summary>
    public void Start()
    {
        //string testJson = "{\"ActionCode\":\"1\",\"StateInfos\":[{\"SubsectionNum\":\"1\",\"IsRunning\":1},{\"SubsectionNum\":\"20\",\"IsRunning\":0}]}";
        //string testJson = "{\"ActionCode\":\"2\",\"data\":\"[{\"id\":\"1\",\"path\":\"rtsp://admin:a12345678@192.168.3.55:554/h264/ch1/main/av_stream \"}," +
        //    "{\"id\":\"2\",\"path\":\"rtsp://admin:a12345678@192.168.3.55:553/h264/ch1/main/av_stream \"}" +
        //    ",{\"id\":\"3\",\"path\":\"rtsp://admin:a12345678@192.168.3.55:554/h264/ch1/main/av_stream \"}" +
        //    ",{\"id\":\"4\",\"path\":\"rtsp://admin:a12345678@192.168.3.55:554/h264/ch1/main/av_stream \"}]\"}";
       // string path = Application.streamingAssetsPath + @"//ConveyorBeltMonitorConfig 1.txt";
       // string localJson = File.ReadAllText(path);
        //string testJson = "{\"ActionCode\":\"2\",\"data\":\"\"aa\"\"}";
        //message.ReadMessageForTest(localJson, ProccessDataCallBack);
        

        try
        {
            // _Listerner.AuthenticationSchemes = AuthenticationSchemes.Anonymous;//指定身份验证 Anonymous匿名访问
            _Listerner.Prefixes.Add(_ListernUrl);
            _Listerner.Start();

            //_Listerner.BeginGetContext(TaskProc,null);

            _Listerner.BeginGetContext(new AsyncCallback(TaskProc), null);

            _IsRunning = true;


        }
        catch (Exception ex)
        {
            Debug.Log(string.Format("空框检测http通讯服务监听启动异常，异常信息{0}...", ex));
            return;
        }
    }


    private void TaskProc(IAsyncResult ar)
    {
        //继续异步监听
        // _Listerner.BeginGetContext(TaskProc, null);
        _Listerner.BeginGetContext(new AsyncCallback(TaskProc), null);


        _HttpContext = _Listerner.EndGetContext(ar);
        _HttpRequest = _HttpContext.Request;
        _HttpResponse = _HttpContext.Response;

        _HttpContext.Response.ContentType = "text/plain;charset=UTF-8";//告诉客户端返回的ContentType类型为纯文本格式，编码为UTF-8
        _HttpContext.Response.AddHeader("Content-type", "text/plain");//添加响应头信息
        _HttpContext.Response.ContentEncoding = Encoding.UTF8;

        #region http接收POST参数
        // string receiveResult = string.Empty;
        ReceiveResult = string.Empty;
        // EmptyPlateDetectionResult data = null; ;
        ResCode resCode = null;

        if (_HttpRequest.HttpMethod.ToUpper() == "POST" && _HttpRequest.InputStream != null)
        {
            resCode = new ResCode();
            //接收POST参数
            try
            {
                Stream stream = _HttpRequest.InputStream;
                System.IO.StreamReader reader = new System.IO.StreamReader(stream, Encoding.UTF8);
                String body = reader.ReadToEnd();
                ReceiveResult = body.ToString();
                message.ReadMessageForHttp(ReceiveResult, ProccessDataCallBack);
            }
            catch (Exception ex)
            {
                resCode.ResponseCode = "2";
                resCode.ResponseMsg = "服务器内部错误";
                Debug.Log(string.Format("空框检测http通讯服务监听接受请求异常，异常信息{0}...", ex));
                this.Write(resCode);
                return;
            }

            try
            {
                if (!string.IsNullOrEmpty(ReceiveResult))
                {
                    //心跳数据
                    if (ReceiveResult.Contains("Status"))
                    {
                        resCode.ResponseCode = "0";
                        resCode.ResponseMsg = "成功，收到心跳数据，";
                        this.Write(resCode);
                        return;
                    }
                    else
                    {
                        Debug.Log(string.Format("收到空框检测数据 : {0}", ReceiveResult));
                        resCode.ResponseCode = "0";
                        resCode.ResponseMsg = "成功,收到空框检测数据";
                        //空框检测图片数据
                        // data = Newtonsoft.Json.JsonConvert.DeserializeObject<EmptyPlateDetectionResult>(receiveResult);
                    }

                }
                else
                {
                    resCode.ResponseCode = "1";
                    resCode.ResponseMsg = "参数错误";
                    Debug.Log(string.Format("空框检测http通讯服务接受参数为空..."));
                    this.Write(resCode);
                    return;
                }

            }
            catch (Exception ex)
            {
                resCode.ResponseCode = "1";
                resCode.ResponseMsg = "参数错误";
                Debug.Log(string.Format("空框检测http通讯服务接受参数为空...", ex));
                this.Write(resCode);
                return;
            }

        }
        #endregion

    }
    /// <summary>
    /// 关闭监听
    /// </summary>
    public void Stop()
    {
        if (_HttpResponse != null)
        {
            _HttpResponse.Close();
            Debug.Log(string.Format("_HttpResponse已关闭"));
        }
        else
        {
            Debug.Log(string.Format("_HttpResponse为空，无法关闭"));
        }
    }
    /// <summary>
    /// 写客户端
    /// </summary>
    /// <param name="msg"></param>
    public void Write(ResCode msg)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(_HttpResponse.OutputStream, Encoding.UTF8))
            {
                //var response = Newtonsoft.Json.JsonConvert.SerializeObject(msg);
                //writer.Write(response);
                ////writer.Write(string.Format("后台收到的处理结果:  {0}<br/>", response));
                //writer.Close();
                _HttpResponse.Close();
            }
        }
        catch (Exception ex)
        {
            Debug.Log(string.Format("回复客户端异常，异常信息{0}...", ex));
            _HttpResponse.Close();
        }


    }

    public override void OnInit()
    {
        base.OnInit();
        _Listerner = new HttpListener();
        Start();//开始监听

    }
    /// <summary>
    /// 接收消息之后的回调
    /// </summary>
    /// <param name="data"></param>
    private void ProccessDataCallBack(ActionCode actionCode, string data)
    {
        facade.HandleMsg(actionCode, data);
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    string testJson = "{\"ActionCode\":\"1\",\"StateInfos\":[{\"SubsectionNum\":\"X\",\"IsRunning\":0,\"IsOcclusio_Ph1\":1,\"IsOcclusio_Ph2\":0}" +
        //        ",{\"SubsectionNum\":\"A1\",\"IsRunning\":0,\"IsOcclusio_Ph1\":1,\"IsOcclusio_Ph2\":0,\"IsAlarm\":1},{\"SubsectionNum\":\"A2\",\"IsRunning\":1,\"IsOcclusio_Ph1\":1,\"IsOcclusio_Ph2\":0},{\"SubsectionNum\":\"A3\",\"IsRunning\":1,\"IsOcclusio_Ph1\":1,\"IsOcclusio_Ph2\":0}" +
        //        ",{\"SubsectionNum\":\"A4\",\"IsRunning\":0,\"IsOcclusio_Ph1\":1,\"IsOcclusio_Ph2\":0,\"IsAlarm\":1},{\"SubsectionNum\":\"A5\",\"IsRunning\":1,\"IsOcclusio_Ph1\":1,\"IsOcclusio_Ph2\":0},{\"SubsectionNum\":\"A6\",\"IsRunning\":1,\"IsOcclusio_Ph1\":1,\"IsOcclusio_Ph2\":0}" +
        //        ",{\"SubsectionNum\":\"A7\",\"IsRunning\":1,\"IsOcclusio_Ph1\":1,\"IsOcclusio_Ph2\":0,\"IsAlarm\":1},{\"SubsectionNum\":\"A8\",\"IsRunning\":1,\"IsOcclusio_Ph1\":1,\"IsOcclusio_Ph2\":0},{\"SubsectionNum\":\"A9\",\"IsRunning\":1,\"IsOcclusio_Ph1\":1,\"IsOcclusio_Ph2\":0}" +
        //        ",{\"SubsectionNum\":\"A10\",\"IsRunning\":1,\"IsOcclusio_Ph1\":1,\"IsOcclusio_Ph2\":0},{\"SubsectionNum\":\"A11\",\"IsRunning\":1,\"IsOcclusio_Ph1\":1,\"IsOcclusio_Ph2\":0},{\"SubsectionNum\":\"A12\",\"IsRunning\":1,\"IsOcclusio_Ph1\":1,\"IsOcclusio_Ph2\":0}" +
        //        ",{\"SubsectionNum\":\"A13\",\"IsRunning\":1,\"IsOcclusio_Ph1\":1,\"IsOcclusio_Ph2\":0},{\"SubsectionNum\":\"B1\",\"IsRunning\":1,\"IsOcclusio_Ph1\":1,\"IsOcclusio_Ph2\":0},{\"SubsectionNum\":\"B2\",\"IsRunning\":1,\"IsOcclusio_Ph1\":1,\"IsOcclusio_Ph2\":0}" +
        //        ",{\"SubsectionNum\":\"B3\",\"IsRunning\":1,\"IsOcclusio_Ph1\":1,\"IsOcclusio_Ph2\":0},{\"SubsectionNum\":\"B4\",\"IsRunning\":1,\"IsOcclusio_Ph1\":1,\"IsOcclusio_Ph2\":0},{\"SubsectionNum\":\"B5\",\"IsRunning\":1,\"IsOcclusio_Ph1\":1,\"IsOcclusio_Ph2\":0}" +
        //        ",{\"SubsectionNum\":\"B6\",\"IsRunning\":1,\"IsOcclusio_Ph1\":1,\"IsOcclusio_Ph2\":0},{\"SubsectionNum\":\"B7\",\"IsRunning\":1,\"IsOcclusio_Ph1\":1,\"IsOcclusio_Ph2\":0},{\"SubsectionNum\":\"B8\",\"IsRunning\":1,\"IsOcclusio_Ph1\":1,\"IsOcclusio_Ph2\":0}" +
        //        ",{\"SubsectionNum\":\"B9\",\"IsRunning\":1,\"IsOcclusio_Ph1\":1,\"IsOcclusio_Ph2\":0},{\"SubsectionNum\":\"B10\",\"IsRunning\":1,\"IsOcclusio_Ph1\":1,\"IsOcclusio_Ph2\":0},{\"SubsectionNum\":\"B11\",\"IsRunning\":1,\"IsOcclusio_Ph1\":1,\"IsOcclusio_Ph2\":0}" +
        //        ",{\"SubsectionNum\":\"B12\",\"IsRunning\":1,\"IsOcclusio_Ph1\":1,\"IsOcclusio_Ph2\":0},{\"SubsectionNum\":\"B13\",\"IsRunning\":1,\"IsOcclusio_Ph1\":1,\"IsOcclusio_Ph2\":0},{\"SubsectionNum\":\"C1\",\"IsRunning\":1,\"IsOcclusio_Ph1\":1,\"IsOcclusio_Ph2\":0}" +
        //        ",{\"SubsectionNum\":\"C2\",\"IsRunning\":1,\"IsOcclusio_Ph1\":1,\"IsOcclusio_Ph2\":0},{\"SubsectionNum\":\"C3\",\"IsRunning\":1,\"IsOcclusio_Ph1\":1,\"IsOcclusio_Ph2\":0},{\"SubsectionNum\":\"C4\",\"IsRunning\":1,\"IsOcclusio_Ph1\":1,\"IsOcclusio_Ph2\":0}" +
        //        ",{\"SubsectionNum\":\"D1\",\"IsRunning\":1,\"IsOcclusio_Ph1\":1,\"IsOcclusio_Ph2\":0},{\"SubsectionNum\":\"D2\",\"IsRunning\":1,\"IsOcclusio_Ph1\":1,\"IsOcclusio_Ph2\":0},{\"SubsectionNum\":\"D3\",\"IsRunning\":1,\"IsOcclusio_Ph1\":1,\"IsOcclusio_Ph2\":0}" +
        //        ",{\"SubsectionNum\":\"D4\",\"IsRunning\":1,\"IsOcclusio_Ph1\":1,\"IsOcclusio_Ph2\":0},{\"SubsectionNum\":\"D5\",\"IsRunning\":1,\"IsOcclusio_Ph1\":1,\"IsOcclusio_Ph2\":0},{\"SubsectionNum\":\"D6\",\"IsRunning\":1,\"IsOcclusio_Ph1\":1,\"IsOcclusio_Ph2\":0}" +
        //        ",{\"SubsectionNum\":\"D7\",\"IsRunning\":1,\"IsOcclusio_Ph1\":1,\"IsOcclusio_Ph2\":0}]}";
        //    //string testJson = "{\"ActionCode\":\"2\",\"Data\":[{\"id\":\"1\",\"path\":\"rtsp://admin:a12345678@192.168.3.55:555/h264/ch1/main/av_stream \"},{\"id\":\"2\",\"path\":\"rtsp://admin:a12345678@192.168.3.55:554/h264/ch1/main/av_stream \"},{\"id\":\"3\",\"path\":\"rtsp://admin:a12345678@192.168.3.55:556/h264/ch1/main/av_stream \"}]}";
        //    message.ReadMessageForHttp(testJson, ProccessDataCallBack);
        //}
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    //string testJson = "{\"ActionCode\":\"1\",\"StateInfos\":[{\"SubsectionNum\":\"X\",\"IsRunning\":0,\"IsOcclusio_Ph1\":0,\"IsOcclusio_Ph2\":1}" +
        //    //    ",{\"SubsectionNum\":\"A1\",\"IsRunning\":1,\"IsOcclusio_Ph1\":0,\"IsOcclusio_Ph2\":1},{\"SubsectionNum\":\"A2\",\"IsRunning\":0,\"IsOcclusio_Ph1\":0,\"IsOcclusio_Ph2\":1},{\"SubsectionNum\":\"A3\",\"IsRunning\":0,\"IsOcclusio_Ph1\":0,\"IsOcclusio_Ph2\":1}" +
        //    //    ",{\"SubsectionNum\":\"A4\",\"IsRunning\":1,\"IsOcclusio_Ph1\":0,\"IsOcclusio_Ph2\":1},{\"SubsectionNum\":\"A5\",\"IsRunning\":0,\"IsOcclusio_Ph1\":0,\"IsOcclusio_Ph2\":1},{\"SubsectionNum\":\"A6\",\"IsRunning\":0,\"IsOcclusio_Ph1\":0,\"IsOcclusio_Ph2\":1}" +
        //    //    ",{\"SubsectionNum\":\"A7\",\"IsRunning\":0,\"IsOcclusio_Ph1\":0,\"IsOcclusio_Ph2\":1},{\"SubsectionNum\":\"A8\",\"IsRunning\":0,\"IsOcclusio_Ph1\":0,\"IsOcclusio_Ph2\":1},{\"SubsectionNum\":\"A9\",\"IsRunning\":0,\"IsOcclusio_Ph1\":0,\"IsOcclusio_Ph2\":1}" +
        //    //    ",{\"SubsectionNum\":\"A10\",\"IsRunning\":0,\"IsOcclusio_Ph1\":0,\"IsOcclusio_Ph2\":1,\"IsAlarm\":1},{\"SubsectionNum\":\"A11\",\"IsRunning\":0,\"IsOcclusio_Ph1\":0,\"IsOcclusio_Ph2\":1},{\"SubsectionNum\":\"A12\",\"IsRunning\":0,\"IsOcclusio_Ph1\":0,\"IsOcclusio_Ph2\":1}" +
        //    //    ",{\"SubsectionNum\":\"A13\",\"IsRunning\":0,\"IsOcclusio_Ph1\":0,\"IsOcclusio_Ph2\":1,\"IsAlarm\":1},{\"SubsectionNum\":\"B1\",\"IsRunning\":0,\"IsOcclusio_Ph1\":0,\"IsOcclusio_Ph2\":1},{\"SubsectionNum\":\"B2\",\"IsRunning\":0,\"IsOcclusio_Ph1\":0,\"IsOcclusio_Ph2\":1}" +
        //    //    ",{\"SubsectionNum\":\"B3\",\"IsRunning\":0,\"IsOcclusio_Ph1\":0,\"IsOcclusio_Ph2\":1},{\"SubsectionNum\":\"B4\",\"IsRunning\":0,\"IsOcclusio_Ph1\":0,\"IsOcclusio_Ph2\":1},{\"SubsectionNum\":\"B5\",\"IsRunning\":0,\"IsOcclusio_Ph1\":0,\"IsOcclusio_Ph2\":1}" +
        //    //    ",{\"SubsectionNum\":\"B6\",\"IsRunning\":0,\"IsOcclusio_Ph1\":0,\"IsOcclusio_Ph2\":1},{\"SubsectionNum\":\"B7\",\"IsRunning\":0,\"IsOcclusio_Ph1\":0,\"IsOcclusio_Ph2\":1},{\"SubsectionNum\":\"B8\",\"IsRunning\":0,\"IsOcclusio_Ph1\":0,\"IsOcclusio_Ph2\":1}" +
        //    //    ",{\"SubsectionNum\":\"B9\",\"IsRunning\":0,\"IsOcclusio_Ph1\":0,\"IsOcclusio_Ph2\":1},{\"SubsectionNum\":\"B10\",\"IsRunning\":0,\"IsOcclusio_Ph1\":0,\"IsOcclusio_Ph2\":1},{\"SubsectionNum\":\"B11\",\"IsRunning\":0,\"IsOcclusio_Ph1\":0,\"IsOcclusio_Ph2\":1}" +
        //    //    ",{\"SubsectionNum\":\"B12\",\"IsRunning\":0,\"IsOcclusio_Ph1\":0,\"IsOcclusio_Ph2\":1},{\"SubsectionNum\":\"B13\",\"IsRunning\":0,\"IsOcclusio_Ph1\":0,\"IsOcclusio_Ph2\":1},{\"SubsectionNum\":\"C1\",\"IsRunning\":0,\"IsOcclusio_Ph1\":0,\"IsOcclusio_Ph2\":1}" +
        //    //    ",{\"SubsectionNum\":\"C2\",\"IsRunning\":0,\"IsOcclusio_Ph1\":0,\"IsOcclusio_Ph2\":1},{\"SubsectionNum\":\"C3\",\"IsRunning\":0,\"IsOcclusio_Ph1\":0,\"IsOcclusio_Ph2\":1},{\"SubsectionNum\":\"C4\",\"IsRunning\":0,\"IsOcclusio_Ph1\":0,\"IsOcclusio_Ph2\":1}" +
        //    //    ",{\"SubsectionNum\":\"D1\",\"IsRunning\":0,\"IsOcclusio_Ph1\":0,\"IsOcclusio_Ph2\":1},{\"SubsectionNum\":\"D2\",\"IsRunning\":0,\"IsOcclusio_Ph1\":0,\"IsOcclusio_Ph2\":1},{\"SubsectionNum\":\"D3\",\"IsRunning\":0,\"IsOcclusio_Ph1\":0,\"IsOcclusio_Ph2\":1}" +
        //    //    ",{\"SubsectionNum\":\"D4\",\"IsRunning\":0,\"IsOcclusio_Ph1\":0,\"IsOcclusio_Ph2\":1},{\"SubsectionNum\":\"D5\",\"IsRunning\":0,\"IsOcclusio_Ph1\":0,\"IsOcclusio_Ph2\":1},{\"SubsectionNum\":\"D6\",\"IsRunning\":0,\"IsOcclusio_Ph1\":0,\"IsOcclusio_Ph2\":1}" +
        //    //    ",{\"SubsectionNum\":\"D7\",\"IsRunning\":0,\"IsOcclusio_Ph1\":0,\"IsOcclusio_Ph2\":1}]}";
        //    ////string testJson = "{\"ActionCode\":\"2\",\"Data\":[{\"id\":\"1\",\"path\":\"rtsp://admin:a12345678@192.168.3.55:555/h264/ch1/main/av_stream \"},{\"id\":\"2\",\"path\":\"rtsp://admin:a12345678@192.168.3.55:554/h264/ch1/main/av_stream \"},{\"id\":\"3\",\"path\":\"rtsp://admin:a12345678@192.168.3.55:556/h264/ch1/main/av_stream \"}]}";
        //    //message.ReadMessageForHttp(testJson, ProccessDataCallBack);
        //    string path = Application.streamingAssetsPath + @"//MoveObjData.txt";
        //    string localJson = File.ReadAllText(path);
        //    message.ReadMessageForTest(localJson.Trim(), ProccessDataCallBack);
            
        //}
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    string path = Application.streamingAssetsPath + @"//TestBenchData.txt";
        //    string localJson = File.ReadAllText(path);
        //    message.ReadMessageForTest(localJson.Trim(), ProccessDataCallBack);
        //    string path1 = Application.streamingAssetsPath + @"//FactFinderData.txt";
        //    string localJson1 = File.ReadAllText(path1);
        //    message.ReadMessageForTest(localJson1.Trim(), ProccessDataCallBack);
        //}
        //if (Input.GetKey(KeyCode.LeftControl))
        //{
        //    if (Input.GetKeyDown(KeyCode.Q))
        //    {
        //        string path = Application.streamingAssetsPath + @"//TestBenchData1.txt";
        //        string localJson = File.ReadAllText(path);
        //        message.ReadMessageForTest(localJson.Trim(), ProccessDataCallBack);
        //    } 
        //}

        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    string path = Application.streamingAssetsPath + @"//AutomatedChannelsData.txt";
        //    string localJson = File.ReadAllText(path);
        //    message.ReadMessageForTest(localJson.Trim(), ProccessDataCallBack);
        //}
        //if (Input.GetKey(KeyCode.LeftControl))
        //{
        //    if (Input.GetKeyDown(KeyCode.W))
        //    {
        //        string path = Application.streamingAssetsPath + @"//AutomatedChannelsData1.txt";
        //        string localJson = File.ReadAllText(path);
        //        message.ReadMessageForTest(localJson.Trim(), ProccessDataCallBack);
        //    }
        //}
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    string path = Application.streamingAssetsPath + @"//PassengerData.txt";
        //    string localJson = File.ReadAllText(path);
        //    message.ReadMessageForTest(localJson.Trim(), ProccessDataCallBack);
        //}
        //if (Input.GetKey(KeyCode.LeftControl))
        //{
        //    if (Input.GetKeyDown(KeyCode.T))
        //    {
        //        string path = Application.streamingAssetsPath + @"//PassengerData1.txt";
        //        string localJson = File.ReadAllText(path);
        //        message.ReadMessageForTest(localJson.Trim(), ProccessDataCallBack);
        //    }
        //}
    }

    public override void OnDestory()
    {
        base.OnDestory();
    }
}
