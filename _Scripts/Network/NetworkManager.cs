using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.ProtoBase;
using SuperSocket.ClientEngine;
using XFramework.Network;
using XFramework.Core;
using System.Collections;
using UnityEngine;
using XFramework.Common;
using System.Net;

namespace XFramework
{
    public class NetworkManager : Singleton<NetworkManager>
    {
        private static object locked = new object();//锁定

        /// <summary>
        /// PC
        /// </summary>
        private EasyClient<NetworkPackageInfo> easyClient;

        /// <summary>
        /// WebGL
        /// </summary>
        private WebSocket m_WebSocket;

        /// <summary>
        /// 是否已经连接
        /// </summary>
        public bool IsConnected { get; private set; }

        /// <summary>
        /// 上一次写时间
        /// </summary>
        public float LastWriteTime { get; set; }

        /// <summary>
        /// 连接监视器
        /// </summary>
        public ConnectionWatchdog ConnectionWatchdog { get; set; }

        /// <summary>
        /// 心跳
        /// </summary>
        public HeartBeat HeartBeat { get; set; }

        /// <summary>
        /// 命令容器
        /// </summary>
        private Dictionary<string, NetworkCommand> m_CommandContainer;

        public event EventHandler Reconnected;
        public event EventHandler Connected;
        public event EventHandler Closed;
        public event EventHandler<ErrorEventArgs> Error;

        /// <summary>
        /// 存储网络包的队列
        /// </summary>
        private Queue<NetworkPackageInfo> packages = new Queue<NetworkPackageInfo>();

        /// <summary>
        /// 私有构造函数
        /// </summary>
        public NetworkManager()
        {
            m_CommandContainer = new Dictionary<string, NetworkCommand>();
        }


        /// <summary>
        /// 设置重连机制
        /// </summary>
        /// <param name="arrempts">重连次数</param>
        /// <param name="timeout">多久重连一次</param>
        public void SetConnectionWatchdog(int arrempts, float timeout)
        {
            ConnectionWatchdog = new ConnectionWatchdog(arrempts, timeout);
            ConnectionWatchdog.Reconnected += ConnectionWatchdog_Reconnected;
        }

        /// <summary>
        /// 设置心跳机制
        /// </summary>
        /// <param name="writerIdleTime">空闲多久，发送一次心跳包</param>
        public void SetHeartBeat(float writerIdleTime)
        {
            HeartBeat = new HeartBeat(writerIdleTime);
        }


#if UNITY_WEBGL
        public void Connect(Uri uri)
        {
            m_WebSocket = new WebSocket(uri);
            CoroutineManager.Instance.StartCoroutine(KeepConnect(uri));
        }
#else
        public void Connect(EndPoint remoteEndPoint)
        {
            easyClient = new EasyClient<NetworkPackageInfo>();
            easyClient.Initialize(new CommonReceiveFilter());
            easyClient.NewPackageReceived += NetworkManager_NewPackageReceived;
            easyClient.Connected += Client_Connected;
            easyClient.Closed += Client_Closed;
            easyClient.Error += Client_Error;
            easyClient.BeginConnect(remoteEndPoint);
        }
#endif
        public void Close()
        {
#if UNITY_WEBGL
            if (m_WebSocket != null)
            {
                m_WebSocket.Close();
                Client_Closed(this, EventArgs.Empty);
            }
#else
            if (easyClient != null)
                easyClient.Close();
#endif
        }

        /// <summary>
        /// 保持WebGL链接
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        IEnumerator KeepConnect(Uri uri)
        {
            yield return m_WebSocket.Connect();
            //判断是否错误
            if (m_WebSocket.error == null)
            {
                Client_Connected(this, EventArgs.Empty);
            }
            else
            {
                ErrorEventArgs e = new ErrorEventArgs(new Exception(m_WebSocket.error));
                Client_Error(this, e);
            }

            while (IsConnected)
            {
                byte[] data = m_WebSocket.Recv();
                //接收数据
                if (data != null)
                {
                    Debug.Log(data.Length);
                    byte[] array = new byte[Header.HEAD_LEN];
                    Buffer.BlockCopy(data, 0, array, 0, Header.HEAD_LEN);
                    Header header = Header.ParseFrom(array);

                    byte[] body = new byte[header.Length];
                    Buffer.BlockCopy(data, Header.HEAD_LEN, body, 0, header.Length);

                    PackageEventArgs<NetworkPackageInfo> e = new PackageEventArgs<NetworkPackageInfo>(new NetworkPackageInfo(header, body));
                    NetworkManager_NewPackageReceived(this, e);
                }
                //判断是否错误
                if (m_WebSocket.error != null)
                {
                    ErrorEventArgs e = new ErrorEventArgs(new Exception(m_WebSocket.error));
                    Client_Error(this, e);
                }

                yield return new WaitForFixedUpdate();
            }
        }

