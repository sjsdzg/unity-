using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ChartAndGraph;
using System;
using System.Collections.Generic;
using System.IO;
using Crosstales.FB;
using XFramework.Common;

namespace XFramework.UI
{
    /// <summary>
    /// 饼状图表单
    /// </summary>
    public class PieChartForm : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        private PieChart m_PieChart;

        /// <summary>
        /// 图表动画组件
        /// </summary>
        private PieAnimation m_PieAnimation;

        /// <summary>
        /// 材质列表
        /// </summary>
        [SerializeField]
        private Material[] materials;

        /// <summary>
        /// 导出图片按钮
        /// </summary>
        private Button buttonImage;

        /// <summary>
        /// 导出PDF按钮
        /// </summary>
        private Button buttonPdf;

        private void Awake()
        {
            buttonImage = transform.Find("Value/ButtonImage").GetComponent<Button>();
            buttonPdf = transform.Find("Value/ButtonPdf").GetComponent<Button>();
            m_PieChart = transform.Find("Value/PieCanvas").GetComponent<PieChart>();
            m_PieAnimation = transform.Find("Value/PieCanvas").GetComponent<PieAnimation>();
            // Event
            buttonImage.onClick.AddListener(buttonImage_onClick);
            buttonPdf.onClick.AddListener(buttonPdf_onClick);
            // Data
            m_PieChart.DataSource.Clear();
            buttonImage.gameObject.SetActive(false);
            buttonPdf.gameObject.SetActive(false);
        }

        public void InitData(List<PieChartData> pieChartDatas)
        {
            m_PieChart.DataSource.Clear();
            for (int i = 0; i < pieChartDatas.Count; i++)
            {
                PieChartData pieChartData = pieChartDatas[i];
                m_PieChart.DataSource.AddCategory(pieChartData.Name, materials[i]);
                m_PieChart.DataSource.SetValue(pieChartData.Name, pieChartData.Amount);
            }
            m_PieAnimation.Animate();
            // 显示按钮
            buttonImage.gameObject.SetActive(true);
            buttonPdf.gameObject.SetActive(true);
        }

        private void buttonImage_onClick()
        {
            RectTransform rectTransform = m_PieChart.GetComponent<RectTransform>();
            Rect rect = rectTransform.rect;
            Vector2 pos = RectTransformUtility.WorldToScreenPoint(null, m_PieChart.transform.position);
            //Vector2 pos = Camera.main.WorldToScreenPoint(m_PieChart.transform.position);
            pos.x -= rect.width / 2;
            pos.y -= rect.height / 2;
            rect.x = pos.x;
            rect.y = pos.y;

            var extensionList = new ExtensionFilter[]
            {
                new ExtensionFilter("Image", "png")
            };
            // Save File
            string path = FileBrowser.SaveFile("Save File", "", "成绩分析图表", extensionList);
            //string path = StandaloneFileBrowser.SaveFilePanel("Save File", "", "成绩分析图表", extensionList);
            if (string.IsNullOrEmpty(path))
                return;

            StartCoroutine(PieChartToPng(rect, path));
        }

        private void buttonPdf_onClick()
        {
            RectTransform rectTransform = m_PieChart.GetComponent<RectTransform>();
            Rect rect = rectTransform.rect;
            Vector2 pos = RectTransformUtility.WorldToScreenPoint(null, m_PieChart.transform.position);
            pos.x -= rect.width / 2;
            pos.y -= rect.height / 2;
            rect.x = pos.x;
            rect.y = pos.y;

            var extensionList = new ExtensionFilter[]
            {
                new ExtensionFilter("PDF", "pdf")
            };
            // Save File
            string path = FileBrowser.SaveFile("Save File", "", "成绩分析图表", extensionList);
            //string path = StandaloneFileBrowser.SaveFilePanel("Save File", "", "成绩分析图表", extensionList);
            if (string.IsNullOrEmpty(path))
                return;

            StartCoroutine(PieChartToPdf(rect, path));
        }

        private IEnumerator PieChartToPng(Rect rect, string filePath)
        {
            // Wait for graphics to render
            yield return new WaitForEndOfFrame();

            // Create a texture to pass to encoding
            Texture2D texture = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);

            // Put buffer into texture
            texture.ReadPixels(rect, 0, 0);
            texture.Apply();

            yield return 0;

            byte[] bytes = texture.EncodeToPNG();
            
            // Save our image
            File.WriteAllBytes(filePath, bytes);

            Destroy(texture);

            MessageBoxEx.Show("导出成功！", "提示", MessageBoxExEnum.SingleDialog, null);
        }

        private IEnumerator PieChartToPdf(Rect rect, string filePath)
        {
            // Wait for graphics to render
            yield return new WaitForEndOfFrame();

            // Create a texture to pass to encoding
            Texture2D texture = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);

            // Put buffer into texture
            texture.ReadPixels(rect, 0, 0);
            texture.Apply();

            yield return 0;

            byte[] bytes = texture.EncodeToPNG();

            try
            {
                // 创建写入器实例，PDF文件将会保存到这里
                using (FileStream fs = new System.IO.FileStream(filePath, FileMode.OpenOrCreate))
                {
                    // 创建文档
                    using (iTextSharp.text.Document document = new iTextSharp.text.Document())
                    {
                        iTextSharp.text.pdf.PdfWriter pdfWriter = iTextSharp.text.pdf.PdfWriter.GetInstance(document, fs);

                        // 打开文档
                        document.Open();

                        Debug.Log("BaseFont");
                        string fontPath = Application.streamingAssetsPath + "/Fonts/simhei.ttf";
                        iTextSharp.text.pdf.BaseFont baseFont = iTextSharp.text.pdf.BaseFont.CreateFont(fontPath, iTextSharp.text.pdf.BaseFont.IDENTITY_H, iTextSharp.text.pdf.BaseFont.NOT_EMBEDDED);
                        iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont);

                        // 加入一行标题
                        Debug.Log("pdfText");
                        string fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
                        iTextSharp.text.Paragraph paragraph = new iTextSharp.text.Paragraph(fileName, font);
                        paragraph.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                        document.Add(paragraph);

                        Debug.Log("pdfImage");
                        iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(bytes);
                        document.Add(pdfImage);
                        //using (MemoryStream ms = new MemoryStream(bytes))
                        //{
                        //    System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                        //    iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(image, System.Drawing.Imaging.ImageFormat.Png);
                        //    document.Add(pdfImage);
                        //}
                    }

                }

                MessageBoxEx.Show("导出成功！", "提示", MessageBoxExEnum.SingleDialog, null);
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("导出Pdf出现异常：{0}", e);
            }

        }
    }


    public class PieChartData
    {
        public string Name { get; set; }

        public double Amount { get; set; }
    }
}
