using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
    /// <summary>
    /// 分数区间
    /// </summary>
    public class ScoreRange
    {
        /// <summary>
        /// 最小值
        /// </summary>
        public int Min { get; set; }
        /// <summary>
        /// 最大值
        /// </summary>
        public int Max { get; set; }

        public ScoreRange()
        {

        }

        public ScoreRange(int min, int max)
        {
            Min = min;
            Max = max;
        }
    }
}
