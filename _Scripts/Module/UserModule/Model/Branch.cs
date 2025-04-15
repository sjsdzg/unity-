using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
    /// <summary>
    /// 专业
    /// </summary>
    public class Branch : Category
    {
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("专业id:" + Id + "  ");
            sb.Append("专业名称:" + Name + "  ");
            sb.Append("专业状态:" + Status + "  ");
            sb.Append("创建人:" + Poster + "  ");
            sb.Append("创建日期:" + CreateTime + "  ");
            sb.Append("修改人:" + Modifier + "  ");
            sb.Append("修改日期:" + UpdateTime + "  ");
            sb.Append("试题库说明:" + Remark + "  ");
            return sb.ToString();
        }
    }
}
