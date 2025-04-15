
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;

namespace XFramework.Common
{
    /// <summary>
    /// 操作对话框
    /// </summary>
    public class MessageBoxEx
    {
        /// <summary>
        /// 对话框
        /// </summary>
        private static BaseDialog baseDialog;

        /// <summary>
        /// 显示具有指定操作文本、标题和按钮的对话框,并设置回调函数
        /// </summary>
        /// <param name="text">对话框操作栏中显示的文本</param>
        /// <param name="caption">对话框标题栏中显示的文本</param>
        /// <param name="type">MessageBoxExEnum 值之一，可指定在显示那种类型的对话框。</param>
        /// <param name="call">回调函数</param>
        /// <param name="_params">操作参数</param>
        public static void Show(string text, string caption, MessageBoxExEnum type, Action<DialogResult> call = null, params object[] _params)
        {
            CoroutineManager.Instance.StartCoroutine(ShowEnumerator(text, caption, type, call, _params));
        }

        /// <summary>
        /// 显示对话框协程
        /// </summary>
        /// <param name="text"></param>
        /// <param name="caption"></param>
        /// <param name="type"></param>
        /// <param name="call"></param>
        /// <param name="_params"></param>
        /// <returns></returns>
        private static IEnumerator ShowEnumerator(string text, string caption, MessageBoxExEnum type, Action<DialogResult> call, params object[] _params)
        {
            if (baseDialog != null)
            {
                UnityEngine.Object.Destroy(baseDialog.gameObject);
                baseDialog = null;
            }

            yield return new WaitForEndOfFrame();

            string path = MessageBoxExDefine.GetMessageBoxPath(type);
            GameObject obj = Resources.Load<GameObject>(path);
            if (obj != null)
            {
                GameObject messageBoxObject = GameObject.Instantiate<GameObject>(obj);
                UnityEngine.Object.DontDestroyOnLoad(messageBoxObject);
                if (messageBoxObject != null)
                {
                    baseDialog = messageBoxObject.GetComponent<BaseDialog>();
                    baseDialog.ShowMessageBoxEx(text, caption, call, _params);
                }
            }
        }
    }
}
