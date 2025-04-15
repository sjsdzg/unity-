using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using UIWidgets;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Common;
using XFramework.Core;
using XFramework.Module;

namespace XFramework.UI
{
    /// <summary>
    /// 工艺仿真界面
    /// </summary>
    public class ProcessSimulationUI : BaseUI
    {
        public override EnumUIType GetUIType()
        {
            return EnumUIType.ProcessSimulationUI;
        }

        /// <summary>
        /// 工艺原理按钮
        /// </summary>
        private Button buttonDocument;

        /// <summary>
        /// 工艺演示按钮
        /// </summary>
        private Button buttonDemo;

        /// <summary>
        /// 工艺列表面板
        /// </summary>
        private ProcessListPanel processListPanel;

        /// <summary>
        /// 工艺流程图栏
        /// </summary>
        private ProcessDiagramBar diagramBar;

        /// <summary>
        /// 工艺仿真模块
        /// </summary>
        private ProcessSimulationModule processModule;

        /// <summary>
        /// 工艺系统组件
        /// </summary>
        private ProcessSystemComponent processSystemComponent = null;

        /// <summary>
        /// 工艺信息列表
        /// </summary>
        private List<ProcessInfo> m_ProcessInfos;

        /// <summary>
        /// 当前选中工艺
        /// </summary>
        private ProcessInfo CurrentProcessInfo { get; set; }

        /// <summary>
        /// 工艺信息框
        /// </summary>
        private ProcessDialog processDialog;

        /// <summary>
        /// 工艺视频播放器
        /// </summary>
        private ProcessMoviePlayer moviePlayer;

        /// <summary>
        /// 工艺文档面板
        /// </summary>
        private PDFPanel pdfPanel;

        /// <summary>
        /// 返回按钮
        /// </summary>
        private Button buttonBack;

        /// <summary>
        /// 透明度滑动条
        /// </summary>
        private Slider sliderTranparent;

        /// <summary>
        /// 透明度文本
        /// </summary>
        private Text textTranparent;

        /// <summary>
        /// 重置按钮
        /// </summary>
        private Button buttonReset;

        protected override void OnAwake()
        {
            ModuleManager.Instance.Register<ProcessSimulationModule>();
            processModule = ModuleManager.Instance.GetModule<ProcessSimulationModule>();

            InitGUI();
            InitEvent();
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            ModuleManager.Instance.Unregister<ProcessSimulationModule>();
            DOTween.KillAll();
        }

        /// <summary>
        /// 初始化UI
        /// </summary>
        private void InitGUI()
        {
            buttonBack = transform.Find("Background/TitleBar/ButtonBack").GetComponent<Button>();
            buttonDocument = transform.Find("Background/MenuBar/ButtonDocument").GetComponent<Button>();
            buttonDemo = transform.Find("Background/MenuBar/ButtonDemo").GetComponent<Button>();

            pdfPanel = transform.Find("Background/PDFPanel").GetComponent<PDFPanel>();
            processListPanel = transform.Find("Background/ProcessListPanel").GetComponent<ProcessListPanel>();
            diagramBar = transform.Find("Background/ProcessDiagramBar").GetComponent<ProcessDiagramBar>();
            processDialog = transform.Find("Background/ProcessDialog").GetComponent<ProcessDialog>();
            moviePlayer = transform.Find("Background/ProcessMoviePlayer").GetComponent<ProcessMoviePlayer>();

            sliderTranparent = transform.Find("Background/Setting/Slider").GetComponent<Slider>();
            textTranparent = transform.Find("Background/Setting/Slider/Label").GetComponent<Text>();
            buttonReset = transform.Find("Background/Setting/ButtonReset").GetComponent<Button>();
        }

        /// <summary>
        /// 初始化Event
        /// </summary>
        private void InitEvent()
        {
            buttonBack.onClick.AddListener(buttonBack_onClick);
            processListPanel.NodeSelected.AddListener(processListPanel_NodeSelected);
            diagramBar.ItemOnClicked.AddListener(diagramBar_ItemOnClicked);
            buttonDemo.onClick.AddListener(buttonDemo_onClick);
            buttonDocument.onClick.AddListener(buttonDocument_onClick);
            sliderTranparent.onValueChanged.AddListener(sliderTranparent_onValueChanged);
            buttonReset.onClick.AddListener(buttonReset_onClick);
        }

        private void buttonBack_onClick()
        {
            SceneLoader.Instance.LoadSceneAsync(SceneType.LoginScene);
        }

        protected override void OnStart()
        {
            base.OnStart();
            processDialog.gameObject.SetActive(false);
            moviePlayer.gameObject.SetActive(false);
            pdfPanel.gameObject.SetActive(false);

            m_ProcessInfos = processModule.GetProcessInfos();
            processListPanel.InitData(m_ProcessInfos);
        }

