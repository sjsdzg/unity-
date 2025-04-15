using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework.UI
{
    /// <summary>
    /// 简答题答案栏
    /// </summary>
    public class EssayKeyBar : MonoBehaviour, IValidate, IClear
    {
        private string key;
        /// <summary>
        /// 答案
        /// </summary>
        public string Key
        {
            get
            {
                key = inputFieldKey.text;
                return key;
            }
            set
            {
                key = value;
                inputFieldKey.text = key;
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
            //题目内容为空
            if (string.IsNullOrEmpty(inputFieldKey.text))
            {
                result = false;
                text = "*请输入试题答案*";
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
