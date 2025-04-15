using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
    /// <summary>
    /// 考试
    /// </summary>
    public class Exam
    {
        /// <summary>
        /// 考试id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 考试名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 软件id
        /// <summary>
        public string SoftwareId { get; set; }
        /// <summary>
        /// 试卷id
        /// </summary>
        public string PaperId { get; set; }
        /// <summary>
        /// 考试分类id
        /// </summary>
        public string CategoryId { get; set; }
        /// <summary>
        /// 考试状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 考试时长
        /// </summary>
        public int Duration { get; set; }
        /// <summary>
        /// 开考时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 成绩公布时间
        /// </summary>
        public DateTime ShowTime { get; set; }
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
        /// 试题排列顺序
        /// </summary>
        public int QuestionOrder { get; set; }
        /// <summary>
        /// 是否公布答案
        /// </summary>
        public int ShowKey { get; set; }
        /// <summary>
        /// 显示形式
        /// </summary>
        public int ShowMode { get; set; }
        /// <summary>
        /// 考试说明
        /// </summary>
        public string Remark { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("考试id:" + Id + "  ");
            sb.Append("考试名称:" + Name + "  ");
            sb.Append("试卷id:" + PaperId + "  ");
            sb.Append("考试分类id:" + CategoryId + "  ");
            sb.Append("考试状态:" + Status + "  ");
            sb.Append("开考时间:" + StartTime + "  ");
            sb.Append("结束时间:" + EndTime + "  ");
            sb.Append("成绩公布时间:" + ShowTime + "  ");
            sb.Append("创建人:" + Poster + "  ");
            sb.Append("创建日期:" + CreateTime + "  ");
            sb.Append("修改人:" + Modifier + "  ");
            sb.Append("修改日期:" + UpdateTime + "  ");
            sb.Append("试题排列顺序:" + QuestionOrder + "  ");
            sb.Append("是否公布答案:" + ShowKey + "  ");
            sb.Append("显示形式:" + ShowMode + "  ");
            sb.Append("试卷说明:" + Remark + "  ");
            return sb.ToString();
        }
    }
}
