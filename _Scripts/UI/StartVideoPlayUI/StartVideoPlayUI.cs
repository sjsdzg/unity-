using UnityEngine;
using System.Collections;
using XFramework.Core;
using System;
using XFramework;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using XFramework.Module;
using UIWidgets;
using XFramework.Common;

namespace XFramework.UI
{
    /// <summary>
    /// 设备仿真UI
    /// </summary>
    public class StartVideoPlayUI : BaseUI
    {
        public override EnumUIType GetUIType()
        {
            return EnumUIType.StartVideoPlayUI;
        }
        /// <summary>
        /// 返回按钮
        /// </summary>
        private Button buttonBack;
        /// <summary>
        /// VideoPanel
        /// </summary>
        private VideoPanel m_VideoPanel;

        private const string VideoPath = "Videos/StartVideoPlay/";

        protected override void OnAwake()
        {
            InitGUI();
            InitEvent();
        }

        /// <summary>
        /// 初始化UI
        /// </summary>
        private void InitGUI()
        {
            buttonBack = transform.Find("Background/ButtonBack").GetComponent<Button>();
            m_VideoPanel = transform.Find("Background/VideoPanel").GetComponent<VideoPanel>();
        }

        /// <summary>
        /// 初始化Event
        /// </summary>
        private void InitEvent()
        {
            buttonBack.onClick.AddListener(buttonBack_onClick);
        }

        private void buttonBack_onClick()
        {
            SceneLoader.Instance.LoadSceneAsync(SceneType.LoginScene);
        }


        protected override void OnStart()
        {
            base.OnStart();
            m_VideoPanel.LoadFromWeb(AppSettings.Settings.AssetServerUrl + VideoPath + "柔性设计平台.mp4", "柔性设计平台");
        }

        /// <summary>
        /// 加载设备模型
        /// </summary>
        //IEnumerator Loading()
        //{
        //    yield return new WaitForEndOfFrame();
        //    //下载模型资源
        //    List<string> paths = new List<string>();
        //    paths.Add("Assets/_Prefabs/Equipments/" + equipment.Name);
        //    yield return Preload(paths);

        //    //加载Asset
        //    string path = "Assets/_Prefabs/Equipments/" + equipment.Name;
        //    AsyncLoadAssetOperation async = Assets.LoadAssetAsync<GameObject>(path);
        //    if (async != null)
        //    {
        //        async.OnCompleted(x =>
        //        {
        //            AsyncLoadAssetOperation loader = x as AsyncLoadAssetOperation;
        //            GameObject prefab = loader.GetAsset<GameObject>();
        //            GameObject obj = Instantiate(prefab);
        //            equipmentComponent = obj.GetComponent<EquipmentComponent>();
        //            equipmentComponent.SetData(equipment);
        //            equipmentComponent.OnItemClicked.AddListener(deviceComponent_OnItemClicked);
        //            //每次加载出新设备，都要初始化其透明度
        //            sliderTranparent.value = 0.8f;

        //            //重置
        //            buttonReset_onClick();
        //            //更新扩展知识点
        //            if (m_ExpansionContentBar.gameObject.activeSelf)
        //            {
        //                buttonExtension_onClick();
        //            }
        //        });
        //    }
        //}
        /// <summary>
        /// 下载模型资源
        /// </summary>
        /// <param name="paths"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        //protected IEnumerator Preload(List<string> paths)
        //{
        //    if (paths == null)
        //        yield break;

        //    AssetBundlePreloadOperation async = Assets.LoadAssetBundleAsync(paths.ToArray());
        //    if (async != null)
        //    {
        //        async.OnUpdate(x =>
        //        {
        //            AssetBundlePreloadOperation loader = x as AssetBundlePreloadOperation;
        //            if (loader.Async != null)
        //            {
        //                LoadingBar.Instance.Show(loader.Progress, "正在下载：[" + loader.Async.assetBundleName + "]...");
        //            }
        //        });
        //    }

        //    yield return async;

        //    LoadingBar.Instance.Show(1, "资源下载完成。");
        //    yield return new WaitForSeconds(0.2f);
        //    LoadingBar.Instance.Hide();
        //}
    }
}