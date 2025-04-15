using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework.UI
{
    /// <summary>
    /// 题目解析栏目
    /// </summary>
    public class QuestionResolveBar : MonoBehaviour, IValidate, IClear
    {
        private string qResolve = "";
        /// <summary>
        /// 试题解析
        /// </summary>
        public string QResolve
        {
            get { return qResolve; }
            set
            {
                qResolve = value;
                inputFieldResolve.text = qResolve;
            }
        }

        /// <summary>
        /// 试题解析InputField
        /// </summary>
        private InputField inputFieldResolve;

        /// <summary>
        /// 警告提示文本
        /// </summary>
        private Text warning;

        void Awake()
        {
            inputFieldResolve = transform.Find("Data/InputField").GetComponent<InputField>();
            warning = transform.Find("Title/Warning").GetComponent<Text>();
            warning.text = "";
            //事件
            inputFieldResolve.onEndEdit.AddListener(x => qResolve = x);
        }

        public bool Validate()
        {
            return true;
        }

        public void Clear()
        {
            inputFieldResolve.text = "";
        }
    }
}
