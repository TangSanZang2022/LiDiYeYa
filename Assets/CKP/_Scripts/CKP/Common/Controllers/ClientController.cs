using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using UnityHttpServer;
using System;
/// <summary>
/// 客户端控制器
/// </summary>
public class ClientController : BaseController
{
    public ClientController(GameFacade gameFacade) : base(gameFacade)
    {

    }

    /// <summary>
    /// ip
    /// </summary>
    private const string ip = "";
    /// <summary>
    /// 端口
    /// </summary>
    private const int port = 8888;

    /// <summary>
    /// socket
    /// </summary>
    public Socket clientSocket;

    private Message message = new Message();
    /// <summary>
    /// 初始化
    /// </summary>
    public override void OnInit()
    {
        base.OnInit();
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            clientSocket.Connect(ip, port);
            Start();
        }
        catch (Exception)
        {
            Debug.Log(string.Format("连接服务器失败，请检查网络"));
        }


    }
    /// <summary>
    /// 开始监听
    /// </summary>
    private void Start()
    {
        clientSocket.BeginReceive(message.Data, message.StartIndex, message.RemainSize, SocketFlags.None, Callback, null);
    }
    /// <summary>
    /// 异步接收服务器消息方法
    /// </summary>
    /// <param name="iAsyncResult"></param>
    private void Callback(IAsyncResult iAsyncResult)
    {
        try
        {
            if (clientSocket == null || clientSocket.Connected == false)
            {
                return;
            }
            int count = clientSocket.EndReceive(iAsyncResult);

            message.ReadMessage(count, OnProcessDataCallback);
            Start();
        }
        catch (Exception e)
        {

            Debug.Log(e);
        }
    }
    /// <summary>
    /// 处理接收的数据
    /// </summary>
    /// <param name="msg"></param>
    private void OnProcessDataCallback(ActionCode actionCode, string msg)
    {
        facade.HandleMsg(actionCode,msg);
    }


    /// <summary>
    /// 发送数据
    /// </summary>
    /// <param name="msg"></param>
    public void SendRequest(string msg)
    {
        byte[] bytes = Message.PackData(msg);
        clientSocket.Send(bytes);
    }
    /// <summary>
    /// 删除
    /// </summary>
    public override void OnDestory()
    {
        base.OnDestory();
        try
        {
            clientSocket.Close();
        }
        catch (Exception)
        {
            Debug.LogError("无法关闭与服务器的链接");
        }
    }


}
