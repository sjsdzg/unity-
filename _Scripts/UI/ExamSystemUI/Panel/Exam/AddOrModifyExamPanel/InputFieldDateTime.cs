using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Common;

namespace XFramework.UI
{
    public class InputFieldDateTime : MonoBehaviour
    {
        /// <summary>
        /// 日期时间选择器
        /// </summary>
        private DateTimePicker dateTimePicker;

        /// <summary>
        /// 输入框
        /// </summary>
        private InputField inputField;

        /// <summary>
        /// Toggle
        /// </summary>
        private Button button;

        private DateTime dateTime = DateTimeUtil.Zero;
        /// <summary>
        /// 日期时间
        /// </summary>
        public DateTime DateTime
        {
            get { return dateTime; }
            set
            {
                dateTime = value;
                if (dateTime == DateTimeUtil.Zero)
                {
                    inputField.text = "";
                }
                else
                {
                    inputField.text = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
        }

        void Start()
        {
            dateTimePicker = transform.Find("DateTimePicker").GetComponent<DateTimePicker>();
            inputField = transform.GetComponent<InputField>();
            button = transform.Find("Button").GetComponent<Button>();
            dateTimePicker.gameObject.SetActive(false);

            button.onClick.AddListener(button_onClick);
            dateTimePicker.OnSubmit.AddListener(dateTimePicker_OnSubmit);
            dateTimePicker.OnClear.AddListener(dateTimePicker_OnClear);
        }

        private void button_onClick()
        {
            if (string.IsNullOrEmpty(inputField.text))
            {
                dateTimePicker.Show();
            }
            else
            {
                dateTimePicker.Show(DateTime);
            }
        }


        private void dateTimePicker_OnSubmit(DateTime time)
        {
            DateTime = time;
            dateTimePicker.Hide();
        }

        private void dateTimePicker_OnClear()
        {
            DateTime = DateTimeUtil.Zero;
        }
    }
}
