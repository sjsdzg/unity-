using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using XFramework.Module;

namespace XFramework.UI
{
    public class QuestionQueryBar : QueryBar
    {
        private int qType = 1;//默认单选题
        /// <summary>
        /// 试题类型
        /// </summary>
        public int QType
        {
            get
            {
                qType = dropdownQType.value;
                return qType;
            }
            set
            {
                qType = value;
                dropdownQType.value = qType;
            }
        }

        /// <summary>
        /// 关键词InputField
        /// </summary>
        private InputField inputFieldKeyword;

        /// <summary>
        /// 试题库Dropdown
        /// </summary>
        private Dropdown dropdownQBank;

        /// <summary>
        /// 试题类型Dropdown
        /// </summary>
        private Dropdown dropdownQType;

        /// <summary>
        /// 试题难度Dropdown
        /// </summary>
        private Dropdown dropdownQLevel;

        /// <summary>
        /// 试题状态Dropdown
        /// </summary>
        private Dropdown dropdownQStatus;

        /// <summary>
        /// 试题库列表
        /// </summary>
        private List<QuestionBank> m_QuestionBanks;

        public override void OnAwake()
        {
            base.OnAwake();
            inputFieldKeyword = transform.Find("Conditions/InputFieldKeyword").GetComponent<InputField>();
            dropdownQBank = transform.Find("Conditions/DropdownQBank").GetComponent<Dropdown>();
            dropdownQType = transform.Find("Conditions/DropdownQType").GetComponent<Dropdown>();
            dropdownQLevel = transform.Find("Conditions/DropdownQLevel").GetComponent<Dropdown>();
            dropdownQStatus = transform.Find("Conditions/DropdownQStatus").GetComponent<Dropdown>();
        }

        protected override List<SqlCondition> BuildSqlConditions()
        {
            List<SqlCondition> sqlConditions = new List<SqlCondition>();
            //软件ID
            sqlConditions.Add(new SqlCondition(Constants.SOFTWARE_ID, SqlOption.Equal, SqlType.String, App.Instance.SoftwareId));
            //题干
            if (!string.IsNullOrEmpty(inputFieldKeyword.text))
            {
                sqlConditions.Add(new SqlCondition(Constants.CONTENT, SqlOption.Like, SqlType.String, "%" + inputFieldKeyword.text + "%"));
            }
            //试题库id
            if (dropdownQBank.value > 0)
            {
                QuestionBank questionBank = m_QuestionBanks[dropdownQBank.value - 1];
                sqlConditions.Add(new SqlCondition(Constants.BANK_ID, SqlOption.Equal, SqlType.String, questionBank.Id));
            }
            //试题类型
            if (dropdownQType.value > 0)
            {
                sqlConditions.Add(new SqlCondition(Constants.TYPE, SqlOption.Equal, SqlType.Int, dropdownQType.value));
            }
            //试题难度
            if (dropdownQLevel.value > 0)
            {
                sqlConditions.Add(new SqlCondition(Constants.LEVEL, SqlOption.Equal, SqlType.Int, dropdownQLevel.value));
            }
            //试题状态
            if (dropdownQStatus.value > 0)
            {
                sqlConditions.Add(new SqlCondition(Constants.STATUS, SqlOption.Equal, SqlType.Int, (dropdownQStatus.value - 1)));
            }
            //排序 按时间降序
            sqlConditions.Add(new SqlCondition(Constants.CREATE_TIME, SqlOption.OrderBy, Constants.DESC));

            return sqlConditions;
        }

        /// <summary>
        /// 设置试题库Dropdown
        /// </summary>
        /// <param name="questionBanks"></param>
        public void SetQBank(List<QuestionBank> questionBanks)
        {
            m_QuestionBanks = questionBanks;

            List<Dropdown.OptionData> optionDatas = new List<Dropdown.OptionData>();
            optionDatas.Add(new Dropdown.OptionData("全部"));
            for (int i = 0; i < m_QuestionBanks.Count; i++)
            {
                QuestionBank questionBank = m_QuestionBanks[i];
                Dropdown.OptionData optionData = new Dropdown.OptionData(questionBank.Name);
                optionDatas.Add(optionData);
            }

            dropdownQBank.options.Clear();
            dropdownQBank.options = optionDatas;

            dropdownQBank.value = 0;
        }
    }
}
