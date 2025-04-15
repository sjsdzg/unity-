using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
    public class ExamQuesJson
    {
        /// <summary>
        /// 试题id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 答案
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 0 错误 1 正确 2未知
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 得分
        /// </summary>
        public int Score { get; set; }
    }
}
