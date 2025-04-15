using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
	/// <summary>
	/// 用户登录日志
	/// @Author: xiongxing
	/// @Data: 2018-11-02T16:09:20.311
	/// @Version 1.0
	/// <summary>
	public class UserLoginLog {
		/// <summary>
		/// id
		/// <summary>
		public string Id { get; set; }

		/// <summary>
		/// 用户名
		/// <summary>
		public string UserName { get; set; }

		/// <summary>
		/// 软件标识
		/// <summary>
		public string SoftwareId { get; set; }

        /// <summary>
        /// 登录类型[0:登录 1:退出 2:重连 3:强制下线]
        /// <summary>
        public int Type { get; set; }

		/// <summary>
		/// 登录时间
		/// <summary>
		public DateTime CreateTime { get; set; }

		/// <summary>
		/// ip
		/// <summary>
		public string Ip { get; set; }

		/// <summary>
		/// 登录地址
		/// <summary>
		public string Address { get; set; }

		/// <summary>
		/// 备注
		/// <summary>
		public string Remark { get; set; }

		public override string ToString() {
			StringBuilder sb = new StringBuilder();
			sb.Append("id:" + Id + "  ");
			sb.Append("用户名:" + UserName + "  ");
			sb.Append("软件标识:" + SoftwareId + "  ");
			sb.Append("登录类型[0:登录 1:退出 2:重连]:" + Type + "  ");
			sb.Append("登录时间:" + CreateTime + "  ");
			sb.Append("ip:" + Ip + "  ");
			sb.Append("登录地址:" + Address + "  ");
			sb.Append("备注:" + Remark + "  ");
			return sb.ToString();
		}
	}
}
