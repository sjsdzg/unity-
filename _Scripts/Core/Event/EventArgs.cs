using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;

namespace XFramework.Core
{
    /// <summary>
    /// 包含Unity事件数据的类的基类。
    /// </summary>
    public class DefaultEventArgs
    {
        public static readonly DefaultEventArgs Empty = new DefaultEventArgs();

        /// <summary>
        /// 初始化 UnityEventArgs 类的新实例。
        /// </summary>
        public DefaultEventArgs() { }
    }

    /// <summary>
    /// 动作执行之前事件数据
    /// </summary>
    public class BeforeEventArgs : DefaultEventArgs
    {
        private bool m_Can = true;
        /// <summary>
        /// 能否执行后面的行为
        /// </summary>
        public bool Can
        {
            get { return m_Can; }
            set { m_Can = value; }
        }

        /// <summary>
        /// 内容
        /// </summary>
        public object Content { get; set; }

        private UnityAction<BeforeEventArgs> m_Action;
        /// <summary>
        /// 回调行为
        /// </summary>
        public UnityAction<BeforeEventArgs> _Action
        {
            get { return m_Action; }
            set { m_Action = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="content"></param>
        /// <param name="action"></param>
        public BeforeEventArgs(object content, UnityAction<BeforeEventArgs> action)
        {
            Content = content;
            _Action = action;
        }

        /// <summary>
        /// 回调
        /// </summary>
        public void CallBack()
        {
            if (m_Action != null && Can)
            {
                m_Action.Invoke(this);
            }
        }
    }

    /// <summary>
    /// 动作执行之后事件数据
    /// </summary>
    public class AfterEventArgs : DefaultEventArgs
    {
        /// <summary>
        /// 内容
        /// </summary>
        public object Content { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="content"></param>
        public AfterEventArgs(object content)
        {
            Content = content;
        }
    }
}
