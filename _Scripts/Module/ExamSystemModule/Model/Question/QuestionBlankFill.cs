using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
    /// <summary>
    /// 填空题 type : 4
    /// </summary>
    public class QuestionBlankFill : Question
    {
        /// <summary>
        /// 填空列表
        /// </summary>
        public List<QBlank> Blanks { get; set; }
        /// <summary>
        /// 是否无序
        /// </summary>
        public bool IsComplex { get; set; }

        public QuestionBlankFill()
        {
            Type = 4;
            Blanks = new List<QBlank>();
        }
    }
}
