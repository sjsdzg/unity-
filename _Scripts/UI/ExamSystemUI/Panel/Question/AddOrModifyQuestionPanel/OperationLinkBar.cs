using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework.UI
{
    /// <summary>
    /// 操作题连接栏
    /// </summary>
    public class OperationLinkBar : MonoBehaviour, IValidate, IClear
    {
        private string link;
        /// <summary>
        /// 答案
        /// </summary>
        public string Link
        {
            get
            {
                link = inputFieldKey.text;
                return link;
            }
            set
            {
                link = value;
                inputFieldKey.text = link;
            }
        }

        /// <summary>
        /// 警告提示文本
        /// </summary>
        private Text warning;

        /// <summary>
        /// 答案InputField
        /// </summary>
        private InputField inputFieldKey;

        void Awake()
        {
            inputFieldKey = transform.Find("Data/InputField").GetComponent<InputField>();
            warning = transform.Find("Title/Warning").GetComponent<Text>();
            warning.text = "";
        }

        public bool Validate()
        {
            bool result = true;

            string text = "";
            //题目链接为空
            if (string.IsNullOrEmpty(inputFieldKey.text))
            {
                result = false;
                text = "*请输入试题链接*";
                result = false;
            }

            warning.text = text;
            return result;
        }

        public void Clear()
        {
            inputFieldKey.text = "";
            warning.text = "";
        }
    }
}
