using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
    /// <summary>
    /// 试卷章节
    /// </summary>
    public class PaperSection
    {
        /// <summary>
        /// 试卷节id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 试卷节名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 试卷节说明
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 题型
        /// </summary>
        public int QType { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// 分数
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// 题目列表
        /// </summary>
        public List<Question> Questions { get; set; }

        public PaperSection()
        {
            Questions = new List<Question>();
        }
    }
}
