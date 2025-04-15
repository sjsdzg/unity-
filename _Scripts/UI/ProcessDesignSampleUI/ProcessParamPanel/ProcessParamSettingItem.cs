using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using XFramework.Module;

namespace XFramework.UI
{
    public class ProcessParamSettingItem : MonoBehaviour
    {
        /// <summary>
        /// 文本
        /// </summary>
        private Text m_Text;

        /// <summary>
        /// 输入框
        /// </summary>
        private InputField m_InputField;

        /// <summary>
        /// 
        /// </summary>
        public Variable Data { get; set; }

        private void Awake()
        {
            m_Text = transform.Find("Text").GetComponent<Text>();
            m_InputField = transform.Find("InputField").GetComponent<InputField>();

            m_InputField.onEndEdit.AddListener(m_InputField_onEndEdit);
        }

        public void SetData(Variable data)
        {
            Data = data;
            m_Text.text = data.Name;
            switch (data.Type)
            {
                case VariableType.Float:
                    m_InputField.contentType = InputField.ContentType.DecimalNumber;
                    break;
                case VariableType.Integer:
                    m_InputField.contentType = InputField.ContentType.IntegerNumber;
                    break;
                case VariableType.String:
                    m_InputField.contentType = InputField.ContentType.Standard;
                    break;
                default:
                    break;
            }
            m_InputField.text = data.Value.ToString();
        }

        /// <summary>
        /// 输入完成
        /// </summary>
        /// <param name="arg0"></param>
        private void m_InputField_onEndEdit(string arg0)
        {
            switch (Data.Type)
            {
                case VariableType.Float:
                    Data.Value = float.Parse(arg0);
                    break;
                case VariableType.Integer:
                    Data.Value = int.Parse(arg0);
                    break;
                case VariableType.String:
                    Data.Value = arg0;
                    break;
                default:
                    break;
            }
        }
    }
}

