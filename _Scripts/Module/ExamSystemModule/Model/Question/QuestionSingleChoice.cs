using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
    /// <summary>
    /// 单选题 type : 1
    /// </summary>
    public class QuestionSingleChoice : Question
    {
        /// <summary>
        /// 选项列表
        /// </summary>
        public List<Option> Options { get; set; }

        public QuestionSingleChoice()
        {
            Type = 1;
            Options = new List<Option>();
        }
    }
}
