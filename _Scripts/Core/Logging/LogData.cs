using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.Core
{
    public class LogData : DataObject<LogData>
    {
        /// <summary>
        /// 日志项集合
        /// </summary>
        public List<LogItemData> Items { get; set; }

        public LogData()
        {
            Items = new List<LogItemData>();
        }

        /// <summary>
        /// 添加日志项
        /// </summary>
        /// <param name="item"></param>
        public void Add(LogItemData item)
        {
            Items.Add(item);
        }

        /// <summary>
        /// 添加日志项
        /// </summary>
        /// <param name="content"></param>
        /// <param name="type"></param>
        public void Add(string content, LogType type)
        {
            LogItemData item = new LogItemData(content, "", type);
            Items.Add(item);
        }
    }
}
