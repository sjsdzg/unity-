﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.Core
{
    public interface ILog
    {
        /// <summary>
        /// 输出日志
        /// </summary>
        void Log(string condition, string stackTrace, LogType type);

        /// <summary>
        /// 开始
        /// </summary>
        void Start();

        /// <summary>
        /// 退出
        /// </summary>
        void Abort();
    }
}
