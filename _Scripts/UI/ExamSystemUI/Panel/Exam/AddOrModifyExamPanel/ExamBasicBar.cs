using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XFramework.Common;
using XFramework.Module;

namespace XFramework.UI
{
    public class ExamBasicBar : MonoBehaviour
    {
        private string examName;
        /// <summary>
        /// 考试名称
        /// </summary>
        public string ExamName
        {
            get
            {
                examName = inputFieldName.text;
                return examName;
            }
            set
            {
                examName = value;
                inputFieldName.text = examName;
            }
        }

        private string categoryId;
        /// <summary>
        /// 考试分类Id
        /// </summary>
        public string CategoryId
        {
            get
            {
                if (dropdownCategory.value > 0)
                {
                    ExamCategory questionBank = m_ExamCategorys[dropdownCategory.value - 1];
                    categoryId = questionBank.Id;
                }
                return categoryId;
            }
            set
            {
                categoryId = value;
                if (m_ExamCategorys != null)
                {
                    for (int i = 0; i < m_ExamCategorys.Count; i++)
                    {
                        if (m_ExamCategorys[i].Id == categoryId)
                        {
                            dropdownCategory.value = i + 1;
                        }
                    }
                }
            }
        }

        private int status = 1;
        /// <summary>
        /// 考试状态
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
                dropdownStatus.value = Status;
            }
        }

        private int durtation;
        /// <summary>
        /// 考试时长
        /// </summary>
        public int Durtation
        {
            get
            {
                durtation = int.Parse(inputFieldDurtation.text);
                return durtation;
            }
            set
            {
                durtation = value;
                inputFieldDurtation.text = durtation.ToString();
            }
        }

        private int showKey;
        /// <summary>
        /// 显示答案
        /// </summary>
        public int ShowKey
        {
            get
            {
                showKey = dropdownShowKey.value;
                return showKey;
            }
            set
            {
                showKey = value;
                dropdownShowKey.value = showKey;
            }
        }

        private DateTime startTime = DateTimeUtil.Zero;
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime
        {
            get
            {
                startTime = inputFieldStartTime.DateTime;
                return startTime;
            }
            set
            {
                startTime = value;
                inputFieldStartTime.DateTime = startTime;
            }
        }

        private DateTime endTime = DateTimeUtil.Zero;
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime EndTime
        {
            get
            {
                endTime = inputFieldEndTime.DateTime;
                return endTime;
            }
            set
            {
                endTime = value;
                inputFieldEndTime.DateTime = endTime;
            }
        }

        private DateTime showTime = DateTimeUtil.Zero;
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime ShowTime
        {
            get
            {
                showTime = inputFieldShowTime.DateTime;
                return showTime;
            }
            set
            {
                showTime = value;
                inputFieldShowTime.DateTime = showTime;
            }
        }

        private int showMode;
        /// <summary>
        /// 显示形式
        /// </summary>
        public int ShowMode
        {
            get
            {
                showMode = dropdownShowMode.value;
                return showMode;
            }
            set
            {
                showMode = value;
                dropdownShowMode.value = showMode;
            }
        }

        private int questionOrder;
        /// <summary>
        /// 试题顺序
        /// </summary>
        public int QuestionOrder
        {
            get
            {
                questionOrder = dropdownQuestionOrder.value;
                return questionOrder;
            }
            set
            {
                questionOrder = value;
                dropdownQuestionOrder.value = questionOrder;
            }
        }

        /// <summary>
        /// 考试名称InputField
        /// </summary>
        private InputField inputFieldName;

        /// <summary>
        /// 试卷名称InputField
        /// </summary>
        private InputField inputFieldPaper;

        /// <summary>
        /// 考试分类Dropdown
        /// </summary>
        private Dropdown dropdownCategory;

        /// <summary>
        /// 考试状态Dropdown
        /// </summary>
        private Dropdown dropdownStatus;

        /// <summary>
        /// 考试时长InputField
        /// </summary>
        private InputField inputFieldDurtation;

        /// <summary>
        /// 显示答案Dropdown
        /// </summary>
        private Dropdown dropdownShowKey;

        /// <summary>
        /// 开始时间InputField
        /// </summary>
        private InputFieldDateTime inputFieldStartTime;

        /// <summary>
        /// 结束时间InputField
        /// </summary>
        private InputFieldDateTime inputFieldEndTime;

        /// <summary>
        /// 公布成绩时间InputField
        /// </summary>
        private InputFieldDateTime inputFieldShowTime;

        /// <summary>
        /// 显示形式Dropdown
        /// </summary>
        private Dropdown dropdownShowMode;

        /// <summary>
        /// 试题顺序Dropdown
        /// </summary>
        private Dropdown dropdownQuestionOrder;

        /// <summary>
        /// 选择按钮
        /// </summary>
        private Button buttonSelect;

        /// <summary>
        /// 警告提示文本
        /// </summary>
        private Text warning;

        /// <summary>
        /// 考试分类列表
        /// </summary>
        private List<ExamCategory> m_ExamCategorys;

        private Paper paper;
        /// <summary>
        /// 当前选择的试卷
        /// </summary>
        public Paper Paper
        {
            get { return paper; }
            set
            {
                paper = value;
                if (paper != null)
                {
                    inputFieldPaper.text = paper.Name;
                }
                else
                {
                    inputFieldPaper.text = "";
                }
            }
        }

        void Awake()
        {
            inputFieldName = transform.Find("Data/Name/InputField").GetComponent<InputField>();
            inputFieldPaper = transform.Find("Data/Paper/InputField").GetComponent<InputField>();
            dropdownCategory = transform.Find("Data/Category/Dropdown").GetComponent<Dropdown>();
            dropdownStatus = transform.Find("Data/Status/Dropdown").GetComponent<Dropdown>();
            inputFieldDurtation = transform.Find("Data/Duration/InputField").GetComponent<InputField>();
            dropdownShowKey = transform.Find("Data/ShowKey/Dropdown").GetComponent<Dropdown>();
            inputFieldStartTime = transform.Find("Data/StartTime/InputFieldDateTime").GetComponent<InputFieldDateTime>();
            inputFieldEndTime = transform.Find("Data/EndTime/InputFieldDateTime").GetComponent<InputFieldDateTime>();
            inputFieldShowTime = transform.Find("Data/ShowTime/InputFieldDateTime").GetComponent<InputFieldDateTime>();
            dropdownShowMode = transform.Find("Data/ShowMode/Dropdown").GetComponent<Dropdown>();
            dropdownQuestionOrder = transform.Find("Data/QuestionOrder/Dropdown").GetComponent<Dropdown>();
            buttonSelect = transform.Find("Data/Paper/ButtonSelect").GetComponent<Button>();

            inputFieldPaper.readOnly = true;
            warning = transform.Find("Title/Warning").GetComponent<Text>();
            warning.text = "";

            inputFieldDurtation.text = "0";

            //事件
            buttonSelect.onClick.AddListener(buttonSelect_onClick);
            inputFieldDurtation.onEndEdit.AddListener(inputFieldDurtation_onEndEdit);
        }

        private void inputFieldDurtation_onEndEdit(string arg0)
        {
            inputFieldDurtation.text = inputFieldDurtation.text.TrimStart('0');
            if (string.IsNullOrEmpty(inputFieldDurtation.text))
            {
                inputFieldDurtation.text = "0";
            }
        }

        private void buttonSelect_onClick()
        {
            Transform popupPanelContainer = PanelManager.Instance.PanelContainers[PanelContainerType.PopupPanelContainer];
            popupPanelContainer.GetComponent<Image>().enabled = true;
            SelectPaperPanel_Data data = new SelectPaperPanel_Data();
            data.ExamBasicBar = this;
            PanelManager.Instance.OpenPanel(popupPanelContainer, EnumPanelType.SelectPaperPanel, data);
        }

        /// <summary>
        /// 设置考试分类Dropdown
        /// </summary>
        /// <param name="questionBanks"></param>
        public void SetCategory(List<ExamCategory> examCategorys)
        {
            if (examCategorys != null && examCategorys.Count > 0)
            {
                m_ExamCategorys = examCategorys;

                List<Dropdown.OptionData> optionDatas = new List<Dropdown.OptionData>();
                optionDatas.Add(new Dropdown.OptionData("请选择"));
                for (int i = 0; i < m_ExamCategorys.Count; i++)
                {
                    ExamCategory examCategory = examCategorys[i];
                    Dropdown.OptionData optionData = new Dropdown.OptionData(examCategory.Name);
                    optionDatas.Add(optionData);
                }

                dropdownCategory.options.Clear();
                dropdownCategory.options = optionDatas;

                //设置默认值
                dropdownCategory.value = 0;
            }

            if (!string.IsNullOrEmpty(categoryId))
            {
                for (int i = 0; i < examCategorys.Count; i++)
                {
                    if (examCategorys[i].Id == categoryId)
                    {
                        dropdownCategory.value = i + 1;
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
            string validation = "";

            if (inputFieldStartTime.DateTime == DateTimeUtil.Zero)
            {
                validation = "*请设置考试开始时间*";
                result = false;
            }

            if (inputFieldEndTime.DateTime == DateTimeUtil.Zero)
            {
                validation = "*请设置考试结束时间*";
                result = false;
            }

            if (inputFieldEndTime.DateTime <= inputFieldStartTime.DateTime)
            {
                validation = "*时间设置有误，请重新设置。*";
                result = false;
            }

            if (inputFieldShowTime.DateTime <= inputFieldEndTime.DateTime)
            {
                validation = "*时间设置有误，请重新设置。*";
                result = false;
            }

            if (inputFieldShowTime.DateTime == DateTimeUtil.Zero)
            {
                validation = "*请设置成绩公布时间*";
                result = false;
            }

            if (string.IsNullOrEmpty(inputFieldName.text))
            {
                validation = "*请输入考试名称*";
                result = false;
            }

            if (dropdownCategory.value == 0)
            {
                validation = "*请选择一个考试分类*";
                result = false;
            }

            if (string.IsNullOrEmpty(inputFieldPaper.text))
            {
                validation = "*请选择一张试卷*";
                result = false;
            }

            if (Durtation <= 0)
            {
                validation = "*请输入考试时长（分钟）*";
                result = false;
            }

            warning.text = validation;
            return result;
        }
    }
}
