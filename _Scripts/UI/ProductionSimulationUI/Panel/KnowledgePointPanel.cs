using UnityEngine;
using System.Collections;
using XFramework.Module;
using XFramework.Core;
using System.Collections.Generic;

namespace XFramework.UI
{
    public class KnowledgePointPanel : MonoBehaviour
    {
        private const string ImagePath = "Assets/Textures/KnowledgePoint";
        private const string VideoPath = "Videos/KnowledgePoint/";
        public const string PdfPath = "Assets/Documents/KnowledgePoint/";

        private KnowTextPanel textPanel;
        private KnowImagePanel imagePanel;
        private PDFPanel pdfPanel;
        private VideoPanel videoPanel;

        /// <summary>
        /// panel字典
        /// </summary>
        private Dictionary<KnowledgePointType, IHide> panels = new Dictionary<KnowledgePointType, IHide>();

        private void Awake()
        {
            imagePanel = transform.Find("KnowImagePanel").GetComponent<KnowImagePanel>();
            textPanel = transform.Find("KnowTextPanel").GetComponent<KnowTextPanel>();
            pdfPanel = transform.Find("PDFPanel").GetComponent<PDFPanel>();
            videoPanel = transform.Find("VideoPanel").GetComponent<VideoPanel>();

            imagePanel.OnClosed.AddListener(() => Hide());
            textPanel.OnClosed.AddListener(() => Hide());
            videoPanel.OnClosed.AddListener(() => Hide());
            pdfPanel.OnClosed.AddListener(() => Hide());

            panels.Add(KnowledgePointType.Text, textPanel);
            panels.Add(KnowledgePointType.Image, imagePanel);
            panels.Add(KnowledgePointType.Video, videoPanel);
            panels.Add(KnowledgePointType.Pdf, textPanel);
        }

        public void Show(KnowledgePoint knowledgePoint)
        {
            foreach (var key in panels.Keys)
            {
                if (key != knowledgePoint.Type)
                    panels[key].Hide();
            }

            gameObject.gameObject.SetActive(true);
            switch (knowledgePoint.Type)
            {
                case KnowledgePointType.None:
                    break;
                case KnowledgePointType.Text:
                    imagePanel.Hide();
                    textPanel.Show(knowledgePoint.Name, knowledgePoint.Description);
                    break;
                case KnowledgePointType.Image:
                    AsyncLoadAssetOperation async = Assets.LoadAssetAsync<Sprite>(ImagePath, knowledgePoint.URL);
                    if (async == null)
                        return;

                    async.OnCompleted(x =>
                    {
                        Sprite sprite = async.GetAsset<Sprite>();
                        imagePanel.Show(knowledgePoint.Name, sprite);
                    });
                    break;
                case KnowledgePointType.Video:
                    videoPanel.LoadFromWeb(AppSettings.Settings.AssetServerUrl + VideoPath + knowledgePoint.URL, knowledgePoint.Name);
                    break;
                case KnowledgePointType.Device:
                    break;
                case KnowledgePointType.Pdf:
                    AsyncLoadAssetOperation loader = Assets.LoadAssetAsync<TextAsset>(PdfPath + knowledgePoint.URL + ".pdf");
                    if (loader == null)
                        return;

                    loader.OnCompleted(x =>
                    {
                        TextAsset asset = loader.GetAsset<TextAsset>();
                        pdfPanel.LoadDocument(asset.bytes, knowledgePoint.Name);
                    });
                    break;
                default:
                    break;
            }
        }

        public void Hide()
        {
            gameObject.gameObject.SetActive(false);
        }
    }
}

