using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFramework.Module
{
    public enum VariableType
    {
        Float,
        Integer,
        Boolean,
        String,
    }

    public class Variable
    {
        /// <summary>
        /// 变量名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 变量类型
        /// </summary>
        public VariableType Type { get; set; }

        /// <summary>
        /// 变量值
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        public void ConvertType()
        {
            Type conversionType = null;
            switch (Type)
            {
                case VariableType.Float:
                    conversionType = typeof(float);
                    break;
                case VariableType.Integer:
                    conversionType = typeof(int);
                    break;
                case VariableType.Boolean:
                    conversionType = typeof(bool);
                    break;
                case VariableType.String:
                    conversionType = typeof(string);
                    break;
                default:
                    break;
            }

            if (conversionType != null)
            {
                OnConvertType(conversionType);
            }
        }

        public virtual void OnConvertType(Type conversionType)
        {
            Value = Convert.ChangeType(Value, conversionType);
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <returns></returns>
        public virtual bool Validate()
        {
            return true;
        }
    }
}
