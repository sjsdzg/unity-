using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using XFramework.Module;

namespace XFramework.UI
{
    public class PaperQueryBar : QueryBar
    {
        /// <summary>
        /// 关键词InputField
        /// </summary>
        private InputField inputFieldKeyword;

        /// <summary>
        /// 试卷分类Dropdown
        /// </summary>
        private Dropdown dropdownCategory;

        /// <summary>
        /// 试卷状态Dropdown
        /// </summary>
        private Dropdown dropdownStatus;

        /// <summary>
        /// 试卷分类列表
        /// </summary>
        private List<PaperCategory> m_PaperCategorys;

        public override void OnAwake()
        {
            base.OnAwake();
            inputFieldKeyword = transform.Find("Conditions/InputFieldKeyword").GetComponent<InputField>();
            dropdownCategory = transform.Find("Conditions/DropdownCategory").GetComponent<Dropdown>();
            dropdownStatus = transform.Find("Conditions/DropdownStatus").GetComponent<Dropdown>();
        }

        protected override List<SqlCondition> BuildSqlConditions()
        {
            List<SqlCondition> sqlConditions = new List<SqlCondition>();
            //软件ID
            sqlConditions.Add(new SqlCondition(Constants.SOFTWARE_ID, SqlOption.Equal, SqlType.String, App.Instance.SoftwareId));
            //考试名称
            if (!string.IsNullOrEmpty(inputFieldKeyword.text))
            {
                sqlConditions.Add(new SqlCondition(Constants.NAME, SqlOption.Like, SqlType.String, "%" + inputFieldKeyword.text + "%"));
            }
            //试卷分类id
            if (dropdownCategory.value > 0)
            {
                PaperCategory paperCategory = m_PaperCategorys[dropdownCategory.value - 1];
                sqlConditions.Add(new SqlCondition(Constants.CATEGORY_ID, SqlOption.Equal, SqlType.String, paperCategory.Id));
            }
            //状态
            if (dropdownStatus.value > 0)
            {
                sqlConditions.Add(new SqlCondition(Constants.STATUS, SqlOption.Equal, SqlType.Int, dropdownStatus.value - 1));
            }
            //排序 按时间降序
            sqlConditions.Add(new SqlCondition(Constants.CREATE_TIME, SqlOption.OrderBy, "desc"));
            return sqlConditions;
        }

        /// <summary>
        /// 设置试卷分类Dropdown
        /// </summary>
        /// <param name="questionBanks"></param>
        public void SetCategory(List<PaperCategory> paperCategorys)
        {
            m_PaperCategorys = paperCategorys;

            List<Dropdown.OptionData> optionDatas = new List<Dropdown.OptionData>();
            optionDatas.Add(new Dropdown.OptionData("全部"));
            for (int i = 0; i < m_PaperCategorys.Count; i++)
            {
                PaperCategory paperCategory = m_PaperCategorys[i];
                Dropdown.OptionData optionData = new Dropdown.OptionData(paperCategory.Name);
                optionDatas.Add(optionData);
            }
            dropdownCategory.options.Clear();
            dropdownCategory.options = optionDatas;

            dropdownCategory.value = 0;
        }
    }
}
