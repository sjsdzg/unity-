using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Module;

namespace XFramework.UI
{
    public class UserOperationLogQueryBar : QueryBar
    {
        public const string SOFTWARE_ID = "software_id";//软件ID
        public const string USER_ID = "user_id";//用户id
        public const string REAL_NAME = "description";//日志描述

        /// <summary>
        /// 用户名InputField
        /// </summary>
        private InputField inputFieldUserName;

        /// <summary>
        /// 姓名InputField
        /// </summary>
        private InputField inputFieldRealName;

        /// <summary>
        /// 用户
        /// </summary>
        private User user;

        public override void OnAwake()
        {
            base.OnAwake();
            inputFieldUserName = transform.Find("Conditions/InputFieldUserName").GetComponent<InputField>();
            inputFieldRealName = transform.Find("Conditions/InputFieldKeyword").GetComponent<InputField>();
        }

        protected override List<SqlCondition> BuildSqlConditions()
        {
            List<SqlCondition> sqlConditions = new List<SqlCondition>();
            //软件ID
            sqlConditions.Add(new SqlCondition(SOFTWARE_ID, SqlOption.Equal, App.Instance.SoftwareId));
            //关键字
            sqlConditions.Add(new SqlCondition(USER_ID, SqlOption.Equal, user.Id));
            //日志描述
            if (!string.IsNullOrEmpty(inputFieldRealName.text))
            {
                sqlConditions.Add(new SqlCondition(REAL_NAME, SqlOption.Like, "%" + inputFieldRealName.text + "%"));
            }
            //排序 按时间降序
            sqlConditions.Add(new SqlCondition(Constants.CREATE_TIME, SqlOption.OrderBy, Constants.DESC));
            return sqlConditions;
        }

        /// <summary>
        /// 设置用户
        /// </summary>
        /// <param name="user"></param>
        public void SetUser(User user)
        {
            this.user = user;
            inputFieldUserName.text = user.UserName;
        }
    }
}
