using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
    /// <summary>
    /// 班级
    /// </summary>
    public class Position : Category
    {
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("班级id:" + Id + "  ");
            sb.Append("班级名称:" + Name + "  ");
            sb.Append("班级状态:" + Status + "  ");
            sb.Append("创建人:" + Poster + "  ");
            sb.Append("创建日期:" + CreateTime + "  ");
            sb.Append("修改人:" + Modifier + "  ");
            sb.Append("修改日期:" + UpdateTime + "  ");
            sb.Append("试题库说明:" + Remark + "  ");
            return sb.ToString();
        }
    }
}
