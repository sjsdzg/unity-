using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using XFramework.Core;
using System;

namespace XFramework.Common
{
    /// <summary>
    /// 加载场景UI
    /// </summary>
    public class LoadingSceneUI : BaseUI
    {
        public override EnumUIType GetUIType()
        {
            return EnumUIType.LoadingSceneUI;
        }

        [Header("Settings")]
        public bool useTimeScale = false;// 是否使用时间缩放
        public SceneSkipType SkipType = SceneSkipType.Button;
        [Range(0.5f, 7)]
        public float SceneSmoothLoad = 3;
        [Range(0.5f, 4)]
        public float weight = 1;//下载和加载的权重
        [Range(0.5f, 7)]
        public float FadeInSpeed = 2;
        [Range(0.5f, 7)]
        public float FadeOutSpeed = 2;
        [Header("Background")]
        public bool useBackgrounds = true;
        [Range(1, 60)]
        public float TimePerBackground = 5;
        [Range(0.5f, 7)]
        public float FadeBackgroundSpeed = 2;
        [Range(0.5f, 5)]
        public float TimeBetweenBackground = 0.5f;
        [Header("Tips")]
        public bool RandomTips = false;
        [Range(1, 60)]
        public float TimePerTip = 5;
        [Range(0.5f, 5)]
        public float FadeTipsSpeed = 2;
        [Header("Loading")]
        public bool FadeLoadingSliderOnFinish = false;
        [Range(50, 1000)]
        public float LoadingCircleSpeed = 300;
        [TextArea(2, 2)]
        public string LoadingTextFormat = "{0}";

        [Header("References")]
        [SerializeField]
        private Text DisplayNameText = null;
        [SerializeField]
        private Text DescriptionText = null;
        [SerializeField]
        private Text TipText = null;
        [SerializeField]
        private Image BackgroundImage = null;
        [SerializeField]
        private Image FilledImage = null;//模拟特殊加载条
        [SerializeField]
        private Slider LoadingSlider = null;//加载进度条
        [SerializeField]
        private Text ProgressText = null;//加载进度文本
        [SerializeField]
        private Text StatusText = null;//状态文本
        [SerializeField]
        private GameObject ContinueBar = null;//继续界面
        [SerializeField]
        private Button ContinueButton = null;
        [SerializeField]
        private GameObject SkipKeyText = null;//按任意键进入场景
        [SerializeField]
        private RectTransform LoadingCircle = null;//转动的圆圈
        [SerializeField]
        private CanvasGroup LoadingCircleCanvasGroup = null;
        [SerializeField]
        private CanvasGroup FadeImageCanvasGroup = null;

        private bool isAsyncStart = false;
        private bool FinishLoad = false;
        private CanvasGroup BackgroundImageCanvasGroup = null;
        private CanvasGroup LoadingSliderCanvasGroup = null;
        private int CurrentTip = 0;
        private List<string> cacheTips = new List<string>();
        private int CurrentBackground = 0;
        private List<Sprite> cacheBackgrounds = new List<Sprite>();
        public float Progress { get; private set; }//当前进度值
        private bool canSkipWithKey = false;//是否能按任意键跳过，进入场景
        //异步操作
        private AsyncLoadSceneOperation async = null;
        private SceneLoaderInfo m_SceneLoaderInfo = null;

        protected override void OnAwake()
        {
            base.OnAwake();
            if (ContinueBar != null) { ContinueBar.SetActive(false); }
            if (FadeImageCanvasGroup != null) { FadeImageCanvasGroup.alpha = 1; }
            if (SkipKeyText != null) { SkipKeyText.SetActive(false); }
            if (LoadingSlider != null) { LoadingSliderCanvasGroup = LoadingSlider.GetComponent<CanvasGroup>(); }
            if (BackgroundImage != null) { BackgroundImageCanvasGroup = BackgroundImage.GetComponent<CanvasGroup>(); }
            if (FilledImage != null) { FilledImage.type = Image.Type.Filled; FilledImage.fillAmount = 0; }
        }

        protected override void OnStart()
        {
            base.OnStart();
            m_SceneLoaderInfo = SceneLoader.Instance.Settings.GetInfo(SceneLoader.Instance.AsyncSceneType);
            SetupUI(m_SceneLoaderInfo);

            StartCoroutine(Loading());
        }

