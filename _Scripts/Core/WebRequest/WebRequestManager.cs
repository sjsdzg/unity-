using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using XFramework.Module;

namespace XFramework.Core
{
    public class WebRequestManager : Singleton<WebRequestManager>, IUpdate
    {
        //private string m_BaseRequestURL = "";
        private readonly List<AsyncLoadOperation> m_InProgressOperations = new List<AsyncLoadOperation>();
        private readonly Dictionary<string, string> m_OperationErrors = new Dictionary<string, string>();

        //public string BaseRequestURL { get { return m_BaseRequestURL; } }

        protected override void Init()
        {
            base.Init();
            MonoDriver.Attach(this);
            //if (!m_BaseRequestURL.EndsWith("/"))
            //{
            //    m_BaseRequestURL += "/";
            //}
            //m_BaseRequestURL = AppSettings.Settings.StorageServerUrl;
        }

        public void Update()
        {
            // Update all in progress operations
            for (int i = 0; i < m_InProgressOperations.Count;)
            {
                var operation = m_InProgressOperations[i];
                if (operation.Update())
                {
                    i++;
                }
                else
                {
                    m_InProgressOperations.RemoveAt(i);
                    ProcessFinishedOperation(operation);
                }
            }
        }

        private void ProcessFinishedOperation(AsyncLoadOperation operation)
        {
            if (operation == null)
                return;
        }

        /// <summary>
        /// 提交表单 Get
        /// </summary>
        /// <param name="url"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        public WebRequestOperation Get(string uri)
        {
            UnityWebRequest request = UnityWebRequest.Get(uri);
            WebRequestOperation operation = new WebRequestOperation(request);
            m_InProgressOperations.Add(operation);
            return operation;
        }

        /// <summary>
        /// 提交表单
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        public WebRequestOperation Post(string uri, WWWForm form)
        {
            UnityWebRequest request = UnityWebRequest.Post(uri, form);
            WebRequestOperation operation = new WebRequestOperation(request);
            m_InProgressOperations.Add(operation);
            return operation;
        }

        /// <summary>
        /// 提交字符串内容
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        public WebRequestOperation Post(string uri, string content)
        {
            UnityWebRequest request = UnityWebRequest.Post(uri, content);
            WebRequestOperation operation = new WebRequestOperation(request);
            m_InProgressOperations.Add(operation);
            return operation;
        }

        /// <summary>
        /// 提交请求
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        public WebRequestOperation Post(string uri)
        {
            UnityWebRequest request = UnityWebRequest.Post(uri, "");
            WebRequestOperation operation = new WebRequestOperation(request);
            m_InProgressOperations.Add(operation);
            return operation;
        }

        public UploadFileOperation UploadFile(string uri, string filePath)
        {
            UploadFileOperation operation = new UploadFileOperation(uri, filePath);
            m_InProgressOperations.Add(operation);
            return operation;
        }

        /// <summary>
        /// 上传网络文件
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="parentId"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public UploadNetworkFileOperation UploadNetworkFile(string uri, string parentId, string filePath)
        {
            UploadNetworkFileOperation operation = new UploadNetworkFileOperation(uri, parentId, filePath);
            m_InProgressOperations.Add(operation);
            return operation;
        }

        /// <summary>
        /// 上传网络文件夹
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        public UploadNetworkDirectoryOperation UploadNetworkDirectory(string parentId, string dirPath)
        {
            UploadNetworkDirectoryOperation operation = new UploadNetworkDirectoryOperation(parentId, dirPath);
            m_InProgressOperations.Add(operation);
            return operation;
        }

        /// <summary>
        /// 下载网络文件
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        public DownloadNetworkFileOperation DownloadNetworkFile(string uri, NetworkFile file, string saveDir)
        {
            
            DownloadNetworkFileOperation operation = new DownloadNetworkFileOperation(uri, file, saveDir);
            m_InProgressOperations.Add(operation);
            return operation;
        }
    }
}
