using UnityEngine;
using System.Collections;
using XFramework.Common;
using UnityEngine.UI;
using System;
using XFramework.Module;
using XFramework.Core;

namespace XFramework.UI
{
    public class TaskMonitorItem : ListViewItemBase<TaskMonitorItemData>
    {
        /// <summary>
        /// 文件类型图标
        /// </summary>
        private Image m_FileTypeIcon;
        /// <summary>
        /// 文件名称
        /// </summary>
        private Text m_FileName;
        /// <summary>
        /// 文件大小
        /// </summary>
        private Text m_FileSize;
        /// <summary>
        /// 进度
        /// </summary>
        private Image m_Progress;

        /// <summary>
        /// 进度文本
        /// </summary>
        private Text m_ProgressText;

        /// <summary>
        /// 成功图标
        /// </summary>
        private Image m_Success;

        /// <summary>
        /// 状态文本
        /// </summary>
        private Text m_Status;

        /// <summary>
        /// 中止任务按钮
        /// </summary>
        private Button buttonAbort;

        /// <summary>
        /// 是否主动中止
        /// </summary>
        private bool isAbort = false;

        /// <summary>
        /// 是否结束
        /// </summary>
        public bool IsDone { get; set; }

        protected override void Awake()
        {
            base.Awake();
            m_FileTypeIcon = transform.Find("FileTypeIcon").GetComponent<Image>();
            m_FileName = transform.Find("FileName").GetComponent<Text>();
            m_FileSize = transform.Find("FileSize").GetComponent<Text>();
            m_Progress = transform.Find("Progress").GetComponent<Image>();
            m_Status = transform.Find("Status").GetComponent<Text>();
            m_Success = transform.Find("Success").GetComponent<Image>();
            buttonAbort = transform.Find("Progress/ButtonAbort").GetComponent<Button>();
            m_ProgressText = transform.Find("Progress/Text").GetComponent<Text>();
            // status
            m_Progress.fillAmount = 0;
            m_Progress.gameObject.SetActive(false);
            m_Success.gameObject.SetActive(false);
            m_Status.text = "";
            // Event
            buttonAbort.onClick.AddListener(buttonAbort_onClick);
        }

        public override void SetData(TaskMonitorItemData data)
        {
            base.SetData(data);
            m_FileTypeIcon.sprite = data.FileIcon;
            string fileNameText = string.Empty;
            string fileSizeText = string.Empty;
            if (data.Async is UploadNetworkFileOperation)
            {
                UploadNetworkFileOperation uploadNetworkFileOperation = data.Async as UploadNetworkFileOperation;
                fileNameText = uploadNetworkFileOperation.NetworkFile.Name;
                float size = uploadNetworkFileOperation.NetworkFile.Size / 1024.0f;
                if (size < 1024)
                {
                    fileSizeText = string.Format("{0}kb 上传", size.ToString("F2"));
                }
                else
                {
                    size = size / 1024.0f;
                    fileSizeText = string.Format("{0}M 上传", size.ToString("F2"));
                }
            }
            else if (data.Async is UploadNetworkDirectoryOperation)
            {
                UploadNetworkDirectoryOperation uploadNetworkDirectoryOperation = data.Async as UploadNetworkDirectoryOperation;
                fileNameText = uploadNetworkDirectoryOperation.NetworkFile.Name;
                fileSizeText = string.Format("{0}个文件夹和文件 上传", uploadNetworkDirectoryOperation.FileSystemEntryCount);
            }
            else if (data.Async is DownloadNetworkFileOperation)
            {
                DownloadNetworkFileOperation downloadNetworkFileOperation = data.Async as DownloadNetworkFileOperation;
                fileNameText = downloadNetworkFileOperation.NetworkFile.Name;
                float size = downloadNetworkFileOperation.NetworkFile.Size / 1024.0f;
                if (size < 1024)
                {
                    fileSizeText = string.Format("{0}kb 下载", size.ToString("F2"));
                }
                else
                {
                    size = size / 1024.0f;
                    fileSizeText = string.Format("{0}M 下载", size.ToString("F2"));
                }
            }

            m_FileName.text = fileNameText;
            m_FileSize.text = fileSizeText;
            data.Async.OnUpdateEvent.AddListener(Async_OnUpdate);
            data.Async.OnCompletedEvent.AddListener(Async_OnCompleted);
        }

        private void Async_OnUpdate(AsyncLoadOperation arg0)
        {
            m_Progress.gameObject.SetActive(true);
            m_Progress.fillAmount = arg0.Progress;
            if (arg0.Progress < 0.9f)
            {
                m_ProgressText.text = "";
            }
            else
            {
                m_ProgressText.text = "正在转码中，请稍等。。。";
            }
        }

        private void Async_OnCompleted(AsyncLoadOperation arg0)
        {
            m_Progress.gameObject.SetActive(false);
            if (isAbort)
            {
                m_Status.text = "已取消";
            }
            else if (!string.IsNullOrEmpty(arg0.Error))
            {
                m_Status.text = "<color=red>失败</color>";
            }
            else
            {
                m_Success.gameObject.SetActive(true);
            }
            // 结束
            IsDone = true;
        }

        /// <summary>
        /// 中止任务
        /// </summary>
        private void buttonAbort_onClick()
        {
            isAbort = true;
            this.Data.Async.Abort();
        }

        public override void OnReset()
        {
            base.OnReset();
            m_Progress.fillAmount = 0;
            m_Progress.gameObject.SetActive(false);
            m_Success.gameObject.SetActive(false);
            m_Status.text = "";
            isAbort = false;
        }

        public void OnAbort()
        {
            this.Data.Async.Abort();
        }
    }
}