        /// <summary>
        /// 设备面板选中Node,触发
        /// </summary>
        /// <param name="arg0"></param>
        private void processListPanel_NodeSelected(TreeNode<TreeViewItem> node)
        {
            ProcessInfo info = node.Item.Tag as ProcessInfo;
            if (processSystemComponent != null)
                DOTween.KillAll();

            if (processSystemComponent == null || CurrentProcessInfo.Name != info.Name)
            {
                if (processSystemComponent != null)
                {
                    Debug.Log("上一个选择的是：" + processSystemComponent.name);
                }
                
                Debug.Log("当前点击不为空且选择的是："+ info.Name);
                diagramBar.Clear();
                StartCoroutine(LoadModel(info));
                CurrentProcessInfo = info;
                Debug.Log("选择后的工艺是："+ CurrentProcessInfo.Name);
            }
        }

        /// <summary>
        /// 加载设备模型
        /// </summary>
        IEnumerator LoadModel(ProcessInfo info)
        {
            if (processSystemComponent != null)
            {
                Debug.Log("加载时组件不为空");
                Destroy(processSystemComponent.gameObject);
                Debug.Log("加载时组件不为空就删除这个设备");
            }

            //下载模型资源
            List<string> paths = new List<string>();
            paths.Add(info.PrefabPath);
            Debug.Log("加载设备模型的路径是："+info.PrefabPath);
            paths.Add(info.DocumentDir + "/" + info.Name + ".pdf");
            yield return Preload(paths);

            AsyncLoadAssetOperation async = Assets.LoadAssetAsync<GameObject>(info.PrefabPath);
            if (async == null)
                yield break;

            yield return async;
            GameObject prefab = async.GetAsset<GameObject>();
            GameObject obj = Instantiate(prefab);
            Debug.Log("克隆出一个设备");
            processSystemComponent = obj.GetComponent<ProcessSystemComponent>();
            processSystemComponent.SetProcessInfo(info);
            Debug.Log("加载的设备信息是："+info.Id+"_"+info.Name+"_"+info.PrefabPath);
            diagramBar.AddRange(info.SubProcessInfos);
        }

        /// <summary>
        /// 工艺流程图标Item点击时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void diagramBar_ItemOnClicked(SubprocessItem item)
        {
            SubprocessInfo info = item.Data;
            processSystemComponent.DisplaySubprocess(info);
            processDialog.SetValue(info.Name, info.Desc);
        }

        /// <summary>
        /// 工艺演示点击
        /// </summary>
        private void buttonDemo_onClick()
        {
            moviePlayer.gameObject.SetActive(true);
            moviePlayer.SetProcessInfo(CurrentProcessInfo);
        }

        /// <summary>
        /// 工艺文档按钮点击时，触发
        /// </summary>
        private void buttonDocument_onClick()
        {
            string path = CurrentProcessInfo.DocumentDir + "/" + CurrentProcessInfo.Name + ".pdf";
            AsyncLoadAssetOperation async = Assets.LoadAssetAsync<TextAsset>(path);
            if (async == null)
                return;

            async.OnCompleted(x =>
            {
                TextAsset asset = async.GetAsset<TextAsset>();
                pdfPanel.LoadDocument(asset.bytes, CurrentProcessInfo.Name);
            });
        }

        /// <summary>
        /// 重置按钮点击时，触发
        /// </summary>
        private void buttonReset_onClick()
        {
            processSystemComponent.Reset();
        }

        /// <summary>
        /// 透明滑动条更改时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void sliderTranparent_onValueChanged(float value)
        {
            processSystemComponent.SetTransparent(1 - value);
            textTranparent.text = value.ToString("00%");
        }


        /// <summary>
        /// 下载模型资源
        /// </summary>
        /// <param name="paths"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        protected IEnumerator Preload(List<string> paths)
        {
            if (paths == null)
                yield break;

            AssetBundlePreloadOperation async = Assets.LoadAssetBundleAsync(paths.ToArray());
            if (async != null)
            {
                async.OnUpdate(x =>
                {
                    AssetBundlePreloadOperation loader = x as AssetBundlePreloadOperation;
                    if (loader.Async != null)
                    {
                        LoadingBar.Instance.Show(loader.Progress, "正在下载：[" + loader.Async.assetBundleName + "]...");
                    }
                });
            }

            yield return async;

            LoadingBar.Instance.Show(1, "资源下载完成。");
            yield return new WaitForSeconds(0.2f);
            LoadingBar.Instance.Hide();
        }
    }
}
