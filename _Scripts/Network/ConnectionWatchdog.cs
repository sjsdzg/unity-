using Google.Protobuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using XFramework.Common;
using XFramework.Core;
using XFramework.Proto;

namespace XFramework.Network
{
    /// <summary>
    /// 连接监视器
    /// </summary>
    public class ConnectionWatchdog
    {
        public event EventHandler Reconnected;

        /// <summary>
        /// 是否重连中
        /// </summary>
        private bool reconnecting = false;

        /// <summary>
        /// 尝试次数
        /// </summary>
        private int arrempts = 5;

        /// <summary>
        /// 多久尝试一次
        /// </summary>
        private float timeout = 2f;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arrempts">尝试次数</param>
        /// <param name="timeout">多久重连一次</param>
        public ConnectionWatchdog(int arrempts, float timeout)
        {
            this.arrempts = arrempts;
            this.timeout = timeout;
            NetworkManager.Instance.Connected += Client_Connected;
            NetworkManager.Instance.Closed += Client_Closed;
            NetworkManager.Instance.Error += Client_Error;
            //订阅用户被强制下线的消息
            NetworkManager.Instance.SubscribeMsg(Commands.Gateway.USER_OFFLINE, ReceiveUserOffline);
        }


        private void Client_Connected(object sender, System.EventArgs e)
        {
            Debug.Log("successfully connected to the server!");
            ChannelActive();
        }

        private void Client_Closed(object sender, System.EventArgs e)
        {
            Debug.Log("Closed");
            ChannelInactive();
        }

        private void Client_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            Debug.LogException(e.Exception);
            ChannelInactive();
        }

        /// <summary>
        /// 网络通信有效
        /// </summary>
        private void ChannelActive()
        {
            HandlerDispatcher.Instance.AddAction(() =>
            {
                if (string.IsNullOrEmpty(GlobalManager.token))
                {
                    SceneLoader.Instance.LoadSceneAsync(SceneType.LoginScene);
                }

                //结束重连操作
                if (Reconnected != null)
                {
                    Reconnected(this, EventArgs.Empty);
                }

                if (reconnecting)
                {
                    reconnecting = false;
                }
            });
        }

        /// <summary>
        /// 网络通信无效
        /// </summary>
        private void ChannelInactive()
        {
            HandlerDispatcher.Instance.AddAction(() =>
            {
                if (NetworkManager.Instance.IsConnected)
                    return;

                if (reconnecting)
                    return;

                if (!string.IsNullOrEmpty(GlobalManager.token))
                {
                    HandlerDispatcher.Instance.AddIterator(ReconnectEnumerator());
                }
                else
                {
                    // 登录场景
                    GlobalManager.user = null;

                    MessageBoxEx.Show("请确认网络连接是否正常，点击确认重试。", "提示", MessageBoxExEnum.SingleDialog, result =>
                    {
                        ConnectToServer();
                    });
                }
            });
        }

        /// <summary>
        /// 连接到服务器
        /// </summary>
        public void ConnectToServer()
        {
#if UNITY_WEBGL
            NetworkManager.Instance.Connect(new Uri(AppSettings.Settings.WebSocketUrl));
#else
            NetworkManager.Instance.Connect(new IPEndPoint(IPAddress.Parse(AppSettings.Settings.ServerIP), AppSettings.Settings.Port));
#endif
        }

        /// <summary>
        /// 运行重连操作
        /// </summary>
        /// <returns></returns>
        public IEnumerator ReconnectEnumerator()
        {
            reconnecting = true;
            int count = 0;
            while (reconnecting && count < arrempts)
            {
                ConnectToServer();
                count++;
                ProgressBar.Instance.Show(count / (float)arrempts, "正在重连中......");
                yield return new WaitForSeconds(timeout);
            }
            //隐藏ProgressBar
            ProgressBar.Instance.Hide();

            if (NetworkManager.Instance.IsConnected)
            {
                ReconnectReq req = new ReconnectReq();
                req.UserName = GlobalManager.user.UserName;
                req.Token = GlobalManager.token;
                req.SoftwareId = App.Instance.SoftwareId;
                NetworkManager.Instance.SendMsg(Modules.GATEWAY, Commands.Gateway.RECONNECT, req.ToByteArray(), ReconnectHandler);
            }
            else
            {
                //无效，即重连失败。
                GlobalManager.token = "";
                // 登录场景
                GlobalManager.user = null;
                //正在重连设置false
                reconnecting = false;
                MessageBoxEx.Show("重连失败，请确认网络连接是否正常，点击确认重试。", "提示", MessageBoxExEnum.SingleDialog, result =>
                {
                    ConnectToServer();
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReconnectHandler(NetworkPackageInfo packageInfo)
        {
            ReconnectResp resp = ReconnectResp.Parser.ParseFrom(packageInfo.Body);
            if (resp.Success)
            {
                GlobalManager.token = resp.Token;
                MessageBoxEx.Show(resp.Detail, "提示", MessageBoxExEnum.SingleDialog, null);
            }
            else
            {
                GlobalManager.token = "";
                GlobalManager.user = null;
                MessageBoxEx.Show(resp.Detail, "提示", MessageBoxExEnum.SingleDialog, null);
            }

            if (string.IsNullOrEmpty(GlobalManager.token))
            {
                SceneLoader.Instance.LoadSceneAsync(SceneType.LoginScene);
            }

            reconnecting = false;
            if (Reconnected != null)
            {
                Reconnected(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 接受用户被强制下线的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveUserOffline(NetworkPackageInfo packageInfo)
        {
            GlobalManager.token = "";
            GlobalManager.user = null;
            MessageBoxEx.Show("您已被强制下线。", "提示", MessageBoxExEnum.SingleDialog, null);
        
            if (string.IsNullOrEmpty(GlobalManager.token))
            {
                SceneLoader.Instance.LoadSceneAsync(SceneType.LoginScene);
            }
        }

    }
}
