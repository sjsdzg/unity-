using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
    /// <summary>
    /// 关卡信息
    /// </summary>
    public class LevelInfo
    {
        /// <summary>
        /// 关卡Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 软件id
        /// <summary>
        public string SoftwareId { get; set; }

        /// <summary>
        /// 当前关卡
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 关卡数据
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// 关卡备注
        /// </summary>
        public string Remark { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("关卡id:" + Id + "  ");
            sb.Append("用户id:" + UserId + "  ");
            sb.Append("当前关卡:" + Level + "  ");
            sb.Append("数据记录:" + Data + "  ");
            sb.Append("数据说明:" + Remark + "  ");
            return sb.ToString();
        }
    }
}
