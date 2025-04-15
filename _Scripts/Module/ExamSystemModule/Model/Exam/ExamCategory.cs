using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
    public class ExamCategory : Category
    {
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("考试分类id:" + Id + "  ");
            sb.Append("考试分类名称:" + Name + "  ");
            sb.Append("考试分类状态:" + Status + "  ");
            sb.Append("创建人:" + Poster + "  ");
            sb.Append("创建日期:" + CreateTime + "  ");
            sb.Append("修改人:" + Modifier + "  ");
            sb.Append("修改日期:" + UpdateTime + "  ");
            sb.Append("考试分类说明:" + Remark + "  ");
            return sb.ToString();
        }
    }
}