        /// <summary>
        /// 发送请求消息，注册反馈回调
        /// </summary>
        /// <param name="package"></param>
        /// <param name="handler"></param>
        /// <param name="type"></param>
        public void SendMsg(NetworkPackageInfo package, PackageHandler<NetworkPackageInfo> handler, RegisterType type = RegisterType.Request)
        {
            //如果没有连接成功
            if (!IsConnected)
                return;

            if (handler != null)
            {
                RegisterCommand(package.Key, handler, type);
            }

            byte[] bs = package.ToArray();
            try
            {

#if UNITY_WEBGL
                m_WebSocket.Send(package.ToArray());
                // 最近一次发送消息时间
                LastWriteTime = Time.unscaledTime;
#else
                easyClient.Send(package.ToArray());
#endif
            }
            catch (Exception ex)
            {
                ErrorEventArgs e = new ErrorEventArgs(ex);
                Client_Error(this, e);
            }

        }

        /// <summary>
        /// 发送请求消息，注册反馈回调
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="commandId"></param>
        /// <param name="body"></param>
        /// <param name="handler"></param>
        /// <param name="type"></param>
        public void SendMsg(short moduleId, short commandId, byte[] body, PackageHandler<NetworkPackageInfo> handler, RegisterType type = RegisterType.Request)
        {
            NetworkPackageInfo package = new NetworkPackageInfo(moduleId, commandId, body);
            //发送请求消息
            SendMsg(package, handler, type);
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="commandId"></param>
        /// <param name="handler"></param>
        public void SubscribeMsg(short commandId, PackageHandler<NetworkPackageInfo> handler)
        {
            if (handler != null)
            {
                RegisterCommand(commandId.ToString(), handler, RegisterType.Subscribe);
            }
        }

        /// <summary>
        /// 取消订阅消息
        /// </summary>
        /// <param name="commandId"></param>
        public void UnsubscribeMsg(short commandId)
        {
            UnregisterCommand(commandId.ToString());
        }

        /// <summary>
        /// 接受新的数据包后，处理函数。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NetworkManager_NewPackageReceived(object sender, PackageEventArgs<NetworkPackageInfo> e)
        {
            lock (packages)
            {
                NetworkPackageInfo package = e.Package;
                packages.Enqueue(package);
            }
        }

        /// <summary>
        /// 处理数据包
        /// </summary>
        public void DispatchPackage()
        {
            if (packages.Count == 0)
                return;


#if UNITY_WEBGL
            while (packages.Count > 0)
            {
                var package = packages.Dequeue();
                ExecuteCommand(package);
            }
#else
            lock (packages)
            {
                while (packages.Count > 0)
                {
                    var package = packages.Dequeue();
                    ExecuteCommand(package);
                }
            }
#endif
        }

        private void CommandLoaderOnError(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            Debug.LogException(e.Exception);
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        protected void ExecuteCommand(NetworkPackageInfo package)
        {
            if (m_CommandContainer == null || !m_CommandContainer.ContainsKey(package.Key))
                return;

            if (m_CommandContainer[package.Key] == null)
            {
                UnregisterCommand(package.Key);
                return;
            }

            NetworkCommand command = m_CommandContainer[package.Key];
            command.ExecuteCommand(package);
            switch (command.Type)
            {
                case RegisterType.Request:
                    UnregisterCommand(package.Key);
                    break;
                case RegisterType.Subscribe:
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// 添加命令
        /// </summary>
        /// <param name="response"></param>
        /// <param name="handler"></param>
        public void RegisterCommand(string key, PackageHandler<NetworkPackageInfo> handler, RegisterType type = RegisterType.Request)
        {
            NetworkCommand cmd = new NetworkCommand(key, handler, type);

            if (m_CommandContainer.ContainsKey(cmd.Name))
            {
                Debug.LogWarningFormat("Duplicated name command has been found! Command name: 0x{0}", (Convert.ToInt16(cmd.Name)).ToString("X4"));
                m_CommandContainer.Remove(cmd.Name);
            }

            m_CommandContainer.Add(cmd.Name, cmd);
            Debug.LogFormat("The command has been registered! Command name: {0}", (Convert.ToInt16(cmd.Name)).ToString("X4"));
        }

        /// <summary>
        /// 取消注册命令
        /// </summary>
        /// <param name="response"></param>
        public void UnregisterCommand(string key)
        {
            if (m_CommandContainer.ContainsKey(key))
            {
                Debug.Log("The command has been Unregister! Command name: " + (Convert.ToInt16(key)).ToString("X4"));
                m_CommandContainer.Remove(key);
            }
        }

        private void ConnectionWatchdog_Reconnected(object sender, EventArgs e)
        {
            if (Reconnected != null)
            {
                Reconnected(sender, e);
            }
        }

        private void Client_Connected(object sender, EventArgs e)
        {
            if (Connected != null)
            {
                IsConnected = true;
                Connected(sender, e);
            }
        }

        private void Client_Closed(object sender, EventArgs e)
        {
            if (Closed != null)
            {
                IsConnected = false;
                Closed(sender, e);
            }
        }

        private void Client_Error(object sender, ErrorEventArgs e)
        {
            if (Error != null)
            {
                IsConnected = false;
                Error(sender, e);
            }
        }
    }

}
