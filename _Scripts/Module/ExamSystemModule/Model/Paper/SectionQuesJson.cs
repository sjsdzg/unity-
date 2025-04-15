using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
    public class SectionQuesJson
    {
        /// <summary>
        /// 试题id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 试题类型 1:单选题 2:多选题 3:判断题 4:填空题 5:名词解释 6:简答题
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 试题题干
        /// </summary>
        public string Content { get; set; }
    }
}
