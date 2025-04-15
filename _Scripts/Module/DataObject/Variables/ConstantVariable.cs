using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFramework.Module
{
    public class ConstantVariable : Variable
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public object DefaultValue { get; set; }

        public override void OnConvertType(Type conversionType)
        {
            base.OnConvertType(conversionType);
            DefaultValue = Convert.ChangeType(DefaultValue, conversionType);
        }

        public override bool Validate()
        {
            if (object.Equals(Value, DefaultValue))
            {
                return true;
            }

            return false;
        }
    }
}
