using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XFramework.Core;

namespace XFramework.UI
{
    public class LoadingUI : BaseUI
    {
        /// <summary>
        /// 进度条
        /// </summary>
        private Slider m_Slider;

        /// <summary>
        /// 进度文本
        /// </summary>
        private Text m_ProgressText;

        /// <summary>
        /// 异步加载场景操作
        /// </summary>
        private AsyncOperation async;

        void Awake()
        {
            m_Slider = transform.Find("Panel/Slider").GetComponent<Slider>();
            m_ProgressText = transform.Find("Panel/Slider/ProgressText").GetComponent<Text>();
        }

        public override EnumUIType GetUIType()
        {
            return EnumUIType.LoadingUI;
        }

        //protected override void OnStart()
        //{
        //    base.OnStart();

        //    this.Invoke(0.02f, ParseSceneInfo); 
        //}

        ///// <summary>
        ///// 解析场景信息
        ///// </summary>
        //private void ParseSceneInfo()
        //{
        //    string sceneName = SceneLoader.Instance.GetSceneName(SceneLoader.Instance.AsyncScene);

        //    if (string.IsNullOrEmpty(sceneName))
        //        return;

        //    async = SceneManager.LoadSceneAsync(sceneName);
        //    async.allowSceneActivation = false;
        //    StartCoroutine(LoadingSceneAsync());
        //}

        ///// <summary>
        ///// 设置进度条的值
        ///// </summary>
        ///// <param name="value"></param>
        //private void SetProgerssValue(int value)
        //{
        //    m_Slider.value = (float)value / 100;
        //    m_ProgressText.text = value + "%";
        //}

        ///// <summary>
        ///// 默认加载场景的协程
        ///// </summary>
        ///// <returns></returns>
        //private IEnumerator LoadingSceneAsync()
        //{
        //    int displayProgress = 0;
        //    int toProgress = 0;
        //    async.allowSceneActivation = false;

        //    while (async.progress < 0.9f)
        //    {
        //        toProgress = (int)async.progress * 100;
        //        while (displayProgress < toProgress)
        //        {
        //            ++displayProgress;
        //            SetProgerssValue(displayProgress);
        //            yield return new WaitForEndOfFrame();
        //            yield return new WaitForSeconds(0.02f);
        //        }
        //    }

        //    toProgress = 100;

        //    while (displayProgress < toProgress)
        //    {
        //        ++displayProgress;
        //        SetProgerssValue(displayProgress);
        //        yield return new WaitForEndOfFrame();
        //        yield return new WaitForSeconds(0.02f);
        //    }

        //    async.allowSceneActivation = true;

        //    SceneLoader.Instance.CurrentSceneType = SceneLoader.Instance.AsyncScene;
        //}

        ///// <summary>
        ///// 进入异步场景
        ///// </summary>
        //public void IntoAsyncScene()
        //{
        //    async.allowSceneActivation = true;
        //    SceneLoader.Instance.CurrentSceneType = SceneLoader.Instance.AsyncScene;
        //}
    }
}
