using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIWidgets;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Common;
using XFramework.Core;
using XFramework.Module;

namespace XFramework.UI
{
    /// <summary>
    /// 工程设计界面
    /// </summary>
    public class EngineeringDesignUI : BaseUI
    {
        public override EnumUIType GetUIType()
        {
            return EnumUIType.EngineeringDesignUI;
        }

        /// <summary>
        /// 工程设计根目录
        /// </summary>
        private readonly string rootDir = "EngineeringDesign/";

        /// <summary>
        /// 设计信息面板
        /// </summary>
        private DesignInfoPanel designInfoPanel;

        /// <summary>
        /// 工程设计文档
        /// </summary>
        private DesignDisplayPanel displayPanel;

        /// <summary>
        /// 返回按钮
        /// </summary>
        private Button buttonBack;

        /// <summary>
        /// 工程设计模块
        /// </summary>
        public EngineeringDesignModule designModule;

        protected override void OnAwake()
        {
            base.OnAwake();
            ModuleManager.Instance.Register<EngineeringDesignModule>();
            designModule = ModuleManager.Instance.GetModule<EngineeringDesignModule>();

            InitGUI();
            InitEvent();
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        private void InitGUI()
        {
            designInfoPanel = transform.Find("Background/DesignInfoPanel").GetComponent<DesignInfoPanel>();
            displayPanel = transform.Find("Background/DesignDisplayPanel").GetComponent<DesignDisplayPanel>();
            buttonBack = transform.Find("Background/Header/ButtonBack").GetComponent<Button>();
        }

        /// <summary>
        /// 初始化事件
        /// </summary>
        private void InitEvent()
        {
            designInfoPanel.NodeSelected.AddListener(designInfoPanel_NodeSelected);
            buttonBack.onClick.AddListener(buttonBack_onClick);
        }

        private void buttonBack_onClick()
        {
            SceneLoader.Instance.LoadSceneAsync(SceneType.LoginScene);
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            ModuleManager.Instance.Unregister<EngineeringDesignModule>();
        }

        protected override void OnStart()
        {
            base.OnStart();
            List<Folder> mlist = designModule.GetDesignData();
            designInfoPanel.InitData(mlist);
        }

        /// <summary>
        /// 设计信息面板点击时，触发
        /// </summary>
        /// <param name="node"></param>
        private void designInfoPanel_NodeSelected(TreeNode<TreeViewItem> node)
        {
            object tag = node.Item.Tag;

            if (tag is File)//文件节点
            {
                File file = tag as File;
                switch (file.Type)
                {
                    case FileType.PDF:
                        string path = "Assets/Documents/Engineering/" + file.Name + ".pdf";
                        AsyncLoadAssetOperation async = Assets.LoadAssetAsync<TextAsset>(path);
                        if (async != null)
                        {
                            async.OnCompleted(x =>
                            {
                                AsyncLoadAssetOperation loader = x as AsyncLoadAssetOperation;
                                TextAsset asset = loader.GetAsset<TextAsset>();
                                displayPanel.LoadDocumentWithBuffer(asset.bytes);
                            });
                        }
                        break;
                    case FileType.Image:
                        break;
                    case FileType.Video:
                        break;
                    default:
                        break;
                }

            }
        }
    }
}
