using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Core;
using XFramework.Proto;
using XFramework.Network;
using Google.Protobuf;
using Google.Protobuf.Collections;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using XFramework.Define;

namespace XFramework.Module
{
	/// <summary>
	/// 网络文件模块
	/// @Author: xiongxing
	/// @Data: 2018-11-08T14:25:35.648
	/// @Version 1.0
	/// <summary>
	public class NetworkFileModule : BaseModule 
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
        /// 根据父节点文件Id，获取文件列表
        /// <summary>
        /// <param name="id"></param>
        public WebRequestOperation List(string parentId)
        {
            WWWForm form = new WWWForm();
            form.AddField("parent_id", parentId);
            return WebRequestManager.Instance.Post(BaseRequestURL + WebRequests.NetworkFile.List, form);
        }

        /// <summary>
        /// 新建文件夹
        /// <summary>
        /// <param name="id"></param>
        public WebRequestOperation Mkdir(NetworkFile networkFile)
        {
            networkFile.UserId = App.Instance.SoftwareId;
            string json = networkFile.ToJson();
            return WebRequestManager.Instance.Post(BaseRequestURL + WebRequests.NetworkFile.Mkdir, json);
        }

        /// <summary>
        /// 添加文件
        /// <summary>
        /// <param name="id"></param>
        public WebRequestOperation Add(NetworkFile networkFile)
        {
            networkFile.UserId = App.Instance.SoftwareId;
            string json = networkFile.ToJson();
            return WebRequestManager.Instance.Post(BaseRequestURL + WebRequests.NetworkFile.Add, json);
        }

        /// <summary>
        /// 根据文件Id, 获取文件
        /// <summary>
        /// <param name="id"></param>
        public WebRequestOperation Get(string id)
        {
            WWWForm form = new WWWForm();
            form.AddField("file_id", id);
            return WebRequestManager.Instance.Post(BaseRequestURL + WebRequests.NetworkFile.Get, form);
        }

        /// <summary>
        /// 移动文件
        /// <summary>
        /// <param name="id"></param>
        public WebRequestOperation Move(NetworkFile networkFile)
        {
            WWWForm form = new WWWForm();
            string json = networkFile.ToJson();
            return WebRequestManager.Instance.Post(BaseRequestURL + WebRequests.NetworkFile.Move, json);
        }

        /// <summary>
        /// 根据文件Id,删除文件
        /// <summary>
        /// <param name="id"></param>
        public WebRequestOperation Remove(string id)
        {
            WWWForm form = new WWWForm();
            form.AddField("file_id", id);
            return WebRequestManager.Instance.Post(BaseRequestURL + WebRequests.NetworkFile.Remove, form);
        }

        /// <summary>
        /// 重命名文件
        /// <summary>
        /// <param name="id"></param>
        public WebRequestOperation Rename(NetworkFile networkFile)
        {
            WWWForm form = new WWWForm();
            string json = networkFile.ToJson();
            return WebRequestManager.Instance.Post(BaseRequestURL + WebRequests.NetworkFile.Rename, json);
        }

        /// <summary>
        /// 根据条件，分页文件记录
        /// <summary>
        public WebRequestOperation PageNetworkFileByCondition(int currentPage, int pageSize, List<SqlCondition> conditions)
        {
            PageBeanJson<SqlCondition> pageBeanJson = new PageBeanJson<SqlCondition>();
            pageBeanJson.CurrentPage = currentPage;
            pageBeanJson.PageSize = pageSize;
            pageBeanJson.dataList = conditions;
            string json = pageBeanJson.ToJson();
            return WebRequestManager.Instance.Post(BaseRequestURL + WebRequests.NetworkFile.PageByCondition, json);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="filepPath"></param>
        /// <returns></returns>
        public UploadNetworkFileOperation UploadNetworkFile(string parentId, string filePath)
        {
            return WebRequestManager.Instance.UploadNetworkFile(BaseRequestURL + WebRequests.NetworkFile.Upload, parentId, filePath);
        }

        /// <summary>
        /// 上传文件夹
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        public UploadNetworkDirectoryOperation UploadNetworkDirectory(string parentId, string dirPath)
        {
            return WebRequestManager.Instance.UploadNetworkDirectory(parentId, dirPath);
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="file_id"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        public DownloadNetworkFileOperation DownloadNetworkFile(NetworkFile file, string saveDir)
        {
            return WebRequestManager.Instance.DownloadNetworkFile(BaseRequestURL + WebRequests.NetworkFile.Download, file, saveDir);
        }



        /// <summary>
        /// 根据文件路径，获取NetworkFile
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public NetworkFile GetNetworkFile(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                NetworkFile networkFile = new NetworkFile();
                networkFile.Name = Path.GetFileName(filePath);
                string suffix = Path.GetExtension(filePath);
                if (string.IsNullOrEmpty(suffix) || suffix.Equals(".dir"))
                {
                    networkFile.Type = "unknown";
                }
                else
                {
                    networkFile.Type = suffix.Trim('.').ToLower();
                }
                networkFile.Size = fs.Length;
                return networkFile;
            }
        }
    }
}
