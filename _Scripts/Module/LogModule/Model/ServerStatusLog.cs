using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
	/// <summary>
	/// 服务器状态日志
	/// @Author: xiongxing
	/// @Data: 2018-10-30T16:50:24.156
	/// @Version 1.0
	/// <summary>
	public class ServerStatusLog {
		/// <summary>
		/// id
		/// <summary>
		public string Id { get; set; }

		/// <summary>
		/// 创建时间
		/// <summary>
		public DateTime CreateTime { get; set; }

		/// <summary>
		/// cpu使用率
		/// <summary>
		public float Cpu { get; set; }

		/// <summary>
		/// 内存使用率
		/// <summary>
		public float Memory { get; set; }

		/// <summary>
		/// 连接数量
		/// <summary>
		public int Connections { get; set; }

		/// <summary>
		/// 上传速度 kb/s
		/// <summary>
		public int UploadSpeed { get; set; }

		/// <summary>
		/// 下载速度 kb/s
		/// <summary>
		public int DownloadSpeed { get; set; }

		/// <summary>
		/// 备注
		/// <summary>
		public string Remark { get; set; }

		public override string ToString() {
			StringBuilder sb = new StringBuilder();
			sb.Append("id:" + Id + "  ");
			sb.Append("创建时间:" + CreateTime + "  ");
			sb.Append("cpu使用率:" + Cpu + "  ");
			sb.Append("内存使用率:" + Memory + "  ");
			sb.Append("连接数量:" + Connections + "  ");
			sb.Append("上传速度 kb/s:" + UploadSpeed + "  ");
			sb.Append("下载速度 kb/s:" + DownloadSpeed + "  ");
			sb.Append("备注:" + Remark + "  ");
			return sb.ToString();
		}
	}
}
