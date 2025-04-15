using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Paroxe.PdfRenderer;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework.UI
{
    /// <summary>
    /// 工艺文档
    /// </summary>
    public class ProcessDocumentPanel : MonoBehaviour
    {
        /// <summary>
        /// PDF阅读器
        /// </summary>
        private PDFViewer pdfViewer;

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private Button buttonClose;

        void Awake()
        {
            pdfViewer = transform.Find("PDFViewer").GetComponent<PDFViewer>();
            buttonClose = transform.Find("Header/ButtonClose").GetComponent<Button>();
            buttonClose.onClick.AddListener(() => { gameObject.SetActive(false); });
        }

        /// <summary>
        /// 加载文档
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="fileName"></param>
        public void LoadDocument(string folderPath, string fileName)
        {
            gameObject.SetActive(true);
            pdfViewer.FileSource = PDFViewer.FileSourceType.Resources;
            StopAllCoroutines();
            StartCoroutine(Loading(folderPath, fileName));
        }

        IEnumerator Loading(string folderPath, string fileName)
        {
            yield return new WaitForSeconds(0.2f);
            pdfViewer.LoadDocumentFromResources(folderPath, fileName, "", 0);
            yield return new WaitForSeconds(0.2f);
            pdfViewer.PageFitting = PDFViewer.PageFittingType.Zoom;
        }

        public void LoadDocument(byte[] bytes)
        {
            gameObject.SetActive(true);
            StartCoroutine(LoadDocumentWithBuffer(bytes));
        }

        /// <summary>
        /// 加载文档
        /// </summary>
        /// <param name="bytes"></param>
        public IEnumerator LoadDocumentWithBuffer(byte[] bytes)
        {
            //每次打开文档时候的FileSource
            pdfViewer.FileSource = PDFViewer.FileSourceType.Bytes;
            //打开PDF插件的时候，默认的页面格式
            pdfViewer.PageFitting = PDFViewer.PageFittingType.Zoom;
            yield return new WaitForEndOfFrame();
            pdfViewer.LoadDocumentFromBuffer(bytes, "");
            //打开PDF插件的时候，默认的页面格式
            pdfViewer.PageFitting = PDFViewer.PageFittingType.ViewerWidth;
        }

        public void LoadDocumentFromWeb(string url)
        {
            gameObject.SetActive(true);
            pdfViewer.LoadDocumentFromWeb(url, "");
        }
    }
}
