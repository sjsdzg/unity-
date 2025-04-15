using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using XFramework.Module;
using XFramework.Core;
using System;

namespace XFramework.UI
{
    public class ScoreRangeElement : Element, IValidate
    {
        private UniEvent<bool, string> m_OnValidated = new UniEvent<bool, string>();
        /// <summary>
        /// 验证事件
        /// </summary>
        public UniEvent<bool, string> OnValidated
        {
            get { return m_OnValidated; }
            set { m_OnValidated = value; }
        }

        private UniEvent<ScoreRangeElement> m_OnRemove = new UniEvent<ScoreRangeElement>();
        /// <summary>
        /// 移除事件
        /// </summary>
        public UniEvent<ScoreRangeElement> OnRemove
        {
            get { return m_OnRemove; }
            set { m_OnRemove = value; }
        }


        /// <summary>
        /// 序号文本
        /// </summary>
        private Text textNumber;

        private int number;
        /// <summary>
        /// 序号
        /// </summary>
        public int Number
        {
            get { return number; }
            set
            {
                number = value;
                textNumber.text = number.ToString();
            }
        }

        /// <summary>
        /// 最低分输入框
        /// </summary>
        private InputField inputFieldMin;

        /// <summary>
        /// 最高分输入框
        /// </summary>
        private InputField inputFieldMax;

        /// <summary>
        /// 移除
        /// </summary>
        private Button buttonRemove;

        private ScoreRange m_ScoreRange = new ScoreRange();

        /// <summary>
        /// 分数区间
        /// </summary>
        public ScoreRange ScoreRange
        {
            get
            {
                m_ScoreRange.Min = int.Parse(inputFieldMin.text);
                m_ScoreRange.Max = int.Parse(inputFieldMax.text);
                return m_ScoreRange;
            }
            set
            {
                m_ScoreRange = value;
                inputFieldMin.text = m_ScoreRange.Min.ToString();
                inputFieldMax.text = m_ScoreRange.Max.ToString();
            }
        }


        protected override void OnAwake()
        {
            base.OnAwake();
            textNumber = transform.Find("Key/Text").GetComponent<Text>();
            inputFieldMin = transform.Find("Value/InputFieldMin").GetComponent<InputField>();
            inputFieldMax = transform.Find("Value/InputFieldMax").GetComponent<InputField>();
            buttonRemove = transform.Find("Value/ButtonRemove").GetComponent<Button>();
            // Event
            buttonRemove.onClick.AddListener(buttonRemove_onClick);
        }

        public void SetData(int number, ScoreRange scoreRange)
        {
            Number = number;
            ScoreRange = scoreRange;
            if (number == 1)
            {
                buttonRemove.gameObject.SetActive(false);
            }
            else
            {
                buttonRemove.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            bool result = true;
            string text = "";

            if (string.IsNullOrEmpty(inputFieldMin.text) || string.IsNullOrEmpty(inputFieldMax.text))
            {
                text = "*分数区间，不能为空白*";
                result = false;
            }

            int minNum = int.Parse(inputFieldMin.text);
            if (minNum < 0)
            {
                text = "*分数区间，最小值为0*";
                result = false;
            }

            if (minNum > 9999)
            {
                text = "*分数区间，最大值为9999*";
                result = false;
            }

            int maxNum = int.Parse(inputFieldMax.text);
            if (maxNum < 0)
            {
                text = "*分数区间，最小值为0*";
                result = false;
            }

            if (maxNum > 9999)
            {
                text = "*分数区间，最大值为9999*";
                result = false;
            }

            if (minNum > maxNum)
            {
                text = "*分数区间，后一个值要大于前一个值*";
                result = false;
            }

            OnValidated.Invoke(result, "[" + textNumber + "]" + text);
            return result;
        }

        /// <summary>
        /// 移除按钮点击时，触发
        /// </summary>
        private void buttonRemove_onClick()
        {
            OnRemove.Invoke(this);
        }

    }
}

