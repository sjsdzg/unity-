using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.Common
{
    /// <summary>
    /// 操作数据类
    /// </summary>
    public class DialogResult
    {
        /// <summary>
        /// 操作源
        /// </summary>
        public GameObject Sender { get; private set; }
        /// <summary>
        /// 操作数据内容
        /// </summary>
        public object Content { get; set; }

        /// <summary>
        /// 初始化操作数据实例
        /// </summary>
        /// <param name="sender">操作源</param>
        /// <param name="content">操作数据</param>
        public DialogResult(GameObject sender, object content)
        {
            Sender = sender;
            Content = content;
        }
    }
}
