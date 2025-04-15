using UnityEngine;
using System.Threading;

namespace XFramework.Core
{
    public class Log : Singleton<Log>
    {
        /// <summary>
        /// 主线程ID
        /// </summary>
        private int mainThreadID = -1;

        /// <summary>
        /// 是否可以接收
        /// </summary>
        private static bool isReceived = false;

        /// <summary>
        /// 日志输出
        /// </summary>
        private static ILog m_Logger;

        protected override void Init()
        {
            base.Init();
            m_Logger = new FileLog();
        }

        /// <summary>
        /// 开始记录日志
        /// </summary>
        public void Start()
        {
            isReceived = true;
            m_Logger.Start();

            mainThreadID = Thread.CurrentThread.ManagedThreadId;
            Application.logMessageReceived += Application_logMessageReceived;
            Application.logMessageReceivedThreaded += Application_logMessageReceivedThreaded;
        }

        private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
        {
            if (mainThreadID == Thread.CurrentThread.ManagedThreadId)
            {
                Output(condition, stackTrace, type);
            }
        }

        private void Application_logMessageReceivedThreaded(string condition, string stackTrace, LogType type)
        {
            if (mainThreadID != Thread.CurrentThread.ManagedThreadId)
            {
                Output(condition, stackTrace, type);
            }
        }

        /// <summary>
        /// 输出日志
        /// </summary>
        /// <param name="condition">情况</param>
        /// <param name="stackTrace">堆栈信息</param>
        /// <param name="type">日志类型</param>
        private void Output(string condition, string stackTrace, LogType type)
        {
            if (isReceived)
            {
                m_Logger.Log(condition, stackTrace, type);
            }
        }

        /// <summary>
        /// 关闭监听日志消息
        /// </summary>
        public void Close()
        {
            isReceived = false;
            m_Logger.Abort();
        }
    }
}
