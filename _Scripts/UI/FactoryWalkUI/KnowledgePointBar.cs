using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.Module;

namespace XFramework.UI
{
    /// <summary>
    /// 知识点栏
    /// </summary>
    public class KnowledgePointBar : MonoBehaviour
    {
        private const string VideoPath = "Videos/FactoryWalk/";

        /// <summary>
        /// 文字面板
        /// </summary>
        private TextPanel textPanel;

        /// <summary>
        /// 视频播放面板
        /// </summary>
        private VideoPanel videoPanel;

        void Awake()
        {
            videoPanel = transform.Find("VideoPanel").GetComponent<VideoPanel>();
            textPanel = transform.Find("TextPanel").GetComponent<TextPanel>();
        }

        private void Start()
        {
            textPanel.Hide();
            videoPanel.Hide();
        }

        /// <summary>
        /// 显示知识点
        /// </summary>
        public void Display(KnowledgePoint point)
        {
            gameObject.SetActive(true);
            switch (point.Type)
            {
                case KnowledgePointType.None:
                    break;
                case KnowledgePointType.Text:
                    videoPanel.Hide();
                    textPanel.Show(point.Name, point.Description);
                    break;
                case KnowledgePointType.Image:
                    break;
                case KnowledgePointType.Video:
                    textPanel.Hide();
                    videoPanel.LoadFromWeb(AppSettings.Settings.AssetServerUrl + VideoPath + point.URL, point.Name);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            gameObject.SetActive(false);
            textPanel.Hide();
            videoPanel.Hide();
        }
    }
}
