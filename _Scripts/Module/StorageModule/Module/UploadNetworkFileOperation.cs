using BestHTTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Core;

namespace XFramework.Module
{
    public class UploadNetworkFileOperation : AsyncLoadOperation
    {
        private NetworkFileModule networkFileModule;

        private string url;

        private string parentId;

        private string filePath;

        private HTTPRequest request;

        /// <summary>
        /// 网络文件信息
        /// </summary>
        public NetworkFile NetworkFile { get; set; }

        public UploadNetworkFileOperation(string url, string parentId, string filePath)
        {
            networkFileModule = ModuleManager.Instance.GetModule<NetworkFileModule>();
            NetworkFile = networkFileModule.GetNetworkFile(filePath);

            this.url = url;
            this.parentId = parentId;
            this.filePath = filePath;

            request = new HTTPRequest(new Uri(url), HTTPMethods.Post, (req, resp) =>
            {
                try
                {
                    if (resp.IsSuccess)
                    {
                        OnUploadFinished(resp);
                    }
                    else
                    {
                        IsDone = true;
                        Error = resp.StatusCode.ToString();
                    }
                }
                catch (Exception e)
                {
                    Error = e.Message;
                    IsDone = true;
                }

            });

            request.AddHeader("data", Path.GetExtension(filePath));
            request.UploadStream = new FileStream(filePath, FileMode.Open);
            request.OnUploadProgress = OnUploadProgress;
            request.Send();
        }

        private void OnUploadProgress(HTTPRequest originalRequest, long uploaded, long uploadLength)
        {
            // 计算进度百分比
            Progress = uploaded * 0.9f / (float)uploadLength;
            float progressPercent = (uploaded / (float)uploadLength) * 100.0f;
            Debug.Log("Uploaded: " + progressPercent.ToString("F2") + "%");
        }

        /// <summary>
        /// 上传文件结束，处理
        /// </summary>
        /// <param name="resp"></param>
        private void OnUploadFinished(HTTPResponse resp)
        {
            string json = resp.DataAsText;
            Debug.Log(json);
            ResultJson result = ResultJson.Parser.ParseJson(json);
            if (result.Type == ResultJson.NORMAL)
            {
                NetworkFile.ParentId = parentId;
                NetworkFile.Location = result.Keyword;
                // 添加文件
                WebRequestOperation addFileAsync = networkFileModule.Add(NetworkFile);
                addFileAsync.OnUpdate(x =>
                {
                    Progress = 0.9f + x.Progress;
                });

                addFileAsync.OnCompleted(y =>
                {
                    string addJson = addFileAsync.GetText();
                    Debug.Log(addJson);
                    ResultJson addResult = ResultJson.Parser.ParseJson(addJson);
                    if (addResult.Type == ResultJson.NORMAL)
                    {
                        // TODO
                        NetworkFile.Id = addResult.Keyword;
                        Debug.Log(addResult.Detail);
                        IsDone = true;
                    }
                    else
                    {
                        Debug.LogWarning(result.Detail);
                        Error = addResult.Detail;
                        IsDone = true;
                    }
                });
            }
            else
            {
                Debug.LogWarning(result.Detail);
                Error = result.Detail;
                IsDone = true;
            }
        }

        public override bool Update()
        {
            Notify();
            return !IsDone;
        }

        public override void Abort()
        {
            base.Abort();
            if (request != null)
            {
                Error = "已中止";
                request.Abort();
                IsDone = true;
            }
        }
    }
}
