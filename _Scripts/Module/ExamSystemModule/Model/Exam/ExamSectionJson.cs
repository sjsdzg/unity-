using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
    public class ExamSectionJson
    {
        /// <summary>
        /// 试卷节id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 题目列表
        /// </summary>
        public List<ExamQuesJson> QuesJsons { get; set; }

        public ExamSectionJson()
        {
            QuesJsons = new List<ExamQuesJson>();
        }
    }
}
