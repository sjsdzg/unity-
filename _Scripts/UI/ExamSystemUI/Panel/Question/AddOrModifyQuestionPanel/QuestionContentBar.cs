using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace XFramework.UI
{
    /// <summary>
    /// 试题内容栏目
    /// </summary>
    public class QuestionContentBar : MonoBehaviour, IValidate, IClear
    {
        private const string BLANK = "[BLANK{0}]";

        private string qContent = "";
        /// <summary>
        /// 试题内容
        /// </summary>
        public string QContent
        {
            get { return qContent; }
            set
            {
                qContent = value;
                textBoxContent.text = qContent;
            }
        }

        /// <summary>
        /// 试题来源InputField
        /// </summary>
        private TextBox textBoxContent;

        /// <summary>
        /// 警告提示文本
        /// </summary>
        private Text warning;

        void Awake()
        {
            textBoxContent = transform.Find("Data/TextBox").GetComponent<TextBox>();
            warning = transform.Find("Title/Warning").GetComponent<Text>();
            warning.text = "";
            //事件
            textBoxContent.onEndEdit.AddListener(x => qContent = x);
        }

        /// <summary>
        /// 添加空格区域
        /// </summary>
        /// <param name="order"></param>
        public void AddBlank(int order)
        {
            string blank = string.Format(BLANK, order);
            textBoxContent.Insert(textBoxContent.FinalCaretPosition, blank);
        }

        /// <summary>
        /// 移除空格区域
        /// </summary>
        /// <param name="order"></param>
        public void RemoveBlank(int order)
        {
            string blank = string.Format(BLANK, order);
            textBoxContent.text = textBoxContent.text.Replace(blank, "");
        }

        /// <summary>
        /// 校验
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            bool result = true;

            warning.text = "";
            //题目内容为空
            if (string.IsNullOrEmpty(textBoxContent.text))
            {
                warning.text = "*请输入试题描述*";
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 重置内容
        /// </summary>
        public void Clear()
        {
            textBoxContent.text = "";
            warning.text = "";
        }
    }
}
