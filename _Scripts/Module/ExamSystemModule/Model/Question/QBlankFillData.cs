using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
    public class QBlankFillData
    {
        /// <summary>
        /// 填空列表
        /// </summary>
        public List<QBlank> Blanks { get; set; }

        /// <summary>
        /// 是否无序
        /// </summary>
        public bool IsComplex { get; set; }
    }
}
