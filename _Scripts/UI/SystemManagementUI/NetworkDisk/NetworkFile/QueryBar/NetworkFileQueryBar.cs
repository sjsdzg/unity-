using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using XFramework.Module;

namespace XFramework.UI
{
    public class NetworkFileQueryBar : QueryBar
    {
        /// <summary>
        /// InputField
        /// </summary>
        private InputField inputFieldKeyword;

        /// <summary>
        /// 类型拼接字符串
        /// </summary>
        private string joinTypes = string.Empty;
        
        public override void OnAwake()
        {
            base.OnAwake();
            inputFieldKeyword = transform.Find("Conditions/InputFieldKeyword").GetComponent<InputField>();
        }

        protected override List<SqlCondition> BuildSqlConditions()
        {
            List<SqlCondition> sqlConditions = new List<SqlCondition>();
            //关键字
            sqlConditions.Add(new SqlCondition(Constants.USER_ID, SqlOption.Equal, App.Instance.SoftwareId));
            //sqlConditions.Add(new SqlCondition(Constants.USER_ID, SqlOption.Equal, ""));
            //文件名称
            if (!string.IsNullOrEmpty(inputFieldKeyword.text))
            {
                sqlConditions.Add(new SqlCondition(Constants.NAME, SqlOption.Like, "%" + inputFieldKeyword.text + "%"));
            }
            // 文件类型
            sqlConditions.Add(new SqlCondition(Constants.TYPE, SqlOption.In, joinTypes));
            //排序 按时间降序
            sqlConditions.Add(new SqlCondition(Constants.CREATE_TIME, SqlOption.OrderBy, Constants.DESC));
            return sqlConditions;
        }

        public void SetFileTypes(List<string> types)
        {
            string[] array = types.ToArray();
            joinTypes = string.Join(",", array);
        }
    }
}

