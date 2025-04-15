using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using System;
using System.IO;
using Paroxe.PdfRenderer;
using UnityEngine.UI;
using XFramework.Module;
using XFramework.Core;
using UnityEngine.Networking;

namespace XFramework.UI
{
    /// <summary>
    /// 扩展内容栏
    /// </summary>
    public class ExpansionContentBar : MonoBehaviour
    {
        /// <summary>
        /// 内容
        /// </summary>
        public RectTransform Content;
        /// <summary>
        /// 默认右键菜单项
        /// </summary>
        public GameObject DefaultItem;

        /// <summary>
        /// ImageList
        /// </summary>
        public ImageList m_ImageList;

        /// <summary>
        /// DocumentPanel
        /// </summary>
        private PDFPanel m_PDFPanel;

        /// <summary>
        /// MediaPlayerPanel
        /// </summary>
        //private MediaPlayerPanel m_MediaPlayerPanel;
        private VideoPanel m_VideoPanel;

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private Button buttonClose;

        /// <summary>
        /// 工具项组件列表
        /// </summary>
        public List<ExpansionItemComponent> m_Components { get; private set; }

        /// <summary>
        /// 工具点击事件类
        /// </summary>
        public class PartItemClickEvent : UnityEvent<ExpansionItemComponent> { }

        private PartItemClickEvent m_ItemOnClicked = new PartItemClickEvent();
        /// <summary>
        /// 部件Item点击触发
        /// </summary>
        public PartItemClickEvent ItemOnClicked
        {
            get { return m_ItemOnClicked; }
            set { m_ItemOnClicked = value; }
        }

        public string URL { get; set; }

        /// <summary>
        /// 选中的部件组件
        /// </summary>
        public ExpansionItemComponent SelectedItem { get; private set; }

        void Awake()
        {
            m_Components = new List<ExpansionItemComponent>();

            if (DefaultItem == null)
            {
                throw new NullReferenceException("DefaultItem is null. Set component of type ImageAdvanced to DefaultItem.");
            }
            DefaultItem.gameObject.SetActive(false);

            m_PDFPanel = transform.parent.Find("PDFPanel").GetComponent<PDFPanel>();
            m_VideoPanel = transform.parent.Find("VideoPanel").GetComponent<VideoPanel>();
            buttonClose = transform.Find("TitleBar/ButtonClose").GetComponent<Button>();
            buttonClose.onClick.AddListener(() => Hide());
        }

        public void Initialize(Equipment equipment)
        {
            Show();
            Clear();//清空
            URL = AppSettings.Settings.AssetServerUrl + "EquipmentExtensions/" + equipment.Name + "/";
            StartCoroutine(DownloadJson(equipment));
        }

        IEnumerator DownloadJson(Equipment equipment)
        {
            UnityWebRequest request = UnityWebRequest.Get(URL + equipment.Name + ".json");
            yield return request.Send();
            if (request == null)
                yield break;

            string json = request.downloadHandler.text;
            ExtensionInfo info = ExtensionInfo.Parser.ParseJson(json);

            foreach (var item in info.Items)
            {
                AddItem(item);
            }
        }

        /// <summary>
        /// 增加部件Item
        /// </summary>
        /// <param name="info"></param>
        public void AddItem(ExtensionItemInfo info)
        {
            GameObject obj = Instantiate(DefaultItem);
            obj.SetActive(true);

            ExpansionItemComponent component = obj.GetComponent<ExpansionItemComponent>();

            if (Content != null && component != null)
            {
                component.transform.SetParent(Content, false);
                obj.layer = Content.gameObject.layer;
                Sprite sprite = m_ImageList[info.Sprite];
                component.SetValue(info, sprite);
                m_Components.Add(component);

                component.OnClicked.AddListener(Component_OnClick);
            }
        }

        /// <summary>
        /// 移除部件Item
        /// </summary>
        public void RemoveItem(string name)
        {
            for (int i = 0; i < m_Components.Count; i++)
            {
                ExpansionItemComponent component = m_Components[i];

                if (component.Data.Name.Equals(name))
                {
                    m_Components.Remove(component);
                    Destroy(component.gameObject);
                    break;
                }
            }
        }

        /// <summary>
        /// 清空部件Item
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < m_Components.Count; i++)
            {
                ExpansionItemComponent item = m_Components[i];
                Destroy(item.gameObject);
            }

            m_Components.Clear();
        }

        /// <summary>
        /// 部件Item点击时，触发。
        /// </summary>
        /// <param name="item"></param>
        private void Component_OnClick(ExpansionItemComponent item)
        {
            m_ItemOnClicked.Invoke(item);
            string url = URL + item.Data.Name;
            switch (item.Data.ExpansionType)
            {
                case ExpansionType.PDF:
                    m_PDFPanel.LoadDocumentFromWeb(url, item.Data.Name);
                    break;
                case ExpansionType.VIDEO:
                    m_VideoPanel.LoadFromWeb(url, item.Data.Name);
                    break;
                case ExpansionType.SWF:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 显示
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
            m_PDFPanel.Hide();
            m_VideoPanel.Hide();
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

