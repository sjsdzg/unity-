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
    /// 工艺视频播放器
    /// </summary>
    public class ProcessMoviePlayer : MonoBehaviour
    {
        /// <summary>
        /// 子工艺列表Panel
        /// </summary>
        private SubprocessListPanel m_Panel;

        /// <summary>
        /// 工艺信息
        /// </summary>
        public ProcessInfo Info { get; private set; }

        /// <summary>
        /// 视频播放器
        /// </summary>
        private VideoPanel m_VideoPanel;

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private Button buttonClose;

        void Awake()
        {
            m_Panel = transform.Find("Panel/SubprocessListPanel").GetComponent<SubprocessListPanel>();
            m_VideoPanel = transform.Find("Panel/VideoPanel").GetComponent<VideoPanel>();
            buttonClose = transform.Find("Panel/Buttons/ButtonClose").GetComponent<Button>();

            m_Panel.ItemOnClicked.AddListener(m_Panel_ItemOnClicked);
            buttonClose.onClick.AddListener(buttonClose_onClick);
        }

        /// <summary>
        /// 设置工艺信息
        /// </summary>
        /// <param name="info"></param>
        public void SetProcessInfo(ProcessInfo info)
        {
            Info = info;
            m_Panel.Clear();
            m_Panel.AddRange(info.SubProcessInfos);
        }

        /// <summary>
        /// 子工艺点击时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void m_Panel_ItemOnClicked(Subprocess subprocess)
        {
            string path = AppSettings.Settings.AssetServerUrl + Info.MovieDir + "/" + subprocess.Data.Name + ".mp4";
            Play(path);
        }

        /// <summary>
        /// 播放视频
        /// </summary>
        /// <param name="path"></param>
        public void Play(string path)
        {
            m_VideoPanel.LoadFromWeb(path);
        }

        /// <summary>
        /// 关闭按钮点击时，触发。
        /// </summary>
        private void buttonClose_onClick()
        {
            gameObject.SetActive(false);
        }

    }
}
