using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
    /// <summary>
    /// 试题
    /// </summary>
    public class Question
    {
        /// <summary>
        /// 试题id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 软件id
        /// <summary>
        public string SoftwareId { get; set; }
        /// <summary>
        /// 题库id
        /// </summary>
        public string BankId { get; set; }
        /// <summary>
        /// 试题类型 1:单选题 2:多选题 3:判断题 4:填空题 5:名词解释 6:简答题
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 难度 1:容易 2:一般 3:困难
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 试题来源
        /// </summary>
        public string From { get; set; }
        /// <summary>
        /// 试题状态 0:禁用 1:启用
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 试题题干
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 试题答案
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 试题解析
        /// </summary>
        public string Resolve { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Poster { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public string Modifier { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public string Data { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("试题id:" + Id + "  ");
            sb.Append("题库id:" + BankId + "  ");
            sb.Append("试题类型:" + Type + "  ");
            sb.Append("难度:" + Level + "  ");
            sb.Append("试题来源:" + From + "  ");
            sb.Append("试题状态:" + Status + "  ");
            sb.Append("试题题干:" + Content + "  ");
            sb.Append("试题答案:" + Key + "  ");
            sb.Append("试题解析:" + Resolve + "  ");
            sb.Append("创建人:" + Poster + "  ");
            sb.Append("创建日期:" + CreateTime + "  ");
            sb.Append("修改人:" + Modifier + "  ");
            sb.Append("修改日期:" + UpdateTime + "  ");
            sb.Append("数据:" + Data + "  ");
            return sb.ToString();
        }
    }
}
