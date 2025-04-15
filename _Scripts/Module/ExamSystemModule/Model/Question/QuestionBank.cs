using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
    /// <summary>
    /// 试题库
    /// </summary>
    public class QuestionBank : Category
    {
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("试题库id:" + Id + "  ");
            sb.Append("试题库名称:" + Name + "  ");
            sb.Append("试题库状态:" + Status + "  ");
            sb.Append("创建人:" + Poster + "  ");
            sb.Append("创建日期:" + CreateTime + "  ");
            sb.Append("修改人:" + Modifier + "  ");
            sb.Append("修改日期:" + UpdateTime + "  ");
            sb.Append("试题库说明:" + Remark + "  ");
            sb.Append("试题数量:" + Amount + "  ");
            return sb.ToString();
        }
    }
}
