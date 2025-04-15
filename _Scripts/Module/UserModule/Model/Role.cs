using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
    public class Role
    {
        /// <summary>
        /// 用户角色id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 用户角色名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 用户角色状态 0:禁用 1:启用
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 权限
        /// </summary>
        public string Privilege { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Poster { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public string Modifier { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("用户角色id:" + Id + "  ");
            sb.Append("用户角色名称:" + Name + "  ");
            sb.Append("用户角色状态:" + Status + "  ");
            sb.Append("用户权限:" + Privilege + "  ");
            sb.Append("创建人:" + Poster + "  ");
            sb.Append("创建日期:" + CreateTime + "  ");
            sb.Append("修改人:" + Modifier + "  ");
            sb.Append("修改日期:" + UpdateTime + "  ");
            sb.Append("用户角色说明:" + Remark + "  ");
            return sb.ToString();
        }
    }
}
