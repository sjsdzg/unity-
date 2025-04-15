using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using XFramework.Module;

namespace XFramework.UI
{
    public class MyScoreQueryBar : QueryBar
    {
        public const string EXAM_NAME = "exam_name";//关键词
        public const string EXAM_CATEGORY_ID = "exam_category_id";//试卷分类Id
        public const string USER_NAME = "user_name";//用户名
        /// <summary>
        /// 关键词InputField
        /// </summary>
        private InputField inputFieldKeyword;

        /// <summary>
        /// 试卷分类Dropdown
        /// </summary>
        private Dropdown dropdownCategory;

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 试卷分类列表
        /// </summary>
        private List<ExamCategory> m_ExamCategorys;

        public override void OnAwake()
        {
            base.OnAwake();
            inputFieldKeyword = transform.Find("Conditions/InputFieldKeyword").GetComponent<InputField>();
            dropdownCategory = transform.Find("Conditions/DropdownCategory").GetComponent<Dropdown>();
        }

        protected override List<SqlCondition> BuildSqlConditions()
        {
            List<SqlCondition> sqlConditions = new List<SqlCondition>();
            //软件ID
            sqlConditions.Add(new SqlCondition(Constants.SOFTWARE_ID, SqlOption.Equal, SqlType.String, App.Instance.SoftwareId));
            //考试名称
            if (!string.IsNullOrEmpty(inputFieldKeyword.text))
            {
                sqlConditions.Add(new SqlCondition(Constants.EXAM_NAME, SqlOption.Like, SqlType.String, "%" + inputFieldKeyword.text + "%"));
            }
            //试卷分类id
            if (dropdownCategory.value > 0)
            {
                ExamCategory examCategory = m_ExamCategorys[dropdownCategory.value - 1];
                sqlConditions.Add(new SqlCondition(Constants.EXAM_CATEGORY_ID, SqlOption.Equal, SqlType.String, examCategory.Id));
            }
            //用户名称
            sqlConditions.Add(new SqlCondition(Constants.USER_NAME, SqlOption.Equal, SqlType.String, UserName));
            //排序 按时间降序
            sqlConditions.Add(new SqlCondition(Constants.END_TIME, SqlOption.OrderBy, Constants.DESC));
            return sqlConditions;
        }

        /// <summary>
        /// 设置试卷分类Dropdown
        /// </summary>
        /// <param name="examCategorys"></param>
        public void SetCategory(List<ExamCategory> examCategorys)
        {
            m_ExamCategorys = examCategorys;

            List<Dropdown.OptionData> optionDatas = new List<Dropdown.OptionData>();
            optionDatas.Add(new Dropdown.OptionData("全部"));
            for (int i = 0; i < m_ExamCategorys.Count; i++)
            {
                ExamCategory examCategory = m_ExamCategorys[i];
                Dropdown.OptionData optionData = new Dropdown.OptionData(examCategory.Name);
                optionDatas.Add(optionData);
            }
            dropdownCategory.options.Clear();
            dropdownCategory.options = optionDatas;

            dropdownCategory.value = 0;
        }

        public void SetUserName(string userName)
        {

        }
    }
}
