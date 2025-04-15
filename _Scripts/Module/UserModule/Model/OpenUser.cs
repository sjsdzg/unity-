using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
	/// <summary>
	/// 第三方用户登录
	/// @Author: xiongxing
	/// @Data: 2019-05-06T15:27:02.485
	/// @Version 1.0
	/// <summary>
	public class OpenUser {
		/// <summary>
		/// id
		/// <summary>
		public string Id { get; set; }

		/// <summary>
		/// 代表第三方用户身份的ID
		/// <summary>
		public string OpenId { get; set; }

		/// <summary>
		/// 第三方类型 ilab-x（实验空间）
		/// <summary>
		public string OpenType { get; set; }

		/// <summary>
		/// 关联用户ID
		/// <summary>
		public string UserId { get; set; }

		/// <summary>
		/// token
		/// <summary>
		public string AccessToken { get; set; }

		/// <summary>
		/// 授权过期时间
		/// <summary>
		public int ExpiredTime { get; set; }

		/// <summary>
		/// 昵称
		/// <summary>
		public string Nickname { get; set; }

		/// <summary>
		/// 头像（url）
		/// <summary>
		public string Avatar { get; set; }

		public override string ToString() {
			StringBuilder sb = new StringBuilder();
			sb.Append("id:" + Id + "  ");
			sb.Append("代表第三方用户身份的ID:" + OpenId + "  ");
			sb.Append("第三方类型 ilab-x（实验空间）:" + OpenType + "  ");
			sb.Append("关联用户ID:" + UserId + "  ");
			sb.Append("token:" + AccessToken + "  ");
			sb.Append("授权过期时间:" + ExpiredTime + "  ");
			sb.Append("昵称:" + Nickname + "  ");
			sb.Append("头像（url）:" + Avatar + "  ");
			return sb.ToString();
		}
	}
}
