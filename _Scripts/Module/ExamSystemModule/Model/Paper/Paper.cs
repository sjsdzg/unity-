using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
    /// <summary>
    /// 试卷
    /// </summary>
    public class Paper
    {
        /// <summary>
        /// 试卷id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 试卷名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 软件id
        /// <summary>
        public string SoftwareId { get; set; }
        /// <summary>
        /// 试卷种类id
        /// </summary>
        public string CategoryId { get; set; }
        /// <summary>
        /// 试卷状态：0 禁用 1 启用
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 总分
        /// </summary>
        public int TotalScore { get; set; }
        /// <summary>
        /// 及格分数
        /// </summary>
        public int PassScore { get; set; }
        /// <summary>
        /// 试卷数据
        /// </summary>
        public string Data { get; set; }
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
        /// 试卷说明
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 试卷节列表
        /// </summary>
        public List<PaperSection> Sections { get; set; }

        public Paper()
        {
            Sections = new List<PaperSection>();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("试卷id:" + Id + "  ");
            sb.Append("试卷名称:" + Name + "  ");
            sb.Append("试卷种类id:" + CategoryId + "  ");
            sb.Append("试卷状态:" + Status + "  ");
            sb.Append("总分:" + TotalScore + "  ");
            sb.Append("及格分数:" + PassScore + "  ");
            sb.Append("试卷数据:" + Data + "  ");
            sb.Append("创建人:" + Poster + "  ");
            sb.Append("创建日期:" + CreateTime + "  ");
            sb.Append("修改人:" + Modifier + "  ");
            sb.Append("修改日期:" + UpdateTime + "  ");
            sb.Append("试卷说明:" + Remark + "  ");
            return sb.ToString();
        }
    }
}
