using Paroxe.PdfRenderer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Module;
using XFramework.Core;

namespace XFramework.UI
{
    /// <summary>
    /// 文档展示面板
    /// </summary>
    public class DocumentPanel : MonoBehaviour
    {
        /// <summary>
        /// 获取PDF
        /// </summary>
        [HideInInspector]
        public PDFViewer pdf_Viewer;

        /// <summary>
        /// 缩略图的text
        /// </summary>
        //public Text text_Thumbnail;

        /// <summary>
        /// 物品图标路径
        /// </summary>
        public const string PATH = "EquipmentSimulation";

        /// <summary>
        /// 标题
        /// </summary>
        private Text m_Title;

        /// <summary>
        /// 工作原理部分
        /// </summary>
        //private GameObject m_Principle;

        /// <summary>
        /// 主要部件部分
        /// </summary>
        //private GameObject m_Parts;

        /// <summary>
        /// 内容
        /// </summary>
        //private RectTransform Content;

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private Button buttonClose;

        private Equipment m_Equipment = null;
        /// <summary>
        /// 设备信息
        /// </summary>
        public Equipment Equipment
        {
            get { return m_Equipment; }
            private set { m_Equipment = value; }
        }

        void Awake()
        {
            m_Title = transform.Find("Header/Text").GetComponent<Text>();
            buttonClose = transform.Find("Header/ButtonClose").GetComponent<Button>();
            pdf_Viewer = transform.Find("PDFViewer").GetComponent<PDFViewer>();

            //事件
            buttonClose.onClick.AddListener(() => gameObject.SetActive(false));
        }

        /// <summary>
        /// 显示工作原理
        /// </summary>
        public void DisplayPrinciple(Equipment equipment)
        {
            m_Title.text = "工作原理";
            //显示工作原理
            Equipment = equipment;
            //string path_Principle = string.Format("{0}/{1}/Documents/", PATH, Equipment.Name);
            string path = "Assets/Documents/Equipments/" + Equipment.Name + "_工作原理.pdf";
            //LoadDocument(path_Principle, Equipment.Name + "_工作原理");
            AsyncLoadAssetOperation async = Assets.LoadAssetAsync<TextAsset>(path);
            if (async == null)
                return;

            async.OnCompleted(x =>
            {
                AsyncLoadAssetOperation loader = x as AsyncLoadAssetOperation;
                TextAsset asset = loader.GetAsset<TextAsset>();
                if (asset != null)
                {
                    StartCoroutine(LoadDocumentWithBuffer(asset.bytes));
                }
            });
        }

        /// <summary>
        /// 显示主要部件
        /// </summary>
        public void DisplayParts(Equipment equipment)
        {
            m_Title.text = "主要部件";
            //显示主要部件
            Equipment = equipment;
            string path = "Assets/Documents/Equipments/" + Equipment.Name + "_主要部件.pdf";
            AsyncLoadAssetOperation async = Assets.LoadAssetAsync<TextAsset>(path);
            if (async == null)
                return;

            async.OnCompleted(x =>
            {
                AsyncLoadAssetOperation loader = x as AsyncLoadAssetOperation;
                TextAsset asset = loader.GetAsset<TextAsset>();
                if (asset != null)
                {
                    StartCoroutine(LoadDocumentWithBuffer(asset.bytes));
                }
            });
        }
    
        /// <summary>
        /// 加载文档
        /// </summary>
        /// <param name="bytes"></param>
        public IEnumerator LoadDocumentWithBuffer(byte[] bytes)
        {
            Show();
            //每次打开文档时候的FileSource
            pdf_Viewer.FileSource = PDFViewer.FileSourceType.Bytes;
            //打开PDF插件的时候，默认的页面格式
            pdf_Viewer.PageFitting = PDFViewer.PageFittingType.Zoom;
            yield return new WaitForEndOfFrame();
            pdf_Viewer.LoadDocumentFromBuffer(bytes, "");
            yield return new WaitForSeconds(0.2f);
            //打开PDF插件的时候，默认的页面格式
            pdf_Viewer.PageFitting = PDFViewer.PageFittingType.ViewerWidth;
        }

        public void LoadDocumentFromWeb(string url)
        {
            Show();
            pdf_Viewer.LoadDocumentFromWeb(url, "");
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