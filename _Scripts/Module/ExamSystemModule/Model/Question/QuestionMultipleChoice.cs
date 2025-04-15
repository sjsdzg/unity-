using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
    /// <summary>
    /// 多选题 type : 2
    /// </summary>
    public class QuestionMultipleChoice : Question
    {
        /// <summary>
        /// 选项列表
        /// </summary>
        public List<Option> Options { get; set; }

        public QuestionMultipleChoice()
        {
            Type = 2;
            Options = new List<Option>();
        }
    }
}
