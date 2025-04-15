using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework.UI
{
    public class QuestionBankBasicBar : MonoBehaviour, IValidate
    {
        private string bankName = "";
        /// <summary>
        /// 题库名称
        /// </summary>
        public string BankName
        {
            get
            {
                bankName = inputFieldName.text;
                return bankName;
            }
            set
            {
                bankName = value;
                inputFieldName.text = bankName;
            }
        }

        private int status = 1;//默认开启
        /// <summary>
        /// 试题状态
        /// </summary>
        public int Status
        {
            get
            {
                status = dropdownStatus.value;
                return status;
            }
            set
            {
                status = value;
                dropdownStatus.value = status;
            }
        }

        private string remark = "";
        /// <summary>
        /// 题库说明
        /// </summary>
        public string Remark
        {
            get
            {
                remark = inputFieldRemark.text;
                return remark;
            }
            set
            {
                remark = value;
                inputFieldRemark.text = remark;
            }
        }

        /// <summary>
        /// 题库名称InputField
        /// </summary>
        private InputField inputFieldName;
        /// <summary>
        /// 题库状态Dropdown
        /// </summary>
        private Dropdown dropdownStatus;
        /// <summary>
        /// 题库说明InputField
        /// </summary>
        private InputField inputFieldRemark;
        /// <summary>
        /// 警告提示文本
        /// </summary>
        private Text warning;

        void Awake()
        {
            inputFieldName = transform.Find("Data/InputFieldName").GetComponent<InputField>();
            dropdownStatus = transform.Find("Data/DropdownStatus").GetComponent<Dropdown>();
            inputFieldRemark = transform.Find("Data/InputFieldRemark").GetComponent<InputField>();
            warning = transform.Find("Title/Warning").GetComponent<Text>();
            warning.text = "";
        }

        public bool Validate()
        {
            bool result = true;
            string text = "";

            if (string.IsNullOrEmpty(inputFieldName.text))
            {
                text = "*请输入题库名称*";
                result = false;
            }

            warning.text = text;
            return result;
        }
    }
}