        private void SetupUI(SceneLoaderInfo info)
        {
            //背景
            if (useBackgrounds && BackgroundImage != null && info.Backgrounds != null)
            {
                if (info.Backgrounds.Length > 1)
                {
                    cacheBackgrounds.AddRange(info.Backgrounds);
                    StartCoroutine(BackgroundTransition());
                    BackgroundImage.color = Color.white;
                }
                else if (info.Backgrounds.Length > 0)
                {
                    BackgroundImage.sprite = info.Backgrounds[0];
                    BackgroundImage.color = Color.white;
                }
            }
            //场景名称
            if (DisplayNameText != null)
            {
                DisplayNameText.text = info.DisplayName;
            }
            //场景描述
            if (DescriptionText != null)
            {
                DescriptionText.text = info.Description;
            }
            //加载条
            if (LoadingSlider != null)
            {
                LoadingSlider.value = 0;
            }
            //加载进度文本
            if (ProgressText != null)
            {
                ProgressText.text = string.Format(LoadingTextFormat, 0);
            }
            //提示文本
            if (TipText != null && info.Tips != null)
            {
                cacheTips.AddRange(info.Tips);
                if (RandomTips)
                {
                    CurrentTip = UnityEngine.Random.Range(0, cacheTips.Count);
                    TipText.text = cacheTips[CurrentTip];
                }
                else
                {
                    TipText.text = cacheTips[0];
                }
                StartCoroutine(TipsLoop());
            }
            //加载圈
            if (LoadingCircle != null)
            {
                StartCoroutine(LoadingCircleRotation());
            }
        }

        /// <summary>
        /// 加载中
        /// </summary>
        /// <returns></returns>
        private IEnumerator Loading()
        {
            if (FadeImageCanvasGroup != null)
                yield return FadeOut(FadeImageCanvasGroup);
            yield return PreLoadOperation();
            yield return LoadSceneOperation();
        }

        private void Update()
        {
            SkipWithKey();
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (FilledImage != null)
            {
                FilledImage.fillAmount = Progress;
            }

            if (LoadingSlider != null)
            {
                LoadingSlider.value = Progress;
            }

            if (ProgressText != null)
            {
                string percent = (Progress * 100).ToString("F1");
                ProgressText.text = string.Format(LoadingTextFormat, percent);
            }
        }

        private IEnumerator PreLoadOperation()
        {
            AssetBundlePreloadOperation async = SceneLoaderUtils.LoadAsssetBundleAsync(m_SceneLoaderInfo.SceneName);
            if (async != null)
            {
                async.OnUpdate(x =>
                {
                    AssetBundlePreloadOperation loader = x as AssetBundlePreloadOperation;
                    if (loader.Async != null)
                    {
                        StatusText.text = string.Format("正在加载场景所需资源 : {0}...", loader.Async.assetBundleName);
                    }
                    Progress = loader.Progress * weight / (weight + 1);
                });

            }

            yield return async;
        }

        private IEnumerator LoadSceneOperation()
        {
            async = SceneLoaderUtils.LoadSceneAsync(m_SceneLoaderInfo.SceneName, false);
            async.OnStart(x => async.AllowSceneActivation = false);

            float progress = 0;
            while (Progress < 1)
            {
                StatusText.text = "正在加载场景, 请稍等...";
                progress += SceneSmoothLoad / 100f;
                progress = Mathf.Clamp(progress, 0, async.Progress);
                Progress = (weight + progress) / (weight + 1);
                yield return new WaitForEndOfFrame();
            }
            //完成之后
            OnCompleted();
        }

        /// <summary>
        /// 背景过渡
        /// </summary>
        /// <returns></returns>
        IEnumerator BackgroundTransition()
        {
            while (true)
            {
                BackgroundImage.sprite = cacheBackgrounds[CurrentBackground];
                while (BackgroundImageCanvasGroup.alpha < 1)
                {
                    BackgroundImageCanvasGroup.alpha += DeltaTime * FadeBackgroundSpeed * 0.8f;
                    yield return new WaitForEndOfFrame();
                }
                yield return new WaitForSeconds(TimePerBackground);
                while (BackgroundImageCanvasGroup.alpha > 0)
                {
                    BackgroundImageCanvasGroup.alpha -= DeltaTime * FadeBackgroundSpeed;
                    yield return new WaitForEndOfFrame();
                }
                CurrentBackground = (CurrentBackground + 1) % cacheBackgrounds.Count;
                yield return new WaitForSeconds(TimeBetweenBackground);
            }
        }

