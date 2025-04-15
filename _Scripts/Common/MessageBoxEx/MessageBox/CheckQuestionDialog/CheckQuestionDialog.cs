using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XFramework.Module;

namespace XFramework.Common
{
    public class CheckQuestionDialog : BaseDialog
    {
        private Text Caption;// 标题
        /// <summary>
        /// 考核题目组件
        /// </summary>
        private CheckQuestionComponent m_Component;
        private Button ButtonSubmit;// 确认按钮
        private Button ButtonClose;// 关闭按钮
        private Action<DialogResult> handler; // 操作回调函数
        /// <summary>
        /// 提示
        /// </summary>
        private Text Hint;

        void Awake()
        {
            Caption = GameObject.Find("Background/Panel/TitleBar/Caption").GetComponent<Text>();
            ButtonClose = GameObject.Find("Background/Panel/TitleBar/ButtonClose").GetComponent<Button>();
            m_Component = GameObject.Find("Background/Panel/Content/CheckQuestion").GetComponent<CheckQuestionComponent>();
            ButtonSubmit = GameObject.Find("Background/Panel/Content/ButtonSubmit").GetComponent<Button>();
            Hint = GameObject.Find("Background/Panel/Content/Text").GetComponent<Text>();
            //init
            ButtonClose.interactable = false;
            ButtonSubmit.interactable = true;
            Hint.text = "";
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
            return MessageBoxExEnum.CheckQuestionDialog;
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
            Caption.text = caption;
            handler = action;
            if (_params.Length == 1)
            {
                m_Component.Question = _params[0] as CheckQuestion;
            }
        }

        /// <summary>
        /// 提交操作
        /// </summary>
        public override void OnSubmit()
        {
            if (m_Component.Completed)
            {
                ButtonClose.interactable = true;
                ButtonSubmit.interactable = false;

                if (m_Component.IsEquals())
                {
                    Hint.text = "回答正确。";
                    m_Component.Question.Score = m_Component.Question.Value;
                }
                else
                {
                    Hint.text = "<color=red>回答错误，标准答案：" + m_Component.Question.Key + "。</color>";
                }

                if (handler != null)
                {
                    DialogResult handleData = new DialogResult(gameObject, m_Component.Question);
                    handler(handleData);
                }
            }
            else
            {
                Hint.text = "<color=red>请选择一个选项。</color>";
            }

        }

        /// <summary>
        /// 取消操作
        /// </summary>
        public override void OnCancel()
        {
            Destroy(gameObject);
        }
    }
}
