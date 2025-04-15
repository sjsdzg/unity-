using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Core;
using XFramework.Proto;
using XFramework.Network;
using Google.Protobuf;
using Google.Protobuf.Collections;
using UnityEngine.Networking;
using UnityEngine;
using Newtonsoft.Json;
using XFramework.Define;

namespace XFramework.Module
{
	/// <summary>
	/// 网络磁盘模块
	/// @Author: xiongxing
	/// @Data: 2018-11-08T14:25:35.648
	/// @Version 1.0
	/// <summary>
	public class NetworkDiskModule : BaseModule 
	{
        private string m_BaseRequestURL = "";

        public string BaseRequestURL { get { return m_BaseRequestURL; } }

        protected override void OnLoad()
        {
            base.OnLoad();
            if (!m_BaseRequestURL.EndsWith("/"))
            {
                m_BaseRequestURL += "/";
            }
            m_BaseRequestURL = AppSettings.Settings.WebServerUrl;
        }

        /// <summary>
        /// 根据磁盘名称，获取磁盘
        /// <summary>
        /// <param name="id"></param>
        public WebRequestOperation Add(NetworkDisk disk)
        {
            string content = JsonConvert.SerializeObject(disk);
            return WebRequestManager.Instance.Post(BaseRequestURL + WebRequests.NetworkDisk.GetByName, content);
        }

        /// <summary>
        /// 根据磁盘名称，获取磁盘
        /// <summary>
        /// <param name="id"></param>
        public WebRequestOperation GetByName(string name)
        {
            WWWForm form = new WWWForm();
            form.AddField("disk_name", name);
            return WebRequestManager.Instance.Post(BaseRequestURL + WebRequests.NetworkDisk.GetByName, form);
        }

        /// <summary>
        /// 获取磁盘列表
        /// <summary>
        /// <param name="id"></param>
        public WebRequestOperation ListAll()
        {
            return WebRequestManager.Instance.Post(BaseRequestURL + WebRequests.NetworkDisk.ListAll);
        }

        /// <summary>
        /// 根据Id, 移除磁盘
        /// <summary>
        /// <param name="id"></param>
        public WebRequestOperation Remove(string id)
        {
            return WebRequestManager.Instance.Post(BaseRequestURL + WebRequests.NetworkDisk.Remove, id);
        }
    }
}
