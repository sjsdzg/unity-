using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Module;

namespace XFramework.UI
{
    /// <summary>
    /// 操作数据类
    /// </summary>
    public class DocumentResult
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
        public DocumentResult(GameObject sender, object content)
        {
            Sender = sender;
            Content = content;
        }
    }

    /// <summary>
    /// 文件基类
    /// </summary>
    public abstract class BaseDocument : MonoBehaviour
    {
        /// <summary>
        /// 获取文件类型
        /// </summary>
        /// <returns></returns>
        public abstract DocumentType GetDocumentType();

        /// <summary>
        /// 文件
        /// </summary>
        public Document Document { get; set; }

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="action"></param>
        /// <param name="_params"></param>
        public abstract void SetParams(Document item, Action<DocumentResult> action, params object[] _params);

        /// <summary>
        /// 提交操作
        /// </summary>
        public abstract void Submit();

        /// <summary>
        /// 取消操作
        /// </summary>
        public abstract void Cancel();
    }
}