        /// <summary>
        /// 提示更新
        /// </summary>
        /// <returns></returns>
        IEnumerator TipsLoop()
        {
            Color color = TipText.color;
            while (true)
            {
                while (color.a < 1)
                {
                    color.a += DeltaTime * FadeTipsSpeed;
                    TipText.color = color;
                    yield return new WaitForEndOfFrame();
                }
                yield return new WaitForSeconds(TimePerTip);
                while (color.a > 0)
                {
                    color.a -= DeltaTime * FadeTipsSpeed;
                    TipText.color = color;
                    yield return new WaitForEndOfFrame();
                }
                if (RandomTips)
                {
                    CurrentTip = UnityEngine.Random.Range(0, cacheTips.Count);
                    TipText.text = cacheTips[CurrentTip];
                }
                else
                {
                    CurrentTip = (CurrentTip + 1) % cacheTips.Count;
                    TipText.text = cacheTips[CurrentTip];
                }
            }
        }

        /// <summary>
        /// 完成之后
        /// </summary>
        private void OnCompleted()
        {
            FinishLoad = true;
            switch (GetSkipType)
            {
                case SceneSkipType.Button:
                    if (ContinueBar != null)
                    {
                        ContinueBar.SetActive(true);
                    }
                    break;
                case SceneSkipType.Complete:
                    StartCoroutine(LoadScene());
                    break;
                case SceneSkipType.AnyKey:
                    canSkipWithKey = true;
                    if (SkipKeyText != null)
                    {
                        SkipKeyText.SetActive(true);
                    }
                    break;
                default:
                    break;
            }

            if (LoadingCircleCanvasGroup != null){ StartCoroutine(FadeOut(LoadingCircleCanvasGroup, 0.5f)); }
            if (LoadingSliderCanvasGroup != null && FadeLoadingSliderOnFinish) { StartCoroutine(FadeOut(LoadingSliderCanvasGroup, 1f)); }
        }

        /// <summary>
        /// 按任意键跳过
        /// </summary>
        private void SkipWithKey()
        {
            if (!canSkipWithKey)
                return;

            if (Input.anyKeyDown)
            {
                StartCoroutine(LoadScene());
            }
        }

        /// <summary>
        /// 加载圈旋转
        /// </summary>
        /// <returns></returns>
        private IEnumerator LoadingCircleRotation()
        {
            while (true)
            {
                LoadingCircle.Rotate(-Vector3.forward * DeltaTime * LoadingCircleSpeed);
                yield return new WaitForEndOfFrame();
            }
        }

        /// <summary>
        /// 加载下一个场景
        /// </summary>
        /// <returns></returns>
        private IEnumerator LoadScene()
        {
            FadeImageCanvasGroup.alpha = 0;
            while (FadeImageCanvasGroup.alpha < 1)
            {
                FadeImageCanvasGroup.alpha += DeltaTime * FadeInSpeed;
                yield return new WaitForEndOfFrame();
            }
            async.AllowSceneActivation = true;
        }

        /// <summary>
        /// 
        /// </summary>
        private IEnumerator FadeOut(CanvasGroup alpha, float delay = 0)
        {
            yield return new WaitForSeconds(delay);
            while (alpha.alpha > 0)
            {
                alpha.alpha -= DeltaTime * FadeOutSpeed;
                yield return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private IEnumerator FadeIn(CanvasGroup alpha, float delay = 0)
        {
            yield return new WaitForSeconds(delay);
            while (alpha.alpha < 1)
            {
                alpha.alpha += DeltaTime * FadeInSpeed;
                yield return null;
            }
        }

        private SceneSkipType GetSkipType
        {
            get
            {
                return m_SceneLoaderInfo.SkipType;
            }
        }

        private float DeltaTime
        {
            get { return useTimeScale ? Time.deltaTime : Time.unscaledDeltaTime; }
        }
    }
}

