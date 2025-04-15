using BestHTTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Module;

namespace XFramework.Core
{
    public class UploadNeworkFilesOperation : AsyncLoadOperation
    {
        private NetworkFileModule fileModule;

        public UploadFileOperation Async { get; set; }

        private List<string> filePaths;

        private string parentId;

        private string url;

        private int index = -1;
        /// <summary>
        /// 进度索引
        /// </summary>
        public int Index
        {
            get { return index; }
            private set
            {
                if (index == value)
                    return;

                index = value;
                Next();
            }
        }

        public UploadNeworkFilesOperation(string url, string parentId, List<string> filePaths)
        {
            fileModule = ModuleManager.Instance.GetModule<NetworkFileModule>();
            this.url = url;
            this.parentId = parentId;
            this.filePaths = filePaths;
            index = 0;
        }

        private void Next()
        {
            if (Index < filePaths.Count)
            {
                string filePath = filePaths[index];
                Async = WebRequestManager.Instance.UploadFile(url, filePath);
                Async.OnCompleted(x =>
                {
                    if (!string.IsNullOrEmpty(Async.Error))
                    {
                        Debug.LogWarningFormat("index : {0} error : {1}", Index, Async.Error);
                        Index++;
                    }

                    FileStream fs = new FileStream(filePath, FileMode.Open);
                    string json = Async.Text;
                    Debug.Log(json);
                    ResultJson result = ResultJson.Parser.ParseJson(json);
                    if (result.Type == ResultJson.NORMAL)
                    {
                        NetworkFile networkFile = new NetworkFile();
                        networkFile.ParentId = parentId;
                        networkFile.Name = Path.GetFileName(filePath);
                        networkFile.Type = Path.GetExtension(filePath);
                        networkFile.Size = fs.Length;
                        networkFile.Location = result.Keyword;
                        WebRequestOperation addFileAsync = fileModule.Add(networkFile);
                        addFileAsync.OnCompleted(y =>
                        {
                            if (!string.IsNullOrEmpty(Async.Error))
                            {
                                Debug.LogWarningFormat("index : {0} error : {1}", Index, addFileAsync.Error);
                            }

                            string addFilejson = Async.Text;
                            Debug.Log(json);
                            ResultJson addFileResult = ResultJson.Parser.ParseJson(addFilejson);
                            if (addFileResult.Type == ResultJson.NORMAL)
                            {
                                Debug.LogWarningFormat("index : {0} error : {1}", Index, addFileAsync.Error);
                            }
                            else
                            {
                                Debug.LogWarningFormat("index : {0} error : {1}", Index, addFileResult.Detail);
                            }

                            Index++;
                        });
                    }
                    else
                    {
                        Debug.LogWarningFormat("index : {0} error : {1}", Index, result.Detail);
                        Index++;
                    }
                });
            }
            else
            {
                IsDone = true;
            }
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
            if (Async != null)
                Progress = (Index + Async.Progress) / filePaths.Count;
            else
                Progress = (float)Index / filePaths.Count;
            Notify();
            return !IsDone;
        }
    }
}
