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
    public class PaperBasicBar : MonoBehaviour, IValidate
    {
        private string paperName;
        /// <summary>
        /// 试卷名称
        /// </summary>
        public string PaperName
        {
            get
            {
                paperName = inputFieldName.text;
                return paperName;
            }
            set
            {
                paperName = value;
                inputFieldName.text = paperName;
            }
        }

        private string categoryId;
        /// <summary>
        /// 试卷分类Id
        /// </summary>
        public string CategoryId
        {
            get
            {
                if (dropdownCategory.value > 0)
                {
                    PaperCategory questionBank = m_PaperCategorys[dropdownCategory.value - 1];
                    categoryId = questionBank.Id;
                }
                return categoryId;
            }
            set
            {
                categoryId = value;
                if (m_PaperCategorys != null)
                {
                    for (int i = 0; i < m_PaperCategorys.Count; i++)
                    {
                        if (m_PaperCategorys[i].Id == categoryId)
                        {
                            dropdownCategory.value = i + 1;
                        }
                    }
                }
            }
        }

        private int status = 1;
        /// <summary>
        /// 试卷状态
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

        private int totalScore;
        /// <summary>
        /// 试卷总分
        /// </summary>
        public int TotalScore
        {
            get
            {
                TotalScore = int.Parse(inputFieldTotalScore.text);
                return totalScore; 
            }
            set
            {
                totalScore = value;
                inputFieldTotalScore.text = totalScore.ToString();
            }
        }

        private int passScore;
        /// <summary>
        /// 及格分数
        /// </summary>
        public int PassScore
        {
            get
            {
                passScore = int.Parse(inputFieldPassScore.text);
                return passScore;
            }
            set
            {
                passScore = value;
                inputFieldPassScore.text = passScore.ToString();
            }
        }

        //private int durtation;
        ///// <summary>
        ///// 考试时长
        ///// </summary>
        //public int Durtation
        //{
        //    get
        //    {
        //        durtation = int.Parse(inputFieldDurtation.text);
        //        return durtation;
        //    }
        //    set
        //    {
        //        durtation = value;
        //        inputFieldDurtation.text = durtation.ToString();
        //    }
        //}

        /// <summary>
        /// 试卷名称InputField
        /// </summary>
        private InputField inputFieldName;

        /// <summary>
        /// 试卷分类Dropdown
        /// </summary>
        private Dropdown dropdownCategory;

        /// <summary>
        /// 试卷状态Dropdown
        /// </summary>
        private Dropdown dropdownStatus;

        /// <summary>
        /// 总分InputField
        /// </summary>
        private InputField inputFieldTotalScore;

        /// <summary>
        /// 及格分数InputField
        /// </summary>
        private InputField inputFieldPassScore;

        /// <summary>
        /// 考试时长InputField
        /// </summary>
        //private InputField inputFieldDurtation;

        /// <summary>
        /// 试卷分类列表
        /// </summary>
        private List<PaperCategory> m_PaperCategorys;

        /// <summary>
        /// 警告提示文本
        /// </summary>
        private Text warning;

        void Awake()
        {
            inputFieldName = transform.Find("Data/Name/InputField").GetComponent<InputField>();
            dropdownCategory = transform.Find("Data/Category/Dropdown").GetComponent<Dropdown>();
            dropdownStatus = transform.Find("Data/Status/Dropdown").GetComponent<Dropdown>();
            inputFieldTotalScore = transform.Find("Data/TotalScore/InputField").GetComponent<InputField>();
            inputFieldPassScore = transform.Find("Data/PassScore/InputField").GetComponent<InputField>();
            //inputFieldDurtation = transform.Find("Data/Duration/InputField").GetComponent<InputField>();
            warning = transform.Find("Title/Warning").GetComponent<Text>();
            warning.text = "";

            inputFieldTotalScore.text = "0";
            inputFieldTotalScore.readOnly = true;
            inputFieldPassScore.text = "0";
            //inputFieldDurtation.text = "0";

            //事件
            inputFieldPassScore.onEndEdit.AddListener(inputFieldPassScore_onEndEdit);
            //inputFieldDurtation.onEndEdit.AddListener(inputFieldDurtation_onEndEdit);
        }


        private void inputFieldPassScore_onEndEdit(string arg0)
        {
            inputFieldPassScore.text = inputFieldPassScore.text.TrimStart('0');
            if (string.IsNullOrEmpty(inputFieldPassScore.text))
            {
                inputFieldPassScore.text = "0";
            }
        }

        //private void inputFieldDurtation_onEndEdit(string arg0)
        //{
        //    inputFieldDurtation.text = inputFieldDurtation.text.TrimStart('0');
        //    if (string.IsNullOrEmpty(inputFieldDurtation.text))
        //    {
        //        inputFieldDurtation.text = "0";
        //    }
        //}

        /// <summary>
        /// 设置试题库Dropdown
        /// </summary>
        /// <param name="questionBanks"></param>
        public void SetCategory(List<PaperCategory> paperCategorys)
        {
            m_PaperCategorys = paperCategorys;
            List<Dropdown.OptionData> optionDatas = new List<Dropdown.OptionData>();
            optionDatas.Add(new Dropdown.OptionData("请选择"));
            for (int i = 0; i < m_PaperCategorys.Count; i++)
            {
                PaperCategory paperCategory = paperCategorys[i];
                Dropdown.OptionData optionData = new Dropdown.OptionData(paperCategory.Name);
                optionDatas.Add(optionData);
            }

            dropdownCategory.options.Clear();
            dropdownCategory.options = optionDatas;

            //设置默认值
            dropdownCategory.value = 0;

            if (!string.IsNullOrEmpty(categoryId))
            {
                for (int i = 0; i < paperCategorys.Count; i++)
                {
                    if (paperCategorys[i].Id == categoryId)
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

            if (string.IsNullOrEmpty(inputFieldName.text))
            {
                validation = "*请输入试卷名称*";
                result = false;
            }

            if (dropdownCategory.value == 0)
            {
                validation = "*请选择一个试卷分类*";
                result = false;
            }

            if (PassScore <= 0)
            {
                validation = "*请输入及格分数*";
                result = false;
            }

            //if (Durtation <= 0)
            //{
            //    validation = "*请输入考试时长（分钟）*";
            //    result = false;
            //}

            warning.text = validation;
            return result;
        }
    }
}
