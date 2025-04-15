﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Module;

namespace XFramework.UI
{
    public class ExamCategoryQueryBar : QueryBar
    {
        /// <summary>
        /// 关键词InputField
        /// </summary>
        private InputField inputFieldKeyword;

        /// <summary>
        /// 题库状态Dropdown
        /// </summary>
        private Dropdown dropdownStatus;

        public override void OnAwake()
        {
            base.OnAwake();
            inputFieldKeyword = transform.Find("Conditions/InputFieldKeyword").GetComponent<InputField>();
            dropdownStatus = transform.Find("Conditions/DropdownStatus").GetComponent<Dropdown>();
        }

        protected override List<SqlCondition> BuildSqlConditions()
        {
            List<SqlCondition> sqlConditions = new List<SqlCondition>();
            //软件ID
            sqlConditions.Add(new SqlCondition(Constants.SOFTWARE_ID, SqlOption.Equal, SqlType.String, App.Instance.SoftwareId));
            //考试分类名称
            if (!string.IsNullOrEmpty(inputFieldKeyword.text))
            {
                sqlConditions.Add(new SqlCondition(Constants.NAME, SqlOption.Like, SqlType.String, "%" + inputFieldKeyword.text + "%"));
            }
            //考试分类状态
            if (dropdownStatus.value > 0)
            {
                sqlConditions.Add(new SqlCondition(Constants.STATUS, SqlOption.Equal, SqlType.Int, (dropdownStatus.value - 1)));
            }
            //排序 按时间降序
            sqlConditions.Add(new SqlCondition(Constants.CREATE_TIME, SqlOption.OrderBy, Constants.DESC));
            return sqlConditions;
        }
    }
}
