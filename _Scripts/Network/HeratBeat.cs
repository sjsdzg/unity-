using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using XFramework.Core;

namespace XFramework.Network
{
    public class HeartBeat
    {
        /// <summary>
        /// 写空闲时间
        /// </summary>
        public float WriterIdleTime { get; set; }

        /// <summary>
        /// 心跳线程
        /// </summary>
        private Thread heartBeatThread;

        public HeartBeat(float writerIdleTime)
        {
            WriterIdleTime = writerIdleTime;
            NetworkManager.Instance.Connected += Instance_Connected;
        }

        private void Instance_Connected(object sender, EventArgs e)
        {
            //心跳包
            //HandlerDispatcher.Instance.AddIterator(OnHeratBeat());
#if UNITY_WEBGL
            HandlerDispatcher.Instance.AddIterator(OnHeratBeat());
#else
            heartBeatThread = new Thread(new ThreadStart(HeratBeatHandler));
            heartBeatThread.IsBackground = true;
            heartBeatThread.Start();
#endif
        }

        /// <summary>
        /// 每隔一定时间，发送心跳包
        /// </summary>
        /// <returns></returns>
        IEnumerator OnHeratBeat()
        {
            while (NetworkManager.Instance.IsConnected)
            {
                if (Time.unscaledTime - NetworkManager.Instance.LastWriteTime > WriterIdleTime)
                {
                    Debug.Log("write heartbeat to server");
                    SendHeratBeatMsg();
                }
                yield return new WaitForFixedUpdate();
            }
        }

        /// <summary>
        /// 心跳线程处理
        /// </summary>
        private void HeratBeatHandler()
        {
            while (NetworkManager.Instance.IsConnected)
            {
                SendHeratBeatMsg();
                Thread.Sleep((int)WriterIdleTime * 1000);
            }
            heartBeatThread.Abort();
        }

        /// <summary>
        /// 发送心跳消息
        /// </summary>
        public void SendHeratBeatMsg()
        {
            Header header = new Header();
            header.Tag = Tag.HEARTBEAT;
            NetworkPackageInfo package = new NetworkPackageInfo(header, null);
            NetworkManager.Instance.SendMsg(package, null, RegisterType.None);
        }
    }
}
