using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
    /// <summary>
    /// Sql条件
    /// </summary>
    public class SqlCondition
    {
        /// <summary>
        /// 条件名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Sql运算符
        /// </summary>
        public SqlOption Option { get; set; }
        /// <summary>
        /// Sql类型
        /// </summary>
        public SqlType Type { get; set; }
        /// <summary>
        /// 条件值
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 构造函数，SqlType默认String类型
        /// </summary>
        /// <param name="name"></param>
        /// <param name="option"></param>
        /// <param name="value"></param>
        public SqlCondition(string name, SqlOption option, Object value)
        {
            this.Name = name;
            this.Option = option;
            this.Type = SqlType.String;
            this.Value = value;
        }

        public SqlCondition(string name, SqlOption option, SqlType type, Object value)
        {
            this.Name = name;
            this.Option = option;
            this.Type = type;
            this.Value = value;
        }

        /// <summary>
        /// 获取通用Sql条件列表
        /// </summary>
        /// <returns></returns>
        public static List<SqlCondition> ListBySoftwareId() 
        {
            List<SqlCondition> sqlConditions = new List<SqlCondition>();
            //软件ID
            sqlConditions.Add(new SqlCondition(Constants.SOFTWARE_ID, SqlOption.Equal, SqlType.String, App.Instance.SoftwareId));
            return sqlConditions;
        }
    }
}
