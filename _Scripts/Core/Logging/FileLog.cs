using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

namespace XFramework.Core
{
    public class FileLog : ILog
    {
        private const string logDir = "/Logs/";
        private Queue<LogItemData> writingLogQueue;
        private Queue<LogItemData> waitingLogQueue;
        private object logLock;
        private Thread logThread;
        private bool isRunning = false;
        private StreamWriter logWriter;

        public FileLog()
        {
            writingLogQueue = new Queue<LogItemData>();
            waitingLogQueue = new Queue<LogItemData>();
            logLock = new object();
            DateTime now = DateTime.Now;
            string logName = string.Format("{0}{1}{2}.log", now.Year, now.Month, now.Day);
            string dir = Application.streamingAssetsPath + logDir;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string logPath = dir + logName;
            logWriter = new StreamWriter(logPath, true);
            logWriter.AutoFlush = true;
            logThread = new Thread(new ThreadStart(WriteLog));
        }

        /// <summary>
        /// 输出日志
        /// </summary>
        private void WriteLog()
        {
            while (isRunning)
            {
                if (writingLogQueue.Count == 0)
                {
                    lock (logLock)
                    {
                        //if (waitingLogQueue.Count == 0)
                        //{
                        //    Monitor.Wait(logLock);
                        //}
                        Queue<LogItemData> tempQueue = writingLogQueue;
                        writingLogQueue = waitingLogQueue;
                        waitingLogQueue = tempQueue;
                    }
                }
                else
                {
                    while (writingLogQueue.Count > 0)
                    {
                        LogItemData data = writingLogQueue.Dequeue();
                        if (data.type == LogType.Error || data.type == LogType.Exception)
                        {
                            logWriter.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                            logWriter.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t" + data.Log);
                            logWriter.WriteLine(data.Track);
                            logWriter.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                        }
                        else
                        {
                            logWriter.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t" + data.Log);
                        }
                    }
                }
            }
        }

        public void Log(string condition, string stackTrace, LogType type)
        {
            lock (logLock)
            {
                LogItemData data = new LogItemData(condition, stackTrace, type);
                waitingLogQueue.Enqueue(data);
                //Monitor.Pulse(logLock);
            }
        }

        /// <summary>
        /// 开始
        /// </summary>
        public void Start()
        {
            if (isRunning == false)
            {
                isRunning = true;
                logThread.Start();
            }
        }

        /// <summary>
        /// 退出
        /// </summary>
        public void Abort()
        {
            if (isRunning == true)
            {
                isRunning = false;
                logWriter.Close();
            }
        }
    }
}
