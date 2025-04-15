using System;
using System.Collections.Generic;
using UnityEngine;
using XFramework.Core;
namespace XFramework.Module
{
    /// <summary>
    /// 范例讲解声音管理
    /// </summary>
	public class ArchiteIntroduceAudioController : MonoBehaviour
    {
        public static ArchiteIntroduceAudioController Instance;

        /// <summary>
        /// 声音播放器(包含流程引导)
        /// </summary>
        public AudioSource guideAudioSource;
        
        private Queue<AudioClip> m_ClipQueue = new Queue<AudioClip>();

        bool isPlayQueue=false;
        public string RootPath;
        /// <summary>
        /// 播放开始事件
        /// </summary>
        public event Action<string> EventPlayStart;

        /// <summary>
        /// 播放结束事件
        /// </summary>
        public event Action<string> EventPlayEnd;
        /// <summary>
        /// 监控播放状态
        /// </summary>
        bool _lastPlayStatus;
        /// <summary>
        /// 当前播放音频名称
        /// </summary>
        string currentName;
        void Awake()
        {
            if (Instance == null)
                Instance = this;
            _lastPlayStatus = false;
        }
        /// <summary>
        /// 监测一下当前播放状态
        /// </summary>
        private void UpdatePlaySstatus()
        {
            if (_lastPlayStatus == false && guideAudioSource.isPlaying == true)
            {
                if (EventPlayStart != null)
                {
                    EventPlayStart(currentName);//发出事件：开始播放
                }
            }
            if (_lastPlayStatus == true && guideAudioSource.isPlaying == false)
            {
                if (EventPlayEnd != null)
                {
                    EventPlayEnd(currentName);//发出事件：播放停止
                }
            }
            _lastPlayStatus = guideAudioSource.isPlaying;
        }
        void Update()
        {
            while (m_ClipQueue.Count > 0)
            {
                if (guideAudioSource.isPlaying)
                {
                    return;
                }
                AudioClip clip = m_ClipQueue.Dequeue();
                guideAudioSource.clip = clip;
                guideAudioSource.Play();
            }
            UpdatePlaySstatus();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        public void Play(string audioName)
        {
            currentName = audioName;
            string audioPath = RootPath+ audioName;
            AsyncLoadAssetOperation async = Assets.LoadAssetAsync<AudioClip>(audioPath);
            if (async == null)
                return;
            AudioClip clip = async.GetAsset<AudioClip>();
            m_ClipQueue.Enqueue(clip);
            async.OnCompleted(x =>
            {
                //Debug.Log("444444444444444");
                //AudioClip clip = async.GetAsset<AudioClip>();
                //m_ClipQueue.Enqueue(clip);
            });
        }
        public void Play(AudioClip clip)
        {
            m_ClipQueue.Enqueue(clip);
        }       
        public void Stop()
        {
            if (guideAudioSource.isPlaying)
                guideAudioSource.Stop();
        }

        public void OnDestroy()
        {
            Instance = null;
           
        }
    }
}