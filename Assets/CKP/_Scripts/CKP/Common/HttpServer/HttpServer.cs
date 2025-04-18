using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Threading;
using System.IO;

namespace UnityHttpServer
{
    /// <summary>
    /// http 监听 服务类
    /// </summary>
    public class HttpServer
    {
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
        string _ListernUrl = string.Empty;

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
        /// 
        /// </summary>
        /// <param name="listernUrl"></param>
        public HttpServer(string listernUrl)
        {
            this._ListernUrl = listernUrl;
            _Listerner = new HttpListener();
        }

        
        /// <summary>
        /// 启动服务
        /// </summary>
        public void Start()
        {
            
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
                LogHelp.Info(string.Format("空框检测http通讯服务监听启动异常，异常信息{0}...", ex));
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
                   //处理获取的数据
                }
                catch (Exception ex)
                {
                    resCode.ResponseCode = "2";
                    resCode.ResponseMsg = "服务器内部错误";
                    LogHelp.Info(string.Format("空框检测http通讯服务监听接受请求异常，异常信息{0}...", ex));
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
                            LogHelp.Info(string.Format("收到空框检测数据 : {0}", ReceiveResult));
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
                        LogHelp.Info(string.Format("空框检测http通讯服务接受参数为空..."));
                        this.Write(resCode);
                        return;
                    }

                }
                catch (Exception ex)
                {
                    resCode.ResponseCode = "1";
                    resCode.ResponseMsg = "参数错误";
                    LogHelp.Info(string.Format("空框检测http通讯服务接受参数为空...", ex));
                    this.Write(resCode);
                    return;
                }

                //创建回复PLC空框检测的结果
                //EmptyPlateDetectionResCmd entity = new EmptyPlateDetectionResCmd();

                //if (entity != null)
                //{

                //    //把获得的图片信息，补全到指令信息里面
                //    entity.DetectionResult = Convert.ToByte(data.ResIsEmpBox);
                //    this.Write(resCode);

                //    //发送PLC
                //    MqManager.Publish("EmptyPlateDetectionResCmdConsumer", entity);

                //}

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
                LogHelp.Info(string.Format("_HttpResponse已关闭"));
            }
            else
            {
                LogHelp.Info(string.Format("_HttpResponse为空，无法关闭"));
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
                LogHelp.Info(string.Format("回复客户端异常，异常信息{0}...", ex));
                _HttpResponse.Close();
            }


        }

        

    }
}
