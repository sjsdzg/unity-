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
    /// 漫游声音管理
    /// </summary>
	public class RamingAudioController : MonoBehaviour
    {


        /// <summary>
        /// 声音播放器
        /// </summary>
        public AudioSource m_AudioSource;

        public static RamingAudioController Instance;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        public void PlayAudio(string audio)
        {
            m_AudioSource.Stop();

            m_AudioSource.clip = GetAudioClip(audio);
            if (m_AudioSource.clip == null)
                return;
            m_AudioSource.Play();
        }
        private AudioClip GetAudioClip(string audioname)
        {
            AudioClip temp = null;
            temp = ResourceManager.Instance.Load(audioname) as AudioClip;

            return temp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        public void Play(string url)
        {
            string audioPath = "Assets/Audios/WorkshopRoaming/" + url;
            AsyncLoadAssetOperation async = Assets.LoadAssetAsync<AudioClip>(audioPath);
            if (async == null)
                return;

            async.OnCompleted(x =>
            {
                AudioClip clip = async.GetAsset<AudioClip>();
                m_AudioSource.clip = clip;
                m_AudioSource.Play();
            });
        }


        public void Stop()
        {
            if (m_AudioSource.isPlaying)
                m_AudioSource.Stop();

        }

        public void OnDestroy()
        {
            Instance = null;
        }

    }
}