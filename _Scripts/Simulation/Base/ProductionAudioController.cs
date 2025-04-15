using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XFramework.Core;
using XFramework.Module;
using XFramework.UI;
namespace XFramework.Module
{
    /// <summary>
    /// 生产操作声音管理
    /// </summary>
	public class ProductionAudioController : MonoBehaviour
    {
        public static ProductionAudioController Instance;

        /// <summary>
        /// 声音播放器(包含流程引导)
        /// </summary>
        public AudioSource guideAudioSource;
        /// <summary>
        /// 背景音乐播放器
        /// </summary>
        public AudioSource bgmAudioSource;
        /// <summary>
        /// 音效播放器
        /// </summary>
        public AudioSource effectAudioSource;

        [HideInInspector]
        public string stageName;
        
        private Queue<AudioClip> m_ClipQueue = new Queue<AudioClip>();

        bool isPlayQueue=false;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
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
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        public void Play(string audioName)
        {
            string audioPath = "Assets/Audios/ProductionSimulation/"+ stageName +"/"+ audioName;
            AsyncLoadAssetOperation async = Assets.LoadAssetAsync<AudioClip>(audioPath);
            if (async == null)
                return;

            async.OnCompleted(x =>
            {
                AudioClip clip = async.GetAsset<AudioClip>();
                m_ClipQueue.Enqueue(clip);
            });
        }

        public void Play(AudioClip clip)
        {
            m_ClipQueue.Enqueue(clip);
        }

        public void PlayEffect(string audioName)
        {
            string audioPath = "Assets/Audios/Effect/" + audioName;
            AsyncLoadAssetOperation async = Assets.LoadAssetAsync<AudioClip>(audioPath);
            if (async == null)
                return;

            async.OnCompleted(x =>
            {
                AudioClip clip = async.GetAsset<AudioClip>();
                effectAudioSource.clip = clip;
                effectAudioSource.Play();
            });
        }

        public void PlayEffect(AudioClip clip)
        {
            effectAudioSource.clip = clip;
            effectAudioSource.Play();
        }
        
        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="url"></param>
        public void PlayBGM()
        {
            string audioPath = "Assets/Audios/BGM/生产操作背景音乐";
            AsyncLoadAssetOperation async = Assets.LoadAssetAsync<AudioClip>(audioPath);
            if (async == null)
                return;

            async.OnCompleted(x =>
            {
                AudioClip clip = async.GetAsset<AudioClip>();
                bgmAudioSource.clip = clip;
                bgmAudioSource.Play();
            });
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