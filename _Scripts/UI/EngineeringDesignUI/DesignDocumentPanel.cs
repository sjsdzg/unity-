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
    public class DesignDocumentPanel : MonoBehaviour
    {
        /// <summary>
        /// PDF阅读器
        /// </summary>
        private PDFViewer pdfViewer;

        void Awake()
        {
            pdfViewer = transform.Find("PDFViewer").GetComponent<PDFViewer>();
        }

        /// <summary>
        /// 加载文档
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="fileName"></param>
        public void LoadDocument(string folderPath, string fileName)
        {
            pdfViewer.FileSource = PDFViewer.FileSourceType.Resources;
            pdfViewer.PageFitting = PDFViewer.PageFittingType.ViewerWidth;
            StopAllCoroutines();
            StartCoroutine(Loading(folderPath, fileName));
        }

        IEnumerator Loading(string folderPath, string fileName)
        {
            yield return new WaitForSeconds(0.2f);
            pdfViewer.LoadDocumentFromResources(folderPath, fileName, "", 0);
        }
    }
}
