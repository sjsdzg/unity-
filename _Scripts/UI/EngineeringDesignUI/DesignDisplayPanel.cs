using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Paroxe.PdfRenderer;
using UnityEngine;

namespace XFramework.UI
{
    /// <summary>
    /// 设计文档面板
    /// </summary>
    public class DesignDisplayPanel : MonoBehaviour
    {
        /// <summary>
        /// PDF阅读器
        /// </summary>
        private PDFViewer pdfViewer;

        /// <summary>                                                                                                     
        /// 视频播放器
        /// </summary>
        private VideoPanel m_VideoPanel;

        void Awake()
        {
            pdfViewer = transform.Find("PDFViewer").GetComponent<PDFViewer>();
            m_VideoPanel = transform.Find("VideoPanel").GetComponent<VideoPanel>();
        }

        void Start()
        {
            m_VideoPanel.Hide();
        }

        public void LoadDocumentFromWeb(string url)
        {
            pdfViewer.LoadDocumentFromWeb(url, "");
        }

        public void LoadDocumentWithBuffer(byte[] bytes)
        {
            m_VideoPanel.Hide();
            pdfViewer.gameObject.SetActive(true);
            pdfViewer.LoadDocumentFromBuffer(bytes, "");
#if UNITY_WEBGL
            pdfViewer.PageFitting = PDFViewer.PageFittingType.Zoom;
#else
            pdfViewer.PageFitting = PDFViewer.PageFittingType.ViewerWidth;
#endif
        }

        /// <summary>
        /// 播放视频
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="fileName"></param>
        public void PlayVideo(string url)
        {
            pdfViewer.gameObject.SetActive(false);
            m_VideoPanel.LoadFromWeb(url, "");
        }
    }
}
