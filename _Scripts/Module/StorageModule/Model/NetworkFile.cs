using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Core;
using Newtonsoft.Json;

namespace XFramework.Module
{
	/// <summary>
	/// 文件信息
	/// @Author: xiongxing
	/// @Data: 2018-12-12T09:48:22.710
	/// @Version 1.0
	/// <summary>
	public class NetworkFile : DataObject<NetworkFile>
    {
		/// <summary>
		/// 文件id
		/// <summary>
        [JsonProperty("id")]
		public string Id { get; set; }

        /// <summary>
        /// 父节点id
        /// <summary>
        [JsonProperty("parentId")]
        public string ParentId { get; set; }

        /// <summary>
        /// 用户id
        /// <summary>
        [JsonProperty("userId")]
        public string UserId { get; set; }

        /// <summary>
        /// 文件名称
        /// <summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 创建时间
        /// <summary>
        [JsonProperty("createTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 文件大小
        /// <summary>
        [JsonProperty("size")]
        public long Size { get; set; }

        /// <summary>
        /// 文件类型 dir:文件夹
        /// <summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// 状态 0:正常 1:锁定
        /// <summary>
        [JsonProperty("status")]
        public int Status { get; set; }

        /// <summary>
        /// md5
        /// <summary>
        [JsonProperty("md5")]
        public string Md5 { get; set; }

        /// <summary>
        /// 链接地址
        /// <summary>
        [JsonProperty("location")]
        public string Location { get; set; }

        /// <summary>
        /// 描述
        /// <summary>
        [JsonProperty("description")]
        public string Description { get; set; }

		public override string ToString() {
			StringBuilder sb = new StringBuilder();
			sb.Append("文件id:" + Id + "  ");
			sb.Append("父节点id:" + ParentId + "  ");
			sb.Append("用户id:" + UserId + "  ");
			sb.Append("文件名称:" + Name + "  ");
			sb.Append("创建时间:" + CreateTime + "  ");
			sb.Append("文件大小:" + Size + "  ");
			sb.Append("文件类型 dir:文件夹:" + Type + "  ");
			sb.Append("状态 0:正常 1:锁定:" + Status + "  ");
			sb.Append("md5:" + Md5 + "  ");
			sb.Append("链接地址:" + Location + "  ");
			sb.Append("描述:" + Description + "  ");
			return sb.ToString();
		}

        /// <summary>
        /// 获取文档格式集合
        /// </summary>
        /// <returns></returns>
        public static List<string> GetDocTypes()
        {
            List<string> types = new List<string>();
            types.Add("txt");
            types.Add("doc");
            types.Add("docx");
            types.Add("xls");
            types.Add("xlsx");
            types.Add("ppt");
            types.Add("pptx");
            types.Add("pdf");
            types.Add("rtf");
            return types;
        }

        /// <summary>
        /// 是否为文档格式
        /// </summary>
        /// <returns></returns>
        public bool IsDocument()
        {
            if ((this.Type.Equals("xls") || this.Type.Equals("xlsx") || this.Type.Equals("ppt") 
                || this.Type.Equals("pptx") || this.Type.Equals("docx") || this.Type.Equals("doc")  
                || this.Type.Equals("txt")) || this.Type.Equals("pdf") || this.Type.Equals("rtf"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取图片格式集合
        /// </summary>
        /// <returns></returns>
        public static List<string> GetImageTypes()
        {
            List<string> types = new List<string>();
            types.Add("jpg");
            types.Add("jpeg");
            types.Add("png");
            types.Add("gif");
            types.Add("bmp");
            return types;
        }

        /// <summary>
        /// 是否为图片格式
        /// </summary>
        /// <returns></returns>
        public bool IsImage()
        {
            if ((this.Type.Equals("jpg") || this.Type.Equals("jpeg") 
                || this.Type.Equals("png") || this.Type.Equals("gif") || this.Type.Equals("bmp")))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取音频格式集合
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAudioTypes()
        {
            List<string> types = new List<string>();
            types.Add("aif");
            types.Add("wav");
            types.Add("mp3");
            types.Add("ogg");
            types.Add("wma");
            return types;
        }

        /// <summary>
        /// 是否为音频格式
        /// </summary>
        /// <returns></returns>
        public bool IsAudio()
        {
            if ((this.Type.Equals("aif") || this.Type.Equals("wav") 
                || this.Type.Equals("mp3") || this.Type.Equals("ogg") || this.Type.Equals("wma")))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取视频格式集合
        /// </summary>
        /// <returns></returns>
        public static List<string> GetVideoTypes()
        {
            List<string> types = new List<string>();
            types.Add("wmv9");
            types.Add("rm");
            types.Add("rmvb");
            types.Add("asx");
            types.Add("asf");
            types.Add("mpg");
            types.Add("wmv");
            types.Add("3gp");
            types.Add("mp4");
            types.Add("mov");
            types.Add("avi");
            types.Add("flv");
            return types;
        }

        /// <summary>
        /// 是否为音频格式
        /// </summary>
        /// <returns></returns>
        public bool IsVideo()
        {
            if (this.Type.Equals("wmv9") || this.Type.Equals("rm") || this.Type.Equals("rmvb")
                || this.Type.Equals("asx") || this.Type.Equals("asf")
                || this.Type.Equals("mpg") || this.Type.Equals("wmv")
                || this.Type.Equals("3gp") || this.Type.Equals("mp4")
                || this.Type.Equals("mov") || this.Type.Equals("avi") || this.Type.Equals("flv"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 是否为压缩格式
        /// </summary>
        /// <returns></returns>
        public bool IsCompress()
        {
            if (this.Type.Equals("rar") || this.Type.Equals("zip") 
                || this.Type.Equals("7z") )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 是否文件夹
        /// </summary>
        /// <returns></returns>
        public bool IsDirectory()
        {
            if (Type.Equals("dir"))
            {
                return true;
            }

            return false;
        }


    }
}
