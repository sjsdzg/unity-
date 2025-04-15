using UnityEngine;
using System.Collections;
using Paroxe.PdfRenderer;

namespace XFramework.UI
{
    public class HelpDisplayPanel : MonoBehaviour
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
            pdfViewer.PageFitting = PDFViewer.PageFittingType.ViewerWidth;
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
