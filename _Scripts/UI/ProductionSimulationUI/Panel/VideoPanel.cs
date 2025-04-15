using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;
using XFramework.Core;

namespace XFramework.UI
{
    public class VideoPanel : MonoBehaviour, IDisplay, IHide
    {
        /// <summary>
        /// 视频播放器
        /// </summary>
        private VideoPlayer m_VideoPlayer;

        private AudioSource m_AudioSource;

        /// <summary>
        /// 文本
        /// </summary>
        private Text m_Text;

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private Button buttonClose;

        /// <summary>
        /// 播放暂停Toggle
        /// </summary>
        private Toggle togglePlay;

        /// <summary>
        /// 视频滑动条
        /// </summary>
        private Slider videoSlider;

        /// <summary>
        /// 是否拖拽
        /// </summary>
        private bool isDrag = false;

        /// <summary>
        /// 
        /// </summary>
        private RenderTexture m_RenderTexture;

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
            m_VideoPlayer = transform.Find("Video Player").GetComponent<VideoPlayer>();

            //AudioSource
            m_AudioSource = m_VideoPlayer.gameObject.GetOrAddComponent<AudioSource>();
            m_VideoPlayer.playOnAwake = true;
            m_AudioSource.playOnAwake = true;
            m_AudioSource.Pause();

            m_VideoPlayer.source = VideoSource.Url;
            m_VideoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
            m_VideoPlayer.controlledAudioTrackCount = 1;
            m_VideoPlayer.EnableAudioTrack(0, true);
            m_VideoPlayer.SetTargetAudioSource(0, m_AudioSource);

            m_Text = transform.Find("TitleBar/Text").GetComponent<Text>();
            videoSlider = transform.Find("ControllerBar/Slider").GetComponent<Slider>();
            togglePlay = transform.Find("ControllerBar/TogglePlay").GetComponent<Toggle>();
            buttonClose = transform.Find("TitleBar/ButtonClose").GetComponent<Button>();
            //事件
            buttonClose.onClick.AddListener(buttonClose_onClick);
            togglePlay.onValueChanged.AddListener(togglePlay_onValueChanged);
            videoSlider.onValueChanged.AddListener(x => OnVideoSeekSlider());

            m_RenderTexture = m_VideoPlayer.targetTexture;
        }

        /// <summary>
        /// 关闭按钮点击时，触发
        /// </summary>
        private void buttonClose_onClick()
        {
            OnClosed.Invoke();
            Hide();
        }

        private void Update()
        {
            if (m_VideoPlayer.clip == null && string.IsNullOrEmpty(m_VideoPlayer.url))
                return;

            if (isDrag)
                return;

            videoSlider.value = (float)m_VideoPlayer.frame / m_VideoPlayer.frameCount;
        }

        /// <summary>
        /// 在Slider拖拽之前
        /// </summary>
        public void OnVideoSliderDown()
        {
            isDrag = true;
            if (m_VideoPlayer)
            {
                if (m_VideoPlayer.isPlaying)
                {
                    m_VideoPlayer.Pause();
                }
                OnVideoSeekSlider();
            }
            
        }

        public void OnVideoSeekSlider()
        {
            if (m_VideoPlayer && isDrag)
            {
                //m_VideoPlayer.time = m_VideoPlayer.clip.length * videoSlider.value;
                m_VideoPlayer.frame = (long)(m_VideoPlayer.frameCount * videoSlider.value);
            }
        }

        /// <summary>
        /// 在Slider拖拽之后
        /// </summary>
        public void OnVideoSliderUp()
        {
            if (m_VideoPlayer)
            {
                OnVideoSeekSlider();
                m_VideoPlayer.Play();
                isDrag = false;
            }
        }

        /// <summary>
        /// 播放视频
        /// </summary>
        /// <param name="path"></param>
        public void LoadFromClip(VideoClip clip, string text = "")
        {
            gameObject.SetActive(true);
#if UNITY_WEBGL
            CreateVideoPlayer();
#endif
            m_VideoPlayer.source = VideoSource.VideoClip;
            m_VideoPlayer.clip = clip;
            m_VideoPlayer.frame = 0;
            m_Text.text = text;
        }

#if UNITY_WEBGL
        /// <summary>
        /// 创建视频播放组件
        /// </summary>
        public void CreateVideoPlayer()
        {
            if (m_VideoPlayer != null)
            {
                m_VideoPlayer.Stop();
                m_VideoPlayer.targetTexture.Release();
                Destroy(m_VideoPlayer);
            }
            
            m_VideoPlayer = transform.Find("Video Player").gameObject.AddComponent<VideoPlayer>();
            m_VideoPlayer.targetTexture = m_RenderTexture;

            //m_AudioSource.playOnAwake = false;
            //m_VideoPlayer.playOnAwake = false;
            //m_AudioSource.Pause();

            m_VideoPlayer.source = VideoSource.Url;
            m_VideoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
            m_VideoPlayer.controlledAudioTrackCount = 1;
            m_VideoPlayer.EnableAudioTrack(0, true);
            m_VideoPlayer.SetTargetAudioSource(0, m_AudioSource);
        }
#endif

        /// <summary>
        /// 播放视频
        /// </summary>
        /// <param name="path"></param>
        public void LoadFromWeb(string url, string text = "")
        {

            gameObject.SetActive(true);
#if UNITY_WEBGL
            CreateVideoPlayer();
#endif
            m_VideoPlayer.source = VideoSource.Url;
            m_VideoPlayer.url = url;
            m_Text.text = text;
            m_VideoPlayer.time = 0;
        }

        /// <summary>
        /// 播放Toggle改变时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void togglePlay_onValueChanged(bool b)
        {
            if (b)
            {
                m_VideoPlayer.Play();
            }
            else
            {
                m_VideoPlayer.Pause();
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 关闭视频
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
            if (m_VideoPlayer)
                m_VideoPlayer.Stop();
        }
    }
}
