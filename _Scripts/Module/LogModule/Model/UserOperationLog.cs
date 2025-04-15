using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
	/// <summary>
	/// 用户操作日志
	/// @Author: xiongxing
	/// @Data: 2018-10-31T14:09:36.202
	/// @Version 1.0
	/// <summary>
	public class UserOperationLog {
		/// <summary>
		/// id
		/// <summary>
		public string Id { get; set; }

		/// <summary>
		/// 用户id
		/// <summary>
		public string UserId { get; set; }

		/// <summary>
		/// 软件id
		/// <summary>
		public string SoftwareId { get; set; }

		/// <summary>
		/// 创建时间
		/// <summary>
		public DateTime CreateTime { get; set; }

		/// <summary>
		/// 更新时间
		/// <summary>
		public DateTime UpdateTime { get; set; }

		/// <summary>
		/// 软件模块
		/// <summary>
		public string SoftwareModule { get; set; }

		/// <summary>
		/// 模块细节 
		/// <summary>
		public string SoftwareModuleDetail { get; set; }

		/// <summary>
		/// 用户操作数据
		/// <summary>
		public string Data { get; set; }

		/// <summary>
		/// 操作内容
		/// <summary>
		public string Description { get; set; }

		public override string ToString() {
			StringBuilder sb = new StringBuilder();
			sb.Append("id:" + Id + "  ");
			sb.Append("用户id:" + UserId + "  ");
			sb.Append("软件id:" + SoftwareId + "  ");
			sb.Append("创建时间:" + CreateTime + "  ");
			sb.Append("更新时间:" + UpdateTime + "  ");
			sb.Append("软件模块:" + SoftwareModule + "  ");
			sb.Append("模块细节 :" + SoftwareModuleDetail + "  ");
			sb.Append("用户操作数据:" + Data + "  ");
			sb.Append("操作内容:" + Description + "  ");
			return sb.ToString();
		}
	}
}
