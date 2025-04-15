using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFramework.Module
{
    public class RangeVariable : Variable
    {
        /// <summary>
        /// 最小值
        /// </summary>
        public object MinValue { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public object MaxValue { get; set; }

        public override void OnConvertType(Type conversionType)
        {
            base.OnConvertType(conversionType);
            MinValue = Convert.ChangeType(MinValue, conversionType);
            MaxValue = Convert.ChangeType(MaxValue, conversionType);
        }

        public override bool Validate()
        {
            if (Type == VariableType.Float)
            {
                if ((float)Value > (float)MinValue && (float)Value < (float)MaxValue)
                {
                    return true;
                }
            }
            else if (Type == VariableType.Integer)
            {
                if ((int)Value > (int)MinValue && (int)Value < (int)MaxValue)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
