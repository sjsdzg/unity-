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
    public class DownloadNetworkFileOperation : AsyncLoadOperation
    {
        private NetworkFileModule networkFileModule;

        /// <summary>
        /// 文件Id
        /// </summary>
        public NetworkFile NetworkFile { get; set; }

        /// <summary>
        /// request
        /// </summary>
        private HTTPRequest request;

        public DownloadNetworkFileOperation(string url, NetworkFile file, string saveDir)
        {
            networkFileModule = ModuleManager.Instance.GetModule<NetworkFileModule>();
            Debug.Log(url);

            this.NetworkFile = file;
            string savePath = string.Empty;
            if (!saveDir.EndsWith("/"))
            {
                savePath = saveDir + "/" + NetworkFile.Name;
            }

            // 避免下载大文件耗尽内存，采取缓存已经下载的响应的方式
            // 需要 request.UseStreaming = true;
            request = new HTTPRequest(new Uri(url), HTTPMethods.Post, (req, resp) =>
            {
                try
                {
                    List<byte[]> fragments = resp.GetStreamedFragments();
                    using (FileStream fs = new FileStream(savePath, FileMode.Append))
                    {
                        foreach (byte[] data in fragments)
                        {
                            fs.Write(data, 0, data.Length);
                        }

                        if (resp.IsStreamingFinished)
                        {
                            Debug.Log("download finished");
                            IsDone = true;
                        }

                    }
                }
                catch (Exception e)
                {
                    Error = e.Message;
                    IsDone = true;
                }
            });

            request.AddField("file_id", NetworkFile.Id);
            request.UseStreaming = true;
            request.StreamFragmentSize = 1024 * 1024;
#if !UNITY_WEBGL
            request.DisableCache = true; // 数据已经本地持久化了，就不需要开启缓存了
#endif
            request.OnProgress = OnProgress;
            request.Send();
        }

        private void OnProgress(HTTPRequest originalRequest, long downloaded, long downloadLength)
        {
            // 计算进度百分比
            Progress = downloaded / (float)downloadLength;
            float progressPercent = (downloaded / (float)downloadLength) * 100.0f;
            Debug.Log("Downloaded: " + progressPercent.ToString("F2") + "%");
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
