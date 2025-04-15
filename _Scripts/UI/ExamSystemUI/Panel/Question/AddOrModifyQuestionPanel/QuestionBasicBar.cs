using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XFramework.Module;

namespace XFramework.UI
{
    /// <summary>
    /// 试题基本数据栏
    /// </summary>
    public class QuestionBasicBar : MonoBehaviour, IValidate
    {
        public class OnValueChangedEvent : UnityEvent<int, int> { }

        private OnValueChangedEvent m_QTypeChanged = new OnValueChangedEvent();
        /// <summary>
        /// 题目类型更改时，触发
        /// </summary>
        public OnValueChangedEvent QTypeChanged
        {
            get { return m_QTypeChanged; }
            set { m_QTypeChanged = value; }
        }

        private int qType = 1;//默认单选题
        /// <summary>
        /// 试题类型
        /// </summary>
        public int QType
        {
            get
            {
                qType = dropdownQType.value + 1;
                return qType;
            }
            set
            {
                qType = value;
                dropdownQType.value = qType - 1;
            }
        }

        private string qBankId = "";
        /// <summary>
        /// 试题库Id
        /// </summary>
        public string QBankId
        {
            get
            {
                if (dropdownQBank.value > 0)
                {
                    QuestionBank questionBank = m_QuestionBanks[dropdownQBank.value - 1];
                    qBankId = questionBank.Id;
                }
                return qBankId;
            }
            set
            {
                qBankId = value;
                if (m_QuestionBanks != null)
                {
                    for (int i = 0; i < m_QuestionBanks.Count; i++)
                    {
                        if (m_QuestionBanks[i].Id == qBankId)
                        {
                            dropdownQBank.value = i + 1;
                        }
                    }
                }
            }
        }

        private int qLevel = 2;//默认一般
        /// <summary>
        /// 试题难度
        /// </summary>
        public int QLevel
        {
            get
            {
                qLevel = dropdownQLevel.value + 1;
                return qLevel;
            }
            set
            {
                qLevel = value;
                dropdownQLevel.value = qLevel - 1;
            }
        }

        private int qStatus = 1;//默认开启
        /// <summary>
        /// 试题状态
        /// </summary>
        public int QStatus
        {
            get
            {
                qStatus = dropdownQStatus.value;
                return qStatus;
            }
            set
            {
                qStatus = value;
                dropdownQStatus.value = qStatus;
            }
        }

        private string qFrom = "原创";
        /// <summary>
        /// 试题来源
        /// </summary>
        public string QFrom
        {
            get
            {
                qFrom = inputFieldFrom.text;
                return qFrom;
            }
            set
            {
                qFrom = value;
                inputFieldFrom.text = qFrom;
            }
        }

        /// <summary>
        /// 试题类型Dropdown
        /// </summary>
        private Dropdown dropdownQType;

        /// <summary>
        /// 试题库Dropdown
        /// </summary>
        private Dropdown dropdownQBank;

        /// <summary>
        /// 试题难度Dropdown
        /// </summary>
        private Dropdown dropdownQLevel;

        /// <summary>
        /// 试题状态Dropdown
        /// </summary>
        private Dropdown dropdownQStatus;

        /// <summary>
        /// 试题来源InputField
        /// </summary>
        private InputField inputFieldFrom;

        /// <summary>
        /// 试题库列表
        /// </summary>
        private List<QuestionBank> m_QuestionBanks;

        /// <summary>
        /// 警告提示文本
        /// </summary>
        private Text warning;

        void Awake()
        {
            dropdownQType = transform.Find("Data/DropdownQType").GetComponent<Dropdown>();
            dropdownQBank = transform.Find("Data/DropdownQBank").GetComponent<Dropdown>();
            dropdownQLevel = transform.Find("Data/DropdownQLevel").GetComponent<Dropdown>();
            dropdownQStatus = transform.Find("Data/DropdownQStatus").GetComponent<Dropdown>();
            inputFieldFrom = transform.Find("Data/InputFieldQFrom").GetComponent<InputField>();
            warning = transform.Find("Title/Warning").GetComponent<Text>();
            warning.text = "";

            //事件
            dropdownQType.onValueChanged.AddListener(x => 
            {
                QTypeChanged.Invoke(qType, QType);
                qType = QType;
            });
        }

        /// <summary>
        /// 设置试题库Dropdown
        /// </summary>
        /// <param name="questionBanks"></param>
        public void SetQBank(List<QuestionBank> questionBanks)
        {
            if (questionBanks != null && questionBanks.Count > 0)
            {
                m_QuestionBanks = questionBanks;

                List<Dropdown.OptionData> optionDatas = new List<Dropdown.OptionData>();
                optionDatas.Add(new Dropdown.OptionData("所属题库"));
                for (int i = 0; i < m_QuestionBanks.Count; i++)
                {
                    QuestionBank questionBank = m_QuestionBanks[i];
                    Dropdown.OptionData optionData = new Dropdown.OptionData(questionBank.Name);
                    optionDatas.Add(optionData);
                }

                dropdownQBank.options.Clear();
                dropdownQBank.options = optionDatas;

                //设置默认值
                dropdownQBank.value = 0;
            }

            if (!string.IsNullOrEmpty(qBankId))
            {
                for (int i = 0; i < m_QuestionBanks.Count; i++)
                {
                    if (m_QuestionBanks[i].Id == qBankId)
                    {
                        dropdownQBank.value = i + 1;
                    }
                }
            }

        }

        /// <summary>
        /// 校验
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            bool result = true;

            string text = "";
            if (dropdownQBank.value == 0)
            {
                text = "*请选择一个试题库*";
                result = false;
            }

            warning.text = text;
            return result;
        }
    }
}
