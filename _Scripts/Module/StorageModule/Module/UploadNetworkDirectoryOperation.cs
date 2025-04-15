using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Core;

namespace XFramework.Module
{
    public class UploadNetworkDirectoryOperation : AsyncLoadOperation
    {
        private NetworkFileModule fileModule;

        /// <summary>
        /// 是否中止
        /// </summary>
        private bool isAbort;

        /// <summary>
        /// 文件系统实体数量
        /// </summary>
        public int FileSystemEntryCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public NetworkFile NetworkFile { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private List<UploadNetworkFileOperation> uploadFileList = new List<UploadNetworkFileOperation>();

        /// <summary>
        /// 
        /// </summary>
        public int Index { get; set; }

        public UploadNetworkDirectoryOperation(string parentId, string dirPath)
        {
            fileModule = ModuleManager.Instance.GetModule<NetworkFileModule>();
            FileSystemEntryCount = FileUtils.GetFileSystemEntryCount(dirPath);
            FileSystemEntryCount++;//添加自己
            Debug.Log(FileSystemEntryCount);
            // 开始任务
            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
            NetworkFile = new NetworkFile();
            NetworkFile.ParentId = parentId;
            NetworkFile.Name = dirInfo.Name;
            NetworkFile.Type = "dir";
            Next(dirInfo, parentId);
        }

        private void Next(FileSystemInfo entry, string parentId)
        {
            if (isAbort)
                return;

            if (entry is DirectoryInfo)
            {
                DirectoryInfo dirInfo = entry as DirectoryInfo;
                NetworkFile file = new NetworkFile();
                file.ParentId = parentId;
                file.Name = dirInfo.Name;
                file.Type = "dir";
                WebRequestOperation mkdirAsync = fileModule.Mkdir(file);
                mkdirAsync.OnCompleted(x =>
                {
                    Index++;
                    if (!string.IsNullOrEmpty(x.Error))
                    {
                        Debug.LogError(x.Error);
                        Error = x.Error;
                    }
                    // 获取新建文件夹请求结果
                    string json = mkdirAsync.GetText();
                    Debug.Log(json);
                    ResultJson result = ResultJson.Parser.ParseJson(json);
                    if (result.Type == ResultJson.NORMAL)
                    {
                        // parentId = result.Keyword;
                        foreach (DirectoryInfo nextDir in dirInfo.GetDirectories())
                        {
                            Next(nextDir, result.Keyword);
                        }

                        foreach (FileInfo nextFile in dirInfo.GetFiles())
                        {
                            Next(nextFile, result.Keyword);
                        }
                    }
                    else
                    {
                        Debug.LogError(result);
                        Error = result.Detail;
                    }

                });
            }
            else
            {
                FileInfo fileInfo = entry as FileInfo;
                UploadNetworkFileOperation upload = fileModule.UploadNetworkFile(parentId, fileInfo.FullName);
                upload.OnCompleted(x =>
                {
                    Index++;
                    if (!string.IsNullOrEmpty(upload.Error))
                    {
                        Debug.LogError(upload.Error);
                        Error = x.Error;
                    }
                });
            }
        }

        public override bool Update()
        {
            Progress = (float)Index / FileSystemEntryCount;
            if (Index == FileSystemEntryCount)
            {
                IsDone = true;
            }
            Notify();
            return !IsDone;
        }

        public override void Abort()
        {
            base.Abort();
            for (int i = 0; i < uploadFileList.Count; i++)
            {
                UploadNetworkFileOperation async = uploadFileList[i];
                async.Abort();
            }
            Error = "已中止";
            IsDone = true;
        }
    }
}
