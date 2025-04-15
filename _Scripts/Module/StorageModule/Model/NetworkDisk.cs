using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Core;

namespace XFramework.Module
{
	/// <summary>
	/// 磁盘信息
	/// @Author: xiongxing
	/// @Data: 2018-12-12T09:48:22.683
	/// @Version 1.0
	/// <summary>
	public class NetworkDisk : DataObject<NetworkDisk>
    {
        /// <summary>
        /// 磁盘id
        /// <summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// 名称
        /// <summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 创建用户id
        /// <summary>
        [JsonProperty("userId")]
        public string UserId { get; set; }

        /// <summary>
        /// 磁盘根目录id
        /// <summary>
        [JsonProperty("homeId")]
        public string HomeId { get; set; }

        /// <summary>
        /// 总共大小
        /// <summary>
        [JsonProperty("totalSize")]
        public long TotalSize { get; set; }

        /// <summary>
        /// 使用大小
        /// <summary>
        [JsonProperty("usedSize")]
        public long UsedSize { get; set; }

        /// <summary>
        /// 文件数量
        /// <summary>
        [JsonProperty("fileNumber")]
        public int FileNumber { get; set; }

        /// <summary>
        /// 访问限制 0;公有 1：私有
        /// <summary>
        [JsonProperty("access")]
        public int Access { get; set; }

        /// <summary>
        /// 描述
        /// <summary>
        [JsonProperty("description")]
        public string Description { get; set; }

		public override string ToString() {
			StringBuilder sb = new StringBuilder();
			sb.Append("磁盘id:" + Id + "  ");
			sb.Append("名称:" + Name + "  ");
			sb.Append("创建用户id:" + UserId + "  ");
			sb.Append("磁盘根目录id:" + HomeId + "  ");
			sb.Append("总共大小:" + TotalSize + "  ");
			sb.Append("使用大小:" + UsedSize + "  ");
			sb.Append("文件数量:" + FileNumber + "  ");
			sb.Append("访问限制 0;公有 1：私有:" + Access + "  ");
			sb.Append("描述:" + Description + "  ");
			return sb.ToString();
		}
	}
}
