using UnityEngine;
using System.Collections;
using Paroxe.PdfRenderer;
using UnityEngine.UI;
using XFramework.Core;
using UnityEngine.Events;

namespace XFramework.UI
{
    public class PDFPanel : MonoBehaviour, IHide
    {
        /// <summary>
        /// PDF阅读器
        /// </summary>
        private PDFViewer pdfViewer;

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private Button buttonClose;

        /// <summary>
        /// 标题
        /// </summary>
        private Text m_Title;

        private UnityEvent m_OnClosed = new UnityEvent();
        /// <summary>
        /// 关闭
        /// </summary>
        public UnityEvent OnClosed
        {
            get { return m_OnClosed; }
            set { m_OnClosed = value; }
        }

        void Awake()
        {
            pdfViewer = transform.Find("PDFViewer").GetComponent<PDFViewer>();
            m_Title = transform.Find("Header/Text").GetComponent<Text>();
            buttonClose = transform.Find("Header/ButtonClose").GetComponent<Button>();
            buttonClose.onClick.AddListener(buttonClose_onClick);
        }

        private void buttonClose_onClick()
        {
            OnClosed.Invoke();
            Hide();
        }

        /// <summary>
        /// 加载文档
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="fileName"></param>
        public void LoadDocument(string folderPath, string documentName)
        {
            gameObject.SetActive(true);
            m_Title.text = documentName;
            pdfViewer.FileSource = PDFViewer.FileSourceType.Resources;
            pdfViewer.PageFitting = PDFViewer.PageFittingType.ViewerWidth;
            pdfViewer.LoadDocumentFromResources(folderPath, documentName, "", 0);
        }

        public void LoadDocument(byte[] bytes, string documentName)
        {
            gameObject.SetActive(true);
            m_Title.text = documentName;
            pdfViewer.FileSource = PDFViewer.FileSourceType.Bytes;
#if UNITY_WEBGL
            pdfViewer.PageFitting = PDFViewer.PageFittingType.Zoom;
#else
            pdfViewer.PageFitting = PDFViewer.PageFittingType.ViewerWidth;
#endif
            pdfViewer.LoadDocumentFromBuffer(bytes, "");
        }

        public void LoadDocumentFromWeb(string url, string documentName)
        {
            gameObject.SetActive(true);
            m_Title.text = documentName;
//#if UNITY_WEBGL
//            pdfViewer.PageFitting = PDFViewer.PageFittingType.Zoom;
//#else
//            pdfViewer.PageFitting = PDFViewer.PageFittingType.ViewerWidth;
//#endif
            pdfViewer.PageFitting = PDFViewer.PageFittingType.ViewerWidth;
            pdfViewer.LoadDocumentFromWeb(url, "");
        }

        /// <summary>
        /// 显示
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}

