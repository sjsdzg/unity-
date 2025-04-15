using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;

namespace XFramework.Common
{
    public class SingleDialog : BaseDialog
    {
        private Text Caption;// 标题
        private Text Content;// 操作提示文本
        private Button ButtonSubmit;// 确认按钮
        private Button ButtonClose;// 关闭按钮
        private Action<DialogResult> handler; // 操作回调函数

        void Awake()
        {
            Caption = transform.Find("Background/Panel/TitleBar/Caption").GetComponent<Text>();
            ButtonClose = transform.Find("Background/Panel/TitleBar/ButtonClose").GetComponent<Button>();
            Content = transform.Find("Background/Panel/Content/Text").GetComponent<Text>();
            ButtonSubmit = transform.Find("Background/Panel/Buttons/ButtonSubmit").GetComponent<Button>();
        }

        void Start()
        {
            ButtonSubmit.onClick.AddListener(OnSubmit);
            ButtonClose.onClick.AddListener(OnCancel);
        }

        /// <summary>
        /// 显示具有指定操作文本、标题和操作类型的消息框,并设置回调函数。
        /// </summary>
        /// <param name="caption">要在消息框的标题栏中显示的文本</param>
        /// <param name="action">回调函数</param>
        /// <param name="labels">操作文本标签</param>
        public override MessageBoxExEnum GetMessageBoxExType()
        {
            return MessageBoxExEnum.SingleDialog;
        }

        /// <summary>
        /// 显示对话框
        /// </summary>
        /// <param name="text"></param>
        /// <param name="caption"></param>
        /// <param name="action"></param>
        /// <param name="_params"></param>
        public override void ShowMessageBoxEx(string text, string caption, Action<DialogResult> action, params object[] _params)
        {
            Content.text = text;
            Caption.text = caption;
            handler = action;
        }

        /// <summary>
        /// 提交操作
        /// </summary>
        public override void OnSubmit()
        {
            if (handler != null)
            {
                DialogResult handleData = new DialogResult(gameObject, true);
                handler(handleData);
            }
            Destroy(gameObject);
        }

        /// <summary>
        /// 取消操作
        /// </summary>
        public override void OnCancel()
        {
            if (handler != null)
            {
                DialogResult handleData = new DialogResult(gameObject, false);
                handler(handleData);
            }
            Destroy(gameObject);
        }
    }
}
