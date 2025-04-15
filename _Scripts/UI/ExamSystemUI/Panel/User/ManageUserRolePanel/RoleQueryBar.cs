﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Module;

namespace XFramework.UI
{
    public class RoleQueryBar : QueryBar
    {
        public const string NAME = "name";//关键词

        public const string STATUS = "status";//用户角色状态

        /// <summary>
        /// 关键词InputField
        /// </summary>
        private InputField inputFieldKeyword;

        /// <summary>
        /// 用户角色状态Dropdown
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
            //角色名称
            if (!string.IsNullOrEmpty(inputFieldKeyword.text))
            {
                sqlConditions.Add(new SqlCondition(Constants.NAME, SqlOption.Like, SqlType.String, "%" + inputFieldKeyword.text + "%"));
            }
            //角色状态
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
