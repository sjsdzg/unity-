using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.Common
{
    public abstract class BaseDialog : MonoBehaviour
    {
        /// <summary>
        /// 获取当前对话框的类型
        /// </summary>
        /// <returns></returns>
        public abstract MessageBoxExEnum GetMessageBoxExType();

        /// <summary>
        /// 显示具有指定操作文本、标题和对话框类型的对话框,并设置回调函数。
        /// </summary>
        /// <param name="text"></param>
        /// <param name="caption"></param>
        /// <param name="type"></param>
        /// <param name="action"></param>
        /// <param name="_params"></param>
        public abstract void ShowMessageBoxEx(string text, string caption, Action<DialogResult> action, params object[] _params);

        /// <summary>
        /// 执行操作
        /// </summary>
        public abstract void OnSubmit();

        /// <summary>
        /// 取消操作
        /// </summary>
        public abstract void OnCancel();
    }
}
