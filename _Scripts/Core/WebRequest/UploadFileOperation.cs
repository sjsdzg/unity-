using BestHTTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.Core
{
    public class UploadFileOperation : AsyncLoadOperation
    {
        private HTTPRequest request;

        public string Text { get; set; }

        public UploadFileOperation(string url, string filePath)
        {
            request = new HTTPRequest(new Uri(url), HTTPMethods.Post, (req, resp) =>
            {
                if (resp.IsSuccess)
                {
                    Text = resp.DataAsText;
                    IsDone = true;
                    Debug.Log("Finish");
                }
                else
                {
                    Error = resp.StatusCode.ToString();
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
            Progress = uploaded / (float)uploadLength;
            float progressPercent = (uploaded / (float)uploadLength) * 100.0f;
            Debug.Log("Uploaded: " + progressPercent.ToString("F2") + "%");
        }

        public override bool Update()
        {
            Notify();
            return !IsDone;
        }
    }
}
