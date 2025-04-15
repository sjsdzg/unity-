using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 用户name
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户password
        /// </summary>
        public string UserPassword { get; set; }
        /// <summary>
        /// 性别 0-男 1-女
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// 部门（专业）
        /// </summary>
        public string BranchId { get; set; }
        /// <summary>
        /// 年级
        /// </summary>
        public string Grade { get; set; }
        /// <summary>
        /// 职务（班级）
        /// </summary>
        public string PositionId { get; set; }
        /// <summary>
        /// 真实名称
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 证件编号
        /// </summary>
        public string UserNo { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 邮件
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 状态 0禁用，1正常，9已删除
        /// </summary>
        public int Status { get; set; }
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
        /// 试题库说明
        /// </summary>
        public string Remark { get; set; }
    }
}
