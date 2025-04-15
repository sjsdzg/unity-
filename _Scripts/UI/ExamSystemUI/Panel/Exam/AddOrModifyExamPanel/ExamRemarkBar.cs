using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework.UI
{
    public class ExamRemarkBar : MonoBehaviour, IValidate
    {

        private string content = "";
        /// <summary>
        /// 考试说明
        /// </summary>
        public string Content
        {
            get
            {
                content = inputField.text;
                return content;
            }
            set
            {
                content = value;
                inputField.text = content;
            }
        }

        /// <summary>
        /// 考试来源InputField
        /// </summary>
        private InputField inputField;

        /// <summary>
        /// 警告提示文本
        /// </summary>
        private Text warning;

        void Awake()
        {
            inputField = transform.Find("Data/InputField").GetComponent<InputField>();
            warning = transform.Find("Title/Warning").GetComponent<Text>();
            warning.text = "";
        }

        /// <summary>
        /// 校验
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            return true;
        }
    }
}
